using BL.Contracts.IMapper;
using BL.Contracts.Service.Customer;
using BL.Extensions;
using BL.Services.Base;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Customer;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Resources;
using Serilog;
using Shared.DTOs.Customer;
using Shared.GeneralModels;
using System.Linq.Expressions;

namespace BL.Services.Customer
{
	public class CustomerService : BaseService<TbCustomer, CustomerDto>, ICustomerService
	{
		private readonly ITableRepository<TbOrder> _orderRepository;
		private readonly ILogger _logger;
		private readonly ICustomerRepository _customerRepository;
		private readonly IBaseMapper _mapper;

		// Constructor تقليدي
		public CustomerService(
			ICustomerRepository customerRepository,
			IBaseMapper mapper,
			ILogger logger,
			ITableRepository<TbOrder> orderRepository)
			: base(customerRepository, mapper) // نمرر للـ base class
		{
			_orderRepository = orderRepository;
			_logger = logger;
			_customerRepository = customerRepository;
			_mapper = mapper;
		}
		/// <summary>
		/// Get all customers
		/// </summary>
		public async Task<ResponseModel<IEnumerable<CustomerDto>>> GetAllAsync()
		{
			try
			{
				var customers = await _customerRepository.GetAllAsync();
				var dtos = _mapper.MapList<TbCustomer, CustomerDto>(customers);
				return new ResponseModel<IEnumerable<CustomerDto>>
				{
					Success = true,
					Data = dtos
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel<IEnumerable<CustomerDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Get customer by ID
		/// </summary>
		public async Task<ResponseModel<CustomerDto>> GetByIdAsync(Guid id)
		{
			try
			{
				var customer = await _customerRepository.FindByIdWithUserAsync(id);
				if (customer == null)
					return new ResponseModel<CustomerDto>
					{
						Success = false,
						Message = NotifiAndAlertsResources.NoDataFound
					};

				var dto = _mapper.MapModel<TbCustomer, CustomerDto>(customer);
				return new ResponseModel<CustomerDto>
				{
					Success = true,
					Data = dto
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel<CustomerDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Get customer by user ID
		/// </summary>
		public async Task<CustomerDto> GetByUserIdAsync(string userId)
		{
			try
			{
				var customer = await _customerRepository.GetCustomerByUserIdAsync(userId);
				var dto = _mapper.MapModel<TbCustomer, CustomerDto>(customer);
				return dto;
            }
			catch (Exception ex)
			{
				_logger.Error(ex, "Error while getting customer by user ID: {UserId}", userId);
                throw new Exception ("Error while getting customer.");
			}
		}

		/// <summary>
		/// Search customers with pagination and filtering
		/// </summary>
		public async Task<ResponseModel<AdvancedPagedResult<CustomerDto>>> SearchAsync(
	BaseSearchCriteriaModel criteriaModel,
	CancellationToken cancellationToken = default)
		{
			try
			{
				if (criteriaModel == null)
					throw new ArgumentNullException(nameof(criteriaModel));
				if (criteriaModel.PageNumber < 1)
					throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);
				if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
					throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

				// Build filter expression
				Expression<Func<TbCustomer, bool>> filter = x => !x.IsDeleted;

				var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
				if (!string.IsNullOrWhiteSpace(searchTerm))
				{
					filter = filter.And(x =>
						(x.User.FirstName != null && x.User.FirstName.ToLower().Contains(searchTerm)) ||
						(x.User.LastName != null && x.User.LastName.ToLower().Contains(searchTerm)) ||
						(x.User.Email != null && x.User.Email.ToLower().Contains(searchTerm))
					);
				}

				// Build ordering expression
				Func<IQueryable<TbCustomer>, IOrderedQueryable<TbCustomer>> orderByExpression;

				if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
				{
					switch (criteriaModel.SortBy.ToLowerInvariant())
					{
						case "name":
						case "firstname":
							orderByExpression = criteriaModel.SortDirection?.ToLowerInvariant() == "desc"
								? q => q.OrderByDescending(x => x.User.FirstName).ThenByDescending(x => x.User.LastName)
								: q => q.OrderBy(x => x.User.FirstName).ThenBy(x => x.User.LastName);
							break;

						case "lastname":
							orderByExpression = criteriaModel.SortDirection?.ToLowerInvariant() == "desc"
								? q => q.OrderByDescending(x => x.User.LastName).ThenByDescending(x => x.User.FirstName)
								: q => q.OrderBy(x => x.User.LastName).ThenBy(x => x.User.FirstName);
							break;

						case "email":
							orderByExpression = criteriaModel.SortDirection?.ToLowerInvariant() == "desc"
								? q => q.OrderByDescending(x => x.User.Email)
								: q => q.OrderBy(x => x.User.Email);
							break;

						case "lastlogin":
						case "lastlogindate":
							orderByExpression = criteriaModel.SortDirection?.ToLowerInvariant() == "desc"
								? q => q.OrderByDescending(x => x.User.LastLoginDate)
								: q => q.OrderBy(x => x.User.LastLoginDate);
							break;

						case "createddateutc":
						default:
							orderByExpression = criteriaModel.SortDirection?.ToLowerInvariant() == "desc"
								? q => q.OrderByDescending(x => x.CreatedDateUtc)
								: q => q.OrderBy(x => x.CreatedDateUtc);
							break;
					}
				}
				else
				{
					orderByExpression = q => q.OrderByDescending(x => x.CreatedDateUtc);
				}

				// Get paginated data WITHOUT includes first
				var pagedResult = await _customerRepository.GetPageAsync(
					criteriaModel.PageNumber,
					criteriaModel.PageSize,
					filter,
					orderBy: orderByExpression,
					cancellationToken: cancellationToken
				);

				// Now load the related entities for the items we got
				var customerIds = pagedResult.Items.Select(x => x.Id).ToList();

				var customersWithDetails = await _customerRepository
					.GetQueryable()
					.Include(x => x.User)
					.Where(x => customerIds.Contains(x.Id))
					.ToListAsync(cancellationToken);

				// Sort the loaded items to match the original order
				var orderedCustomers = customerIds
					.Select(id => customersWithDetails.FirstOrDefault(c => c.Id == id))
					.Where(c => c != null)
					.ToList();

				// Map to DTO
				var itemsDto = _mapper.MapList<TbCustomer, CustomerDto>(orderedCustomers);

				var totalPages = (int)Math.Ceiling((double)pagedResult.TotalRecords / criteriaModel.PageSize);

				var result = new AdvancedPagedResult<CustomerDto>
				{
					Items = itemsDto.ToList(),
					TotalRecords = pagedResult.TotalRecords,
					PageSize = criteriaModel.PageSize,
					PageNumber = criteriaModel.PageNumber,
					TotalPages = totalPages
				};

				return new ResponseModel<AdvancedPagedResult<CustomerDto>>
				{
					Success = true,
					Data = result
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel<AdvancedPagedResult<CustomerDto>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
		//public async Task<ResponseModel<AdvancedPagedResult<CustomerDto>>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
		//{
		//	try
		//	{
		//		if (criteriaModel == null)
		//			throw new ArgumentNullException(nameof(criteriaModel));

		//		if (criteriaModel.PageNumber < 1)
		//			throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

		//		if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
		//			throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

		//		// Base filter
		//		Expression<Func<TbCustomer, bool>> filter = x => !x.IsDeleted;

		//		// Combine expressions manually
		//		var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
		//		if (!string.IsNullOrWhiteSpace(searchTerm))
		//		{
		//			filter = filter.And(x =>
		//				(x.User.FirstName != null && x.User.FirstName.ToLower().Contains(searchTerm)) ||
		//				(x.User.LastName != null && x.User.LastName.ToLower().Contains(searchTerm)) ||
		//				(x.User.Email != null && x.User.Email.ToLower().Contains(searchTerm))
		//			);
		//		}

		//		var customers = await _customerRepository.GetPageWithUserAsync(
		//			criteriaModel.PageNumber,
		//			criteriaModel.PageSize,
		//			filter,
		//			orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc));

		//		var itemsDto = _mapper.MapList<TbCustomer, CustomerDto>(customers.Items);

		//		var totalPages = (int)Math.Ceiling((double)customers.TotalRecords / criteriaModel.PageSize);
		//		var result = new AdvancedPagedResult<CustomerDto>
		//		{
		//			Items = itemsDto.ToList(),
		//			TotalRecords = customers.TotalRecords,
		//			PageSize = criteriaModel.PageSize,
		//			PageNumber = criteriaModel.PageNumber,
		//			TotalPages = totalPages
		//		};

		//		return new ResponseModel<AdvancedPagedResult<CustomerDto>>
		//		{
		//			Success = true,
		//			Data = result
		//		};
		//	}
		//	catch (Exception ex)
		//	{
		//		return new ResponseModel<AdvancedPagedResult<CustomerDto>>
		//		{
		//			Success = false,
		//			Message = ex.Message
		//		};
		//	}
		//}

		/// <summary>
		/// Save or update customer
		/// </summary>
		public async Task<ResponseModel<CustomerDto>> SaveAsync(CustomerDto dto)
		{
			try
			{
				TbCustomer entity;

				if (dto.Id == Guid.Empty)
				{
					// Create new
					entity = _mapper.MapModel<CustomerDto, TbCustomer>(dto);
					await _customerRepository.CreateAsync(entity);
				}
				else
				{
					// Update existing
					entity = await _customerRepository.FindByIdAsync(dto.Id);
					if (entity == null)
						return new ResponseModel<CustomerDto>
						{
							Success = false,
							Message = NotifiAndAlertsResources.NoDataFound
						};

					_mapper.MapModel<CustomerDto, TbCustomer>(dto);
					await _customerRepository.UpdateAsync(entity);
				}

				var resultDto = _mapper.MapModel<TbCustomer, CustomerDto>(entity);
				return new ResponseModel<CustomerDto>
				{
					Success = true,
					Data = resultDto,
					Message = NotifiAndAlertsResources.SavedSuccessfully
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel<CustomerDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Delete customer by ID
		/// </summary>
		public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
		{
			try
			{
				var result = await _customerRepository.HardDeleteAsync(id);
				if (!result)
					return new ResponseModel<bool>
					{
						Success = false,
						Message = NotifiAndAlertsResources.NoDataFound
					};

				return new ResponseModel<bool>
				{
					Success = true,
					Message = NotifiAndAlertsResources.DeletedSuccessfully
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Change customer account status
		/// </summary>
		public async Task<ResponseModel<bool>> ChangeStatusAsync(Guid customerId, UserStateType newStatus)
		{
			try
			{
				var customer = await _customerRepository.FindByIdWithUserAsync(customerId);
				if (customer == null)
					return new ResponseModel<bool>
					{
						Success = false,
						Message = NotifiAndAlertsResources.NoDataFound
					};

				if (customer.User != null)
				{
					customer.User.UserState = newStatus;
					await _customerRepository.UpdateAsync(customer);
				}

				return new ResponseModel<bool>
				{
					Success = true,
					Message = "Account status updated successfully"
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Get customer account status
		/// </summary>
		public async Task<ResponseModel<UserStateType>> GetStatusAsync(Guid customerId)
		{
			try
			{
				var customer = await _customerRepository.FindByIdWithUserAsync(customerId);
				if (customer == null || customer.User == null)
					return new ResponseModel<UserStateType>
					{
						Success = false,
						Message = NotifiAndAlertsResources.NoDataFound
					};

				return new ResponseModel<UserStateType>
				{
					Success = true,
					Data = customer.User.UserState
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel<UserStateType>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
		public async Task<ResponseModel<AdvancedPagedResult<OrderHistoryDto>>> GetOrderHistoryAsync(Guid customerId, BaseSearchCriteriaModel criteria)
		{
			try
			{
				var customer = await _customerRepository.FindAsync(c => c.Id == customerId);

				if (customer == null)
					return new ResponseModel<AdvancedPagedResult<OrderHistoryDto>>
					{
						Success = false,
						Message = NotifiAndAlertsResources.NoDataFound
					};

				// Await the query first to get IEnumerable<TbOrder>
				var ordersData = await _orderRepository.GetAsync(o => o.UserId == customer.UserId);

				var totalRecords = ordersData.Count();
				var totalPages = (int)Math.Ceiling((double)totalRecords / criteria.PageSize);

				// Now you can use LINQ methods on the actual collection
				var orders = ordersData
					.OrderByDescending(o => o.CreatedDateUtc)
					.Skip((criteria.PageNumber - 1) * criteria.PageSize)
					.Take(criteria.PageSize)
					.Select(o => new OrderHistoryDto
					{
						Id = o.Id,
						OrderNumber = o.Number,
						OrderDate = o.CreatedDateUtc,
						TotalAmount = o.SubTotal,
						Status = o.OrderStatus,
					})
					.ToList();

				var result = new AdvancedPagedResult<OrderHistoryDto>
				{
					Items = orders,
					TotalRecords = totalRecords,
					PageSize = criteria.PageSize,
					PageNumber = criteria.PageNumber,
					TotalPages = totalPages
				};

				return new ResponseModel<AdvancedPagedResult<OrderHistoryDto>>
				{
					Success = true,
					Data = result,
					Message = NotifiAndAlertsResources.DataRetrieved
				};
			}
			catch (Exception ex)
			{
				_logger?.Error(ex, "Error getting order history for customer {CustomerId}", customerId);
				return new ResponseModel<AdvancedPagedResult<OrderHistoryDto>>
				{
					Success = false,
					Message = NotifiAndAlertsResources.Error
				};
			}
		}
		/// <summary>
		/// Get customer order history
		/// </summary>
		//public async Task<ResponseModel<AdvancedPagedResult<object>>> GetOrderHistoryAsync(Guid customerId, BaseSearchCriteriaModel criteria)
		//{
		//    try
		//    {
		//        var customer = await _customerRepository.FindByIdWithUserAsync(customerId);
		//        if (customer == null || customer.User == null)
		//            return new ResponseModel<AdvancedPagedResult<object>>
		//            {
		//                Success = false,
		//                Message = NotifiAndAlertsResources.NoDataFound
		//            };

		//        // Get orders from the customer's user
		//        var query = customer.User.Orders.AsQueryable();
		//        var totalRecords = query.Count();
		//        var totalPages = (int)Math.Ceiling((double)totalRecords / criteria.PageSize);

		//        var orders = await query
		//            .OrderByDescending(o => o.CreatedDateUtc)
		//            .Skip((criteria.PageNumber - 1) * criteria.PageSize)
		//            .Take(criteria.PageSize)
		//            .Cast<object>()
		//            .ToListAsync();

		//        var result = new AdvancedPagedResult<object>
		//        {
		//            Items = orders,
		//            TotalRecords = totalRecords,
		//            PageSize = criteria.PageSize,
		//            PageNumber = criteria.PageNumber,
		//            TotalPages = totalPages
		//        };

		//        return new ResponseModel<AdvancedPagedResult<object>>
		//        {
		//            Success = true,
		//            Data = result
		//        };
		//    }
		//    catch (Exception ex)
		//    {
		//        return new ResponseModel<AdvancedPagedResult<object>>
		//        {
		//            Success = false,
		//            Message = ex.Message
		//        };
		//    }
		//}

		/// <summary>
		/// Get customer wallet transaction history
		/// </summary>
		public async Task<ResponseModel<AdvancedPagedResult<object>>> GetWalletHistoryAsync(Guid customerId, BaseSearchCriteriaModel criteria)
		{
			try
			{
				var transactions = await _customerRepository.GetWalletTransactionsAsync(
					customerId,
					criteria.PageNumber,
					criteria.PageSize);

				var totalPages = (int)Math.Ceiling((double)transactions.TotalRecords / criteria.PageSize);

				var result = new AdvancedPagedResult<object>
				{
					Items = transactions.Items.Cast<object>().ToList(),
					TotalRecords = transactions.TotalRecords,
					PageSize = criteria.PageSize,
					PageNumber = criteria.PageNumber,
					TotalPages = totalPages
				};

				return new ResponseModel<AdvancedPagedResult<object>>
				{
					Success = true,
					Data = result
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel<AdvancedPagedResult<object>>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Get customer wallet balance
		/// </summary>
		public async Task<ResponseModel<decimal>> GetWalletBalanceAsync(Guid customerId)
		{
			try
			{
				var customer = await _customerRepository.FindByIdWithUserAsync(customerId);
				if (customer == null || customer.User == null)
					return new ResponseModel<decimal>
					{
						Success = false,
						Message = NotifiAndAlertsResources.NoDataFound
					};

				var totalBalance = customer.User.CustomerWallets
					.Sum(w => w.Balance);

				return new ResponseModel<decimal>
				{
					Success = true,
					Data = totalBalance
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel<decimal>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
	}
}

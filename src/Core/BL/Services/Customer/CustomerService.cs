using BL.Contracts.IMapper;
using BL.Contracts.Service.Customer;
using BL.Extensions;
using BL.Services.Base;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Contracts.Repositories.Customer;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Customer;
using Microsoft.EntityFrameworkCore;
using Resources;
using Shared.DTOs.Customer;
using Shared.GeneralModels;
using System.Linq.Expressions;

namespace BL.Services.Customer
{
    public class CustomerService(ICustomerRepository _customerRepository, IBaseMapper _mapper)
        : BaseService<TbCustomer, CustomerDto>(_customerRepository, _mapper), ICustomerService
    {

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
        /// Search customers with pagination and filtering
        /// </summary>
        public async Task<ResponseModel<AdvancedPagedResult<CustomerDto>>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
        {
            try
            {
                if (criteriaModel == null)
                    throw new ArgumentNullException(nameof(criteriaModel));

                if (criteriaModel.PageNumber < 1)
                    throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

                if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                    throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

                // Base filter
                Expression<Func<TbCustomer, bool>> filter = x => !x.IsDeleted;

                // Combine expressions manually
                var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    filter = filter.And(x =>
                        (x.User.FirstName != null && x.User.FirstName.ToLower().Contains(searchTerm)) ||
                        (x.User.LastName != null && x.User.LastName.ToLower().Contains(searchTerm)) ||
                        (x.User.Email != null && x.User.Email.ToLower().Contains(searchTerm))
                    );
                }

                var customers = await _customerRepository.GetPageWithUserAsync(
                    criteriaModel.PageNumber,
                    criteriaModel.PageSize,
                    filter,
                    orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc));

                var itemsDto = _mapper.MapList<TbCustomer, CustomerDto>(customers.Items);

                var totalPages = (int)Math.Ceiling((double)customers.TotalRecords / criteriaModel.PageSize);
                var result = new AdvancedPagedResult<CustomerDto>
                {
                    Items = itemsDto.ToList(),
                    TotalRecords = customers.TotalRecords,
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

        /// <summary>
        /// Get customer order history
        /// </summary>
        public async Task<ResponseModel<AdvancedPagedResult<object>>> GetOrderHistoryAsync(Guid customerId, BaseSearchCriteriaModel criteria)
        {
            try
            {
                var customer = await _customerRepository.FindByIdWithUserAsync(customerId);
                if (customer == null || customer.User == null)
                    return new ResponseModel<AdvancedPagedResult<object>>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NoDataFound
                    };

                // Get orders from the customer's user
                var query = customer.User.Orders.AsQueryable();
                var totalRecords = query.Count();
                var totalPages = (int)Math.Ceiling((double)totalRecords / criteria.PageSize);

                var orders = await query
                    .OrderByDescending(o => o.CreatedDateUtc)
                    .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                    .Take(criteria.PageSize)
                    .Cast<object>()
                    .ToListAsync();

                var result = new AdvancedPagedResult<object>
                {
                    Items = orders,
                    TotalRecords = totalRecords,
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

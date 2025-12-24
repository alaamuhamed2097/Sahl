using Dashboard.Constants;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.General;
using Resources;
using Shared.DTOs.Catalog.Category;
using Shared.GeneralModels;

namespace Dashboard.Services.ECommerce.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IApiService _apiService;

        public CategoryService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all categories with optional filters.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<CategoryDto>>($"{ApiEndpoints.Category.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                Console.WriteLine($"GetAllAsync Error: {ex}");
                return new ResponseModel<IEnumerable<CategoryDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get category by ID.
        /// </summary>
        public async Task<ResponseModel<CategoryDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<CategoryDto>($"{ApiEndpoints.Category.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                Console.WriteLine($"GetByIdAsync Error: {ex}");
                return new ResponseModel<CategoryDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Save or update a category.
        /// </summary>
        public async Task<ResponseModel<bool>> SaveAsync(CategoryDto category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            try
            {
                var response = await _apiService.PostAsync<CategoryDto, bool>($"{ApiEndpoints.Category.Save}", category);

                // Log response for easier debugging when save fails
                if (response == null)
                {
                    Console.WriteLine("SaveAsync: null response received from API.");
                    return new ResponseModel<bool> { Success = false, Message = "No response from server" };
                }

                if (!response.Success)
                {
                    // Print detailed response to console to help debug
                    Console.WriteLine($"SaveAsync failed. Message: {response.Message}");
                    if (response.Errors != null && response.Errors.Any())
                    {
                        Console.WriteLine("Errors:");
                        foreach (var err in response.Errors)
                        {
                            Console.WriteLine(err);
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                // Log error here
                Console.WriteLine($"SaveAsync Exception: {ex}");
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary> 
        /// Change tree view serials.
        /// </summary>
        public async Task<ResponseModel<bool>> ReorderTreeAsync(Dictionary<Guid, string> serialAssignments)
        {
            if (serialAssignments == null) throw new ArgumentNullException(nameof(serialAssignments));

            try
            {
                var response = await _apiService.PostAsync<Dictionary<Guid, string>, bool>($"{ApiEndpoints.Category.ChangeTreeViewSerials}", serialAssignments);
                return response;
            }
            catch (Exception ex)
            {
                // Log error here
                Console.WriteLine($"ReorderTreeAsync Error: {ex}");
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        /// <summary>
        /// Delete a category by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, bool>($"{ApiEndpoints.Category.Delete}", id);
                return result;
            }
            catch (Exception)
            {
                // Log error here
                Console.WriteLine($"DeleteAsync failed for id: {id}");
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.DeleteFailed
                };
            }
        }

    }
}

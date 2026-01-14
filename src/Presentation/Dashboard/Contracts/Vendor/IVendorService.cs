using Shared.DTOs.Vendor;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Vendor
{
    public interface IVendorService
    {
        Task<ResponseModel<IEnumerable<VendorDto>>> GetAllAsync();
        Task<ResponseModel<VendorDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<VendorRegistrationResponseDto>> CreateVendorAsync(VendorRegistrationRequestDto request);
        Task<ResponseModel<VendorUpdateResponseDto>> UpdateVendorAsync(VendorUpdateRequestDto request);
        Task<ResponseModel<bool>> UpdateVendorStatusAsync(UpdateVendorStatusDto dto);
        Task<ResponseModel<bool>> UpdateUserStatusAsync(UpdateUserStatusDto dto);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
    }
}

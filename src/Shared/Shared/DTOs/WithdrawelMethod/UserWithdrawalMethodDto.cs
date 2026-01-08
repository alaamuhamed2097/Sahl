using Shared.DTOs.Base;

namespace Shared.DTOs.WithdrawalMethod
{
    public class UserWithdrawalMethodDto : BaseDto
    {
        public Guid WithdrawalMethodId { get; set; }

        public string UserId { get; set; } = string.Empty;
        public List<WithdrawalMethodFieldDto> WithdrawalMethodFields { get; set; } = new List<WithdrawalMethodFieldDto>();
    }

}

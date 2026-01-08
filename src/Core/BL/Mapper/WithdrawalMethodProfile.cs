using Domains.Entities.WithdrawalMethods;
using Domains.Views.WithdrawalMethods;
using Shared.DTOs.WithdrawalMethod;
using Shared.DTOs.WithdrawelMethod;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BL.Mapper
{
    // Withdrawal-related mappings
    public partial class MappingProfile
    {
        private void ConfigureWithdrawalMethodsMappings()
        {
            // Withdrawal main mappings
            CreateMap<TbWithdrawalMethod, WithdrawalMethodDto>()
                .ReverseMap();
            CreateMap<TbWithdrawalMethodField, WithdrawalMethodFieldDto>()
                .ReverseMap();
            CreateMap<TbUserWithdrawalMethod, UserWithdrawalMethodDto>()
               .ForMember(dest => dest.WithdrawalMethodFields, opt => opt.Ignore())
               .ReverseMap();

            // Withdrawal fields
            CreateMap<TbField, FieldDto>()
                .ReverseMap();
            // Withdrawal with fields
            CreateMap<VwWithdrawalMethodsWithFields, WithdrawalMethodDto>()
                .ForMember(dest => dest.Fields, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.WithdrawalMethodFieldsJson)
                        ? JsonSerializer.Deserialize<List<FieldDto>>(src.WithdrawalMethodFieldsJson,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                            })
                        : new List<FieldDto>()));

            // Withdrawal with fields
            CreateMap<VwWithdrawalMethodsFieldsValues, WithdrawalMethodsFieldsValuesDto>()
                .ForMember(dest => dest.FieldsJson, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.FieldsJson)
                        ? JsonSerializer.Deserialize<List<FieldValueModel>>(src.FieldsJson,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                            })
                        : new List<FieldValueModel>()));
        }
    }
}

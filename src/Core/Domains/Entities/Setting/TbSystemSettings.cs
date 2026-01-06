using Common.Enumerations.Settings;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Setting
{
    public class TbSystemSettings : BaseEntity
    {
        public SystemSettingKey SettingKey { get; set; }

        [Required]
        [StringLength(500)]
        public string SettingValue { get; set; } = string.Empty;

        public SystemSettingDataType DataType { get; set; }

        public SystemSettingCategory Category { get; set; }
    }
}

using System.ComponentModel;

namespace Common.Enumerations.Settings
{
    /// <summary>
    /// Data types for system settings
    /// أنواع البيانات للإعدادات
    /// </summary>
    public enum SystemSettingDataType
    {
        [Description("String")]
        String = 1,

        [Description("Integer")]
        Integer = 2,

        [Description("Decimal")]
        Decimal = 3,

        [Description("Boolean")]
        Boolean = 4,

        [Description("DateTime")]
        DateTime = 5,

        [Description("Json")]
        Json = 6
    }
}

using Common.Enumerations.Settings;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Setting
{
	/// <summary>
	/// DTO for updating system setting
	/// </summary>
	public class UpdateSystemSettingDto
	{
		[Required(ErrorMessage = "Setting key is required")]
		public SystemSettingKey key { get; set; }

		[Required(ErrorMessage = "Setting value is required")]
		[StringLength(500, ErrorMessage = "Value cannot exceed 500 characters")]
		public string value { get; set; } = string.Empty;

		[Required(ErrorMessage = "Data type is required")]
		public SystemSettingDataType dataType { get; set; }

		[Required(ErrorMessage = "Category is required")]
		public SystemSettingCategory category { get; set; }
	}
}
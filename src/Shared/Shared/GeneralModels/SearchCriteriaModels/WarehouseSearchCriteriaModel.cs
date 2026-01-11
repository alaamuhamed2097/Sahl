using Common.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.GeneralModels.SearchCriteriaModels
{
	public class WarehouseSearchCriteriaModel : BaseSearchCriteriaModel
	{
		/// <summary>
		/// Filter by active status (true = active, false = inactive, null = all)
		/// </summary>
		public bool? IsActive { get; set; }

		/// <summary>
		/// Filter by warehouse type (true = platform warehouse, false = vendor warehouse, null = all)
		/// </summary>
		public bool? IsDefaultPlatformWarehouse { get; set; }

		/// <summary>
		/// Filter by specific vendor ID (only applicable for vendor warehouses)
		/// </summary>
		public Guid? VendorId { get; set; }

		/// <summary>
		/// Filter by email address
		/// </summary>
		[EmailAddress(ErrorMessage = "Invalid email format")]
		public string? Email { get; set; }

		/// <summary>
		/// Filter by address
		/// </summary>
		public string? Address { get; set; }

		/// <summary>
		/// Filter warehouses created after this date
		/// </summary>
		public DateTime? CreatedDateFrom { get; set; }

		/// <summary>
		/// Filter warehouses created before this date
		/// </summary>
		public DateTime? CreatedDateTo { get; set; }
	}
}

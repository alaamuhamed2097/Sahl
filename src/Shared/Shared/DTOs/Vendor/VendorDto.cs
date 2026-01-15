using Common.Enumerations.IdentificationType;
using Common.Enumerations.User;
using Common.Enumerations.VendorStatus;
using Common.Enumerations.VendorType;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.Vendor
{
    public class VendorDto : BaseDto
    {
        // Administrator's name
        public string UserId { get; set; } = null!;
        public string AdministratorFirstName { get; set; } = null!;
        public string AdministratorLastName { get; set; } = null!;
        public DateOnly BirthDate { get; set; }

        // Identification Details
        public IdentificationType IdentificationType { get; set; }
        public string IdentificationNumber { get; set; } = null!;
        public string IdentificationImageFrontPath { get; set; } = null!;
        public string IdentificationImageBackPath { get; set; } = null!;

        // Business Information
        public VendorType VendorType { get; set; }
        public string StoreName { get; set; } = null!;
        public bool IsRealEstateRegistered { get; set; }

        // Contact Information
        public string PhoneCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Email { get; set; }

        public string Address { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public Guid CityId { get; set; }

        public string CountryName { get; set; } = null!;
        public string StateName { get; set; } = null!;
        public string CityName { get; set; } = null!;

        // Additional Info
        public string? Notes { get; set; }
        public decimal? AverageRating { get; set; }
        public VendorStatus Status { get; set; } = VendorStatus.Pending;
        public UserStateType UserState { get; set; }

        public bool EmailConformed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public DateTime RegistrationDate => CreatedDateUtc;
        public string AdministratorFullName => AdministratorFirstName + ' ' + AdministratorLastName;

        [NotMapped]
        public DateTime CreatedDateUtc { get; set; }
    }
}

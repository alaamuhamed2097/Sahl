using Common.Enumerations.IdentificationType;
using Common.Enumerations.User;
using Common.Enumerations.VendorStatus;
using Common.Enumerations.VendorType;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shared.DTOs.Vendor
{
    public class VendorPreviewDto
    {
        // Administrator's name
        public string AdministratorFirstName { get; set; } = null!;
        public string AdministratorLastName { get; set; } = null!;

        // Business Information
        public VendorType VendorType { get; set; }
        public string StoreName { get; set; } = null!;
        public bool IsRealEstateRegistered { get; set; }
        public string LogoPath { get; set; } 

        public string Address { get; set; } = null!;
        public string PostalCode { get; set; } = null!;

        public string CountryName { get; set; } = null!;
        public string StateName { get; set; } = null!;
        public string CityName { get; set; } = null!;

        // Additional Info
        public decimal? AverageRating { get; set; }
    }
}

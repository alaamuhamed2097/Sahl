namespace Common.Enumerations.Brand
{
    /// <summary>
    /// Types of brands
    /// </summary>
    public enum BrandType
    {
        /// <summary>
        /// Local or private label brand owned by seller
        /// </summary>
        LocalPrivateLabel = 1,

        /// <summary>
        /// Authorized distributor of established brand
        /// </summary>
        AuthorizedDistributor = 2,

        /// <summary>
        /// Generic or unbranded products
        /// </summary>
        Generic = 3,

        /// <summary>
        /// International brand with local representation
        /// </summary>
        International = 4
    }

    /// <summary>
    /// Brand registration request status
    /// </summary>
    public enum BrandRegistrationStatus
    {
        /// <summary>
        /// Draft - not submitted
        /// </summary>
        Draft = 1,

        /// <summary>
        /// Submitted and pending review
        /// </summary>
        Pending = 2,

        /// <summary>
        /// Under review by admin
        /// </summary>
        UnderReview = 3,

        /// <summary>
        /// Needs additional documentation
        /// </summary>
        NeedsMoreInfo = 4,

        /// <summary>
        /// Documents being verified
        /// </summary>
        DocumentVerification = 5,

        /// <summary>
        /// Approved and brand created
        /// </summary>
        Approved = 6,

        /// <summary>
        /// Rejected
        /// </summary>
        Rejected = 7,

        /// <summary>
        /// Cancelled by seller
        /// </summary>
        Cancelled = 8
    }

    /// <summary>
    /// Types of brand documents
    /// </summary>
    public enum BrandDocumentType
    {
        /// <summary>
        /// Trademark registration certificate
        /// </summary>
        TrademarkCertificate = 1,

        /// <summary>
        /// Business license
        /// </summary>
        BusinessLicense = 2,

        /// <summary>
        /// Authorization letter from brand owner
        /// </summary>
        AuthorizationLetter = 3,

        /// <summary>
        /// Distributor agreement
        /// </summary>
        DistributorAgreement = 4,

        /// <summary>
        /// Product certificates
        /// </summary>
        ProductCertificates = 5,

        /// <summary>
        /// Tax registration document
        /// </summary>
        TaxRegistration = 6,

        /// <summary>
        /// Company profile
        /// </summary>
        CompanyProfile = 7,

        /// <summary>
        /// Other supporting documents
        /// </summary>
        Other = 8
    }
}

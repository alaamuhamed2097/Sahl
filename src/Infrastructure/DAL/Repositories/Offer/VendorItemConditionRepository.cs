using Common.Enumerations.Offer;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Offer;
using DAL.Exceptions;
using DAL.ResultModels.DAL.ResultModels;
using Domains.Entities.Offer;
using Domains.Views.Offer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;

namespace DAL.Repositories.Offer
{
    /// <summary>
    /// Implementation of offer repository with transaction support
    /// </summary>
    public class VendorItemConditionRepository : TableRepository<TbOfferCondition>, IVendorItemConditionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public VendorItemConditionRepository(ApplicationDbContext dbContext, ILogger logger)
            : base(dbContext, logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
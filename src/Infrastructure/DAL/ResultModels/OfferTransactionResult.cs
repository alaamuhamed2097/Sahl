namespace DAL.ResultModels
{

namespace DAL.ResultModels
    {
        /// <summary>
        /// Result model for offer transaction operations
        /// </summary>
        public class OfferTransactionResult
        {
            /// <summary>
            /// Indicates if the transaction was successful
            /// </summary>
            public bool Success { get; set; }

            /// <summary>
            /// ID of the offer involved in the transaction
            /// </summary>
            public Guid OfferId { get; set; }

            /// <summary>
            /// ID of the pricing combination involved in the transaction
            /// </summary>
            public Guid PricingId { get; set; }

            /// <summary>
            /// New price after the transaction (if applicable)
            /// </summary>
            public decimal NewPrice { get; set; }

            /// <summary>
            /// New available quantity after the transaction
            /// </summary>
            public int NewQuantity { get; set; }

            /// <summary>
            /// Current reserved quantity after the transaction
            /// </summary>
            public int ReservedQuantity { get; set; }

            /// <summary>
            /// Error message if the transaction failed
            /// </summary>
            public string ErrorMessage { get; set; }
        }
    }
}

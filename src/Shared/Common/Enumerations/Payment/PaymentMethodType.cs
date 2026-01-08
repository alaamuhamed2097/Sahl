namespace Common.Enumerations.Payment;

/// <summary>
/// Payment method types enum
/// UPDATED: Added WalletAndCard for mixed payment support
/// </summary>
public enum PaymentMethodType
{
    /// <summary>
    /// Cash payment on delivery
    /// </summary>
    CashOnDelivery = 1,

    /// <summary>
    /// Payment using customer wallet balance
    /// </summary>
    Wallet = 2,

    /// <summary>
    /// Payment using credit/debit card via gateway
    /// </summary>
    Card = 3,

    /// <summary>
    /// Mixed payment: wallet balance first, then card for remaining amount
    /// </summary>
    WalletAndCard = 4
}
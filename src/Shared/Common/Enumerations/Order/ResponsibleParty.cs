namespace Common.Enumerations.Order
{
    public enum ResponsibleParty
    {
        Vendor = 1,          // Vendor pays return costs
        Customer = 2,        // Customer pays return costs
        Platform = 3,        // Platform/marketplace pays
        Courier = 4          // Shipping company pays (damaged during shipping)
    }
}
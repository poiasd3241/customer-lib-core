namespace CustomerLibCore.Api.Dtos.Customers
{
    public interface ICustomerBasicDetails
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string PhoneNumber { get; set; }
        string Email { get; set; }
        string TotalPurchasesAmount { get; set; }
    }
}

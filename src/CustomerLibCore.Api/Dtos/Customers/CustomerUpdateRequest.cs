namespace CustomerLibCore.Api.Dtos.Customers
{
    public class CustomerUpdateRequest : ICustomerBasicDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TotalPurchasesAmount { get; set; }
    }
}

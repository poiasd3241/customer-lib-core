namespace CustomerLibCore.Domain.Models
{
	/// <summary>
	/// The customer object base.
	/// </summary>
	/// <typeparam name="TTotalPurchasesAmount">
	/// The type to use for the property <see cref="TotalPurchasesAmount"/>.</typeparam>
	public interface ICustomerDetails<TTotalPurchasesAmount> : ICustomerDetailsCore
	{
		TTotalPurchasesAmount TotalPurchasesAmount { get; set; }
	}
}

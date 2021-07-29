namespace CustomerLibCore.Api.Dtos
{
	public interface IResponse
	{
		/// <summary>
		/// The link to the resource.
		/// </summary>
		string Self { get; set; }
	}
}

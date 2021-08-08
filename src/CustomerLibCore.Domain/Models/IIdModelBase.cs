namespace CustomerLibCore.Domain.Models
{
	public interface IIdModelBase<T> : IModelBase<T>
	{
		bool EqualsByValueExcludingId(T obj);
	}
}

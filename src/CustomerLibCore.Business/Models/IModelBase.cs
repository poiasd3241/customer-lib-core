namespace CustomerLibCore.Domain.Models
{
	public interface IModelBase<T>
	{
		bool EqualsByValue(T obj);
		T Copy();
	}
}

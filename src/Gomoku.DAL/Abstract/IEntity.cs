namespace Gomoku.DAL.Abstract;
public interface IEntity<T>
    where T : IComparable, IEquatable<T>
{
    public T Id { get; set; }
}

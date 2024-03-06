namespace Gomoku.DAL.Abstract;
public interface IEntity<T>
{
    public T Id { get; set; }
}

namespace ConsoleApp7.Utils;

public interface IRepository<T> 
{
    void Add(T item);
    void Remove(Guid id);
    T Get(Guid id);
    IEnumerable<T> GetAll();
}

namespace Server;

public class Wrapper<T>
{
    public Wrapper(T[] items)
    {
        this.items = items;
    }

    public T[] items { get; set; }
}
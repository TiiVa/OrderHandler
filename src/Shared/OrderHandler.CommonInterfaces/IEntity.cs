namespace OrderHandler.CommonInterfaces
{
    public interface IEntity<T>
    {
        public T Id { get; set; }
    }
}

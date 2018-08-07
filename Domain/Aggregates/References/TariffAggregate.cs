namespace Domain.Aggregates.References
{
    public class TariffAggregate
    {
        public TariffAggregate(long id, string name, string category, double price, bool deleted)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
            Deleted = deleted;
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Category { get; private set; }
        public double Price { get; private set; }
        public bool Deleted { get; private set; }
    }
}
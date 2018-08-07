namespace ApiContracts.References.Agreement
{
    public class TariffView
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public bool Deleted { get; set; }
    }
}
namespace Domain.Aggregates.References
{
    public class MeterTypeAggregate
    {
        public MeterTypeAggregate(long id, string name, string manufacturerName, string modelName, byte calibrationIntervals, bool deleted)
        {
            Id = id;
            Name = name;
            ManufacturerName = manufacturerName;
            ModelName = modelName;
            CalibrationIntervals = calibrationIntervals;
            Deleted = deleted;
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
        public string ManufacturerName { get; private set; }
        public string ModelName { get; private set; }
        public byte CalibrationIntervals { get; private set; }
        public bool Deleted { get; private set; }
    }
}
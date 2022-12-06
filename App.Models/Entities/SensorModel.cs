namespace App.Models.Entities
{
    public class SensorModel
    {
        public decimal MeasurementValue { get; set; }
        public string DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

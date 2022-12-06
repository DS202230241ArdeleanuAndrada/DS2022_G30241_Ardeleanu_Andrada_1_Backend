namespace App.Models
{
    public class AddDeviceMeasurementsRequest
    {
        public int DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal MeasurementValue { get; set; }
    }
}


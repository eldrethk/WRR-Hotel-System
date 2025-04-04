namespace WRR8_0.Models
{
    public class RackRateModel
    {
        public int RackRateID { get; set; }
        public string RoomName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TierARate { get; set; }
        public decimal TierBRate { get; set; }
        public decimal TierCRate { get; set; }
    }
}

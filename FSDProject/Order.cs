namespace FSDProjectAPI
{
    public class Order
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string medicineNames { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public DateTime Time { get; set; }
    }
}

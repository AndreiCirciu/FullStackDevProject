namespace FSDProjectAPI
{
    public class Cart
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

    }
}

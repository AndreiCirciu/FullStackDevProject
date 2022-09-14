namespace FSDProjectAPI
{
    public class User
    {   
        public int ID { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } 
        public byte[] PasswordSalt { get; set; } 
    }
}

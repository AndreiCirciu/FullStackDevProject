﻿namespace FSDProjectAPI
{
    public class Account
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Funds { get; set; }
        public int IsAdmin { get; set; }
    }
}

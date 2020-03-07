using System;

namespace App.Data.Model
{
    public class User
    {
        public Guid guid { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
    }
}
using System.Collections.Generic;

namespace App.Data.Model
{
    public class BackhandStyle
    {
        public BackhandStyle()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
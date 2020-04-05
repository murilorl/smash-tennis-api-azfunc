using System.Collections.Generic;

namespace App.Data.Model
{
    public class DominantHand
    {
        public DominantHand()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string AltName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
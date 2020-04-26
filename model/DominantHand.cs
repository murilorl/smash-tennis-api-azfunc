using System.Collections.Generic;

using Newtonsoft.Json;

namespace App.Data.Model
{
    public class DominantHand
    {
   /*      public DominantHand()
        {
            Users = new HashSet<User>();
        } */

        public int Id { get; set; }
        public string Name { get; set; }
        public string AltName { get; set; }
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}
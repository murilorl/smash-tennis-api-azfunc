using System.Collections.Generic;

namespace App.Data.Model
{
    public class BackhandStyle
    {
/*         public BackhandStyle()
        {
            Users = new HashSet<User>();
        }
 */
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
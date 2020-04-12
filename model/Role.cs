using System.Collections.Generic;

namespace App.Data.Model
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
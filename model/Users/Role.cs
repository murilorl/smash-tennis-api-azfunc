using System.Collections.Generic;

namespace App.Data.Model.Users
{
    public class Role : BaseEntity
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
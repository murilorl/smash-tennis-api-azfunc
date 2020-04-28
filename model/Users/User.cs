using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Data.Model.Users
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ShortName { get; set; }
        public DateTime? Birthday { get; set; }
        public int PlayStyleId { get; set; }
        public PlayStyle PlayStyle { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public string FacebookId { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }

        [JsonIgnore]
        public DateTime LastLogin { get; set; }
    }
}
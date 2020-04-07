using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Data.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Active { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ShortName { get; set; }
        public DateTime Birthday { get; set; }
        public int DominantHandId { get; set; }
        public DominantHand DominantHand { get; set; }
        public int BackhandStyleId { get; set; }
        public BackhandStyle BackhandStyle { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public string FacebookId { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
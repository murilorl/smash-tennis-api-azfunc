using System;

using Newtonsoft.Json;

namespace App.Data.Model
{
    public class BaseEntity
    {
        [JsonIgnore]
        public DateTime Created { get; set; }

        [JsonIgnore]
        public DateTime Updated { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }
    }
}
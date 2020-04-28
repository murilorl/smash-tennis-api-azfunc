using System;

using Newtonsoft.Json;

namespace App.Data.Model.Tournaments
{
    public class Tournament
    {
        public Guid TournamentId { get; set; }

        [JsonIgnore]
        public DateTime Created { get; set; }

        [JsonIgnore]
        public DateTime Updated { get; set; }
        
        [JsonIgnore]
        public bool IsDeleted { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
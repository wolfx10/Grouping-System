namespace GroupingSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Event
    {


        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Location { get; set; }

        [Column("Tickets available")]
        public int Tickets_available { get; set; }

        public string eventAndTickets { get { return "Event: " + " " + Name + " | Tickets Available: " + Tickets_available; } }
    }
}

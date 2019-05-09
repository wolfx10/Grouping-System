namespace GroupingSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Message
    {

        public int Id { get; set; }

        public string User { get; set; }

        [Column("Message")]
        public string Message1 { get; set; }

        public bool? Seen { get; set; }

        public string Subject { get; set; }

        public string From { get; set; }

        public DateTime Time { get; set; }
    }
}

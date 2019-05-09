namespace GroupingSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Thread
    {
        public int Id { get; set; }

        public string threadTitle { get; set; }

        public string createdBy { get; set; }

        public int category { get; set; }

        [DataType(DataType.MultilineText)]
        public string OP { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}

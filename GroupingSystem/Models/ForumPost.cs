namespace GroupingSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ForumPost
    {
        public int Id { get; set; }

        public string PostedBy { get; set; }

        public string Comment { get; set; }

        public int? InThread { get; set; }

        public int? InCategory { get; set; }

        public DateTime Time { get; set; }

    }
}

namespace GroupingSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ForumCategory")]
    public partial class ForumCategory
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string CategoryDescription { get; set; }
    }
}

namespace GroupingSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Group
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string groupName { get; set; }

        public int groupSize { get; set; }

        [Required]
        [StringLength(50)]
        public string groupOwner { get; set; }

        [StringLength(50)]
        public string member1 { get; set; }

        [StringLength(50)]
        public string member2 { get; set; }

        [StringLength(50)]
        public string member3 { get; set; }

        [StringLength(50)]
        public string member4 { get; set; }

        public bool? accepted { get; set; }

        public string groupDescription { get; set; }

        [StringLength(50)]
        public string groupEvent { get; set; }
    }
}

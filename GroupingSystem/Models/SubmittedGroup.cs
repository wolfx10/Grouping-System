namespace GroupingSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SubmittedGroup
    {
        public int Id { get; set; }

        public int GroupID { get; set; }

        public bool Approved { get; set; }

        public bool Denied { get; set; }

        public string groupOwner { get; set; }

        public string groupEvent { get; set; }
    }
}

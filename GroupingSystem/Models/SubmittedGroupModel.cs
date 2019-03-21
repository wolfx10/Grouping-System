namespace GroupingSystem.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SubmittedGroupModel : DbContext
    {
        public SubmittedGroupModel()
            : base("name=SubmittedGroupModel")
        {
        }

        public virtual DbSet<SubmittedGroup> SubmittedGroups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

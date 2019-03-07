namespace GroupingSystem.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GroupModel : DbContext
    {
        public GroupModel()
            : base("name=GroupModel")
        {
        }

        public virtual DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

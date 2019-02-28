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
            modelBuilder.Entity<Group>()
                .Property(e => e.member1)
                .IsFixedLength();

            modelBuilder.Entity<Group>()
                .Property(e => e.member2)
                .IsFixedLength();

            modelBuilder.Entity<Group>()
                .Property(e => e.member3)
                .IsFixedLength();

            modelBuilder.Entity<Group>()
                .Property(e => e.member4)
                .IsFixedLength();
        }
    }
}

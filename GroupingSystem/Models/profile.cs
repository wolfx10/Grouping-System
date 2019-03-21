namespace GroupingSystem.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class profile : DbContext
    {
        public profile()
            : base("name=userProfile")
        {
        }

        public virtual DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
                .Property(e => e.firstName)
                .IsFixedLength();

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.lastName)
                .IsFixedLength();

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.email)
                .IsFixedLength();
        }
    }
}

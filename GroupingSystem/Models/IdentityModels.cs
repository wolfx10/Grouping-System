using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GroupingSystem.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name ="First Name"), Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name"), Required]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of birth"), Required]
        public DateTime DoB { get; set; }
        
        [Display(Name = "IsAdmin")]
        public bool IsAdmin { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<GroupingSystem.Models.Group> Groups { get; set; }

        public System.Data.Entity.DbSet<GroupingSystem.Models.Event> Events { get; set; }

        public System.Data.Entity.DbSet<GroupingSystem.Models.SubmittedGroup> SubmittedGroups { get; set; }

        public System.Data.Entity.DbSet<GroupingSystem.Models.UserProfile> UserProfiles { get; set; }

        public System.Data.Entity.DbSet<GroupingSystem.Models.ForumCategory> ForumCategories { get; set; }

        public System.Data.Entity.DbSet<GroupingSystem.Models.Thread> Threads { get; set; }

        public System.Data.Entity.DbSet<GroupingSystem.Models.ForumPost> ForumPosts { get; set; }

        public System.Data.Entity.DbSet<GroupingSystem.Models.Message> Messages { get; set; }
    }
}
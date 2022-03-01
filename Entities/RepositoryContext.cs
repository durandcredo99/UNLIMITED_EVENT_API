using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Entities
{
    public class RepositoryContext : IdentityDbContext<AppUser>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<CategoryBlog> CategoriesBlog { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Commercial> Commercials { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Promote> Promotes { get; set; }
        public DbSet<PromoteEvent> PromoteEvents { get; set; }
        public DbSet<EventYear> EventYears { get; set; }


        public DbSet<Workstation> Workstations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<MemberSkill>().HasKey(x => new { x.MemberId, x.SkillId, });
        }
    }
}

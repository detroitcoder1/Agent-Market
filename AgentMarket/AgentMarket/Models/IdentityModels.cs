using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AgentMarket.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public DateTime BirthDate { get; set; }
        [ForeignKey("Language")]
        public short? Language_Id { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Language Language { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<DynamicMenuItem> DynamicMenuItems { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<StaticMenuItem> StaticMenuItems { get; set; }
        //public virtual DbSet<CSSMapping> CSSMappings { get; set; }
        //public virtual DbSet<CSSMappingEntry> CSSMappingEntries { get; set; }
        public virtual DbSet<Product> Products { get; set; }
    }
}
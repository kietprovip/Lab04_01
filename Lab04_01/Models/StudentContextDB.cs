using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Lab04_01.Models
{
    public partial class StudentContextDB : DbContext
    {
        public StudentContextDB()
            : base("name=StudentContextDB2")
        {
        }

        public virtual DbSet<Falcuty> Falcuty { get; set; }
        public virtual DbSet<Student> Student { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Falcuty>()
                .HasMany(e => e.Student)
                .WithRequired(e => e.Falcuty)
                .WillCascadeOnDelete(false);
        }
    }
}

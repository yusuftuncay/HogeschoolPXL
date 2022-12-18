using HogeschoolPXL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HogeschoolPXL.Models.ViewModels;
using System.Data;

namespace HogeschoolPXL.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Gebruiker> Gebruiker { get; set; }
        public DbSet<Vak> Vak { get; set; }
        public DbSet<Handboek> Handboek { get; set; }
        public DbSet<Lector> Lector { get; set; }
        public DbSet<VakLector> VakLector { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Inschrijving> Inschrijving { get; set; }
        public DbSet<Academiejaar> Academiejaar { get; set; }
        public DbSet<LoginViewModel> LoginViewModel { get; set; }
        public DbSet<RegisterViewModel> RegisterViewModel { get; set; }
	}
}

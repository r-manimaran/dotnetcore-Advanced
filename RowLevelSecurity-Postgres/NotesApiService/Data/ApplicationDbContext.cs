using Microsoft.EntityFrameworkCore;
using NotesApiService.Models;

namespace NotesApiService.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Note> Notes { get; set; } = null!;
}

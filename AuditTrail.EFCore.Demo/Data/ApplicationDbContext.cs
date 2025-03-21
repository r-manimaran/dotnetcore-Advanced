﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuditTrail.EFCore.Demo.Models;

namespace AuditTrail.EFCore.Demo.Data
{
    public class ApplicationDbContext : AuditableIdentityContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AuditTrail.EFCore.Demo.Models.Product> Product { get; set; }
    }
}
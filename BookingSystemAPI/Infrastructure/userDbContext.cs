using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class userDbContext : DbContext
    {
        public userDbContext(DbContextOptions<userDbContext> options) : base(options)
        {

        }

        public DbSet<User> users { get; set; }

    }
}

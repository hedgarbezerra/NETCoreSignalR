using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Repository.Configurations
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {

        }
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Migrations only works with this, somehow
            //optionsBuilder.UseSqlServer("Data Source=HED-GAMING;Initial Catalog=NETCoreAPIDb;Integrated Security=True");
        }
    }
}

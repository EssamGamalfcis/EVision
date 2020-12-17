using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class CatalogContext : DbContext
    {
        public CatalogContext() : base()
        {
        }
        public DbSet<Product> Product { get; set; }
    }
}

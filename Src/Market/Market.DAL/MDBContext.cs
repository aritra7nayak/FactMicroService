﻿using Market.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL
{
    public class MDBContext :DbContext
    {
        public MDBContext() : base("TestConnection") { }

        public DbSet<MktStockDelivery> MktStockDeliveries { get; set; }
        public DbSet<MktStockPrice> MktStockPrices { get; set; }

        public void UpdateAuditables()
        {

            var trackables = ChangeTracker.Entries<IAuditable>();
            if (trackables != null)
            {
                foreach (var item in trackables.Where(t => t.State == EntityState.Added))
                {
                    item.Entity.CreatedOn = DateTime.Now;
                    //item.Entity.CreatedBy = HttpContext.Current.User.Identity.GetUserId();
                    item.Entity.UpdatedOn = DateTime.Now;
                    //item.Entity.UpdatedBy = HttpContext.Current.User.Identity.GetUserId();
                }

                foreach (var item in trackables.Where(t => t.State == EntityState.Modified))
                {
                    item.Entity.UpdatedOn = DateTime.Now;
                    //item.Entity.UpdatedBy = HttpContext.Current.User.Identity.GetUserId();
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}

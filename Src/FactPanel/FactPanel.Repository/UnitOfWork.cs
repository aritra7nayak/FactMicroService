using FactPanel.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FactPanel.Model;

namespace FactPanel.Repository
{
    public class UnitOfWork : IDisposable
    {
        private SDBContext _context;

        public UnitOfWork()
        {
            _context = new SDBContext();
            _context.Database.CommandTimeout = 600;
            //if ((bool)IsNonTracking) {
            //    _context.Configuration.AutoDetectChangesEnabled = false;
            //    _context.Configuration.ValidateOnSaveEnabled = false;
            //}
        }

        private GenericRepository<StockPrice> _stockPricesRepo;

        public GenericRepository<StockPrice> StockPricesRepo
        {
            get
            {
                if (_stockPricesRepo == null)
                {
                    _stockPricesRepo = new GenericRepository<StockPrice>(_context);
                }
                return _stockPricesRepo;
            }
        }


        private GenericRepository<StockDelivery> _stockDeliveriesRepo;

        public GenericRepository<StockDelivery> StockDeliveriesRepo
        {
            get
            {
                if (_stockDeliveriesRepo == null)
                {
                    _stockDeliveriesRepo = new GenericRepository<StockDelivery>(_context);
                }
                return _stockDeliveriesRepo;
            }
        }


        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string message = "";
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    // Get entry

                    DbEntityEntry entry = item.Entry;

                    string entityTypeName = entry.Entity.GetType().Name;

                    // Display or log error messages

                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        message += string.Format("Error '{0}' occurred in {1} at {2}",
                                  subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                        case EntityState.Modified:
                            entry.CurrentValues.SetValues(entry.OriginalValues);
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Unchanged;
                            break;
                    }
                }

                throw new Exception(message);
            }
        }

        public void DisableAutoDetectChanges()
        {
            _context.Configuration.AutoDetectChangesEnabled = false;

        }

        public void EnableAutoDetectChanges()
        {
            _context.Configuration.AutoDetectChangesEnabled = true;
        }

        public void DisableValidateOnSave()
        {
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public void EnableValidateOnSave()
        {
            _context.Configuration.ValidateOnSaveEnabled = true;
        }




        // Flag: Has Dispose already been called? 
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
                this._context = null;
            }

            // Free any unmanaged objects here. 
            //
            disposed = true;
        }
    }
}

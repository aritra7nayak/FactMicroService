using FactPanel.Model;
using FactPanel.Repository;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel.Business
{
    public class StockPricesService
    {
        private UnitOfWork _uow;

        public StockPricesService()
        {
            _uow = new UnitOfWork();
        }

        public StockPricesService(UnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<StockPrice> GetAll()
        {
            return _uow.StockPricesRepo.Get();
        }

        public void Add(StockPrice stockPrice, IValidationDictionary validationDictionary)
        {
            try
            {
                if (isUnique(stockPrice))
                {
                    _uow.StockPricesRepo.Insert(stockPrice);
                    _uow.Save();
                }
                else
                {
                    validationDictionary.AddError("Unique", "Business Group already exists");
                }
            }
            catch (Exception ex)
            {
                validationDictionary.AddError("Error", ex.Message);
            }
        }

        public StockPrice GetById(int id)
        {
            return _uow.StockPricesRepo.GetByID(id);
        }



        public void Edit(StockPrice stockPrice, IValidationDictionary validationDictionary)
        {
            try
            {
                if (isUnique(stockPrice, stockPrice.ID))
                {
                    _uow.StockPricesRepo.Update(stockPrice);
                    _uow.Save();
                }
                else
                {
                    validationDictionary.AddError("Unique", "Business Group already exists");
                }
            }
            catch (Exception ex)
            {
                validationDictionary.AddError("Error", ex.Message);
            }
        }

        public void Delete(int id, IValidationDictionary validationDictionary)
        {
            try
            {
                StockPrice stockPrice = _uow.StockPricesRepo.GetByID(id);
                _uow.StockPricesRepo.Delete(stockPrice);
                _uow.Save();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                if (ex is SqlException)
                {
                    var sqlEx = ex as SqlException;
                    if (sqlEx.Number == 547)
                    {
                        validationDictionary.AddError("Error", "Cannot delete as it has a foreign key relation with an existing Industry");
                    }
                }
                else
                {
                    validationDictionary.AddError("Error", ex.Message);
                }
            }
        }

        public bool isUnique(StockPrice stockPrice, int? id = null)
        {
            if (id == null || id == 0)
            {
                if (_uow.StockPricesRepo.Get(filter: x => x.StockId == stockPrice.StockId
                                                            && x.Date == stockPrice.Date).Count() > 0)
                    return false;
            }
            else
            {
                if (_uow.StockPricesRepo.Get(filter: x => x.StockId == stockPrice.StockId
                                                            && x.Date == stockPrice.Date
                                                            && x.ID != stockPrice.ID).Count() > 0)
                    return false;
            }
            return true;
        }
        public IEnumerable<StockPrice> Get(int? StockId,
                                       DateTime? FromDate,
                                       DateTime? ToDate,
                                       int page = 1)
        {
            var predicate = PredicateBuilder.True<StockPrice>();

            if (StockId.HasValue)
            {
                predicate = predicate.And(p => p.StockId == StockId);
            }
           
            if (FromDate.HasValue)
            {
                predicate = predicate.And(p => p.Date >= FromDate);
            }
            if (ToDate.HasValue)
            {
                predicate = predicate.And(p => p.Date <= ToDate);
            }
         
            

            Sorts<StockPrice> sorts = new Sorts<StockPrice>();
            sorts.Add(x => new { x.ID });
            predicate = predicate.Expand();

            return _uow.StockPricesRepo.Get("", predicate, sorts, page, Settings.PageSize);
        }
    }
}

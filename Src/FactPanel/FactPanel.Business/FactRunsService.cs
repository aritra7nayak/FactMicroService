using FactPanel.Model;
using FactPanel.Repository;
using FactPanel.ViewModels;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel.Business
{
    public class FactRunsService
    {
        private UnitOfWork _uow;

        public FactRunsService()
        {
            _uow = new UnitOfWork();
        }

        public FactRunsService(UnitOfWork uow)
        {
            _uow = new UnitOfWork();
        }

        public IEnumerable<FactRun> GetAll()
        {
            return _uow.FactRunRepository.Get();
        }

        public void Add(FactRun factRun, IValidationDictionary validationDictionary)
        {

            //// ------------------------------------------  A. Save a Batches record 
            try
            {

                _uow.FactRunRepository.Insert(factRun);
                _uow.Save();


                List<FactErrorViewModel> factErrors = new List<FactErrorViewModel>();

              //  factErrors = _uow.FactRunRepository.CalculateAndSaveFromBatchSP(factRun);



            }
            catch (Exception ex)
            {
                validationDictionary.AddError("Error", ex.Message);
                return;
            }


            // Import respective data
            try
            {
                
            }
            catch (Exception ex)
            {
                factRun.Success = false;
                factRun.Remarks = ex.Message;
            }


            // Update PriceRun with import Statistics.
            try
            {
                if ((factRun.RowsFailed == 0)
                    && (factRun.RowsWarning == 0)
                    && (factRun.RowsIgnored == 0)
                    )
                {
                    factRun.ErrorFilePath = "";
                }
                _uow.FactRunRepository.Update(factRun);
                _uow.Save();
            }
            catch (Exception ex)
            {
                string message = " Record Saved and Import Processed, but an error occured while updating the run";
                message += "Import Statistics [" +
                            " RowsTotal:" + factRun.RowsTotal.ToString() +
                            " RowsAdded:" + factRun.RowsAdded.ToString() +
                            " RowsUpdated:" + factRun.RowsUpdated.ToString() +
                            " RowsDeleted:" + factRun.RowsDeleted.ToString() +
                            " RowsWarning:" + factRun.RowsWarning.ToString() +
                            " RowsFailed:" + factRun.RowsFailed.ToString() +
                            " RowsIgnored:" + factRun.RowsIgnored.ToString() + "]";
                message += " Exception Message [" + ex.Message + "]";
                validationDictionary.AddError("Error", message);
            }
        }

        public bool isUnique(FactRun factRun, int? id = null)
        {
            if (id == null || id == 0)
            {
                if (_uow.FactRunRepository.Get(filter: x => x.Date == factRun.Date)
                                                        .Count() > 0)
                    return false;
            }
            else
            {
                if (_uow.FactRunRepository.Get(filter: x => x.Date == factRun.Date &&
                                                       x.Id != id)
                                                      .Count() > 0)


                    return false;
            }
            return true;
        }

        public FactRun GetById(int id)
        {
            return _uow.FactRunRepository.GetByID(id);
        }

        public void Edit(FactRun factRun, IValidationDictionary validationDictionary)
        {
            try
            {
                _uow.FactRunRepository.Update(factRun);
                _uow.Save();
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
                FactRun factRun = _uow.FactRunRepository.GetByID(id);
                _uow.FactRepo.DeleteForParent(x => x.FactRunID, factRun.Id);
                _uow.FactRunRepository.Delete(factRun);
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
                        validationDictionary.AddError("Error", "Cannot delete as it has a foreign key relation with Sectors");
                    }
                }
                else
                {
                    validationDictionary.AddError("Error", ex.Message);
                }
            }
        }

        public IEnumerable<FactRun> Get(DateTime? FromDate,
                                                        DateTime? ToDate,
                                                        bool? UploadSuccess,
                                                        int page = 1)
        {

            var predicate = PredicateBuilder.True<FactRun>();

            if (FromDate.HasValue)
            {
                predicate = predicate.And(p => p.Date >= FromDate);
            }
            if (ToDate.HasValue)
            {
                predicate = predicate.And(p => p.Date <= ToDate);
            }
            
            if (UploadSuccess.HasValue)
            {
                predicate = predicate.And(p => p.Success == UploadSuccess);
            }

            Sorts<FactRun> sorts = new Sorts<FactRun>();
            sorts.AddDescending(x => new { x.Id });

            predicate = predicate.Expand();

            return _uow.FactRunRepository.Get("", predicate, sorts, page, Settings.PageSize);
        }
    }
}

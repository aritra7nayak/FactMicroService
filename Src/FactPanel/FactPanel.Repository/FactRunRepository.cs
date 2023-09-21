using FactPanel.DAL;
using FactPanel.Model;
using FactPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel.Repository
{
    public class FactRunRepository : GenericRepository<FactRun>
    {
        public FactRunRepository(SDBContext context)
            : base(context)
        {
        }

        public List<FactErrorViewModel> CalculateAndSaveFromBatchSP(FactRun factRun)
        {

            IEnumerable<int> stockIDs = _context.StockPrices.Select(s=> s.StockId).ToList() ;

            var dates = _context.StockPrices.Select(s => s.Date).Distinct().OrderByDescending(o => o).ToList();

            DateTime latestDate = factRun.Date;
            DateTime previousDate = dates[0].Date;
            DateTime previousDateD3 = dates[1].Date;
            DateTime previousDateD4 = dates[2].Date;
            DateTime previousDateD5 = dates[3].Date;
            DateTime previousDateD6 = dates[4].Date;

            //Range of date for last week, taking starting day of the week as Sunday
            DateTime lastWeekStartDate = (DateTime)latestDate.AddDays(-(int)latestDate.DayOfWeek - 7);
            DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6);

            //Range of date for last month
            DateTime lastMonthStartDate = new DateTime(latestDate.Year, latestDate.Month, 1).AddMonths(-1);
            DateTime lastMonthEndDate = new DateTime(latestDate.Year, latestDate.Month, 1).AddDays(-1);

            //Range of date for last 52 weeks
            DateTime last52WeekStartDate = (DateTime)latestDate.AddDays(-(int)latestDate.DayOfWeek - (7 * 52));
            DateTime last52WeekEndDate = previousDate;//dailyFactRun.Date;

            //Range of 2 years and 5 years date
            DateTime last2YearStartDate = (DateTime)latestDate.AddYears(-2);
            DateTime last2YearEndDate = previousDate;//dailyFactRun.Date;

            DateTime last5YearStartDate = (DateTime)latestDate.AddYears(-5);
            DateTime last5YearEndDate = previousDate;//dailyFactRun.Date;

            //Range Of Picking Date for Current Month 
            DateTime currentMonthStartDate = new DateTime(latestDate.Year, latestDate.Month, 1);
            DateTime currentMonthEndDate = previousDate; //tradingDates[1].Date

            //Last Year StartDate
            DateTime lastYearStartDate = (DateTime)latestDate.AddYears(-1);

            //Range Of Picking All Data
            DateTime StartDateToPickFullData = (DateTime)latestDate.AddYears(-8).AddDays(1);

            //Range of Date for Current week
            DateTime currentWeekStartDate = (DateTime)latestDate.AddDays(-(int)latestDate.DayOfWeek);
            DateTime currentWeekEndDate = latestDate.AddDays(-1);

            //Range of Picking Data for 5 weeks
            DateTime last5WeekStartDate = (DateTime)latestDate.AddDays(-(int)latestDate.DayOfWeek - (7 * 5));


            //Range of Picking Data from 5 months
            DateTime last5MonthStartDate = new DateTime(latestDate.Year, latestDate.Month, 1).AddMonths(-5);


            DateTime baseDate1W = latestDate.AddDays(-7).Date;
            DateTime baseDate1M = latestDate.AddMonths(-1).Date;
            DateTime baseDate3M = latestDate.AddMonths(-3).Date;
            DateTime baseDate6M = latestDate.AddMonths(-6).Date;
            DateTime baseDate1Y = latestDate.AddYears(-1).Date;
            DateTime baseDate2Y = latestDate.AddYears(-2).Date;
            DateTime baseDate5Y = latestDate.AddYears(-5).Date;

            int BatchSize = 100;
            int ProcessedCount = 0;
            List<string> AllListingIDs = new List<string>();
            while (stockIDs.Count() != ProcessedCount)
            {
                IEnumerable<int> batchListingsIds = stockIDs.Skip(ProcessedCount).Take(BatchSize).Select(s => s);
                AllListingIDs.Add(string.Join(",", batchListingsIds));
                ProcessedCount += batchListingsIds.Count();
            }




        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel.ViewModels
{
    public class StockPriceFilterViewModel
    {
        public int? StockId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
        private int _page = 1;

        public int page
        {
            get { return _page; }
            set { _page = value; }
        }


    }
}

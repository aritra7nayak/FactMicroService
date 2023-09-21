using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel.Model
{
    public class Fact:Auditable
    {
        public int Id { get; set; }

        public int? FactRunID { get; set; }

        public int StockId { get; set; }

        public DateTime? Date1 { get; set; }
        public decimal? Price1 { get; set; }
        public decimal? Delivery1 { get; set; }

        public DateTime? Date2 { get; set; }
        public decimal? Price2 { get; set; }
        public decimal? Delivery2 { get; set; }

        public FactRun FactRun { get; set; }

    }
}

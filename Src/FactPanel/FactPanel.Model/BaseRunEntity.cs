using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel.Model
{
    public class BaseRunEntity : Auditable
    {

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

       public bool? Success { get; set; }

        [Display(Name = "Total Rows")]
        public int RowsTotal { get; set; }

        [Display(Name = "Rows Added")]
        public int RowsAdded { get; set; }

        [Display(Name = "Rows Updated")]
        public int RowsUpdated { get; set; }

        [Display(Name = "Rows Deleted")]
        public int RowsDeleted { get; set; }

        [Display(Name = "Rows Ignored")]
        public int RowsIgnored { get; set; }

        [Display(Name = "Rows Failed")]
        public int RowsFailed { get; set; }

        [Display(Name = "Rows Warning")]
        public int RowsWarning { get; set; }

        [Display(Name = "Error File")]
        [UIHint("FileLink")]
        public string ErrorFilePath { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
    }
}

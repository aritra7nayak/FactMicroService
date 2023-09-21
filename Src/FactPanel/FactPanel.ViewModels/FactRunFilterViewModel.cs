using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel.ViewModels
{
    public class FactRunFilterViewModel
    {
        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Upload Success")]
        public bool? UploadSuccess { get; set; }

        private int _page = 1;

        public int page
        {
            get { return _page; }
            set { _page = value; }
        }
    }
}

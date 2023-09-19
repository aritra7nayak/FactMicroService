﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel.Model
{
    public interface IAuditable
    {
        DateTime? CreatedOn { get; set; }
        DateTime? UpdatedOn { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }  
    }
}

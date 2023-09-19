﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel.Model
{
    public class StockPrice: Auditable
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }

        public int StockId  { get; set; }
        public decimal Price { get; set; }
    }
}

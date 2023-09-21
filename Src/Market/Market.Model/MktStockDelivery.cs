﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model
{
    public class MktStockDelivery : Auditable
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int StockId { get; set; }
        public decimal Delivery { get; set; }
    }
}

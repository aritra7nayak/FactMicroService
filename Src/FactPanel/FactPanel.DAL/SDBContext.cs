using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel.DAL
{
    public class SDBContext: DbContext
    {
        public SDBContext(): base("DBConnection") { }
    }
}

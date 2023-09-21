using FactPanel.DAL;
using FactPanel.Model;
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

    }
}

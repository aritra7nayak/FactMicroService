using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel.Repository
{
    public class SortItem<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, Object>> SortExpression { get; set; }
        public bool isDescending { get; set; }
    }
}

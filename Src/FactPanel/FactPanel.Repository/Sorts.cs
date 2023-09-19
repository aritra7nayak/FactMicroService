using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel.Repository
{
    public class Sorts<TEntity> : List<SortItem<TEntity>> where TEntity : class
    {
        public void Add(Expression<Func<TEntity, Object>> sortExpression)
        {
            this.Add(new SortItem<TEntity>() { isDescending = false, SortExpression = sortExpression });
        }
        public void AddDescending(Expression<Func<TEntity, Object>> sortExpression)
        {
            this.Add(new SortItem<TEntity>() { isDescending = true, SortExpression = sortExpression });
        }


    }
}

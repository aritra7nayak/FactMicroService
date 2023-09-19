using FactPanel.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using PagedList;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {

        protected SDBContext _context;

        private DbSet<TEntity> _dbSet;

        public GenericRepository(SDBContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> CreateFilterQuery(string include = null,
                                                     Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (include != null)
            {
                foreach (var includeProperty in include.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public IQueryable<TEntity> CreateSortQuery(IQueryable<TEntity> query, Sorts<TEntity> sorts)
        {
            int i = 0;

            IOrderedQueryable<TEntity> orderedquery = null;

            foreach (var sortitem in sorts)
            {
                i++;
                if (i == 1)
                {
                    if (sortitem.isDescending)
                    {
                        orderedquery = query.OrderByDescending(sortitem.SortExpression);
                    }
                    else
                    {
                        orderedquery = query.OrderBy(sortitem.SortExpression);
                    }
                }
                else
                {
                    if (sortitem.isDescending)
                    {
                        orderedquery = orderedquery.ThenByDescending(sortitem.SortExpression);
                    }
                    else
                    {
                        orderedquery = orderedquery.ThenBy(sortitem.SortExpression);
                    }
                }
            }

            return orderedquery;
        }


        public IQueryable<TResult> CreateSortQuery<TResult>(IQueryable<TResult> query, Sorts<TResult> sorts) where TResult : class
        {
            int i = 0;

            IOrderedQueryable<TResult> orderedquery = null;

            foreach (var sortitem in sorts)
            {
                i++;
                if (i == 1)
                {
                    if (sortitem.isDescending)
                    {
                        orderedquery = query.OrderByDescending(sortitem.SortExpression);
                    }
                    else
                    {
                        orderedquery = query.OrderBy(sortitem.SortExpression);
                    }
                }
                else
                {
                    if (sortitem.isDescending)
                    {
                        orderedquery = orderedquery.ThenByDescending(sortitem.SortExpression);
                    }
                    else
                    {
                        orderedquery = orderedquery.ThenBy(sortitem.SortExpression);
                    }
                }
            }

            return orderedquery;
        }

        public virtual IEnumerable<TEntity> Get(string include,
                                                Expression<Func<TEntity, bool>> filter,
                                                Sorts<TEntity> sorts,
                                                int page,
                                                int pageSize)

        {


            IQueryable<TEntity> query = _dbSet;

            if (include != null)
            {
                foreach (var includeProperty in include.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var sortQuery = CreateSortQuery(query, sorts);

            var result = sortQuery.ToPagedList(page, pageSize);

            return result;
        }

        public virtual IEnumerable<TEntity> Get(string include = null,
                                                Expression<Func<TEntity, bool>> filter = null)
        {
            var filterQuery = CreateFilterQuery(include, filter);

            var result = filterQuery.ToList();

            return result;
        }

        public virtual IEnumerable<TResult> Get<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                         string include,
                                                         Expression<Func<TEntity, bool>> filter,
                                                         Sorts<TResult> sorts,
                                                         int? page = null,
                                                         int? pagesize = null)
                                        where TResult : class
        {
            IQueryable<TEntity> query = _dbSet;

            if (include != null)
            {
                foreach (var includeProperty in include.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }

            IQueryable<TResult> selectedQuery = query.Select(selector);

            if (sorts != null)
            {
                selectedQuery = CreateSortQuery<TResult>(selectedQuery, sorts);
            }



            if (pagesize != null)
            {
                if (page != null)
                {
                    selectedQuery = selectedQuery.Skip(((int)page - 1) * (int)pagesize);
                }
                selectedQuery = selectedQuery.Take((int)pagesize);
            }

            var result = selectedQuery.ToList();

            return result;
        }

        public virtual TEntity GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);

        }

        public virtual void DeleteRange(IEnumerable<TEntity> entitiesToDelete)
        {
            _dbSet.RemoveRange(entitiesToDelete);
        }

        public virtual void DeleteForParent(Expression<Func<TEntity, int?>> parentColumn, int parentID)
        {
            var body = parentColumn.Body;
            if (body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)body;
                string columnName = memberExpression.Member.Name;
                ExecuteDeleteForParentCommand(columnName, parentID);
            }
            else
            {
                throw new Exception("'parentColumn' is not in the correct format. Suggested usage x=>x.ForeignKeyID");
            }
        }

        public virtual void DeleteForParent(Expression<Func<TEntity, int>> parentColumn, int parentID)
        {
            var body = parentColumn.Body;
            if (body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)body;
                string columnName = memberExpression.Member.Name;
                ExecuteDeleteForParentCommand(columnName, parentID);
            }
            else
            {
                throw new Exception("'parentColumn' is not in the correct format. Suggested usage x=>x.ForeignKeyID");
            }
        }

        private void ExecuteDeleteForParentCommand(string columnName, int parentID)
        {
            string sql = "DELETE FROM " + GetTableName() + " WHERE [" + columnName + "] = @p0 ";
            _context.Database.ExecuteSqlCommand(sql, new object[] { parentID });
        }


        public virtual void Update(TEntity entityToUpdate)
        {
            //_dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Detach(TEntity entityToDetach)
        {
            _context.Entry(entityToDetach).State = EntityState.Detached;
        }

        public string GetTableName()
        {
            return "[dbo].[" + (_context as IObjectContextAdapter).ObjectContext.CreateObjectSet<TEntity>().EntitySet.Name + "]";
        }

        public object Get<T1>()
        {
            throw new NotImplementedException();
        }
    }
}

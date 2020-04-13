
using App.Common.Classes.Base.AccessData;
using App.Common.Classes.DTO.Request;
using App.Common.Classes.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace App.Common.Classes.Base.Repositories
{
    public abstract class BaseReadOnlyRepository<TEntity> : IReadRepository<TEntity> where TEntity : class
    {
        protected DbContext _Database;
        protected DbSet<TEntity> _Table;

        public BaseReadOnlyRepository(DbContext context)
        {
            _Database = context;
            _Table = _Database.Set<TEntity>();
        }

        public virtual async Task<TEntity> FindByIdAsync(object id)
        {
            var newId = CastPrimaryKey(id);
            return await _Table.FindAsync(newId);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _Table;
        }

        public IQueryable<TEntity> GetAllPaging(int pageIndex, int pageSize)
        {
            return _Table.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public PagedList<TEntity> GetAllPaged(PagingParams pagingParams)
        {
            return new PagedList<TEntity>(GetAll(), pagingParams.PageNumber, pagingParams.PageSize);
        }

        protected string GetPrimaryKeyName()
        {
            var keyNames = _Database.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties.Select(x => x.Name);
            string keyName = keyNames.FirstOrDefault();

            if (keyNames.Count() > 1)
            {
                throw new ApplicationException("");
            }

            if (keyName == null)
            {
                throw new ApplicationException("");
            }

            return keyName;
        }

        protected object CastPrimaryKey(object id)
        {
            string keyName = GetPrimaryKeyName();
            Type keyType = typeof(TEntity).GetProperty(keyName).PropertyType;
            return Convert.ChangeType(id, keyType);
        }

        public TEntity FindById(object id)
        {
            var newId = CastPrimaryKey(id);
            return _Table.Find(newId);
        }
    }
}

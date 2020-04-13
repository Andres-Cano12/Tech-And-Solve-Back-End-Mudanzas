
using App.Common.Classes.DTO.Request;
using App.Common.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Classes.Base.AccessData
{
    public interface IReadRepository<T> where T : class
    {
        Task<T> FindByIdAsync(object id);
        T FindById(object id);
        IQueryable<T> GetAll();
        IQueryable<T> GetAllPaging(int pageIndex, int pageSize);
        PagedList<T> GetAllPaged(PagingParams pagingParams);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Classes.Base.Repositories
{
    public interface IUpdateRepository<T> where T : class
    {
        Task<T> UpdateAsync(T entity, bool autoSave = true);
        T Update(T entity, bool autoSave = true);
        Task<IEnumerable<T>> BulkUpdateAsync(IEnumerable<T> entities, bool autoSave = true);
    }    
}
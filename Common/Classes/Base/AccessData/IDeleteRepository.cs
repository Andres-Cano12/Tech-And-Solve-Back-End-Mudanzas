using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Classes.Base.Repositories
{
    public interface IDeleteRepository<T> where T : class
    {
        Task DeleteAsync(T entity, bool autoSave = true);
        void Delete(T entity, bool autoSave = true);
    }
}

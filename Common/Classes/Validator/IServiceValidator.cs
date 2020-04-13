using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Classes.Validator
{
    public interface IServiceValidator<T> where T: class
    {
        void PreValidate(T obj);
        void PostDelete(T obj);
        void PostInsert(T obj);
        void PostUpdate(T obj);
        void PreDelete(T obj);
        void PreInsert(T obj);
        void PreUpdate(T obj);
    }
}

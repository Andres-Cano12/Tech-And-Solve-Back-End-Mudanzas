using App.Common.Classes.Base.Repositories;
using App.Common.Classes.DTO.Request;
using App.Common.Classes.Helpers;
using App.Common.Classes.Validator;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using App.Common.Classes.Cache;
using App.Common.Classes.Extensions;
using App.Common.Classes.Enums;

namespace App.Common.Classes.Base.Services
{
    public interface IBaseService<TDTO>
        where TDTO : class
    {
        Task<IEnumerable<TDTO>> GetAllAsync(string token = null);
        Task<IEnumerable<TDTO>> GetAllAsync();
        Task<IEnumerable<TDTO>> GetAllPagingAsync(int pageIndex, int pageSize, string token = null);
        Task<IEnumerable<TDTO>> GetAllPagingAsync(int pageIndex, int pageSize);
        Task<PagedList<TDTO>> GetAllPagedAsync(PagingParams pagingParams, string token = null);
        Task<PagedList<TDTO>> GetAllPagedAsync(PagingParams pagingParams);

        Task<TDTO> CreateAsync(TDTO dto, string token = null);
        Task<TDTO> CreateAsync(TDTO dto, bool autoSave);
        Task<TDTO> CreateAsync(TDTO dto);
        Task DeleteAsync(object id, string token = null);
        Task DeleteAsync(object id, bool autoSave);
        Task DeleteAsync(object id);
        Task<TDTO> UpdateAsync(TDTO dto, string token = null);
        Task<TDTO> UpdateAsync(TDTO dto, bool autoSave);
        Task<TDTO> UpdateAsync(TDTO dto);
        Task<TDTO> FindByIdAsync(object Id, string token = null);
        Task<TDTO> FindByIdAsync(object Id);
        
        TDTO Create(TDTO dto, bool autoSave = true);
        void Delete(object id, bool autoSave = true);
        TDTO FindById(object Id);
        TDTO Update(TDTO dto, bool autoSave = true);
        IEnumerable<TDTO> GetAll();

        Task SaveChangesAsync();
        void SaveChanges();
    }

    public abstract class BaseService<TDTO, TEntity> : IBaseService<TDTO>
        where TDTO : class
        where TEntity : class
    {

        public IBaseCRUDRepository<TEntity> _repository;
        public IMapper _mapperDependency;
        public IConfiguration _configuration;
        public IMemoryCacheManager _memoryCacheManager;

        public string _cacheKey = $"list_{typeof(TDTO).Name}";
        public int _cacheTimeExp = 2;

        public BaseService(IBaseCRUDRepository<TEntity> repository, IMemoryCacheManager memoryCacheManager,
            IMapper mapper, IConfiguration configuration)
        {
            _repository = repository;
            _mapperDependency = mapper;
            _configuration = configuration;
            _memoryCacheManager = memoryCacheManager;
        }

        #region Async CRUD

        public async virtual Task<TDTO> CreateAsync(TDTO dto)
        {
            // Set valor fecha creacion o edicionbulk
            dto = SetFecha(dto, ActionType.Create);

            TEntity entity = _mapperDependency.Map<TEntity>(dto);

            
            entity = await _repository.CreateAsync(entity);


            _memoryCacheManager.Clear();
            return _mapperDependency.Map<TDTO>(entity);
            //TO-DO: Log
        }
        public async virtual Task<TDTO> CreateAsync(TDTO dto, bool autoSave)
        {
            // Set valor fecha creacion o edicion
            dto = SetFecha(dto, ActionType.Create);

            TEntity entity = _mapperDependency.Map<TEntity>(dto);

            //Prevalidacion

            entity = await _repository.CreateAsync(entity, autoSave);

            //PostValidacion

            _memoryCacheManager.Clear();
            return _mapperDependency.Map<TDTO>(entity);
        }
        public async virtual Task<TDTO> CreateAsync(TDTO dto, string token = null)
        {
            // Set valor fecha creacion o edicion
            dto = SetFecha(dto, ActionType.Create);

            TEntity entity = _mapperDependency.Map<TEntity>(dto);

            //Prevalidacion

            entity = await _repository.CreateAsync(entity);

            //PostValidacion

            _memoryCacheManager.Clear();

            return _mapperDependency.Map<TDTO>(entity);
            //TO-DO: Log
        }

        public async virtual Task<TDTO> UpdateAsync(TDTO dto)
        {
            // Set valor fecha creacion o edicion
            dto = SetFecha(dto, ActionType.Update);

            TEntity entity = _mapperDependency.Map<TEntity>(dto);

            //Prevalidacion
            entity = await _repository.UpdateAsync(entity);

            _memoryCacheManager.Clear();
            //TO-DO: Log

            return _mapperDependency.Map<TDTO>(entity);
        }
        public async virtual Task<TDTO> UpdateAsync(TDTO dto, bool autoSave)
        {
            // Set valor fecha creacion o edicion
            dto = SetFecha(dto, ActionType.Update);

            TEntity entity = _mapperDependency.Map<TEntity>(dto);

            entity = await _repository.UpdateAsync(entity, autoSave);

            _memoryCacheManager.Clear();

            return _mapperDependency.Map<TDTO>(entity);
        }
        public async virtual Task<TDTO> UpdateAsync(TDTO dto, string token = null)
        {
            // Set valor fecha creacion o edicion
            dto = SetFecha(dto, ActionType.Update);

            TEntity entity = _mapperDependency.Map<TEntity>(dto);

            entity = await _repository.UpdateAsync(entity);
            

            _memoryCacheManager.Clear();
            //TO-DO: Log

            return _mapperDependency.Map<TDTO>(entity);
        }

        public async virtual Task DeleteAsync(object id)
        {
            TEntity entity = await _repository.FindByIdAsync(id);

            await _repository.DeleteAsync(entity);

            _memoryCacheManager.Clear();

            //TO-DO: Log
        }
        public async virtual Task DeleteAsync(object id, bool autoSave)
        {
            TEntity entity = await _repository.FindByIdAsync(id);

            await _repository.DeleteAsync(entity, autoSave);

            //PostValidacion
            _memoryCacheManager.Clear();
        }
        public async virtual Task DeleteAsync(object id, string token = null)
        {
            TEntity entity = await _repository.FindByIdAsync(id);
            //Prevalidacion

            await _repository.DeleteAsync(entity);

            //PostValidacion

            _memoryCacheManager.Clear();
            //TO-DO: Log
        }

        public async virtual Task<TDTO> FindByIdAsync(object Id)
        {
            var result = await _repository.FindByIdAsync(Id);
            return _mapperDependency.Map<TDTO>(result);
        }
        public async virtual Task<TDTO> FindByIdAsync(object Id, string token = null)
        {
            var result = await _repository.FindByIdAsync(Id);
            return _mapperDependency.Map<TDTO>(result);
        }

        public async virtual Task<IEnumerable<TDTO>> GetAllAsync()
        {
            List<TDTO> list;

            var cacheEntry = await _memoryCacheManager.GetOrCreateAsync(_cacheKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(_cacheTimeExp);
                list = _mapperDependency.Map<List<TDTO>>(_repository.GetAll());
                return Task.FromResult(list);
            });

            return cacheEntry;
        }
        public async virtual Task<IEnumerable<TDTO>> GetAllAsync(string token = null)
        {
            List<TDTO> list;

            var cacheEntry = await _memoryCacheManager.GetOrCreateAsync(_cacheKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(_cacheTimeExp);
                list = _mapperDependency.Map<List<TDTO>>(_repository.GetAll());
                return Task.FromResult(list);
            });

            return cacheEntry;
        }

        #endregion

        #region CRUD

        public virtual TDTO Create(TDTO dto, bool autoSave = true)
        {
            // Set valor fecha creacion o edicion
            dto = SetFecha(dto, ActionType.Create);

            TEntity entity = _mapperDependency.Map<TEntity>(dto);

            entity = _repository.Create(entity, autoSave);

            _memoryCacheManager.Clear();
            return _mapperDependency.Map<TDTO>(entity);
            //TO-DO: Log
        }

        public virtual void Delete(object id, bool autoSave = true)
        {
            TEntity entity = _repository.FindById(id);

            _repository.Delete(entity, autoSave);
            _memoryCacheManager.Clear();

            //TO-DO: Log
        }

        public virtual TDTO FindById(object Id)
        {
            var result = _repository.FindById(Id);
            return _mapperDependency.Map<TDTO>(result);
        }

        public virtual TDTO Update(TDTO dto, bool autoSave = true)
        {
            // Set valor fecha creacion o edicion
            dto = SetFecha(dto, ActionType.Update);

            TEntity entity = _mapperDependency.Map<TEntity>(dto);
            entity = _repository.Update(entity, autoSave);

            _memoryCacheManager.Clear();
            //TO-DO: Log

            return _mapperDependency.Map<TDTO>(entity);
        }

        public virtual IEnumerable<TDTO> GetAll()
        {
            List<TDTO> list;

            var cacheEntry = _memoryCacheManager.GetOrCreate(_cacheKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(_cacheTimeExp);
                var list2 = _repository.GetAll();
                list = _mapperDependency.Map<List<TDTO>>(_repository.GetAll());
                return list;
            });

            return cacheEntry;
        }

        #endregion

        #region Pagination

        public async virtual Task<PagedList<TDTO>> GetAllPagedAsync(PagingParams pagingParams)
        {
            string cacheKey = BuildCacheKey(pagingParams);

            var cacheEntry = await _memoryCacheManager.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(_cacheTimeExp);
                var listPaged = _repository.GetAllPaged(pagingParams);
                var listDTO = _mapperDependency.Map<List<TDTO>>(listPaged.List);
                var pagedList = new PagedList<TDTO>(listDTO, pagingParams.PageNumber, pagingParams.PageSize, listPaged.TotalItems);
                return Task.FromResult(pagedList);
            });

            return cacheEntry;
        }
        public async virtual Task<PagedList<TDTO>> GetAllPagedAsync(PagingParams pagingParams, string token = null)
        {

            string cacheKey = BuildCacheKey(pagingParams);

            var cacheEntry = await _memoryCacheManager.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(_cacheTimeExp);
                var listPaged = _repository.GetAllPaged(pagingParams);
                var listDTO = _mapperDependency.Map<List<TDTO>>(listPaged.List);
                var pagedList = new PagedList<TDTO>(listDTO, pagingParams.PageNumber, pagingParams.PageSize, listPaged.TotalItems);
                return Task.FromResult(pagedList);
            });


            return cacheEntry;

        }

        public async virtual Task<IEnumerable<TDTO>> GetAllPagingAsync(int pageIndex, int pageSize)
        {
            List<TDTO> list;
            string cacheKey = $"{_cacheKey}page={pageIndex}size={pageSize}";
            var cacheEntry = await _memoryCacheManager.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(_cacheTimeExp);
                list = _mapperDependency.Map<List<TDTO>>(_repository.GetAllPaging(pageIndex, pageSize));
                return Task.FromResult(list);
            });

            return cacheEntry;
        }
        public async virtual Task<IEnumerable<TDTO>> GetAllPagingAsync(int pageIndex, int pageSize, string token = null)
        {
            List<TDTO> list;
            string cacheKey = $"{_cacheKey}page={pageIndex}size={pageSize}";
            var cacheEntry = await _memoryCacheManager.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(_cacheTimeExp);
                list = _mapperDependency.Map<List<TDTO>>(_repository.GetAllPaging(pageIndex, pageSize));
                return Task.FromResult(list);
            });

            return cacheEntry;
        }
        
        #endregion

        #region Others

        public async virtual Task SaveChangesAsync()
        {
            await _repository.SaveChangesAsync();
        }

        public virtual void SaveChanges()
        {
             _repository.SaveChanges();
        }

        private TDTO SetFecha(TDTO dto, ActionType option)
        {
            //if (option == ActionType.Create)
            //{
            //    dto.SetPropertyValue("CreatedAt", DateTimeHelper.GetCurrentColombianTime());
            //}
            //else
            //{
            //    dto.SetPropertyValue("UpdatedAt", DateTimeHelper.GetCurrentColombianTime());
            //}

            return dto;
        }

        public string BuildCacheKey(PagingParams pagingParams)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_cacheKey);
            sb.AppendFormat("page={0}", pagingParams.PageNumber);
            sb.AppendFormat("size={0}", pagingParams.PageSize);
            sb.AppendFormat("CountryId={0}", pagingParams.CountryId);


            //Filtros
            if (pagingParams.Filters != null)
            {
                for (int i = 0; i < pagingParams.Filters.Count; i++)
                {
                    var element = pagingParams.Filters.ElementAt(i);
                    sb.AppendFormat("f{0}={1}{2}{3}", i, element.PropertyName, element.Operator, element.Value);
                }
            }

            //Ordenamiento
            sb.AppendFormat("s={0}{1}", pagingParams.SortProperty, pagingParams.SortType);

            return sb.ToString();
        }
      
        #endregion
    }
}

using App.Common.Classes.Base.Services;
using App.Common.Classes.DTO.Common;
using App.Common.Classes.DTO.Request;
using App.Common.Classes.Exceptions;
using App.Common.Classes.Extensions;
using App.Common.Classes.Helpers;
using App.Common.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using App.Common.Classes.ApiGateway;
using Rollbar;

namespace App.Common.Classes.Base.WebApi
{
    public abstract class BaseController<TDTO> : Controller
        where TDTO : class
    {
        IStringLocalizer<GlobalResource> _globalLocalizer;
        IBaseService<TDTO> _service;

        public BaseController(IBaseService<TDTO> service, IStringLocalizer<GlobalResource> globalLocalizer)
        {
            _service = service;
            _globalLocalizer = globalLocalizer;
        }

        public BaseController() { }

        public string Token
        {
            get
            {
                string authHeader = HttpContext.Request.Headers["Authorization"];
                //if (!string.IsNullOrEmpty(authHeader))
                //{
                //   return AuthUserHelper.GetValidToken(authHeader);
                //}
                return string.Empty;
            }
        }

        [HttpGet]
        [Route("getAll")]
        public virtual async Task<IActionResult> Index()
        {
            try
            {
                var data = await _service.GetAllAsync(Token);
                return Json(data.AsResponseDTO((int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.InternalServerError, _globalLocalizer["DefaultError"]));
            }
        }

        [HttpPost]
        [Route("getAll")]
        public virtual async Task<IActionResult> Index([FromBody]PagingParams pagingParams)
        {
            try
            {
                var data = await _service.GetAllPagedAsync(pagingParams, Token);
                return Json(data.AsResponseDTO((int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.InternalServerError, _globalLocalizer["DefaultError"]));
            }
        }

        [HttpGet]
        [Route("details/{id:int}")]
        public virtual async Task<IActionResult> Details(int id)
        {
            try
            {
                var data = await _service.FindByIdAsync(id, Token);
                if (data == null)
                    return Json(ResponseExtension.AsResponseDTO<string>(null,
                        (int)HttpStatusCode.NotAcceptable, _globalLocalizer["ItemNotFoundMessage"]));
                return Json(data.AsResponseDTO((int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.InternalServerError, _globalLocalizer["DefaultError"]));
            }
        }

        [HttpPost]
        [Route("create")]
        public virtual async Task<IActionResult> Create([FromBody]TDTO modelDTO)
        {
            try
            {
                var data = await ServiceCreate(modelDTO, Token);
                return Json(data.AsResponseDTO((int)HttpStatusCode.OK,
                    _globalLocalizer["CreateSuccessMessage"]));
            }
            catch (ValidationServiceException ex)
            {
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.NotAcceptable, ex.ToUlHtmlString()));
            }
            catch (ApplicationException ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.InternalServerError, ex.ToUlHtmlString()));
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.InternalServerError, _globalLocalizer["DefaultError"]));
            }
        }

        [HttpPost]
        [Route("edit")]
        public virtual async Task<IActionResult> Edit([FromBody]TDTO modelDTO)
        {
            try
            {
                int? id = (int?)modelDTO.GetPrimaryKeyValue();
                if (!id.HasValue)
                {
                    return Json(ResponseExtension.AsResponseDTO<string>(null,
                      (int)HttpStatusCode.NotAcceptable, _globalLocalizer["ItemNotFoundMessage"]));
                }

                var data = await _service.FindByIdAsync(id, Token);
                if (data == null)
                {
                    return Json(ResponseExtension.AsResponseDTO<string>(null,
                        (int)HttpStatusCode.NotAcceptable, _globalLocalizer["ItemNotFoundMessage"]));
                }

                var result = await ServiceUpdate(modelDTO, Token);

                return Json(result.AsResponseDTO((int)HttpStatusCode.OK,
                    _globalLocalizer["UpdateSuccessMessage"]));
            }
            catch (ValidationServiceException ex)
            {
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.NotAcceptable, ex.ToUlHtmlString()));
            }
            catch (ApplicationException ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.InternalServerError, _globalLocalizer["UpdateErrorMessage"]));
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.InternalServerError, _globalLocalizer["DefaultError"]));
            }
        }

        [HttpPost]
        [Route("delete")]
        public virtual async Task<IActionResult> Delete([FromBody]RequestDTO request)
        {
            try
            {

                var data = await _service.FindByIdAsync(request.Id);
                if (data == null)
                {
                    return Json(ResponseExtension.AsResponseDTO<string>(null,
                                            (int)HttpStatusCode.NotAcceptable, _globalLocalizer["ItemNotFoundMessage"]));
                }

                await _service.DeleteAsync(request.Id, Token);
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.OK, _globalLocalizer["DeleteSuccessMessage"]));
            }
            catch (ValidationServiceException ex)
            {
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.NotAcceptable, ex.ToUlHtmlString()));
            }
            catch (ApplicationException ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.InternalServerError, ex.ToUlHtmlString()));
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.InternalServerError, _globalLocalizer["DefaultError"]));
            }
        }

        protected virtual async Task<TDTO> ServiceCreate(TDTO model, string token = null)
        {
            return await _service.CreateAsync(model, token);
        }

        protected virtual async Task<TDTO> ServiceUpdate(TDTO model, string token = null)
        {
            return await _service.UpdateAsync(model, token);
        }

    }
}

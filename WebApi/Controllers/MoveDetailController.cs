using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinnesLogic.Services;
using App.Common.Classes.Base.WebApi;
using Common.Classes.BussinesLogic;
using Microsoft.Extensions.Localization;
using App.Common.Resources;
using System.Net;
using App.Common.Classes.Extensions;
using Rollbar;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class MoveDetailController : BaseController<MoveDetailDTO>
    {
        protected IStringLocalizer<GlobalResource> _globalLocalizer;
        protected IConfiguration _configuration;
        protected IMoveDetailService moveDetailService;
        protected IMoveService moveService;

        public MoveDetailController(IStringLocalizer<GlobalResource> globalLocalizer
            ,IConfiguration configuration
            ,IMoveService moveService
            ,IMoveDetailService moveDetailService)
            : base(moveDetailService, globalLocalizer)
        {
            _globalLocalizer = globalLocalizer;
            _configuration = configuration;

            this.moveDetailService = moveDetailService;
            this.moveService = moveService;
        }

        [HttpGet]
        [Route("moveDetails/{id:int}")]
        public async Task<IActionResult> MoveDetails(int id)
        {
            try
            {
                var listMoveDetail = this.moveDetailService.GetMovingGrips(id);

                var listToDownload = this.moveService.GetMovingGrips(listMoveDetail);

                return Json(listToDownload.AsResponseDTO((int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.InternalServerError, _globalLocalizer["DefaultError"]));
            }
        }
    }
}

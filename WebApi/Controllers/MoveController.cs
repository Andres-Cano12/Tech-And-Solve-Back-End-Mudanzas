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
    public class MoveController : BaseController<MoveDTO>
    {
        protected IStringLocalizer<GlobalResource> _globalLocalizer;
        protected IConfiguration _configuration;
        protected IMoveService moveService;

        public MoveController(IStringLocalizer<GlobalResource> globalLocalizer
          , IConfiguration configuration,
            IMoveService moveService)
            : base(moveService, globalLocalizer)
        {
            _globalLocalizer = globalLocalizer;
            _configuration = configuration;

            this.moveService = moveService;
        }

        public List<string> GetMovingGrips(List<MoveDetailDTO> moveDetailsDTO) {
            return this.moveService.GetMovingGrips(moveDetailsDTO);
        }


        [HttpPost]
        [Route("UploadFiles")]
        public async Task<IActionResult> UploadFiles([FromForm]  MoveFileDTO file)
        {
            try
            {
                if (file.File == null || file.File.Length == 0)
                    return Json(_configuration.GetConnectionString("EmptyFile").AsResponseDTO((int)HttpStatusCode.NoContent));

                var listToDownload = await this.moveService.CreateMove(file);
                return Json(listToDownload.AsResponseDTO((int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
                return Json(ResponseExtension.AsResponseDTO<string>(null,
                    (int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}

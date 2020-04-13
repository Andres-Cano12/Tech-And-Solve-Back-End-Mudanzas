using AccessData.Entities;
using AccessData.Repository;
using AccessData.Repository.IMoveDetailRepository;
using App.Common.Classes.Base.Repositories;
using App.Common.Classes.Base.Services;
using App.Common.Classes.Constants;
using App.Common.Classes.DTO.Common;
using App.Common.Classes.Helpers;
using App.Common.Classes.Validator;
using App.Common.Resources;
using App.Common.Resources.Distributors;
using AutoMapper;
using AutoMapper.Configuration;
using Common.Classes.BussinesLogic;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinnesLogic.Services
{
    public class MoveDetailService : BaseService<MoveDetailDTO, MoveDetail>, IMoveDetailService
    {
        private IMoveDetailRepository moveDetailRepository;
        private IMoveRepository moveRepository;
        private IMapper _mapper;

        public MoveDetailService(IBaseCRUDRepository<MoveDetail> repository
            , App.Common.Classes.Cache.IMemoryCacheManager
            memoryCacheManager, IMapper mapper, IServiceValidator<MoveDetail> validation
            , Microsoft.Extensions.Configuration.IConfiguration configuration
            , IMoveDetailRepository moveDetailRepository
            , IMoveRepository moveRepository)
            : base(repository, memoryCacheManager, mapper, configuration)
        {
            this.moveDetailRepository = moveDetailRepository;
            _mapper = mapper;
        }
        //Obtiene los detalles de un mudanza
        List<MoveDetailDTO> IMoveDetailService.GetMovingGrips(int idMoveDetail)
        {
             var listMoveDetail = this.moveDetailRepository.GetAll()
                                                           .Where(r => r.IdMove
                                                           .Equals(idMoveDetail))
                                                           .ToList();

            return _mapperDependency.Map<List<MoveDetailDTO>>(listMoveDetail
                                    .Select(m => m));
        }
    }

    #region Validator

    public class MoveDetailResourceValidator : BaseServiceValidator<MoveDetail, MoveResource>
    {
        IStringLocalizer<GlobalResource> _globalLocalizer;

        public MoveDetailResourceValidator(IStringLocalizer<MoveResource> localizer,
            IStringLocalizer<GlobalResource> globalLocalizer) : base(localizer)
        {
            _globalLocalizer = globalLocalizer;

        }

        #region Insert Rules
        public override void LoadPreInsertRules()
        {

            var tututut = "";
        }

        public override void LoadPostInsertRules()
        {

        }

        #endregion

        #region Update Rules

        public override void LoadPreUpdateRules()
        {
        }

        public override void LoadPostUpdateRules()
        {

        }

        #endregion

        #region Delete Rules

        public override void LoadPreDeleteRules()
        {

        }

        public override void LoadPostDeleteRules()
        {

        }

        #endregion

        #region Advanced Validations



        #endregion
    }

    #endregion
}

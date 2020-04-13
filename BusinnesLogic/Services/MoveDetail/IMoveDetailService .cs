using AccessData.Entities;
using App.Common.Classes.Base.Services;
using App.Common.Classes.DTO.Common;
using Common.Classes.BussinesLogic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinnesLogic.Services
{
    public interface IMoveDetailService: IBaseService<MoveDetailDTO>
    {
        List<MoveDetailDTO> GetMovingGrips(int idMoveDetail);
    }
}

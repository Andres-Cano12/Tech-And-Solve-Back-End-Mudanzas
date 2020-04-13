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
    public interface IMoveService: IBaseService<MoveDTO>
    {
         Task<List<string>> CreateMove(MoveFileDTO file);
         List<string> GetMovingGrips(List<MoveDetailDTO> moveDetailDTO);
    }
}

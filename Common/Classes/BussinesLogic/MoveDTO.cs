using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Classes.BussinesLogic
{
    public class MoveDTO
    {
        public int IdMove { get; set; }
        public string IdentificationCard { get; set; }
        public DateTime DateMove { get; set; }
        public List<MoveDetailDTO> MoveDetailDTO { get; set; }
    }
}

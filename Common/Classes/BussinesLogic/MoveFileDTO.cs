using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Classes.BussinesLogic
{
    public class MoveFileDTO
    {

        public string IdentificationCard { get; set; }
        public IFormFile File { get; set; }

    }
}

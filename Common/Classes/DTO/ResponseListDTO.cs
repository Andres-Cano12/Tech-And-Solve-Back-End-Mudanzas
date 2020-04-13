using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Classes.DTO.Common
{
    public class ResponseListDTO<T>
    {
        public List<T> List { get; set; }

        public decimal TotalValue { get; set; }

        public decimal TotalRegister { get; set; }
    }
}

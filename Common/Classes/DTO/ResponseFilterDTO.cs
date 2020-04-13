using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Classes.DTO.Common
{
    public class ResponseFilterDTO
    {
        public List<FilterDTO> FilterList { get; set; }

        public List<OperatorDTO> ConditionList { get; set; }

        public List<int> PageQuantityList { get; set; }
    }
}

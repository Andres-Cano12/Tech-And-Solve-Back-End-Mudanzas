using App.Common.Classes.DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Classes.Helpers
{
   public static class ListHelper
    {
        public static List<ResponseItemDTO> GenerateResponseList(int start = 30, int step = 30, int maxRep = 15)
        {
            List<ResponseItemDTO> list = new List<ResponseItemDTO>();
            var length = step * maxRep;
            for (int i = start; i <= length; i+= step)
            {
                list.Add(new ResponseItemDTO() { Id = i.ToString(), Value = i.ToString() });
            }

            return list;
        }

        public static List<int> GenerateIntList(int start = 30, int step = 30, int maxRep = 15)
        {
            List<int> list = new List<int>();
            var length = step * maxRep;
            for (int i = start; i <= length; i += step)
            {
                list.Add(i);
            }

            return list;
        }
    }
}

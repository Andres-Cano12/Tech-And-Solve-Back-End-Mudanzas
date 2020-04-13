using App.Common.Classes.Constants;
using App.Common.Classes.DTO.Common;
using App.Common.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace App.Common.Classes.Helpers
{
    public static class FilterHelper
    {
        private static List<OperatorDTO> AllOperators()
        {
            var conditionList = new List<OperatorDTO>()
            {
                new OperatorDTO() { Id = OperatorEnum.Contains.ToString(), Value = GlobalConstants.CONDITION_CONTAINS, TypeValue = GlobalConstants.FORMAT_STRING },
                new OperatorDTO() { Id = OperatorEnum.Equals.ToString(), Value = GlobalConstants.CONDITION_EQUALS, TypeValue = GlobalConstants.FORMAT_NUMERIC },
                new OperatorDTO() { Id = OperatorEnum.EqualsString.ToString(), Value = GlobalConstants.CONDITION_EQUALS, TypeValue = GlobalConstants.FORMAT_STRING },
                new OperatorDTO() { Id = OperatorEnum.GreaterThan.ToString(), Value = GlobalConstants.CONDITION_GREATER_THAN, TypeValue = GlobalConstants.FORMAT_NUMERIC },
                new OperatorDTO() { Id = OperatorEnum.GreaterThanOrEqual.ToString(), Value = GlobalConstants.CONDITION_GREATER_THAN_OR_EQUAL, TypeValue = GlobalConstants.FORMAT_NUMERIC },
                new OperatorDTO() { Id = OperatorEnum.LessThan.ToString(), Value = GlobalConstants.CONDITION_LESS_THAN, TypeValue = GlobalConstants.FORMAT_NUMERIC },
                new OperatorDTO() { Id = OperatorEnum.LessThanOrEqualTo.ToString(), Value = GlobalConstants.CONDITION_LESS_THAN_OR_EQUAL_TO, TypeValue = GlobalConstants.FORMAT_NUMERIC }
            };

            return conditionList;
        }

        private static List<int> PageQuantity()
        {
            var itemList = new  List<int>();

            for (int i = 50; i < 2500; i += 50)
                itemList.Add(i);
            return itemList;
        }

        public static ResponseFilterDTO GetResponseFilter()
        {
            return new ResponseFilterDTO()
            {
                ConditionList = AllOperators().OrderBy(c => c.Value).ToList(),
                PageQuantityList = PageQuantity()
            };
        }
    }
}

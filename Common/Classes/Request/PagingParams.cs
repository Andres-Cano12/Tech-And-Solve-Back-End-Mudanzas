using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Classes.DTO.Request
{
    public partial class PagingParams
    {
        public int PageNumber { get; set; } 
        public int PageSize { get; set; }
        public string SortProperty { get; set; }
        public string SortType { get; set; }
        public int CountryId { get; set; }
        public string RoleName { get; set; }
        public List<FilterParams> Filters { get; set; }
    }
}

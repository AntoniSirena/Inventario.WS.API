using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JS.Base.WS.API.DTO.Common
{
    public class PaginationDTO
    {
        public object Records { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageRow { get; set; }
        public int TotalPage { get; set; }
        public int TotalRecord { get; set; }
    }

}
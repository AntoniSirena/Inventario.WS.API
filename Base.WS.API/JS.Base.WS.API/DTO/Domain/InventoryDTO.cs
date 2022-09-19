using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JS.Base.WS.API.DTO.Domain
{
    public class InventoryDTO
    {
        public long Id { get; set; }
        public string Status { get; set; }
        public string StatuShortName { get; set; }
        public string StatusColour { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string OpenDate { get; set; }
        public string ClosedDate { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
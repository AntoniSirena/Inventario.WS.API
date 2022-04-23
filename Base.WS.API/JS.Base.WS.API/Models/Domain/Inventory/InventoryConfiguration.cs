using JS.Base.WS.API.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JS.Base.WS.API.Models.Domain.Inventory
{
    public class InventoryConfiguration: Audit
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string ShortName { get; set; }
        public int Id { get; set; }
        public bool ShowCost { get; set; }
        public bool ShowPrice { get; set; }
    }
}
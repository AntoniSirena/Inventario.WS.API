using JS.Base.WS.API.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JS.Base.WS.API.Models.Domain
{
    public class Product: Audit
    {
        [Key]
        public long Id { get; set; }
        public string Description { get; set; }
        public string FormattedDescription { get; set; }
        public string ExternalCode { get; set; }
        public string BarCode { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
    }
}
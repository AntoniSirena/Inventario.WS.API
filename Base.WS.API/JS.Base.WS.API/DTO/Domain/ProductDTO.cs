using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JS.Base.WS.API.DTO.Domain
{
    public class ProductDTO
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string ExternalCode { get; set; }
        public string BarCode { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
    }

}
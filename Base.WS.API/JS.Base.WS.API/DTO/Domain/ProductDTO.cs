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
        public decimal OldCost { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public long InventoryId { get; set; }
        public long InventoryDetailId { get; set; }
        public decimal Quantity { get; set; }
        public string UserName { get; set; }
        public string Reference { get; set; }
        public decimal Existence { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Difference { get; set; }

        public int? SectionId { get; set; }
        public int? TariffId { get; set; }
        public string SectionDescription { get; set; }
        public string TariffDescription { get; set; }

        public string Origin { get; set; }
    }

}
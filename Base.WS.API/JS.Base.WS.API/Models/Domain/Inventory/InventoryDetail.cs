using JS.Base.WS.API.Base;
using JS.Base.WS.API.Models.Authorization;
using JS.Base.WS.API.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JS.Base.WS.API.Models.Domain.Inventory
{
    public class InventoryDetail : Audit
    {
        [Key]
        public long Id { get; set; }
        public long InventoryId { get; set; }
        public int? SectionId { get; set; }
        public int? TariffId { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public decimal OldCost { get; set; }
        public decimal OldPrice { get; set; }
        public decimal CurrentCost { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Quantity { get; set; }


        [ForeignKey("InventoryId")]
        public virtual Inventory Inventory { get; set; }

        [ForeignKey("SectionId")]
        public virtual InventorySection Section { get; set; }

        [ForeignKey("TariffId")]
        public virtual InventoryTariff Tariff { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product  { get; set; }
    }
}
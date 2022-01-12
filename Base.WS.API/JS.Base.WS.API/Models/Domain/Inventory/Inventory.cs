using JS.Base.WS.API.Base;
using JS.Base.WS.API.Models.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JS.Base.WS.API.Models.Domain.Inventory
{
    public class Inventory : Audit
    {
        [Key]
        public long Id { get; set; }
        public int StatusId { get; set; }
        public long UserId { get; set; }
        public string Description { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string OpenDateFormatted { get; set; }
        public string ClosedDateFormatted { get; set; }


        [ForeignKey("StatusId")]
        public virtual InventoryStatus InventoryStatus { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<InventoryDetail> InventoryDetails { get; set; }
    }
}
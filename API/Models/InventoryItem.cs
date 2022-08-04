using System;
using System.Collections.Generic;

namespace XshapeAPI.Models
{
    public partial class InventoryItem
    {
        public int ItemId { get; set; }
        public int ShapeId { get; set; }
        public int ProductId { get; set; }
        public int InventoryId { get; set; }
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }

        public virtual Inventory Inventory { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual Shape Shape { get; set; } = null!;
    }
}

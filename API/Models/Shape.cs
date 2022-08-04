using System;
using System.Collections.Generic;

namespace XshapeAPI.Models
{
    public partial class Shape
    {
        public Shape()
        {
            InventoryItems = new HashSet<InventoryItem>();
        }

        public int ShapeId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<InventoryItem> InventoryItems { get; set; }
    }
}

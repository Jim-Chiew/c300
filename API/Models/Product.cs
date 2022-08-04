using System;
using System.Collections.Generic;

namespace XshapeAPI.Models
{
    public partial class Product
    {
        public Product()
        {
            InventoryItems = new HashSet<InventoryItem>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public float Price { get; set; }

        public virtual ICollection<InventoryItem> InventoryItems { get; set; }
    }
}

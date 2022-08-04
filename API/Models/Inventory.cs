using System;
using System.Collections.Generic;

namespace XshapeAPI.Models
{
    public partial class Inventory
    {
        public Inventory()
        {
            InventoryItems = new HashSet<InventoryItem>();
            Users = new HashSet<User>();
        }

        public int InventoryId { get; set; }
        public string Location { get; set; } = null!;

        public virtual ICollection<InventoryItem> InventoryItems { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}

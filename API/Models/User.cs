using System;
using System.Collections.Generic;

namespace XshapeAPI.Models
{
    public partial class User
    {
        public User()
        {
            Inventories = new HashSet<Inventory>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public byte[] Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ContactNo { get; set; } = null!;
        public string ApiKey { get; set; } = null!;
        public DateTime? LastLogin { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
    }
}

namespace XshapeAPI.Models
{
    public partial class InventoryItemUpdate
    {
        public string devKey { get; set; } = null!;
        public int ItemId { get; set; }

        public int Quantity { get; set; }
    }
}

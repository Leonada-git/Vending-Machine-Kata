namespace TDDVendingMachine
{
    public class Inventory
    {
        private readonly List<InventoryItem> inventory = [];

        public IReadOnlyList<InventoryItem> InventoryItems
        {
            get { return inventory.AsReadOnly(); }
        }

        public void AddProduct(InventoryItem item)
        {
            inventory.Add(item);
        }

        public void RemoveProduct(InventoryItem item)
        {
            if (IsInStock(item))
            {
                inventory.Remove(item);
            }
            else
            {
                return;
            }
        }

        private static bool IsInStock(InventoryItem item)
        {
            return item.Stock <= 0;
        }

        public InventoryItem SearchProduct(string name)
        {
            var item = inventory.FirstOrDefault(p => p.Product.Name == name);

            return item ?? InventoryItem.Empty;
        }

    }
}

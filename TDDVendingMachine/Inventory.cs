namespace TDDVendingMachine
{
    public class Inventory
    {
        private readonly List<InventoryItem> inventory = [];

        public IReadOnlyList<InventoryItem> InventoryItems
        {
            get { return inventory.AsReadOnly(); }
        }

        public static string GetName(InventoryItem product)
        {
            return InventoryItem.ExtractName(product.Product);
        }
        public static int GetPrice(InventoryItem product)
        {
            return InventoryItem.ExtractPrice(product.Product);
        }
        public static int GetStock(InventoryItem product)
        {
            return product.Stock;
        }

        public void AddProduct(InventoryItem item)
        {
            inventory.Add(item);
        }

        public void RemoveProduct(InventoryItem item)
        {
            if (item.Stock == 0)
            {
                inventory.Remove(item);
            }
            else
            {
                return;
            }
        }

        public InventoryItem SearchProduct(string name)
        {
            var item = inventory.FirstOrDefault(p => p.Product.Name == name);

            return item ?? InventoryItem.Empty;
        }

    }
}

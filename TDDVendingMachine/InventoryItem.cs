

namespace TDDVendingMachine
{
    public class InventoryItem(int stock, Product product)
    {
        private readonly Product product = product;
        private int stock = stock;

        public static readonly InventoryItem Empty = new(0, new Product(string.Empty, 0));

        public bool IsEmpty => this.Equals(InventoryItem.Empty);

        public Product Product
        {
            get { return product; }
        }

        public int Stock
        {
            get { return stock; }
        }

        public static string ExtractName(Product product)
        {
            return product.Name;
        }
        public static int ExtractPrice(Product product)
        {
            return product.Price;
        }

        internal void UpdatingStock()
        {
            stock--;
        }
    }
}

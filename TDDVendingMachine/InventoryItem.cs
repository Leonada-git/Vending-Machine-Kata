

namespace TDDVendingMachine
{
    public class InventoryItem(Product product, int stock)
    {
        private readonly Product product = product;
        private int stock = stock;

        public static readonly InventoryItem Empty = new(new Product(string.Empty, 0), 0);

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

        internal void DecrementStock()
        {
            stock--;
        }
    }
}

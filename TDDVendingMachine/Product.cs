namespace TDDVendingMachine
{
    public class Product(string name, int price)
    {
        private readonly string name = name;
        private readonly int price = price;

        public string Name
        {
            get { return name; }
        }
        public int Price
        {
            get { return price; }
        }
    }

}

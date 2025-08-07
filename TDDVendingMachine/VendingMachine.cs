namespace TDDVendingMachine
{
    public class VendingMachine(Inventory inventory)
    {
        private readonly Inventory vendingMachine = inventory;
        private readonly MoneyCompartement moneyCompartement = new();
        private int returnedChange = 0;


        public IEnumerable<InventoryItem> Products
        {
            get { return vendingMachine.InventoryItems; }
        }

        public MoneyCompartement MoneyCompartement
        {
            get { return moneyCompartement; }
        }

        public InventoryItem FindProduct(string name)
        {
            return vendingMachine.SearchProduct(name);
        }

        public int GetChange()
        {
            return moneyCompartement.GetChange();
        }

        public int GetCurrentAmount()
        {
            return moneyCompartement.GetCurrentAmount();
        }
        public int GetReturnedChange()
        {
            return returnedChange;
        }

        private int AddToChange(int coin)
        {
            return moneyCompartement.AddToChange(coin);
        }

        public int AddToCurrentAmount(int coin)
        {
            Display();
            return moneyCompartement.AddToCurrentAmount(coin);
        }

        public InventoryItem DispenseSelectedProduct(string product)
        {
            InventoryItem item = FindProduct(product);

            DisplayStock(Inventory.GetName(item));

            int itemPrice = Inventory.GetPrice(item);
            int currentAmount = GetCurrentAmount();

            if (itemPrice <= GetCurrentAmount() && Inventory.GetStock(item) > 0)
            {
                int coinsToReturns = CalculateReturnedChange(currentAmount, itemPrice);

                try
                {
                    AddToChange(currentAmount);
                    returnedChange = ReturnChange(coinsToReturns);
                    ResetCurrentAmount();

                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Invalid operation: {ex.Message}");
                    ReturnChange(currentAmount);
                    //make sure not needed
                    //AddToCurrentAmount(coinsToReturns);

                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Invalid argument: {ex.Message}");
                    ResetCurrentAmount();

                }

                RemoveProductFromStock(item);

                return item;
            }
            else
            {
                return InventoryItem.Empty;
            }
        }

        private static int CalculateReturnedChange(int currentAmount, int itemPrice)
        {
            int coinsToReturns = currentAmount - itemPrice;
            return coinsToReturns;
        }

        private int ReturnChange(int amount)
        {
            return moneyCompartement.TryReturnChange(amount);
        }

        public string Display()
        {
            if (GetCurrentAmount() == 0)
            {
                return "INSERT COIN";
            }
            return GetCurrentAmount().ToString();
        }

        public string DisplayStock(string name)
        {
            InventoryItem product = FindProduct(name);

            if (product.IsEmpty)
            {
                return "Sold Out";
            }
            return name;
        }

        private void ResetCurrentAmount()
        {
            ResetCurrentAmount();
            Display();
        }

        public void RemoveProductFromStock(InventoryItem product)
        {

            if (product.IsEmpty)
            {
                return;
            }
            else
            {
                product.UpdatingStock();
                RemoveProductWithNoStock(product);
            }

        }

        private void RemoveProductWithNoStock(InventoryItem product)
        {
            vendingMachine.RemoveProduct(product);
        }

    }
}

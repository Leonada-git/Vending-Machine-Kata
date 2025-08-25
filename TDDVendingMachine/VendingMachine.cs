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
            return moneyCompartement.Change;
        }

        public int GetCurrentAmount()
        {
            return moneyCompartement.CurrentAmount;
        }
        public int GetReturnedChange()
        {
            return returnedChange;
        }

        private int AddToChange(int coin)
        {
            moneyCompartement.AddToChange(coin);
            return GetChange();
        }

        public int AddToCurrentAmount(int coin)
        {
            Display();
            moneyCompartement.AddToCurrentAmount(coin);
            return GetCurrentAmount();
        }

        public InventoryItem DispenseSelectedProduct(string product)
        {
            InventoryItem item = FindProduct(product);

            DisplayStock(Inventory.GetName(item));

            int itemPrice = Inventory.GetPrice(item);
            int currentAmount = GetCurrentAmount();

            if (CanAfford(itemPrice) && IsInStock(item))
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

                }

                RemoveProductFromStock(item);

                return item;
            }
            else
            {
                return InventoryItem.Empty;
            }
        }

        private static bool IsInStock(InventoryItem item)
        {
            return Inventory.GetStock(item) > 0;
        }

        private bool CanAfford(int itemPrice)
        {
            return itemPrice <= GetCurrentAmount();
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
            if (CurrentAmountIsEmpty())
            {
                return "INSERT COIN";
            }
            return GetCurrentAmount().ToString();
        }

        private bool CurrentAmountIsEmpty()
        {
            return GetCurrentAmount() == 0;
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
            moneyCompartement.ResetCurrentAmount();
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
                product.DecrementStock();
                RemoveProductWithNoStock(product);
            }

        }

        private void RemoveProductWithNoStock(InventoryItem product)
        {
            vendingMachine.RemoveProduct(product);
        }

    }
}

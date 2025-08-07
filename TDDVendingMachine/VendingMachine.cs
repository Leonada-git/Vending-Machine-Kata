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

            if (itemPrice <= GetCurrentAmount() && Inventory.GetStock(item) > 0)
            {
                try
                {
                    int currentAmount = GetCurrentAmount();
                    returnedChange = CalculateReturendChange(itemPrice);
                    AddToChange(currentAmount);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ResetCurrentAmount();

                }
                ResetCurrentAmount();
                RemoveProductFromStock(item);

                return item;
            }
            else
            {
                return item ?? InventoryItem.Empty;
            }
        }

        private int CalculateReturendChange(int itemPrice)
        {
            int coinsToReturns = GetCurrentAmount() - itemPrice;
            return ReturnChange(coinsToReturns);
        }

        public int ReturnChange(int amount)
        {
            return moneyCompartement.TryReturnChange(amount);
        }

        public string Display()
        {
            if (GetCurrentAmount() == 0 && GetChange() <= 100)
            {
                return "EXACT CHANGE ONLY";
            }
            else if (GetCurrentAmount() == 0)
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

        private int ResetCurrentAmount()
        {
            Display();
            return moneyCompartement.ResetCurrentAmount();
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
            if (product.Stock == 0)
            {
                vendingMachine.RemoveProduct(product);
            }
            else
            {
                return;
            }
        }

    }
}

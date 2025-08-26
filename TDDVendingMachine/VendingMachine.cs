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

        private void AddToChange(int coin)
        {
            moneyCompartement.AddToChange(coin);
        }

        public void AddToCurrentAmount(int coin)
        {
            Display();
            moneyCompartement.AddToCurrentAmount(coin);
        }

        public InventoryItem DispenseSelectedProduct(string product)
        {
            DisplayStock(product);
            var item = FindProduct(product);
            var itemPrice = Inventory.GetPrice(item);

            if (CanAfford(itemPrice) && IsInStock(item))
            {
                var coinsToReturns = CalculateReturnedChange(itemPrice);

                HandleTansaction(coinsToReturns);

                RemoveProductFromStock(item);

                return item;
            }
            else
            {
                return InventoryItem.Empty;
            }
        }

        private void HandleTansaction(int coinsToReturns)
        {
            AddToChange(GetCurrentAmount());
            returnedChange = ReturnChange(coinsToReturns);
            ResetCurrentAmount();
        }

        private static bool IsInStock(InventoryItem item)
        {
            return Inventory.GetStock(item) > 0;
        }

        private bool CanAfford(int itemPrice)
        {
            return itemPrice <= GetCurrentAmount();
        }

        private int CalculateReturnedChange(int itemPrice)
        {
            int coinsToReturns = GetCurrentAmount() - itemPrice;

            return coinsToReturns;
        }

        private int ReturnChange(int amount)
        {
            return moneyCompartement.TryReturnChange(amount);
        }

        private void ResetCurrentAmount()
        {
            moneyCompartement.ResetCurrentAmount();
            Display();
        }

        public void CancelPurchase()
        {
            returnedChange = GetCurrentAmount();
            ResetCurrentAmount();
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

        public void RemoveProductFromStock(InventoryItem product)
        {
            product.DecrementStock();
            RemoveProductWithNoStock(product);
        }

        private void RemoveProductWithNoStock(InventoryItem product)
        {
            vendingMachine.RemoveProduct(product);
        }

    }
}

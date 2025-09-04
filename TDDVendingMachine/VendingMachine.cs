namespace TDDVendingMachine
{
    public class VendingMachine(Inventory inventory)
    {
        private readonly Inventory vendingMachine = inventory;
        private readonly MoneyCompartement moneyCompartement = new();
        private int returnedChange = 0;

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

        public string Display()
        {
            if (CurrentAmountIsEmpty())
            {
                return "INSERT COIN";
            }
            return GetCurrentAmount().ToString();
        }

        public InventoryItem DispenseSelectedProduct(string productName)
        {
            DisplayStock(productName);
            return FetchProduct(productName);
        }

        private InventoryItem FetchProduct(string productName)
        {
            var product = FindProduct(productName);
            var productPrice = product.Price;

            if (CanNotAfford(productPrice))
            {
                throw new ArgumentException("Insuffisent coins to dispence product");
            }
            else if (IsInStock(product))
            {
                var coinsToReturns = CalculateReturnedChange(productPrice);

                HandleTansaction(coinsToReturns);

                RemoveProductFromStock(product);

                return product;
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
            return item.Stock > 0;
        }

        private bool CanNotAfford(int itemPrice)
        {
            return itemPrice > GetCurrentAmount();
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

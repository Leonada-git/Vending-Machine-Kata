namespace TDDVendingMachine
{
    public class VendingMachine(Inventory inventory)
    {
        private readonly Inventory vendingMachine = inventory;
        private readonly MoneyCompartment moneyCompartement = new();
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

            if (CanNotAfford(product))
            {
                throw new ArgumentException("Insuffisent coins to dispence product");
            }
            else if (IsInStock(product))
            {
                HandelProductSale(product);

                return product;
            }
            else
            {
                return InventoryItem.Empty;
            }
        }

        private void HandelProductSale(InventoryItem product)
        {
            var coinsToReturns = CalculateReturnedChange(product);

            HandleMoneyTansaction(coinsToReturns);
            RemoveProductFromStock(product);
        }

        private void HandleMoneyTansaction(int coinsToReturns)
        {
            AddToChange(GetCurrentAmount());
            returnedChange = ReturnChange(coinsToReturns);
            ResetCurrentAmount();
        }

        private void AddToChange(int coin)
        {
            moneyCompartement.AddToChange(coin);
        }

        private static bool IsInStock(InventoryItem item)
        {
            return item.Stock > 0;
        }

        private bool CanNotAfford(InventoryItem product)
        {
            return product.Price > GetCurrentAmount();
        }

        private int CalculateReturnedChange(InventoryItem product)
        {
            int coinsToReturns = GetCurrentAmount() - product.Price;

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

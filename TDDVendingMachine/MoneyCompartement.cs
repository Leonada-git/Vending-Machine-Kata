namespace TDDVendingMachine
{
    public class MoneyCompartement
    {
        private readonly List<CoinsType> currentAmount = [];
        private readonly List<CoinsType> change =
        [
            CoinsType.quarters,
            CoinsType.quarters,
            CoinsType.quarters,
            CoinsType.dimes,
            CoinsType.dimes,
            CoinsType.dimes,
            CoinsType.nickels,
            CoinsType.nickels,
            CoinsType.nickels,
        ];

        public int GetChange()
        {
            return change.Select(c => (int)c).Sum();
        }

        public int GetCurrentAmount()
        {
            return currentAmount.Select(c => (int)c).Sum();
        }

        public int AddToChange(int coin)
        {
            if (Enum.IsDefined(typeof(CoinsType), coin))
            {
                CoinsType coinValue = (CoinsType)coin;
                change.Add(coinValue);
                return GetChange();
            }
            else
            {
                throw new ArgumentException($"Invalid coin value: {coin}");
            }
        }

        public int AddToCurrentAmount(int coin)
        {
            if (Enum.IsDefined(typeof(CoinsType), coin))
            {
                CoinsType coinValue = (CoinsType)coin;
                currentAmount.Add(coinValue);
                return GetCurrentAmount();
            }
            else
            {
                throw new ArgumentException($"Invalid coin value: {coin}");
            }
        }

        public int ResetCurrentAmount()
        {
            currentAmount.Clear();

            return GetCurrentAmount();
        }

        public int TryReturnChange(int amount)
        {
            var backup = new List<CoinsType>(change);
            try
            {
                ReturnChange(amount);
                return amount;
            }
            catch
            {
                change.Clear();
                change.AddRange(backup);
                return 0;
            }
        }

        private void ReturnChange(int amount)
        {
            int[] coinValues = [25, 10, 5];

            foreach (int coinValue in coinValues)
            {
                CoinsType coin = (CoinsType)coinValue;

                while (amount >= coinValue && change.Contains(coin))
                {
                    change.Remove(coin);
                    amount -= coinValue;
                }
            }

            if (amount > 0)
                throw new InvalidOperationException("Insufficient coins to remove the exact amount.");

        }
    }
}

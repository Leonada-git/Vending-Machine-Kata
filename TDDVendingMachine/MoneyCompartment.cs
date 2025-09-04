namespace TDDVendingMachine
{
    public class MoneyCompartment
    {
        private readonly List<CoinsType> currentAmount = [];
        private readonly List<CoinsType> change =
        [
            CoinsType.quarters,
            CoinsType.quarters,
            CoinsType.quarters,
            CoinsType.nickels,
            CoinsType.nickels,
            CoinsType.nickels,
        ];

        public int Change
        {
            get { return change.Sum(c => (int)c); }
        }

        public int CurrentAmount
        {
            get { return currentAmount.Sum(c => (int)c); }
        }

        public void ResetCurrentAmount()
        {
            currentAmount.Clear();
        }
        public void AddToChange(int amount)
        {
            foreach (var coin in BreakAmountIntoCoins(amount))
            {
                change.Add(coin);
            }
        }

        private static IEnumerable<CoinsType> BreakAmountIntoCoins(int amount)
        {
            int[] coinValues = [25, 10, 5];
            var coins = new List<CoinsType>();

            foreach (var coinValue in coinValues)
            {
                var coin = (CoinsType)coinValue;

                while (CanBreakFurther(amount, coinValue))
                {
                    coins.Add(coin);
                    amount -= coinValue;
                }
            }

            return coins;
        }

        private static bool CanBreakFurther(int amount, int coinValue)
        {
            return amount >= coinValue;
        }

        public void AddToCurrentAmount(int coinValue)
        {
            if (IsValideCoin(coinValue))
            {
                currentAmount.Add((CoinsType)coinValue);
            }
            else
            {
                throw new ArgumentException($"Invalid coin value: {coinValue}.");
            }
        }

        private static bool IsValideCoin(int coinValue)
        {
            return Enum.IsDefined(typeof(CoinsType), coinValue);
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
                throw;
            }
        }

        private void ReturnChange(int amount)
        {
            var coinsToReturn = BreakAmountIntoCoins(amount);

            foreach (var coin in coinsToReturn)
            {
                if (!TryUseCoin(coin))
                    throw new InvalidOperationException("Insufficient coins to return the exact amount.");
            }

        }

        private bool TryUseCoin(CoinsType coin)
        {
            return change.Remove(coin);
        }
    }
}
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

        public void AddToChange(int amount)
        {
            amount = BreakIntoCoins(amount, coin => change.Add(coin));

        }

        private int BreakIntoCoins(int amount, Action<CoinsType> coinAction)
        {
            int[] coinValues = [25, 10, 5];

            foreach (int coinValue in coinValues)
            {
                CoinsType coin = (CoinsType)coinValue;

                while (amount >= coinValue && change.Contains(coin))
                {
                    coinAction(coin);
                    amount -= coinValue;
                }
            }

            return amount;
        }
        public void AddToCurrentAmount(int coin)
        {
            if (IsValideCoin(coin))
            {
                CoinsType coinValue = (CoinsType)coin;
                currentAmount.Add(coinValue);
            }
            else
            {
                throw new ArgumentException($"Invalid coin value: {coin}.");
            }
        }

        private static bool IsValideCoin(int coin)
        {
            return Enum.IsDefined(typeof(CoinsType), coin);
        }

        public void ResetCurrentAmount()
        {
            currentAmount.Clear();
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
            amount = BreakIntoCoins(amount, coin => change.Remove(coin));

            if (amount > 0)
                throw new InvalidOperationException("Insufficient coins to return the exact amount.");

        }
    }
}

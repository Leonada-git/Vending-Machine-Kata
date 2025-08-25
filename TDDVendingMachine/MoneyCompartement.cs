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
            int[] coinValues = [25, 10, 5];

            foreach (int coinValue in coinValues)
            {
                CoinsType coin = (CoinsType)coinValue;

                while (amount >= coinValue && change.Contains(coin))
                {
                    change.Add(coin);
                    amount -= coinValue;
                }
            }
        }

        public void AddToCurrentAmount(int coin)
        {
            if (isValideCoin(coin))
            {
                CoinsType coinValue = (CoinsType)coin;
                currentAmount.Add(coinValue);
            }
            else
            {
                throw new ArgumentException($"Invalid coin value: {coin}.");
            }
        }

        private static bool isValideCoin(int coin)
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
                return 0;
                throw;
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
                throw new InvalidOperationException("Insufficient coins to return the exact amount.");

        }
    }
}

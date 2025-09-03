using TDDVendingMachine;

namespace TDDVendingMachineTests
{
    using FluentAssertions;
    using Xunit;

    public class VendingMachineTests
    {
        private readonly VendingMachine sut;
        private readonly Inventory inventory = new();

        public VendingMachineTests()
        {
            inventory.AddProduct(new InventoryItem(new Product(name: "cola", price: 100), stock: 8));
            inventory.AddProduct(new InventoryItem(new Product(name: "chips", price: 50), stock: 6));
            inventory.AddProduct(new InventoryItem(new Product(name: "candy", price: 65), stock: 12));
            inventory.AddProduct(new InventoryItem(new Product(name: "banana", price: 5), stock: 1));

            sut = new VendingMachine(inventory);
        }

        [Fact]
        public void Change_is_90_upon_creation()
        {
            var change = sut.GetChange();

            change.Should().Be(90);

        }

        [Fact]
        public void Current_amount_is_zero_upon_creation()
        {
            var change = sut.GetCurrentAmount();

            change.Should().Be(0);

        }

        [Fact]
        public void Inserting_invalide_coin_throws_exception()
        {
            Action action = () => sut.AddToCurrentAmount(4);

            action.Should().Throw<ArgumentException>()
               .WithMessage($"Invalid coin value: 4.");
        }

        [Fact]
        public void Inserting_coins_adds_to_current_amount()
        {
            sut.AddToCurrentAmount(25);

            var CurrentAmount = sut.GetCurrentAmount();
            CurrentAmount.Should().Be(25);
        }

        [Fact]
        public void Purchase_cancelation_returns_current_amount()
        {
            InsertFourQuarters();

            sut.CancelPurchase();

            var returnedChange = sut.GetReturnedChange();
            returnedChange.Should().Be(100);

        }
        private void InsertFourQuarters()
        {
            int[] insertedCoin = [25, 25, 25, 25];

            foreach (var coin in insertedCoin)
            {
                sut.AddToCurrentAmount(coin);
            }
        }

        [Fact]
        public void Dispense_selected_product()
        {
            InsertFourQuarters();

            var product = sut.DispenseSelectedProduct("cola");

            var productName = product.Name;
            productName.Should().Be("cola");
        }

        [Fact]
        public void Throws_if_coins_are_insufficient_to_return_the_change()
        {
            InsertFourQuarters();

            var action = () => sut.DispenseSelectedProduct("banana");

            action.Should().Throw<InvalidOperationException>()
                           .WithMessage("Insufficient coins to return the exact amount.");
        }

        [Fact]
        public void Purchase_adds_product_price_to_change_compartment()
        {
            sut.AddToCurrentAmount(5);

            sut.DispenseSelectedProduct("banana");

            var change = sut.GetChange();
            change.Should().Be(95);
        }

        [Fact]
        public void Purchase_product_sets_current_amount_to_zero()
        {
            sut.AddToCurrentAmount(5);

            sut.DispenseSelectedProduct("banana");

            var currentAmount = sut.GetCurrentAmount();
            currentAmount.Should().Be(0);
        }

        [Fact]
        public void Change_is_returned_if_available()
        {
            sut.AddToCurrentAmount(10);

            sut.DispenseSelectedProduct("banana");

            var returnedChange = sut.GetReturnedChange();
            returnedChange.Should().Be(5);
        }

        [Fact]
        public void Displays_INSERT_COIN_if_currentAmount_is_zero()
        {
            var message = sut.Display();

            message.Should().Be("INSERT COIN");
        }

        [Fact]
        public void Displays_Sold_Out_If_out_of_stock()
        {
            var message = sut.DisplayStock("pizza");

            message.Should().Be("Sold Out");
        }

        [Fact]
        public void Displays_currentAmount_when_inserting_coins()
        {
            sut.AddToCurrentAmount(25);

            var message = sut.Display();

            message.Should().Be("25");

        }

        [Fact]
        public void Product_returned_if_found()
        {
            InventoryItem product = sut.FindProduct("cola");

            product.Name.Should().Be("cola");
        }

        [Fact]
        public void Empty_product_returned_if_Not_Found()
        {
            InventoryItem product = sut.FindProduct("pizza");

            product.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void Decrement_stock_of_dispensed_product()
        {
            AddTowJuiceProductsToStock();
            sut.AddToCurrentAmount(25);

            sut.DispenseSelectedProduct("juice");

            var juiceNewStock = sut.FindProduct("juice").Stock;
            juiceNewStock.Should().Be(1);
        }

        private void AddTowJuiceProductsToStock()
        {
            inventory.AddProduct(new InventoryItem(new Product(name: "juice", price: 10), stock: 2));
        }

        [Fact]
        public void Removes_Product_With_No_Stock()
        {
            AddTowJuiceProductsToStock();
            sut.AddToCurrentAmount(10);
            sut.DispenseSelectedProduct("juice");

            sut.AddToCurrentAmount(10);
            sut.DispenseSelectedProduct("juice");

            InventoryItem verifyProduct = sut.FindProduct("juice");
            verifyProduct.IsEmpty.Should().BeTrue();
        }

    }
}
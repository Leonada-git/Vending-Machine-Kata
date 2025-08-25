using TDDVendingMachine;

namespace TDDVendingMachineTests
{
    using FluentAssertions;
    using Xunit;

    public class VendingMachineTests
    {
        private readonly VendingMachine sut;
        private Inventory inventory = new();

        public VendingMachineTests()
        {
            inventory.AddProduct(new InventoryItem(new Product("cola", 100), 8));
            inventory.AddProduct(new InventoryItem(new Product("chips", 50), 6));
            inventory.AddProduct(new InventoryItem(new Product("candy", 65), 12));
            inventory.AddProduct(new InventoryItem(new Product("banana", 5), 1));

            sut = new VendingMachine(inventory);
        }

        [Fact]
        public void Change_is_90_upon_creation()
        {
            var change = sut.GetChange();

            change.Should().Be(90);

        }

        [Fact]
        public void Inserting_invalide_coin_throws_exception()
        {
            Action act = () => sut.AddToCurrentAmount(4);

            act.Should().Throw<ArgumentException>()
               .WithMessage($"Invalid coin value: 4.");
        }

        [Fact]
        public void Inserting_coins_adds_to_current_amount()
        {
            sut.AddToCurrentAmount(25);

            var newCurrentAmount = sut.GetCurrentAmount();
            newCurrentAmount.Should().Be(25);
        }

        [Fact]
        public void Dispense_selected_product()
        {
            InsertFourQuarters();

            var product = sut.DispenseSelectedProduct("cola");

            var productName = Inventory.GetName(product);
            productName.Should().Be("cola");
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
        public void Purchase_adds_product_price_to_change_compartment()
        {
            sut.AddToCurrentAmount(5);

            sut.DispenseSelectedProduct("banana");

            var newChange = sut.GetChange();
            newChange.Should().Be(95);
        }

        [Fact]
        public void Insufficient_coins_to_return_the_change_returns_zero()
        {
            InsertFourQuarters();

            sut.DispenseSelectedProduct("banana");

            var returnedChange = sut.GetReturnedChange();
            returnedChange.Should().Be(0);
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
        public void Inserting_coins_displays_currentAmount()
        {
            sut.AddToCurrentAmount(25);

            var message = sut.Display();

            message.Should().Be("25");

        }
        [Fact]
        public void Product_returned_if_found()
        {
            InventoryItem product = sut.FindProduct("cola");

            Inventory.GetName(product).Should().Be("cola");
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
            AddProductJuiceWithTwoInStock();
            sut.AddToCurrentAmount(25);

            sut.DispenseSelectedProduct("juice");

            InventoryItem verifyProduct = sut.FindProduct("juice");
            var currentStock = Inventory.GetStock(verifyProduct);
            currentStock.Should().Be(1);
        }

        private void AddProductJuiceWithTwoInStock()
        {
            inventory.AddProduct(new InventoryItem(new Product("juice", 15), 2));
        }

        [Fact]
        public void Removes_Product_With_No_Stock()
        {
            sut.AddToCurrentAmount(10);

            sut.DispenseSelectedProduct("banana");

            InventoryItem verifyProduct = sut.FindProduct("banana");
            verifyProduct.IsEmpty.Should().BeTrue();
        }


    }
}
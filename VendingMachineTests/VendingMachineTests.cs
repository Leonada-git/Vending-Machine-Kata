using TDDVendingMachine;

namespace TDDVendingMachineTests
{
    using FluentAssertions;
    using Xunit;

    public class VendingMachineTests
    {
        private readonly Product _cola = new("cola", 100);
        private readonly Product _chips = new("chips", 50);
        private readonly Product _candy = new("candy", 65);
        private readonly Product _banana = new("banana", 25);

        private readonly InventoryItem _colas;
        private readonly InventoryItem _bagsOfChips;
        private readonly InventoryItem _candies;
        private readonly InventoryItem _bananas;


        private readonly Inventory _inventory;
        private readonly VendingMachine sut;

        public VendingMachineTests()
        {
            _colas = new InventoryItem(8, _cola);
            _bagsOfChips = new InventoryItem(6, _chips);
            _candies = new InventoryItem(12, _candy);
            _candies = new InventoryItem(12, _candy);
            _bananas = new InventoryItem(1, _banana);


            _inventory = new Inventory();
            _inventory.AddProduct(_colas);
            _inventory.AddProduct(_bagsOfChips);
            _inventory.AddProduct(_candies);
            _inventory.AddProduct(_bananas);

            sut = new VendingMachine(_inventory);
        }

        [Fact]
        public void Adding_invalide_coin_throws_exception()
        {
            int coin = 4;
            Action act = () => sut.AddToCurrentAmount(coin);

            act.Should().Throw<ArgumentException>()
               .WithMessage($"Invalid coin value: 4");
        }

        [Fact]
        public void Adds_to_Current_Amount()
        {
            int coin = 25;

            sut.AddToCurrentAmount(coin);

            int newChange = sut.GetCurrentAmount();
            newChange.Should().Be(25);
        }

        [Fact]
        public void Dispense_selected_product()
        {
            InventoryItem product = sut.DispenseSelectedProduct("cola");

            product.Product.Name.Should().Be("cola");
        }

        [Fact]
        public void Balance_is_120_upon_account_creation()
        {
            int newChange = sut.GetChange();
            newChange.Should().Be(120);

        }

        [Fact]
        public void Add_increases_change_amount()
        {
            int coin = 25;
            sut.AddToChange(coin);
            int newChange = sut.GetChange();

            newChange.Should().Be(145);
        }

        [Fact]
        public void Returns_Total_change_available()
        {
            int result = sut.GetChange();

            result.Should().Be(120);
        }


        [Fact]
        public void Change_exceeds_available_change_return_false()
        {
            int amount = 1000;
            int result = sut.ReturnChange(amount);

            result.Should().Be(0);
        }

        [Fact]
        public void Change_less_than_available_change_return_true()
        {
            int amount = 50;
            int result = sut.ReturnChange(amount);

            result.Should().Be(50);
        }

        [Fact]
        public void Displays_INSERT_COIN_if_currentAmount_is_zero()
        {
            string message = sut.Display();

            message.Should().Be("INSERT COIN");
        }

        [Fact]
        public void Displays_Sold_Out_If_OutOfStock()
        {
            string message = sut.DisplayStock("pizza");

            message.Should().Be("Sold Out");
        }

        [Fact]
        public void Displays_EXACT_CHANGE_ONLY_if_change_available_is_limited()
        {
            //Emptying Change
            sut.ReturnChange(100);

            string message = sut.Display();

            message.Should().Be("EXACT CHANGE ONLY");
        }

        [Fact]
        public void Inserting_coins_displays_currentAmount()
        {
            sut.AddToCurrentAmount(25);

            string message = sut.Display();

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
        public void Stock_decrement_of_dispensed_product()
        {
            InventoryItem product = sut.FindProduct("cola");


            sut.RemoveProductFromStock(product);

            InventoryItem verifyProduct = sut.FindProduct("cola");
            int newStock = Inventory.GetStock(verifyProduct);
            newStock.Should().Be(7);
        }

        [Fact]
        public void Removes_Product_With_No_Stock()
        {
            InventoryItem product = sut.FindProduct("banana");

            sut.RemoveProductFromStock(product);

            InventoryItem verifyProduct = sut.FindProduct("banana");
            verifyProduct.IsEmpty.Should().BeTrue();
        }


    }
}
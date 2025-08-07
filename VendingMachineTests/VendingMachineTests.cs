using TDDVendingMachine;

namespace TDDVendingMachineTests
{
    using FluentAssertions;
    using Xunit;

    public class VendingMachineTests
    {
        private readonly Product cola = new("cola", 100);
        private readonly Product chips = new("chips", 50);
        private readonly Product candy = new("candy", 65);
        private readonly Product banana = new("banana", 5);

        private readonly InventoryItem colas;
        private readonly InventoryItem bagsOfChips;
        private readonly InventoryItem candies;
        private readonly InventoryItem bananas;


        private readonly Inventory inventory;
        private readonly VendingMachine sut;

        public VendingMachineTests()
        {
            colas = new InventoryItem(8, cola);
            bagsOfChips = new InventoryItem(6, chips);
            candies = new InventoryItem(12, candy);
            candies = new InventoryItem(12, candy);
            bananas = new InventoryItem(1, banana);


            inventory = new Inventory();
            inventory.AddProduct(colas);
            inventory.AddProduct(bagsOfChips);
            inventory.AddProduct(candies);
            inventory.AddProduct(bananas);

            sut = new VendingMachine(inventory);
        }

        [Fact]
        public void Inserting_invalide_coin_throws_exception()
        {
            int coin = 4;
            Action act = () => sut.AddToCurrentAmount(coin);

            act.Should().Throw<ArgumentException>()
               .WithMessage($"Invalid coin value: 4");
        }

        [Fact]
        public void Adds_coins_to_current_amount()
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
        public void Change_is_120_upon_account_creation()
        {
            int newChange = sut.GetChange();

            newChange.Should().Be(120);

        }

        [Fact]
        public void Purchase_adds_product_price_to_change_compartment()
        {
            sut.AddToCurrentAmount(5);

            sut.DispenseSelectedProduct("banana");

            int newChange = sut.GetChange();
            newChange.Should().Be(125);
        }

        [Fact]
        public void Change_exceeds_available_change_return_false()
        {
            int[] insertedCoin = [25, 25, 25, 25, 25, 25];

            foreach (var coin in insertedCoin)
            {
                sut.AddToCurrentAmount(coin);
            }

            sut.DispenseSelectedProduct("banana");

            int returnedChange = sut.GetReturnedChange();
            returnedChange.Should().Be(0);
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
namespace CoverGoChallengeTests.ApplicationLayer
{
    [TestFixture]
    public class ShoppingCartServiceTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IShoppingCartRepository> _cartRepositoryMock;
        private ShoppingCartService _shoppingCartService;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _cartRepositoryMock = new Mock<IShoppingCartRepository>();
            _shoppingCartService = new ShoppingCartService(_productRepositoryMock.Object, _cartRepositoryMock.Object);
        }

        [Test]
        public void AddProductToCart_WhenProductExistsAndInStock_ShouldAddProductToCartAndDecreaseStock()
        {
            // Arrange
            string cartId = "cartId";
            string productId = "productId";
            int quantity = 2;
            int productOriginalStock = 5;
            var product = new Product(Guid.NewGuid(), "Test Product", "Product description", productOriginalStock, 10);
            _productRepositoryMock.Setup(r => r.GetById(productId)).Returns(product);
            var cart = new ShoppingCart(Guid.NewGuid());
            _cartRepositoryMock.Setup(r => r.GetCart(cartId)).Returns(cart);

            // Act
            _shoppingCartService.AddProductToCart(cartId, productId, quantity);

            // Assert
            Assert.AreEqual(1, cart.GetItems().Count);
            Assert.AreEqual(quantity, cart.GetItems()[0].Quantity);
            Assert.AreEqual(productOriginalStock - quantity, product.Stock);
            _cartRepositoryMock.Verify(r => r.SaveCart(cart), Times.Once);
        }

        [Test]
        public void AddProductToCart_WhenProductDoesNotExist_ShouldThrowArgumentException()
        {
            // Arrange
            string cartId = "cartId";
            string productId = "productId";
            int quantity = 2;
            _productRepositoryMock.Setup(r => r.GetById(productId)).Returns((Product)null);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _shoppingCartService.AddProductToCart(cartId, productId, quantity));
            _cartRepositoryMock.Verify(r => r.SaveCart(It.IsAny<ShoppingCart>()), Times.Never);
        }

        [Test]
        public void AddProductToCart_WhenProductOutOfStock_ShouldThrowInvalidOperationException()
        {
            // Arrange
            string cartId = "cartId";
            string productId = "productId";
            int quantity = 2;
            var product = new Product(Guid.NewGuid(), "Test Product", "Product description", 1, 10);
            _productRepositoryMock.Setup(r => r.GetById(productId)).Returns(product);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _shoppingCartService.AddProductToCart(cartId, productId, quantity));
            _cartRepositoryMock.Verify(r => r.SaveCart(It.IsAny<ShoppingCart>()), Times.Never);
        }

        [Test]
        public void AddProductToCart_WhenCartDoesNotExist_ShouldCreateCartAndAddProduct()
        {
            // Arrange
            string cartId = Guid.NewGuid().ToString();
            string productId = Guid.NewGuid().ToString();
            int quantity = 2;
            var product = new Product(Guid.NewGuid(), "Test Product", "Product description", 5, 10);
            _productRepositoryMock.Setup(r => r.GetById(productId)).Returns(product);
            _cartRepositoryMock.Setup(r => r.GetCart(cartId)).Returns((ShoppingCart)null);

            // Act
            _shoppingCartService.AddProductToCart(cartId, productId, quantity);

            // Assert
            _cartRepositoryMock.Verify(r => r.SaveCart(It.Is<ShoppingCart>(c => c.GetItems().Count == 1
                                                                && c.GetItems()[0].Product.Id == product.Id
                                                                && c.GetItems()[0].Quantity == quantity)), Times.Once);
        }

        [Test]
        public void RemoveProductFromCart_WhenProductExistsAndInCart_ShouldRemoveProductFromCartAndIncreaseStock()
        {
            // Arrange
            string cartId = "cartId";
            string productId = "productId";
            int quantity = 1;
            int productOriginalStock = 5;
            var product = new Product(Guid.NewGuid(), "Test Product", "Product description", productOriginalStock, 10);
            var cartItem = new CartItem(product, quantity);
            var cart = new ShoppingCart(Guid.NewGuid());
            cart.AddItem(product, quantity);
            _productRepositoryMock.Setup(r => r.GetById(productId)).Returns(product);
            _cartRepositoryMock.Setup(r => r.GetCart(cartId)).Returns(cart);

            // Act
            _shoppingCartService.RemoveProductFromCart(cartId, productId, quantity);

            // Assert
            Assert.AreEqual(0, cart.GetItems().Count);
            Assert.AreEqual(productOriginalStock + quantity, product.Stock);
            _cartRepositoryMock.Verify(r => r.SaveCart(cart), Times.Once);
        }

        [Test]
        public void RemoveProductFromCart_WhenProductDoesNotExist_ShouldThrowArgumentException()
        {
            // Arrange
            string cartId = "cartId";
            string productId = "productId";
            int quantity = 1;
            _productRepositoryMock.Setup(r => r.GetById(productId)).Returns((Product)null);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _shoppingCartService.RemoveProductFromCart(cartId, productId, quantity));
            _cartRepositoryMock.Verify(r => r.SaveCart(It.IsAny<ShoppingCart>()), Times.Never);
        }

        [Test]
        public void RemoveProductFromCart_WhenCartDoesNotExist_ShouldThrowArgumentException()
        {
            // Arrange
            string cartId = "cartId";
            string productId = "productId";
            int quantity = 1;
            var product = new Product(Guid.NewGuid(), "Test Product", "Product description", 5, 10);
            _productRepositoryMock.Setup(r => r.GetById(productId)).Returns(product);
            _cartRepositoryMock.Setup(r => r.GetCart(cartId)).Returns((ShoppingCart)null);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _shoppingCartService.RemoveProductFromCart(cartId, productId, quantity));
            _cartRepositoryMock.Verify(r => r.SaveCart(It.IsAny<ShoppingCart>()), Times.Never);
        }

        [Test]
        public void CalculateTotalPrice_WhenCartDoesNotExist_ShouldThrowArgumentException()
        {
            // Arrange
            string cartId = "cartId";
            _cartRepositoryMock.Setup(r => r.GetCart(cartId)).Returns((ShoppingCart)null);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _shoppingCartService.CalculateTotalPrice(cartId));
        }

        [Test]
        public void CalculateTotalPrice_WhenCartNotEmpty_ShouldReturnTotalPrice()
        {
            // Arrange
            string cartId = Guid.NewGuid().ToString();
            string productId = Guid.NewGuid().ToString();
            int quantity = 2;
            var product = new Product(Guid.NewGuid(), "Test Product", "Product description", 5, 10);
            var cartItem = new CartItem(product, quantity);
            var cart = new ShoppingCart(Guid.NewGuid());
            cart.AddItem(product, quantity);
            _cartRepositoryMock.Setup(r => r.GetCart(cartId)).Returns(cart);

            // Act
            decimal totalPrice = _shoppingCartService.CalculateTotalPrice(cartId);

            // Assert
            Assert.AreEqual(product.Price * quantity, totalPrice);
        }

        [Test]
        public void CalculateTotalPrice_WhenCartIsEmpty_ShouldReturnZero()
        {
            // Arrange
            string cartId = Guid.NewGuid().ToString();
            var cart = new ShoppingCart(Guid.NewGuid());
            _cartRepositoryMock.Setup(r => r.GetCart(cartId)).Returns(cart);

            // Act
            decimal totalPrice = _shoppingCartService.CalculateTotalPrice(cartId);

            // Assert
            Assert.AreEqual(0, totalPrice);
        }
    }
}

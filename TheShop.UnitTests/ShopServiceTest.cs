using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using TheShop.Models.Entities;
using TheShop.Repositories.Interfaces;
using TheShop.Services.Interfaces;
using TheShop.UnitTests.Utils.TestData;

namespace TheShop.UnitTests
{
	[TestClass]
	public class ShopServiceTest
	{
		private Mock<IArticleRepository> _articleRepository;
		private Mock<ISupplierApiService> _supplierApiService;
		private Mock<ILoggerService> _logger;

		[TestInitialize]
		public void Initialize()
		{
			_articleRepository = new Mock<IArticleRepository>();
			_supplierApiService = new Mock<ISupplierApiService>();
			_logger = new Mock<ILoggerService>();
			SetupConsoleLogger();
		}

		[TestMethod]
		public void OrderAndSellArticle_VerifyGetSuppliersCalled()
		{
			_supplierApiService.Setup(m => m.GetSuppliers()).Returns(() => TestSupplierModels.List());

			var _testedInstance = new ShopService(_articleRepository.Object, _supplierApiService.Object, _logger.Object);

			_testedInstance.OrderAndSellArticle(1, 459, 10);

			_supplierApiService.Verify(mock => mock.GetSuppliers(), Times.Once());
		}

		[TestMethod]
		public void OrderAndSellArticle_Sucess()
		{
			var supplierId = 1;
			var buyerId = 10;
			var articleId = 1;
			var maxExpecgedPrice = 459;
			var articleName = $"Article from supplier{supplierId}";
			var articleFromSupplier = TestArticleModels.ArticleModel(articleId, articleName, maxExpecgedPrice - 1);

			 Article addedArticle = null;

			_articleRepository.Setup(m => m.Create(It.IsAny<Article>())).Callback<Article>((a) =>
			{
				addedArticle = a;
			});

			_supplierApiService.Setup(m => m.GetSuppliers()).Returns(() => TestSupplierModels.List());

			var _testedInstance = new ShopService(_articleRepository.Object, _supplierApiService.Object, _logger.Object);

			_testedInstance.OrderAndSellArticle(articleId, maxExpecgedPrice, buyerId);

			Assert.IsNotNull(addedArticle);
			Assert.AreEqual(buyerId, addedArticle.BuyerUserId);
			Assert.AreEqual(articleFromSupplier.ID, addedArticle.ExternalId);
			Assert.AreEqual(articleFromSupplier.ArticlePrice, addedArticle.Price);
			Assert.AreEqual(supplierId, addedArticle.SupplierId);
			Assert.AreEqual(articleFromSupplier.Name_of_article, addedArticle.Name);
		}

		[TestMethod]
		public void OrderAndSellArticle_ErrorNoArticle()
		{
			var _testedInstance = new ShopService(_articleRepository.Object, _supplierApiService.Object, _logger.Object);

			Assert.ThrowsException<Exception>(() => _testedInstance.OrderAndSellArticle(1, 20, 10));
		}

		[TestMethod]
		public void GetById_Sucess()
		{
			var articleId = 2;
			var createdArticle = TestArticles.Valid(articleId);

			_articleRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => createdArticle);

			var _testedInstance = new ShopService(_articleRepository.Object, _supplierApiService.Object, _logger.Object);

			var article = _testedInstance.GetById(articleId);

			Assert.IsNotNull(article);
			Assert.AreEqual(createdArticle.Name, article.Name);
			Assert.AreEqual(createdArticle.Price, article.Price);
			Assert.AreEqual(createdArticle.SupplierId, article.SupplierId);
		}

		[TestMethod]
		public void GetById_ErrorNoArticle()
		{
			Article existedArticle = null;

			_articleRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => existedArticle);

			var _testedInstance = new ShopService(_articleRepository.Object, _supplierApiService.Object, _logger.Object);

			var article = _testedInstance.GetById(55);

			Assert.IsNull(article);
		}

		[TestMethod]
		public void ShowArticleByExternalIdInConsole_Sucess()
		{
			using (StringWriter sw = new StringWriter())
			{
				Console.SetOut(sw);

				var articleId = 2;

				var createdArticle = TestArticles.Valid(articleId);

				_articleRepository.Setup(m => m.GetByExternalId(It.IsAny<int>())).Returns(() => createdArticle);

				var _testedInstance = new ShopService(_articleRepository.Object, _supplierApiService.Object, _logger.Object);

				_testedInstance.ShowArticleByExternalId(articleId);

				Assert.IsTrue(sw.ToString().Contains("Found article"));
			}
		}

		[TestMethod]
		public void ShowArticleByExternalIdInConsole_Error()
		{
			using (StringWriter sw = new StringWriter())
			{
				Console.SetOut(sw);

				Article createdArticle = null;

				_articleRepository.Setup(m => m.GetByExternalId(It.IsAny<int>())).Returns(() => createdArticle);

				var _testedInstance = new ShopService(_articleRepository.Object, _supplierApiService.Object, _logger.Object);

				_testedInstance.ShowArticleByExternalId(1);

				Assert.IsTrue(sw.ToString().Contains("not found"));
			}
		}

		private void SetupConsoleLogger()
		{
			_logger.Setup(m => m.Debug(It.IsAny<string>())).Callback((string message) => Console.WriteLine("Debug: " + message));
			_logger.Setup(m => m.Info(It.IsAny<string>())).Callback((string message) => Console.WriteLine("Info: " + message));
			_logger.Setup(m => m.Error(It.IsAny<string>())).Callback((string message) => Console.WriteLine("Error: " + message));
		}
	}
}


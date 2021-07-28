using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using TheShop.Models.Entities;
using TheShop.Models.RequestModels;
using TheShop.Repositories.Interfaces;

namespace TheShop.UnitTests
{
	[TestClass]
	public class ShopServiceTest
	{
		private Mock<IArticleRepository> _articleRepository;

		[TestInitialize]
		public void Initialize()
		{
			_articleRepository = new Mock<IArticleRepository>();
		}

		[TestMethod]
		public void OrderAndSellArticle_Sucess()
		{
			var articleFromSupplier = new ArticleModel
			{
				ID = 1,
				Name_of_article = "Article from supplier1",
				ArticlePrice = 458
			};

			Article addedArticle = null;

			_articleRepository.Setup(m => m.Create(It.IsAny<Article>())).Callback<Article>((a) =>
			{
				addedArticle = a;
			});

			var _testedInstance = new ShopService(_articleRepository.Object);

			_testedInstance.OrderAndSellArticle(1, 459, 10);

			Assert.IsTrue(addedArticle != null);
			Assert.IsTrue(addedArticle.BuyerUserId == 10);
			Assert.IsTrue(addedArticle.ExternalId == articleFromSupplier.ID);
			Assert.IsTrue(addedArticle.Price == articleFromSupplier.ArticlePrice);
			Assert.IsTrue(addedArticle.SupplierId == 1);
			Assert.IsTrue(addedArticle.Name == articleFromSupplier.Name_of_article);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void OrderAndSellArticle_ErrorNoArticle()
		{
			var _testedInstance = new ShopService(_articleRepository.Object);

			_testedInstance.OrderAndSellArticle(1, 20, 10);
		}

		[TestMethod]
		public void GetById_Sucess()
		{
			var createdArticle = new Article
			{
				Id = 2,
				ExternalId = 1,
				Name = "Article from supplier 2",
				Price = 459,
				IsSold = true,
				SoldDate = DateTime.Now,
				BuyerUserId = 10,
				SupplierId = 2
			};

			_articleRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => createdArticle);

			var _testedInstance = new ShopService(_articleRepository.Object);

			var article = _testedInstance.GetById(2);

			Assert.IsTrue(article != null);
			Assert.IsTrue(article.Name == createdArticle.Name);
			Assert.IsTrue(article.Price == createdArticle.Price);
			Assert.IsTrue(article.SupplierId == createdArticle.SupplierId);
		}

		[TestMethod]
		public void GetById_ErrorNoArticle()
		{
			Article existedArticle = null;

			_articleRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => existedArticle);

			var _testedInstance = new ShopService(_articleRepository.Object);

			var article = _testedInstance.GetById(55);

			Assert.IsTrue(article == null);
		}

		[TestMethod]
		public void ShowArticleByExternalIdInConsole_Sucess()
		{
			using (StringWriter sw = new StringWriter())
			{
				Console.SetOut(sw);

				var createdArticle = new Article
				{
					Id = 2,
					ExternalId = 1,
					Name = "Article from supplier 2",
					Price = 459,
					IsSold = true,
					SoldDate = DateTime.Now,
					BuyerUserId = 10,
					SupplierId = 2
				};

				_articleRepository.Setup(m => m.GetByExternalId(It.IsAny<int>())).Returns(() => createdArticle);

				var _testedInstance = new ShopService(_articleRepository.Object);

				_testedInstance.ShowArticleByExternalId(1);

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

				var _testedInstance = new ShopService(_articleRepository.Object);

				_testedInstance.ShowArticleByExternalId(1);

				Assert.IsTrue(sw.ToString().Contains("not found"));
			}
		}
	}
}


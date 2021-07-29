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
			var supplierId = 1;
			var buyerId = 10;
			var articleId = 1;
			var maxExpecgedPrice = 459;

			var articleFromSupplier = new ArticleModel
			{
				ID = articleId,
				Name_of_article = $"Article from supplier{supplierId}",
				ArticlePrice = maxExpecgedPrice - 1
			};

			Article addedArticle = null;

			_articleRepository.Setup(m => m.Create(It.IsAny<Article>())).Callback<Article>((a) =>
			{
				addedArticle = a;
			});

			var _testedInstance = new ShopService(_articleRepository.Object);

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
			var _testedInstance = new ShopService(_articleRepository.Object);

			Assert.ThrowsException<Exception>(() => _testedInstance.OrderAndSellArticle(1, 20, 10));
		}

		[TestMethod]
		public void GetById_Sucess()
		{
			var articleId = 2;

			var createdArticle = new Article
			{
				Id = articleId,
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

			var _testedInstance = new ShopService(_articleRepository.Object);

			var article = _testedInstance.GetById(55);

			Assert.IsNull(article);
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


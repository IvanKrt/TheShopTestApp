using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
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

			Assert.IsNotNull(addedArticle);
			Assert.AreEqual(10, addedArticle.BuyerUserId);
			Assert.AreEqual(articleFromSupplier.ID, addedArticle.ArticleCode);
			Assert.AreEqual(articleFromSupplier.ArticlePrice, addedArticle.Price);
			Assert.AreEqual(1, addedArticle.SupplierId);
			Assert.AreEqual(articleFromSupplier.Name_of_article, addedArticle.Name);
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
				ArticleCode = 1,
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
	}
}


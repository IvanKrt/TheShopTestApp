using System;
using TheShop.Models.Entities;
using TheShop.Models.RequestModels;
using TheShop.Repositories.Interfaces;
using TheShop.Services;

namespace TheShop
{
	public class ShopService : IShopService
	{
		private IArticleRepository _articleRepository;
		private Logger logger;

		private Supplier1Model Supplier1;
		private Supplier2Model Supplier2;
		private Supplier3Model Supplier3;

		public ShopService(IArticleRepository articleRepository)
		{
			_articleRepository = articleRepository;
			logger = new Logger();
			Supplier1 = new Supplier1Model();
			Supplier2 = new Supplier2Model();
			Supplier3 = new Supplier3Model();
		}

		public void OrderAndSellArticle(int externalId, int maxExpectedPrice, int buyerId)
		{
			var article = OrderArticle(externalId, maxExpectedPrice);
			if (article == null)
			{
				throw new Exception("Could not order article");
			}

			SellArticle(article, buyerId, externalId);
		}

		public Article GetById(int id)
		{
			return _articleRepository.GetById(id);
		}

		public void ShowArticleByExternalId(int externalId)
		{
			var articles = _articleRepository.GetByExternalId(externalId);

			if (articles == null)
			{
				logger.Info($"Article with external ID \"{externalId}\" not found");
			}
			else
			{
				logger.Info($"Found article with external ID: \"{externalId}\"");
			}
		}

		private Article OrderArticle(int externalId, int maxExpectedPrice)
		{
			Article article = null;
			Article tempArticle = null;
			var articleExists = Supplier1.ArticleInInventory(externalId);
			if (articleExists)
			{
				tempArticle = Supplier1.GetArticle(externalId).ConvertToArticle();
				if (maxExpectedPrice < tempArticle.Price)
				{
					articleExists = Supplier2.ArticleInInventory(externalId);
					if (articleExists)
					{
						tempArticle = Supplier2.GetArticle(externalId).ConvertToArticle();
						if (maxExpectedPrice < tempArticle.Price)
						{
							articleExists = Supplier3.ArticleInInventory(externalId);
							if (articleExists)
							{
								tempArticle = Supplier3.GetArticle(externalId).ConvertToArticle();
								if (maxExpectedPrice < tempArticle.Price)
								{
									article = tempArticle;
								}
							}
						}
					}
				}
			}

			article = tempArticle;

			return article;
		}

		private void SellArticle(Article article, int buyerId, int externalId)
		{
			logger.Debug("Trying to sell article with external Id=" + externalId);

			article.IsSold = true;
			article.SoldDate = DateTime.Now;
			article.BuyerUserId = buyerId;

			try
			{
				_articleRepository.Create(article);
				logger.Info("Article with external id=" + externalId + " is sold.");
			}
			catch (ArgumentNullException ex)
			{
				logger.Error("Could not save article with external id=" + externalId);
				throw new Exception("Could not save article with external id");
			}
			catch (Exception)
			{
			}
		}
	}

	public class Logger
	{
		public void Info(string message)
		{
			Console.WriteLine("Info: " + message);
		}

		public void Error(string message)
		{
			Console.WriteLine("Error: " + message);
		}

		public void Debug(string message)
		{
			Console.WriteLine("Debug: " + message);
		}
	}
}

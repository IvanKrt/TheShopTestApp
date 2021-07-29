using System;
using TheShop.Models.Entities;
using TheShop.Models.RequestModels;
using TheShop.Repositories.Interfaces;
using TheShop.Services;

namespace TheShop
{
	public class ShopService: IShopService
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

		public void OrderAndSellArticle(int id, int maxExpectedPrice, int buyerId)
		{
			#region ordering article

			Article article = null;
			Article tempArticle = null;
			var articleExists = Supplier1.ArticleInInventory(id);
			if (articleExists)
			{
				tempArticle = Supplier1.GetArticle(id).ConvertToArticle();
				if (maxExpectedPrice < tempArticle.Price)
				{
					articleExists = Supplier2.ArticleInInventory(id);
					if (articleExists)
					{
						tempArticle = Supplier2.GetArticle(id).ConvertToArticle();
						if (maxExpectedPrice < tempArticle.Price)
						{
							articleExists = Supplier3.ArticleInInventory(id);
							if (articleExists)
							{
								tempArticle = Supplier3.GetArticle(id).ConvertToArticle();
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
			#endregion

			#region selling article

			if (article == null)
			{
				throw new Exception("Could not order article");
			}

			logger.Debug("Trying to sell article with id=" + id);

			article.IsSold = true;
			article.SoldDate = DateTime.Now;
			article.BuyerUserId = buyerId;
			
			try
			{
				_articleRepository.Create(article);
				logger.Info("Article with id=" + id + " is sold.");
			}
			catch (ArgumentNullException ex)
			{
				logger.Error("Could not save article with id=" + id);
				throw new Exception("Could not save article with id");
			}
			catch (Exception)
			{
			}

			#endregion
		}

		public Article GetById(int id)
		{
			return _articleRepository.GetById(id);
		}

		public void ShowArticleByExternalId(int externalId)
		{
			var article = _articleRepository.GetByExternalId(externalId);

			if (article == null)
			{
				logger.Info($"Article with external ID \"{externalId}\" not found");
			}
			else
			{
				logger.Info($"Found article with external ID: \"{externalId}\"");
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

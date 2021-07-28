using System;
using System.Linq;
using TheShop.Models.Entities;
using TheShop.Repositories.Interfaces;
using TheShop.Services;
using TheShop.Services.Interfaces;

namespace TheShop
{
	public class ShopService : IShopService
	{
		private IArticleRepository _articleRepository;
		private Logger logger;

		private ISupplierApiService _supplierApiService;

		public ShopService(
			IArticleRepository articleRepository,
			ISupplierApiService supplierApiService
			)
		{
			_articleRepository = articleRepository;
			logger = new Logger();
			_supplierApiService = supplierApiService;
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
			var suppliers = _supplierApiService.GetSuppliers();

			var valuableArticleList = suppliers
				.SelectMany(_ => _.Articles)
				.Where(_ => _.ID == externalId && _.ArticlePrice <= maxExpectedPrice).ToList();

			if (valuableArticleList.Count == 0)
			{
				return null;
			}

			var article = valuableArticleList.FirstOrDefault().ConvertToArticle();

			if (article == null)
			{
				logger.Error("Article from supplyer has invalid data.");
				return null;
			}

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

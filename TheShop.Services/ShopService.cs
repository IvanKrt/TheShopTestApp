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
		private ILoggerService _logger;
		private ISupplierApiService _supplierApiService;

		public ShopService(
			IArticleRepository articleRepository,
			ISupplierApiService supplierApiService,
			ILoggerService logger
			)
		{
			_articleRepository = articleRepository;
			_logger = logger;
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
			var article = _articleRepository.GetByExternalId(externalId);

			if (article == null)
			{
				_logger.Info($"Article with external ID \"{externalId}\" not found");
			}
			else
			{
				_logger.Info($"Found article with external ID: \"{externalId}\"");
			}
		}

		private Article OrderArticle(int externalId, int maxExpectedPrice)
		{
			var suppliers = _supplierApiService.GetSuppliers();

			if (suppliers == null || suppliers.Count == 0)
			{
				return null;
			}

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
				_logger.Error("Article from supplier has invalid data.");
				return null;
			}

			return article;
		}

		private void SellArticle(Article article, int buyerId, int externalId)
		{
			_logger.Debug("Trying to sell article with external Id=" + externalId);

			article.IsSold = true;
			article.SoldDate = DateTime.Now;
			article.BuyerUserId = buyerId;

			_articleRepository.Create(article);
			_logger.Info("Article with external id=" + externalId + " is sold.");
		}
	}
}

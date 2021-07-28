using TheShop.Models.Entities;

namespace TheShop.Services
{
	public interface  IShopService
	{
		void OrderAndSellArticle(int externalId, int maxExpectedPrice, int buyerId);

		Article GetById(int id);

		void ShowArticleByExternalId(int externalId);
	}
}

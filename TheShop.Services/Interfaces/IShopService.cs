namespace TheShop.Services
{
	public interface  IShopService
	{
		void OrderAndSellArticle(int id, int maxExpectedPrice, int buyerId);

		Article GetById(int id);
	}
}

namespace TheShop.Models.RequestModels
{
	public class Supplier3Model
	{
		public bool ArticleInInventory(int id)
		{
			return true;
		}

		public ArticleModel GetArticle(int id)
		{
			return new ArticleModel()
			{
				ID = 1,
				Name_of_article = "Article from supplier3",
				ArticlePrice = 460
			};
		}
	}
}

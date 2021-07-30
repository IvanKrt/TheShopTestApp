using System;
using TheShop.Repositories;
using TheShop.Repositories.Context;
using TheShop.Services;

namespace TheShop
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			IShopService shopService = new ShopService(
				new ArticleRepository(new DBContext()),
				new SupplierApiService(),
				new ConsoleLoggerService()
				);

			try
			{
				shopService.OrderAndSellArticle(1, 20, 10);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			shopService.ShowArticleByExternalId(1);

			shopService.ShowArticleByExternalId(12);

			Console.ReadKey();
		}
	}
}
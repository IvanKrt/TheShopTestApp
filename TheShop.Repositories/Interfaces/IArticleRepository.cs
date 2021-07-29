using TheShop.Models.Entities;

namespace TheShop.Repositories.Interfaces
{
	public interface IArticleRepository : IBaseRepository<Article>
	{
		Article GetByExternalId(int externalId);
	}
}

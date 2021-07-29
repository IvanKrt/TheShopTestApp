using System.Collections.Generic;
using TheShop.Models.Base;
using TheShop.Repositories.Context;

namespace TheShop.Repositories.Interfaces
{
	public interface IBaseRepository<TEntity> where TEntity : BaseModel
	{
		IDBContext DBContext { get; set; }

		IEnumerable<TEntity> GetAll();

		TEntity GetById(int id);

		int Create(TEntity entity);

		bool Update(TEntity entity);

		void Remove(int id);
	}
}

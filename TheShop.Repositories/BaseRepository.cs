using System.Collections.Generic;
using TheShop.Models.Base;
using TheShop.Repositories.Context;
using TheShop.Repositories.Interfaces;

namespace TheShop.Repositories
{
	public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseModel
	{
		public BaseRepository(IDBContext context)
		{
			DBContext = context;
		}

		protected abstract int GetLastId();

		public IDBContext DBContext { get; set; }

		public abstract int Create(TEntity entity);

		public abstract IEnumerable<TEntity> GetAll();

		public abstract TEntity GetById(int id);

		public abstract void Remove(int id);

		public abstract bool Update(TEntity entity);
	}
}

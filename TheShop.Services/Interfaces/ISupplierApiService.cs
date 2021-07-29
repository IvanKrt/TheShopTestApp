using System.Collections.Generic;
using TheShop.Models.RequestModels;

namespace TheShop.Services.Interfaces
{
	public interface ISupplierApiService
	{
		List<SupplierModel> GetSuppliers();
	}
}

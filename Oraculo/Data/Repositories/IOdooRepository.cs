using Oraculo.Models;

namespace Oraculo.Data.Repositories
{
    public interface IOdooRepository
    {
        Task<List<OdooProductStockPrice>> GetProductsAsync(int environment);
    }
}
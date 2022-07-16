using StockTicker.Models;
namespace StockTicker.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Stock> Stock { get; }

        int Commit();
        Task<int> CommitAsync();
    }
}

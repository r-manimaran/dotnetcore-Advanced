
namespace BlazorAppHybridSearchQdrant.Services
{
    public interface IDataService
    {
        Task<List<string>> GetResortInfo(string query);
        Task LoadData(string filePath);
    }
}
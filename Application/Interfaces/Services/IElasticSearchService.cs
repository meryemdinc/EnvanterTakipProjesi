namespace Application.Interfaces.Services
{
    public interface IElasticSearchService
    {
        // Elastic'e yeni eşya ekler
        Task IndexDocumentAsync<T>(T document, string indexName) where T : class;

        // Elastic'te kelime araması yapar
        Task<List<T>> SearchAsync<T>(string keyword, string indexName) where T : class;
    }
}
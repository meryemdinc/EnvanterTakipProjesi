using Application.Interfaces.Services;
using Elastic.Clients.Elasticsearch;
// Fuzziness kullanımı için gerekebilir (Visual Studio önermezse silme)
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace Infrastructure.Services
{
    public class ElasticSearchService(ElasticsearchClient elasticClient) : IElasticSearchService
    {
        public async Task IndexDocumentAsync<T>(T document, string indexName) where T : class
        {
            // Veriyi ElasticSearch'e indeksle (kaydet)
            var response = await elasticClient.IndexAsync(document, idx => idx.Index(indexName));

            if (!response.IsValidResponse)
            {
                // Hata durumunda konsola yazdır
                Console.WriteLine($"ElasticSearch Index Hatası: {response.DebugInformation}");
            }
        }

        public async Task<List<T>> SearchAsync<T>(string keyword, string indexName) where T : class
        {
            // Eğer arama kelimesi boşsa boş liste dön
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<T>();

            // Full-Text Search (Tüm alanlarda arama yapar - v8 Uyumlu)
            var response = await elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Query(q => q
                    .MultiMatch(m => m
                        .Query(keyword)
                        // 🚀 DÜZELTİLEN KISIM (v8 Sürümüne Uygun):
                        .Fields(new[] { "itemCode", "brand", "model", "serialNumber" })
                        .Fuzziness(new Fuzziness("AUTO")) // Yazım hatalarını tolere et (Örn: Lnovo -> Lenovo)
                    )
                )
            );

            return response.IsValidResponse ? response.Documents.ToList() : new List<T>();
        }
    }
}
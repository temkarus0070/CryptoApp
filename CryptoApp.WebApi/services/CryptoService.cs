using CryptoApp.database;

namespace CryptoApp.WebApi.services
{
    public class CryptoService
    {
        private IHttpClientFactory _httpClientFactory;
        private CryptoAppDbContext _dbContext;
        public CryptoService(IHttpClientFactory httpClientFactory, CryptoAppDbContext cryptoAppDbContext)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = cryptoAppDbContext;
        }


        public async Task<IEnumerable<CryptoAsset>> CryptoAssets()
        {
            return _dbContext.CryptoAssets;
        }
    }
}

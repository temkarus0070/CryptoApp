using CryptoApp.database;
using CryptoApp.WebApi.models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading;

namespace CryptoApp.WebApi.services
{
    public class CryptoAssetsLoader : BackgroundService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private CryptoAppDbContext db;
        private ILogger<CryptoAssetsLoader> logger;

        public CryptoAssetsLoader(IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider,ILogger<CryptoAssetsLoader> logger
           )
        {
            this.httpClientFactory = httpClientFactory;
            db = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<CryptoAppDbContext>();
            this.logger = logger;
        }

        private async Task<IEnumerable<CryptoAsset>> GetSymbolDatas()
        {
            var client = httpClientFactory.CreateClient("binance");
            HttpRequestMessage httpRequest = new(HttpMethod.Get, "exchangeInfo");
            var a = await client.SendAsync(httpRequest).Result.Content.ReadFromJsonAsync<ExchangeInfo>();
            return a.Symbols.Select(asset =>
                        new CryptoAsset() { BaseAsset = asset.BaseAsset, Symbol = asset.Symbol, QuoteAsset = asset.QuoteAsset });

        }

        private async Task SaveAssetChanges(IEnumerable<CryptoAsset> cryptoAssets)
        {
            var newAssets = cryptoAssets.Except(db.CryptoAssets, new CryptoAssetComparer());
            if (newAssets is not null)
            {

                await db.CryptoAssets.AddRangeAsync(newAssets);
                await db.SaveChangesAsync();
            }
        }

  

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IEnumerable<CryptoAsset> data = await GetSymbolDatas();
            await SaveAssetChanges(data);

            using (PeriodicTimer periodicTimer = new(TimeSpan.FromHours(1)))
            {
                try
                {
                    while (await periodicTimer.WaitForNextTickAsync(stoppingToken))
                    {
                        data = await GetSymbolDatas();
                        await SaveAssetChanges(data);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    logger.LogError(ex.Message);
                }

            }
        }
    }
}
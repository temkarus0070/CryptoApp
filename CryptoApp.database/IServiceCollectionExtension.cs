using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.database
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddCryptoAppDbContext(this IServiceCollection collection, string connectionStr = "Host=localhost;Port=5432;Database=crypto_app;Username=postgres;Password=postgres")
        {
            collection.AddDbContext<CryptoAppDbContext>(options =>
            {
                options.UseNpgsql(connectionStr);
            });
         return   collection;
        }
    }
}

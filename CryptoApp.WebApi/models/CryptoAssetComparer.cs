using CryptoApp.database;

namespace CryptoApp.WebApi.models
{
    public class CryptoAssetComparer : IEqualityComparer<CryptoAsset>
    {
        bool IEqualityComparer<CryptoAsset>.Equals(CryptoAsset? x, CryptoAsset? y)
        {
            if (x != null && y != null)
            {
                return x.Equals(y);
            }
            else return x==y;
        }

        int IEqualityComparer<CryptoAsset>.GetHashCode(CryptoAsset obj)
        {
         return  HashCode.Combine(obj.BaseAsset,obj.QuoteAsset,obj.QuoteAsset);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.database
{
   public class CryptoAsset
    {
        [Key]
        public string Symbol { get; set; }


        public string BaseAsset { get; set; }

        public string QuoteAsset { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) throw new ArgumentNullException();
            if(ReferenceEquals(this, obj)) return true;
            if (obj.GetType() == this.GetType())
            {
                var obj1=obj as CryptoAsset;
                return obj1.QuoteAsset== QuoteAsset&&obj1.Symbol== Symbol&& obj1.BaseAsset==BaseAsset;
            }
            else
                return false;
        }
    }
}

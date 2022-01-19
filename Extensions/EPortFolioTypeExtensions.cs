using FreebirdTech.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Extensions
{
    public static class EPortFolioTypeExtensions
    {
        public static int ToInt(this EPortfolioType i) => (int)i;
        public static EPortfolioType ConvertToPortfolioType(this string s)
        {
            if (!Enum.TryParse(s, out EPortfolioType portfolioType)) return EPortfolioType.NONE;

            return portfolioType;
        }
    }
}

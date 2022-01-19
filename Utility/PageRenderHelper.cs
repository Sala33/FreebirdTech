using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreebirdTech.Utility
{
    public static class PageRenderHelper
    {
        public static string ValidateProfilePicture(string URI) => string.IsNullOrEmpty(URI) ? @"/img/profile-pic.jpg" : URI;
        public static string ValidateCoverImage(string URI) => string.IsNullOrEmpty(URI) ? "/img/img-Placeholder-Job.jpg" : URI;
        public static string GetPortfolioCapa() => "Imagem de Capa";

        public static string GetLiteralString(string s)
        {
            var normalizedString = s.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string GetDay(DateTime date)
        {
            return date.Date == DateTime.Today ? "Hoje" : string.Format("{0} dias atrás", (DateTime.Today - date.Date).TotalDays.ToString());
        }
    }
}

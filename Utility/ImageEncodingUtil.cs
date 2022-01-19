using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Utility
{
    public class ImageEncodingUtil
    {
        public static byte[] FromDataURL(string DataURL)
        {
            var base64URL = DataURL.Split(',')[1];

            return Convert.FromBase64String(base64URL);
        }
    }
}

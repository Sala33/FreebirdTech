using FreebirdTech.Models;
using FreebirdTech.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Extensions
{
    public enum EImageType { PORTFOLIO, AVATAR };
    public static class ImageExtensions
    {

        public static string DecodeImage(this Image image)
        {
            Image img = image;
            string imageBase64Data = Convert.ToBase64String(img.ImageData);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);

            return imageDataURL;
        }

        public static Image EncodeImage(this string URI, string name, EImageType imageType, Image image = null)
        {
            if (!string.IsNullOrEmpty(URI))
            {
                var imgByteArr = ImageEncodingUtil.FromDataURL(URI);
                if (imgByteArr.Length < 2097152)
                {
                    Image artistIMG;
                    if (image == null)
                        artistIMG = new Image();
                    else
                        artistIMG = image;

                    artistIMG.ImageData = imgByteArr;
                    artistIMG.ImageTitle = name + "-" + imageType.ToString()+".jpg";
                    return artistIMG;
                }
            }

            return null;
        }
    }
}

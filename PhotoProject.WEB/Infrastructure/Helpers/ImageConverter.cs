using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PhotoProject.WEB.Infrastructure.Helpers
{
    public static class ImageConverter
    {
        public static byte[] ConvertImage(HttpPostedFileBase upload)
        {
            byte[] resultImage = null;
            string[] ext = { ".jpg", ".png", ".gif", ".bmp", ".jpeg" };
            if (upload != null && upload.ContentLength > 0
                && ext.Contains(Path.GetExtension(upload.FileName).ToLower()))
            {
                using (var br = new BinaryReader(upload.InputStream))
                {
                    resultImage = br.ReadBytes(upload.ContentLength);
                }
            }
            else
            {
                resultImage = CreateDefaultAvatar();
            }
            return resultImage;
        }

        public static byte[] CreateDefaultAvatar()
        {
            byte[] resultImage = null;
            string defaultAvatarPath =
                  HttpContext.Current.Server.MapPath(@"~/Images/noImg.png");
            FileInfo fileInfo = new FileInfo(defaultAvatarPath);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(defaultAvatarPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            resultImage = br.ReadBytes((int)imageFileLength);
            return resultImage;
        }
    }
}
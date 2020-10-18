using MDLX_CORE.Models.Base;
using System.Drawing;
using System.Drawing.Imaging;

namespace MDLX_CORE.Models
{
    public class ImageManager
    {
        public ImageManager()
        {
        }

        public void SaveBigPhoto(Image img, Attachment photo)
        {
            Image resizedImg;
            string fname = Attachment.ConstructFullFilePath(photo, "B");
            //Path.Combine(HostingEnvironment.MapPath("~/Uploads/"), photo.Id + "-" + photo.ParentObjectId + "B.jpg");
            resizedImg = resizeImageKeepAcpectRatio(img, 1920);
            resizedImg.Save(fname, format: ImageFormat.Jpeg);
        }
        public void SaveMidPhoto(Image img, Attachment photo)
        {
            Image resizedImg;
            string fname = Attachment.ConstructFullFilePath(photo, "D");
            //Path.Combine(HostingEnvironment.MapPath("~/Uploads/"), photo.Id + "-" + photo.ParentObjectId + "D.jpg");
            resizedImg = resizeImageKeepAcpectRatio(img, 800);
            resizedImg.Save(fname, format: ImageFormat.Jpeg);
        }
        public void SaveMiniPhoto(Image img, Attachment photo)
        {
            Image resizedImg;
            string fname = Attachment.ConstructFullFilePath(photo, "M");
            //Path.Combine(HostingEnvironment.MapPath("~/Uploads/"), photo.Id + "-" + photo.ParentObjectId + "M.jpg");
            resizedImg = resizeImageToSquare(img, 100);
            resizedImg.Save(fname, format: ImageFormat.Jpeg);
        }

        public void DeleteBigPhoto(Attachment photo)
        {
            System.IO.File.Delete(Attachment.ConstructFullFilePath(photo, "B"));
        }
        public void DeleteMidPhoto(Attachment photo)
        {
            System.IO.File.Delete(Attachment.ConstructFullFilePath(photo, "D"));
        }
        public void DeleteMiniPhoto(Attachment photo)
        {
            System.IO.File.Delete(Attachment.ConstructFullFilePath(photo, "M"));
        }

        private Image resizeImageKeepAcpectRatio(Image imgPhoto, int newWidth)
        {
            
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            int newHeight = (int)((float)sourceHeight * ((float)newWidth / (float)sourceWidth));

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            return bmPhoto;
        }
        private Image resizeImageToSquare(Image imgPhoto, int newSize)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            
            int shorterSidePx = (sourceHeight > sourceWidth) ? sourceWidth : sourceHeight;
            bool isPortrait = (sourceHeight > sourceWidth);

            int destX = 0, destY = 0;
            int sourceX = (isPortrait) ? 0 : ((sourceWidth - shorterSidePx) / 2); 
            int sourceY = (isPortrait) ? ((sourceHeight - shorterSidePx) / 2) : 0;

            Bitmap bmPhoto = new Bitmap(newSize, newSize, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, newSize, newSize),
                new Rectangle(sourceX, sourceY, shorterSidePx, shorterSidePx),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            return bmPhoto;
        }
        private Image resizeImage(Image imgPhoto, int newWidth, int newHeight)
        {
            //Image imgPhoto = Image.FromFile(stPhotoPath);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            return bmPhoto;
        }
    }
}
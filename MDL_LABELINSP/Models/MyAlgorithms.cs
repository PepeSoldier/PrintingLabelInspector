using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_LABELINSP.Models
{
    public class MyAlgorithms
    {
        Image<Bgr, byte> img;

        public MyAlgorithms(Image<Bgr, byte> img)
        {
            this.img = img;
        }

        public Image<Gray, byte> MakeGray()
        {
            Image<Gray, byte> imageGray = new Image<Gray, byte>(img.Cols, img.Rows);

            for(int c = 0; c < img.Cols; c++)
            {
                for(int r = 0; r < img.Rows; r++)
                {
                    imageGray[r,c] = new Gray((img[r,c].Red + img[r, c].Green + img[r, c].Blue) / 3);
                }
            }

            return imageGray;
        }
        public Image<Gray, byte> Pixelize(Image<Gray, byte> sourceImg)
        {
            int pixels = 8;
            Image<Gray, byte> imageBlur = new Image<Gray, byte>(sourceImg.Cols, sourceImg.Rows);

            for (int r = 0; r < img.Rows; r+=pixels)
            {
                for (int c = 0; c < img.Cols; c+=pixels)
                {
                    double sum = 0;
                    for(int r2 = r; r2 < r + pixels && r2 < img.Rows; r2++)
                    {
                        for (int c2 = c; c2 < c + pixels && c2 < img.Cols; c2++)
                        {
                            sum += sourceImg[r2, c2].Intensity;
                        }
                    }

                    double avg = sum / (pixels * pixels);

                    for (int r2 = r; r2 < r + pixels && r2 < img.Rows; r2++)
                    {
                        for (int c2 = c; c2 < c + pixels && c2 < img.Cols; c2++)
                        {
                            imageBlur[r2, c2] = new Gray(avg);
                        }
                    }
                }
            }

            return imageBlur;
        }
        public Image<Gray, byte> Blur(Image<Gray, byte> sourceImg)
        {
            int pixels = 16;
            Image<Gray, byte> imageBlur = new Image<Gray, byte>(sourceImg.Cols, sourceImg.Rows);

            for (int r = 0; r < img.Rows; r += 1)
            {
                for (int c = 0; c < img.Cols; c += 1)
                {
                    double sum = 0;
                    for (int r2 = Math.Max(r-pixels/2, 0); r2 < r + pixels*0.5 && r2 < img.Rows; r2++)
                    {
                        for (int c2 = Math.Max(c-pixels/2,0); c2 < c + pixels*0.5 && c2 < img.Cols; c2++)
                        {
                            //int distanceX = Math.Abs(c2 - c);
                            //int distanceY = Math.Abs(r2 - r);

                            //5,10,4
                            //avg=19/3                  =~  6.33
                            //0.25*5 + 0.5*10 + 0.25*4
                            //1.2    + 5      + 1       =   7.2  

                            sum += sourceImg[r2, c2].Intensity;
                        }
                    }

                    double avg = sum / (pixels * pixels);

                    imageBlur[r, c] = new Gray(avg);    
                }
            }

            return imageBlur;
        }
        public Image<Gray, byte> Contours(Image<Gray, byte> sourceImg)
        {
            int pixels = 2;
            Image<Gray, byte> image = new Image<Gray, byte>(sourceImg.Cols, sourceImg.Rows);
            int[] sobelY = new int[9] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            int[] sobelX = new int[9] { -1, -2, -1, 0, 0, 0, 1, 2, 1 };


            for (int r = 0; r < img.Rows; r += 1)
            {
                for (int c = 0; c < img.Cols; c += 1)
                {
                    double gY = 0;
                    double gX = 0;
                    int s = 0;
                    for (int r2 = Math.Max(r - pixels / 2, 0); r2 <= r + pixels * 0.5 && r2 < img.Rows; r2++)
                    {
                        for (int c2 = Math.Max(c - pixels / 2, 0); c2 <= c + pixels * 0.5 && c2 < img.Cols; c2++)
                        {
                            gY += sourceImg[r2, c2].Intensity * sobelY[s];
                            gX += sourceImg[r2, c2].Intensity * sobelX[s];
                            s++;
                        }
                    }
                    double g = gY; //Math.Sqrt(Math.Pow(gY, 2) + Math.Pow(gX, 2));

                    //image[r, c] = new Gray(
                    //   Math.Min(240, Math.Max(0, 255 - g))
                    //);

                    image[r, c] = new Gray(
                       Math.Min(255, Math.Max(0, 255 - (g > 128? 255 : 0)))
                    );
                }
            }

            return image;
        }
    }
}

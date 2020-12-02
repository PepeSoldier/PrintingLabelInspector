using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Emgu.CV.Features2D.Features2DToolbox;

namespace MDL_LABELINSP.Models
{
    public class MyAlgorithms
    {
        Image<Bgr, byte> img;

        public MyAlgorithms(Image<Bgr, byte> img)
        {
            this.img = img;
        }
        public MyAlgorithms()
        {
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


            for (int r = 0; r < sourceImg.Rows; r += 1)
            {
                for (int c = 0; c < sourceImg.Cols; c += 1)
                {
                    double gY = 0;
                    double gX = 0;
                    int s = 0;
                    for (int r2 = Math.Max(r - pixels / 2, 0); r2 <= r + pixels * 0.5 && r2 < sourceImg.Rows; r2++)
                    {
                        for (int c2 = Math.Max(c - pixels / 2, 0); c2 <= c + pixels * 0.5 && c2 < sourceImg.Cols; c2++)
                        {
                            gY += sourceImg[r2, c2].Intensity * sobelY[s];
                            gX += sourceImg[r2, c2].Intensity * sobelX[s];
                            s++;
                        }
                    }
                    //double g = gY; //Math.Sqrt(Math.Pow(gY, 2) + Math.Pow(gX, 2));
                    double g = Math.Sqrt(Math.Pow(gY, 2) + Math.Pow(gX, 2));

                    //image[r, c] = new Gray(
                    //   Math.Min(240, Math.Max(0, 255 - g))
                    //);

                    image[r, c] = new Gray(
                       Math.Min(255, Math.Max(0, 255 - (g > 16? 255 : 0)))
                    );
                }
            }

            return image;
        }

        public void Match(Image<Gray, byte> sourceImg)
        {
            var shape = new Image<Bgr, byte>(@"C:\temp\IKEA\shape_1.png");
            var img = new Image<Bgr, byte>(@"C:\Temp\IKEA\20201107_190718.jpg");
            
            var bf = new BFMatcher(DistanceType.Hamming);
            //bf.KnnMatch(img, shape., 2);
        }

        public static void FindMatch(Mat modelImage, Mat observedImage, out long matchTime, out VectorOfKeyPoint modelKeyPoints, out VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, out Mat mask, out Mat homography)
        {
            int k = 2;
            double uniquenessThreshold = 0.80;

            Stopwatch watch;
            homography = null;

            modelKeyPoints = new VectorOfKeyPoint();
            observedKeyPoints = new VectorOfKeyPoint();

            using (UMat uModelImage = modelImage.GetUMat(AccessType.Read))
            using (UMat uObservedImage = observedImage.GetUMat(AccessType.Read))
            {
                KAZE featureDetector = new KAZE();

                //extract features from the object image
                Mat modelDescriptors = new Mat();
                featureDetector.DetectAndCompute(uModelImage, null, modelKeyPoints, modelDescriptors, false);

                watch = Stopwatch.StartNew();

                // extract features from the observed image
                Mat observedDescriptors = new Mat();
                featureDetector.DetectAndCompute(uObservedImage, null, observedKeyPoints, observedDescriptors, false);
                BFMatcher matcher = new BFMatcher(DistanceType.L2);

                matcher.Add(modelDescriptors);

                matcher.KnnMatch(observedDescriptors, matches, k, null);
                mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                mask.SetTo(new MCvScalar(255));
                Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);

                int nonZeroCount = CvInvoke.CountNonZero(mask);
                if (nonZeroCount >= 4)
                {
                    nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, matches, mask, 1.5, 20);

                    if (nonZeroCount >= 4)
                        homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, matches, mask, 2);
                }

                watch.Stop();

            }
            matchTime = watch.ElapsedMilliseconds;
        }

        //public bool Recognize(Image<Gray, Byte> observedImage, out PointF[] Region)
        //{
        //    // extract features from the observed image
        //    var observedKeyPoints = new VectorOfKeyPoint();
        //    Matrix<float> observedDescriptors = surfCPU.DetectAndCompute(observedImage, null, observedKeyPoints);
        //    BruteForceMatcher<float> matcher = new BruteForceMatcher<float>(DistanceType.L2);
        //    matcher.Add(modelDescriptors);
        //    indices = new Matrix<int>(observedDescriptors.Rows, k);
        //    using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Rows, k))
        //    {
        //        matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
        //        mask = new Matrix<byte>(dist.Rows, 1);
        //        mask.SetValue(255);
        //        Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
        //    }
        //    int nonZeroCount = CvInvoke.cvCountNonZero(mask);
        //    if (nonZeroCount >= requiredNonZeroCount)
        //    {
        //        nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, scaleIncrement, RotationBins);
        //        if (nonZeroCount >= requiredNonZeroCount)
        //            homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, ransacReprojThreshold);
        //    }
        //    bool ObjectFound;
        //    if (homography != null)
        //    {  //draw a rectangle along the projected model
        //        Rectangle rect = modelImage.ROI;
        //        Region = new PointF[] {
        //         new PointF(rect.Left, rect.Bottom),
        //         new PointF(rect.Right, rect.Bottom),
        //         new PointF(rect.Right, rect.Top),
        //         new PointF(rect.Left, rect.Top)};
        //        homography.ProjectPoints(Region);
        //        ObjectFound = true;
        //    }
        //    else
        //    {
        //        Region = null;
        //        ObjectFound = false;
        //    }
        //    return ObjectFound;
        //}

        //public static void DrawMatches(
        //    IInputArray modelImage, VectorOfKeyPoint modelKeypoints,
        //    IInputArray observerdImage, VectorOfKeyPoint observedKeyPoints,
        //    VectorOfVectorOfDMatch matches,
        //    IInputOutputArray result,
        //    MCvScalar matchColor, MCvScalar singlePointColor,
        //    IInputArray mask = null,
        //    KeypointDrawType flags = KeypointDrawType.Default)
        //{
        //    using (InputArray iaModelImage = modelImage.GetInputArray())
        //    using (InputArray iaObserverdImage = observerdImage.GetInputArray())
        //    using (InputOutputArray ioaResult = result.GetInputOutputArray())
        //    using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
        //        CvInvoke.drawMatchedFeatures(iaObserverdImage, observedKeyPoints, iaModelImage,
        //           modelKeypoints, matches, ioaResult, ref matchColor, ref singlePointColor, iaMask, flags);
        //}
    }
}

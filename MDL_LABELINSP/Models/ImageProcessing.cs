using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZXing;

namespace MDL_LABELINSP.Models
{
    public class ImageProcessing
    {
        int treschold = 64;
        int maxValue = 230;
        int a = 16;
        int b = 5;

        public int Treschold { set { treschold = value; } }
        public int MaxValue { set { maxValue = value; } }
        public int A { set { a = value; } }
        public int B { set { b = value; } }

        public string BarcodeBig { get; private set; }
        public string BarcodeSmall { get; private set; }
        public string IkeaProductCode { get; private set; }
        public string ProductName{ get; private set; }

        public Image<Bgr, byte> SourceImageFullSize { get; set; }
        public Image<Bgr, byte> SourceImage { get; set; }
        public Mat ProcessedImageStep1 { get; set; }
        public Mat ProcessedImageStep2 { get; set; }
        public Mat ProcessedImageStep3 { get; set; }
        public Mat ProcessedImageStep4 { get; set; }
        public Mat ProcessedImageStep5 { get; set; }
        public Mat ProcessedImageStep6 { get; set; }
        public Mat ExtractedImage { get; set; }
        public Mat FinalPreviewImage { get; set; }
        
        public ImageProcessing()
        {
            ProcessedImageStep1 = new Mat();
            ExtractedImage = new Mat();
        }

        public void SetImage(string path)
        {
            ClearDataAndImages();
            SourceImageFullSize = new Image<Bgr, byte>(path);
            SourceImage = ResizeImage(SourceImageFullSize.Mat, 50).ToImage<Bgr, byte>();
            //SourceImage = ResizeImage(SourceImage.Mat, 50).ToImage<Bgr, byte>();
            FinalPreviewImage = SourceImage.Copy().Mat;
        }
        public void SetImage(Image<Bgr, byte> img)
        {
            ClearDataAndImages();
            SourceImageFullSize = img;
            SourceImage = ResizeImage(SourceImageFullSize.Mat, 50).ToImage<Bgr, byte>();
            //SourceImage = ResizeImage(SourceImage.Mat, 50).ToImage<Bgr, byte>();
            FinalPreviewImage = SourceImage.Copy().Mat;
        }
        private void ClearDataAndImages()
        {
            BarcodeBig = "";
            BarcodeSmall = "";
            IkeaProductCode = "";
            ProductName = "";
            ProcessedImageStep1 = new Mat();
            ProcessedImageStep2 = new Mat();
            ProcessedImageStep3 = new Mat();
            ProcessedImageStep4 = new Mat();
            ProcessedImageStep5 = new Mat();
            ProcessedImageStep6 = new Mat();
            ExtractedImage = new Mat();
            FinalPreviewImage = new Mat();
        }

        public string BarcodeDetectReadAddFrame_Big(string expectedValue = "")
        {
            Rectangle rectBarcode = DetectBarcode(SourceImage, a, b);
            Image<Bgr, byte> imgCropped = CropImage(SourceImage, rectBarcode);
            string[] barcodeData = ReadBarcode_Auto(imgCropped.Mat);

            DrawFrame(FinalPreviewImage, rectBarcode, barcodeData[0], expectedValue, true);
            BarcodeBig = barcodeData[0];
            return BarcodeBig;
        }
        public string BarcodeDetectReadAddFrame_Small(string expectedValue = "")
        {
            //Algorytm nie radzi sobie z czytaniem pomniejszonego barcode'u, wiec przeszukanie barcode 
            //odbywa się na obrazku o bokach 50% oryginalnego, a sam odczyt dokonywany jest z dużego obrazka.
            Rectangle rectAreaWithSmallBarcode = DetectBarcode(CropImage(SourceImage,0,0,100,30), a, b);
            Rectangle rectForFullSizeImage = new Rectangle()
            {
                Height = rectAreaWithSmallBarcode.Height * 2,
                Width = rectAreaWithSmallBarcode.Width * 2,
                X = rectAreaWithSmallBarcode.X * 2 + 25,
                Y = rectAreaWithSmallBarcode.Y * 2,
            };
            
            Image<Bgr, byte> imgCropped = CropImage(SourceImageFullSize, rectForFullSizeImage);
            string[] barcodeData = ReadBarcode(imgCropped.Mat);

            DrawFrame(FinalPreviewImage, rectAreaWithSmallBarcode, barcodeData[0], expectedValue, true);
            BarcodeSmall = barcodeData[0];
            return BarcodeSmall;
        }
        public string ReadModelName(string expectedValue = "")
        {
            //Rectangle areaWithModelName = new Rectangle() { X = 270, Y = 45, Height = 70, Width = 310 };
            Rectangle rectAreaWithModelName = new Rectangle() { 
                X = (int)(13.82 * SourceImageFullSize.Width / 100), 
                Y = (int)(1.86 * SourceImageFullSize.Height / 100), 
                Height = (int)(2.9 * SourceImageFullSize.Height / 100), 
                Width = (int)(15.88 * SourceImageFullSize.Width / 100)
            };

            rectAreaWithModelName = _TrimRectangleToMat(SourceImageFullSize.Mat, rectAreaWithModelName);
            List<Rectangle> rectanglesWihtText = DetectText(rectAreaWithModelName);
            StringBuilder modelNameSB = new StringBuilder(); ;

            for (int i = rectanglesWihtText.Count - 1; i >= 0; i--)
            {
                Rectangle rectWithText = rectanglesWihtText[i];
                Rectangle rectWithTextAbsolute = rectAbsolutePosition(rectWithText, rectAreaWithModelName);

                var imgGrayCropped = CropImage(SourceImage, rectWithTextAbsolute).Convert<Gray, byte>();
                CvInvoke.Blur(imgGrayCropped, imgGrayCropped, new Size(2, 2), new Point(0, 0), BorderType.Default);
                //_RemoveVerticalLines(imgGrayCropped);
                modelNameSB.Append(OCR(imgGrayCropped.Convert<Gray, byte>(), "deu"));
                
                CvInvoke.Rectangle(FinalPreviewImage, rectWithTextAbsolute, new MCvScalar(255, 0, 255), 3);
            }

            //string modelName = modelNameSB.ToString();
            string modelName = Regex.Replace(modelNameSB.ToString(), "[^a-zA-Z0-9ÖÄ]", String.Empty);

            if (rectanglesWihtText.Count > 0)
            {
                DrawFrame(FinalPreviewImage, rectAbsolutePosition(rectanglesWihtText[0], rectAreaWithModelName), modelName, expectedValue);
            }
            ProductName = modelName;
            return ProductName;
        }
        public string ReadIKEAProductCode(string expectedValue = "")
        {
            //Rectangle areaWithProductCode = new Rectangle() { X = 270, Y = 200, Height = 80, Width = 310 };
            Rectangle areaWithProductCode = new Rectangle()
            {
                X = (int)(27.64 * SourceImage.Width / 100),
                Y = (int)(14.5 * SourceImage.Height / 100),
                Height = (int)(9.0 * SourceImage.Height / 100),
                Width = (int)(31.76 * SourceImage.Width / 100)
            };
            Image<Gray, byte> imgCroppedGray = CropImage(SourceImage, areaWithProductCode).Convert<Gray, byte>();
            Mat imgThreshold = new Mat();
            Mat imgBlur = new Mat();
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

            CvInvoke.Threshold(imgCroppedGray, imgThreshold, 0, 255, ThresholdType.BinaryInv | ThresholdType.Otsu);
            CvInvoke.Blur(imgThreshold, imgBlur, new Size(1, 1), new Point(0, 0), BorderType.Default);
            CvInvoke.FindContours(imgBlur, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
            
            Rectangle rectWithProductNumber = _FindLargestRectFromContours(contours);
            Image<Gray, byte> matCroppedGray = CropImage(imgBlur.ToImage<Gray, byte>(), rectWithProductNumber);

            string productCode = OCR(matCroppedGray, "eng");
            productCode = Regex.Replace(productCode, "[^0-9]", String.Empty);

            DrawFrame(FinalPreviewImage, rectAbsolutePosition(rectWithProductNumber, areaWithProductCode), productCode, expectedValue);

            ProcessedImageStep1 = imgCroppedGray.Mat;
            ProcessedImageStep2 = imgThreshold;
            ProcessedImageStep3 = imgBlur;
            ProcessedImageStep4 = matCroppedGray.Mat;
            ProcessedImageStep5 = new Mat();
            ProcessedImageStep6 = new Mat();
            ExtractedImage = matCroppedGray.Mat;

            IkeaProductCode = productCode;
            return IkeaProductCode;
        }

        public Image<Bgr, byte> CropImage(Image<Bgr, byte> frame, int xPercent, int yPercent, int widthPercent, int heightPercent)
        {
            Image<Bgr, byte> img = frame.Copy();

            Rectangle roi = new Rectangle(
                 (int)(xPercent * img.Width / 100)
                , (int)(yPercent * img.Height / 100)
                , (int)(widthPercent * img.Width / 100)
                , (int)(heightPercent * img.Height / 100)
            );

            Mat src = img.Mat;
            Mat dst = new Mat(src.Height, src.Width, DepthType.Cv8U, 3);
            Mat srcROI = new Mat(src, roi);
            Mat dstROI = new Mat(dst, roi);

            srcROI.CopyTo(dst);

            return dst.ToImage<Bgr, byte>();
        }
        public Image<Bgr, byte> CropImage(Image<Bgr, byte> frame, Rectangle roi)
        {
            Image<Bgr, byte> img = frame.Copy();

            Mat src = img.Mat;
            Mat dst = new Mat(src.Height, src.Width, DepthType.Cv8U, 3);

            roi = _TrimRectangleToMat(src, roi);

            Mat srcROI = new Mat(src, roi);
            Mat dstROI = new Mat(dst, roi);

            srcROI.CopyTo(dst);

            return dst.ToImage<Bgr, byte>();
        }
        public Image<Gray, byte> CropImage(Image<Gray, byte> frame, Rectangle roi)
        {
            Image<Gray, byte> img = frame.Copy();

            Mat src = img.Mat;
            Mat dst = new Mat(src.Height, src.Width, DepthType.Cv8U, 3);

            roi = _TrimRectangleToMat(src, roi);

            Mat srcROI = new Mat(src, roi);
            Mat dstROI = new Mat(dst, roi);

            srcROI.CopyTo(dst);

            return dst.ToImage<Gray, byte>();
        }
        public Mat ResizeImage(Mat frame, int percent)
        {
            Mat mat = new Mat();
            CvInvoke.Resize(frame, mat, new Size(frame.Width * percent / 100, frame.Height * percent / 100), interpolation: Inter.Lanczos4);
            return mat;
        }
        public Mat ResizeImage(Mat frame, int maxWidth, int maxHeight)
        {
            int percentW = maxWidth * 100 / frame.Width;
            int percentH = maxHeight * 100 / frame.Height;

            return ResizeImage(frame, Math.Min(percentH, percentW));
        }

        public List<Rectangle> DetectText(Rectangle rect)
        {
            //img = cv2.imread(file_name)
            //Rectangle rect = new Rectangle() { X = 270, Y = 45, Height = 70, Width = 310 };
            //Image<Bgr, byte> img = Image.em Image<Bgr, byte>();
            Image<Bgr, byte> img = SourceImage; //.Convert<Gray, byte>();

            if (!rect.IsEmpty)
            {
                img = CropImage(SourceImage, rect);
            }

            ProcessedImageStep1 = img.Mat;

            //img_final = cv2.imread(file_name)
            //var img_final = CropImage(SourceImage, rect).Convert<Gray, byte>();
            //img2gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
            var img2gray = img.Convert<Gray, byte>();
            _RemoveVerticalLines(img2gray);

            Mat mask = new Mat();
            CvInvoke.Threshold(img2gray, mask, 180, 255, ThresholdType.Binary);
            Mat image_final = new Mat();
            CvInvoke.BitwiseAnd(img2gray, img2gray, image_final, mask);
            Mat new_img = new Mat();
            CvInvoke.Threshold(image_final, new_img, 180, 255, ThresholdType.BinaryInv);
            Mat kernel = new Mat();
            CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(12, 12), new Point(-1, -1));
            Mat dilated = new Mat();
            //DetectionImage = kernel;
            CvInvoke.Dilate(new_img, dilated, kernel, new Point(0, 0), 4, BorderType.Default, new MCvScalar(0, 0, 0));
            
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(dilated, contours, null, RetrType.External, ChainApproxMethod.ChainApproxNone);

            List<Rectangle> rectangles = new List<Rectangle>();

            for (int i = 0; i < contours.Size; i++)
            {
                Rectangle br = CvInvoke.BoundingRectangle(contours[i]);
                if (br.Width < 35 || br.Width > 500 || br.Height > 300) continue;

                //CvInvoke.Rectangle(img, br, new MCvScalar(255, 0, 255), 3);
                rectangles.Add(br);
            }

            ProcessedImageStep2 = img2gray.Mat;
            ProcessedImageStep3 = image_final;
            ProcessedImageStep4 = new_img;
            ProcessedImageStep5 = dilated;

            ExtractedImage = img.Mat;

            return rectangles;
        }
        public Rectangle DetectBarcode(Image<Bgr, byte> capturedFrame, int a = 21, int b = 7)
        {
            Mat grayscaleFrame = new Mat();
            Mat gradX = new Mat();
            Mat gradY = new Mat();
            Mat absGradX = new Mat();
            Mat absGradY = new Mat();
            Mat fullGrad = new Mat();
            Mat bluredFrame = new Mat();
            Mat thresholdFrame = new Mat();
            //Mat verticalRectangle = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(16, 11), new Point(1, 1));
            //Mat horizontalRectangle = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(3, 6), new Point(1, 1));
            Mat verticalRectangle = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(Math.Max(a, 1), Math.Max(b, 1)), new Point(1, 1));
            Mat horizontalRectangle = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(6, 6), new Point(1, 1));

            //Convert to grayscale
            CvInvoke.CvtColor(capturedFrame, grayscaleFrame, ColorConversion.Bgr2Gray);
            // Gradient X AND Y
            CvInvoke.Sobel(grayscaleFrame, gradX, Emgu.CV.CvEnum.DepthType.Cv8U, 1, 0);
            CvInvoke.Sobel(grayscaleFrame, gradY, Emgu.CV.CvEnum.DepthType.Cv8U, 0, 1);
            //Abs values of gradient
            CvInvoke.ConvertScaleAbs(gradX, absGradX, (double)2, (double)0);
            CvInvoke.ConvertScaleAbs(gradY, absGradY, (double)2, (double)0);
            //Detection of vertical lines
            CvInvoke.Subtract(absGradX, absGradY, fullGrad);
            //Blur
            CvInvoke.Blur(fullGrad, bluredFrame, new Size(9, 9), new Point(-1, -1));
            //Binarization            
            CvInvoke.Threshold(bluredFrame, thresholdFrame, 80, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
            // Closure
            CvInvoke.MorphologyEx(thresholdFrame, thresholdFrame, Emgu.CV.CvEnum.MorphOp.Close, verticalRectangle, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Constant, new MCvScalar((double)0));
            //Erosion
            CvInvoke.MorphologyEx(thresholdFrame, thresholdFrame, Emgu.CV.CvEnum.MorphOp.Erode, horizontalRectangle, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Constant, new MCvScalar((double)0));

            ProcessedImageStep1 = gradX;
            ProcessedImageStep2 = gradY;
            ProcessedImageStep3 = fullGrad;
            ProcessedImageStep4 = bluredFrame;
            ProcessedImageStep5 = thresholdFrame;

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(thresholdFrame, contours, null, Emgu.CV.CvEnum.RetrType.List, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxNone);

            Rectangle r = _FindLargestRectFromContours(contours);
            Rectangle newrectangle = new Rectangle((int)(r.X - r.Width / 8), (int)(r.Y - r.Height / 8), (int)(r.Width * 1.25m), (int)(r.Height * 1.25m));
            return newrectangle;
        }
        public Rectangle DrawFrame(Mat capturedFrame, Rectangle rect, string value, string expectedValue, bool text = false)
        {
            bool? isValueCorrect = expectedValue.Length > 0 ? (bool?)(value == expectedValue) : null;
            MCvScalar colorB = new MCvScalar(0, 0, 255); //red is default
            MCvScalar colorF = new MCvScalar(0, 0, 128); //red is default

            rect = _TrimRectangleToMat(capturedFrame, rect);

            if (isValueCorrect == null || isValueCorrect.HasValue == false)
            {
                colorB = new MCvScalar(245, 135, 66);
                colorF = new MCvScalar(122, 67, 33);
            }
            else if (isValueCorrect.HasValue == true && isValueCorrect.Value == true)
            {
                colorB = new MCvScalar(0, 255, 0);
                colorF = new MCvScalar(0, 128, 0);
            }
            

            CvInvoke.Rectangle(capturedFrame, rect, colorB, 3);

            if (text)
            {
                CvInvoke.Rectangle(FinalPreviewImage, new Rectangle() { X = rect.X + 5, Y = rect.Y - 10, Height = 17, Width = (int)(value.Length * 15.5) }, colorB, 15);
                //CvInvoke.PutText(PreviewImage, value, new Point(rect.X + 3, rect.Y + 3), FontFace.HersheyDuplex, 0.75, new MCvScalar(90, 90, 90), 2, LineType.Filled);
                //CvInvoke.PutText(PreviewImage, value, new Point(rect.X + 7, rect.Y + 7), FontFace.HersheyDuplex, 0.75, new MCvScalar(90, 90, 90), 2, LineType.Filled);
                CvInvoke.PutText(FinalPreviewImage, value, new Point(rect.X + 5, rect.Y + 5), FontFace.HersheyDuplex, 0.75, colorF, 2, LineType.Filled);
            }
            return rect;
        }
        public Rectangle DrawGreenFrame(Mat capturedFrame, Rectangle rect)
        {
            rect = _TrimRectangleToMat(capturedFrame, rect);
            CvInvoke.Rectangle(capturedFrame, rect, new MCvScalar(0.0, 255.0, 0.0), 3);
            return rect;
        }
        public Rectangle DrawRedFrame(Mat capturedFrame, Rectangle rect)
        {
            rect = _TrimRectangleToMat(capturedFrame, rect);
            CvInvoke.Rectangle(capturedFrame, rect, new MCvScalar(0.0, 0.0, 255.0), 3);
            return rect;
        }

        public string[] ReadBarcode_Auto(Mat frame)
        {
            string[] data = new string[2] { "NoRead", "" };
            IBarcodeReader reader = new BarcodeReader();
            var grayFrame = frame.ToImage<Gray, byte>();//frame.Convert<Gray, byte>();

            reader.Options.TryHarder = true;
            reader.Options.PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.CODE_39, BarcodeFormat.CODE_128, BarcodeFormat.EAN_8, BarcodeFormat.EAN_13, BarcodeFormat.ITF };
            reader.Options.UseCode39ExtendedMode = true;
            reader.Options.UseCode39RelaxedExtendedMode = true;

            //BarcodeReadImage = null;

            int i = 64;
            int try_ = 0;
            while (i < 257)
            {
                grayFrame = frame.ToImage<Gray, byte>();
                grayFrame = grayFrame.ThresholdBinary(new Gray(i), new Gray(maxValue));
                var result = reader.Decode(grayFrame.ToBitmap());
                if (result != null)
                {
                    data[0] = result.Text;
                    data[1] = result.BarcodeFormat.ToString();
                    ExtractedImage = grayFrame.Mat;
                    i = 999;
                }
                i += 16;
                try_++;
            }

            if (i != 999)
            {
                grayFrame = frame.ToImage<Gray, byte>();
                grayFrame = grayFrame.ThresholdBinary(new Gray(128), new Gray(255));
                ExtractedImage = grayFrame.Mat;
            }

            return data;
        }
        public string[] ReadBarcode(Mat frame)
        {
            string[] data = new string[2] { "NoRead", "" };
            IBarcodeReader reader = new BarcodeReader();
            var grayFrame = frame.ToImage<Gray, byte>(); //.Convert<Gray, byte>();
            grayFrame = grayFrame.ThresholdBinary(new Gray(treschold), new Gray(maxValue));

            reader.Options.TryHarder = true;
            reader.Options.PossibleFormats = new List<BarcodeFormat> {BarcodeFormat.ITF, BarcodeFormat.CODE_128, BarcodeFormat.AZTEC };
            
            //reader.Options.PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.CODE_39, BarcodeFormat.CODE_128, BarcodeFormat.EAN_8, BarcodeFormat.EAN_13 };
            //reader.Options.UseCode39ExtendedMode = true;
            //reader.Options.UseCode39RelaxedExtendedMode = true;

            reader.Options.AllowedLengths = new int[14];
            reader.Options.PureBarcode = true;
            reader.Options.Hints.Add(DecodeHintType.TRY_HARDER_WITHOUT_ROTATION, Boolean.TrueString);// zXing

            var result = reader.Decode(grayFrame.ToBitmap());
            if (result != null)
            {
                data[0] = result.Text;
                data[1] = result.BarcodeFormat.ToString();
            }

            //var bs = new BarcodeTypeSelector();
            //bs.ITF14 = true;

            //Bytescout.BarCodeReader.Reader reader1 = new Bytescout.BarCodeReader.Reader();
            //reader1.BarcodeTypesToFind = bs;
            //FoundBarcode[] r = reader1.ReadFrom(grayFrame.ToBitmap());

            //if (r != null && r.Length > 0)
            //{
            //    data[0] = r[0].Value;
            //    data[1] = "";
            //}

            ExtractedImage = grayFrame.Mat;
            //pictureBox3.Image = grayFrame.ToBitmap();

            return data;
        }
        
        public string OCR(Image<Gray, byte> temp, string lang = "eng")
        {
            ExtractedImage = temp.Mat;
            var OCRz = new Tesseract(@"./tessdata", lang, OcrEngineMode.Default);
            //var OCRz = new Tesseract();
            OCRz.SetImage(temp);
            OCRz.Recognize();
            string text = OCRz.GetUTF8Text();
            //var rr = OCRz.GetCharacters();
            return text;
        }
        public void DetectShapes(Image<Bgr, byte> imgInput, Image<Gray, byte> temp)
        {
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat m = new Mat();
            CvInvoke.FindContours(temp, contours, m, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            for (int i = 0; i < contours.Size; i++)
            {
                double perimeter = CvInvoke.ArcLength(contours[i], true);
                VectorOfPoint approx = new VectorOfPoint();
                CvInvoke.ApproxPolyDP(contours[i], approx, 0.04 * perimeter, true);
                CvInvoke.DrawContours(imgInput, contours, i, new MCvScalar(0, 255, 0), 2);
                //pictureBox2.Image = imgInput.ToBitmap();
                //PutDescriptions(imgInput, contours, i, approx);
            }
        }
        public void PutDescriptions(Image<Bgr, byte> imgInput, VectorOfVectorOfPoint contours, int i, VectorOfPoint approx)
        {
            var moments = CvInvoke.Moments(contours[i]);
            int x = (int)(moments.M10 / moments.M00);
            int y = (int)(moments.M01 / moments.M00);

            if (approx.Size == 3)
            {
                CvInvoke.PutText(imgInput, "Triangle", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
            }
            if (approx.Size == 4)
            {
                Rectangle rect = CvInvoke.BoundingRectangle(contours[i]);

                if (Math.Abs(1 - (rect.Width / rect.Height)) < 0.06)
                    CvInvoke.PutText(imgInput, "Square", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                else
                    CvInvoke.PutText(imgInput, "Rectangle", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
            }
            if (approx.Size == 5)
            {
                CvInvoke.PutText(imgInput, "Pentagon", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
            }
        }

        //Helpers
        private static void _RemoveVerticalLines(Image<Gray, byte> imageGray)
        {
            Mat tresh = new Mat();
            CvInvoke.Threshold(imageGray, tresh, 0, 255, ThresholdType.BinaryInv | ThresholdType.Otsu);
            Mat horizontal_kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(1, 60), new Point(-1, -1));
            Mat detected_lines = new Mat();
            CvInvoke.MorphologyEx(tresh, detected_lines, MorphOp.Open, horizontal_kernel, new Point(-1, -1), 2, BorderType.Default, new MCvScalar(0));

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(detected_lines, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            for (int i = 0; i < contours.Size; i++)
            {
                CvInvoke.DrawContours(imageGray, contours, i, new MCvScalar(255, 255, 255), thickness: 1);
            }
        }
        private static Rectangle _FindLargestRectFromContours(VectorOfVectorOfPoint contours)
        {
            //IF CONTOURS WERE FOUND ON THE IMAGE,
            //OBTAIN THE BIGGEST CONTOUR
            double largestArea = 0;
            int largestAreaIndex = 0;
            for (int i = 0; i < contours.Size; i++)
            {
                double A = CvInvoke.ContourArea(contours[i]);
                if (A > largestArea)
                {
                    largestArea = A;
                    largestAreaIndex = i;
                }
            }
            RotatedRect r_rect = CvInvoke.MinAreaRect(contours[largestAreaIndex]);
            PointF[] vertixles = r_rect.GetVertices();
            int x = (int)vertixles[1].X;
            int y = (int)vertixles[2].Y;
            //OBTAIN RECTANGLE THAT SURROUNDS THE DETECTED CONTOUR
            int width = (int)((vertixles[3].X) - (vertixles[1].X));
            int height = (int)((vertixles[0].Y) - (vertixles[2].Y));

            Rectangle rectangle = new Rectangle(x, y, width, height);
            return rectangle;
        }
        private Rectangle _TrimRectangleToMat(Mat src, Rectangle roi)
        {
            if (roi.X < 0) roi.X = 0;
            if (roi.Y < 0) roi.Y = 0;

            int exceedWidth = (roi.X + roi.Width) - src.Width;
            int exceedHeight = (roi.Y + roi.Height) - src.Height;

            if (exceedWidth > 0)
            {
                int realExceed = Math.Max(exceedWidth - roi.X, 0);
                roi.X -= exceedWidth - realExceed;
                roi.Width -= realExceed + 1;
            }
            if (exceedHeight > 0)
            {
                int realExceed = Math.Max(exceedHeight - roi.Y, 0);
                roi.Y -= exceedHeight - realExceed;
                roi.Height -= realExceed + 1;
            }

            if (roi.Height < 1) roi.Height = 1;
            if (roi.Width < 1) roi.Width = 1;

            if (roi.Y > src.Height) roi.Y = src.Height - 1;
            if (roi.X > src.Width) roi.X = src.Width - 1;

            return roi;
        }
        private Rectangle rectAbsolutePosition(Rectangle rectToDraw, Rectangle rectContainer)
        {
            Rectangle br = new Rectangle() { X = rectContainer.X + rectToDraw.X, Y = rectContainer.Y + rectToDraw.Y, Width = rectToDraw.Width, Height = rectToDraw.Height, };
            return br;
        }
    }
}


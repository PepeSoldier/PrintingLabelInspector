using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using MDL_LABELINSP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace _LABELINSP_FORMAPP
{
    public partial class Form1 : Form
    {
        int treschold = 64;
        int maxValue = 230;
        int A = 16;
        int B = 5;
        private delegate void _delegate(Bitmap image);
        _delegate processImageDelegate;
        ImageProcessing ip = null;
        Image<Bgr, byte> image;

        public Form1()
        {
            InitializeComponent();
            trackBar1.Value = treschold;
            trackBar2.Value = maxValue;
            //tbImageName.Text = @"\IKEA\911535086.png";
            tbImageName.Text = @"\IKEA\20201106160822_01536495020045254784_Label_F.png";
            processImageDelegate = ProcessCapturedImage;
            ip = new ImageProcessing();
            //_delegate = SetImage(Bitmap b);
            //Process();   
            //Test();
            //image = new Image<Bgr, byte>(@"C:\Temp\" + tbImageName.Text);
            ProcessLoadedImage();
        }

        private void ProcessLoadedImage(int command = 0)
        {
            Process(@"C:\Temp\" + tbImageName.Text, command);
        }
        private void ProcessCapturedImage(Bitmap image)
        {
            ProcessLive(image);
        }
        private void Process(string imagePath, int command = 0) 
        {
            ip.SetImage(imagePath);

            string expectedB = "2409110790362103412345";
            string expectedS = "30385789215529";
            string expectedN = "HJÄLPSAM";
            string expectedP = "30385789";
            string expectedW = "35";

            switch (command)
            {
                case 1: ip.BarcodeDetectReadAddFrame_Big(expectedB); break;
                case 2: ip.BarcodeDetectReadAddFrame_Small(expectedS); break;
                case 3: ip.ReadModelName(expectedN); break;
                case 4: ip.ReadIKEAProductCode(expectedP); break;
                case 5: ip.ReadWeightBig(expectedW); break;
                default:
                    //ip.BarcodeDetectReadAddFrame_Big(expectedB);
                    ip.BarcodeDetectReadAddFrame_Small(expectedS);
                    //ip.ReadModelName(expectedN);
                    //ip.ReadIKEAProductCode(expectedP);
                    //ip.ReadWeightBig(expectedW);
                    break;
            }

            ip.SaveFinalPreviewImage(@"C:\Temp\IKEA\detection.jpg");
            
            DisplayProcessedImages(ip);
            richTextBox1.Text = "";
            richTextBox1.Text += ip.BarcodeBig + System.Environment.NewLine;
            richTextBox1.Text += ip.BarcodeSmall + System.Environment.NewLine;
            richTextBox1.Text += ip.ProductName + System.Environment.NewLine;
            richTextBox1.Text += ip.IkeaProductCode + System.Environment.NewLine;
        }
        private void ProcessLive(Bitmap image)
        {
            ip.SetImage(image);

            string expectedB = "";
            ip.BarcodeDetectReadAddFrame_Big(expectedB);
            
            DisplayProcessedImages(ip);
        }

        private void StartCameraCapture()
        {
            var capture = new CameraCapture(0, processImageDelegate, 600, 300, 250);
            capture.Start();
        }
        private void Test()
        {
            var SourceImage = new Image<Bgr, byte>(@"C:\Temp\bc2.jpg");
            MyAlgorithms myA = new MyAlgorithms(SourceImage);
            Image<Gray,byte> imageGray = myA.MakeGray();

            //pictureBox1.Image = SourceImage.ToBitmap();
            pbSourceImage.Image = SourceImage.ToBitmap();
            pbPrcessedImageStep1.Image = myA.Pixelize(myA.Contours(imageGray)).ToBitmap();
            
        }

        public void DisplayProcessedImages(ImageProcessing ip)
        {
            try
            {
                DisplayImg(pbSourceImage, ip.SourceImage.Mat);
                DisplayImg(pbPrcessedImageStep1, ip.ProcessedImageStep1);
                DisplayImg(pbPrcessedImageStep2, ip.ProcessedImageStep2);
                DisplayImg(pbPrcessedImageStep3, ip.ProcessedImageStep3);
                DisplayImg(pbPrcessedImageStep4, ip.ProcessedImageStep4);
                DisplayImg(pbPrcessedImageStep5, ip.ProcessedImageStep5);
                DisplayImg(pbPrcessedImageStep6, ip.ProcessedImageStep6);
                DisplayImg(pbExtractedImage, ip.ExtractedImage);
                DisplayImg(pbFinalPreviewImage, ip.FinalPreviewImage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public void DisplayImg(PictureBox pictureBox, Mat image)
        {
            if (image != null && !image.IsEmpty)
            {
                pictureBox.Image = ip.ResizeImage(image, pictureBox.Width, pictureBox.Height).ToBitmap();
            }
            else
            {
                pictureBox.Image = null;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            treschold = trackBar1.Value;
            ip.Treschold = treschold;
            lbTeschold.Text = treschold.ToString();
            ProcessLoadedImage();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            maxValue = trackBar2.Value;
            ip.MaxValue = maxValue;
            lbMaxValue.Text = maxValue.ToString();
            ProcessLoadedImage();
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            A = trackBar3.Value;
            ip.A = A;
            lbA.Text = A.ToString();
            ProcessLoadedImage();
        }
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            B = trackBar4.Value;
            ip.B = B;
            lbB.Text = B.ToString();
            ProcessLoadedImage();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            ProcessLoadedImage(1);
        }
        private void btnCamera_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(StartCameraCapture);
            t.Start();
            //StartCameraCapture();
        }
        private void btnOCR_Click(object sender, EventArgs e)
        {
            ProcessLoadedImage(3);
        }
        private void btnProdCode_Click(object sender, EventArgs e)
        {
            ProcessLoadedImage(4);
        }
        private void btnBarcodeSmall_Click(object sender, EventArgs e)
        {
            ProcessLoadedImage(2);
        }

        private void btnWeightBig_Click(object sender, EventArgs e)
        {
            ProcessLoadedImage(5);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //tbImageName.Text = @"\IKEA\20201107_190718.jpg";
            //string imagePath = @"C:\Temp\" + tbImageName.Text;
            //string imagePath = @"C:\temp\IKEA\sim.png";
            //ip.SetImage(imagePath);

            //var img = new Image<Bgr, byte>(@"C:\Temp\IKEA\sim.png");
            var img1 = new Image<Bgr, byte>(@"C:\Temp\IKEA\20201107_190718.jpg");
            var img = new MyAlgorithms().Contours(img1.Convert<Gray, byte>());

            //img.ToBitmap().Save(@"C:\Temp\IKEA\Shape__.png", ImageFormat.Png);

            var shape = new Image<Bgr, byte>(@"C:\temp\IKEA\shape_2.png");
            VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();

            Mat res = new Mat();
            CvInvoke.MatchTemplate(img, shape, res, TemplateMatchingType.CcoeffNormed);

            var img2 = res.ToImage<Gray, byte>();
            var data = res.GetData();
            double[] max = new double[3]{ 0, 0, 0 };

            for (int r = 0; r < res.Rows; r += 1)
            {
                for (int c = 0; c < res.Cols; c += 1)
                {
                    if(Convert.ToDouble(data.GetValue(r,c)) > max[2])
                    {
                        max[0] = r;
                        max[1] = c;
                        max[2] = Convert.ToDouble(data.GetValue(r, c));
                    }
                }
            }

            ip.DrawGreenFrame(img.Mat, new Rectangle(new Point((int)max[1],(int)max[0]), new Size(shape.Width, shape.Height)), 1);
            
            


            //MyAlgorithms.FindMatch(shape.Mat, ip.SourceImage.Mat, out long matchTime, out VectorOfKeyPoint modelKeyPoints, out VectorOfKeyPoint observedKeyPoints, matches, out Mat mask, out Mat homography);


            //List<MDMatch[]> good = new List<MDMatch[]>();
            //List<MKeyPoint> points1 = new List<MKeyPoint>();

            //int idx = 0;
            //foreach (MDMatch[] m in matches.ToArrayOfArray())
            //{
            //    if (m[0].Distance < 0.85 * m[1].Distance)
            //    {
            //        good.Add(m);
            //        points1.Add(observedKeyPoints[idx]);
            //    }
            //    idx++;
            //}

            //foreach (MKeyPoint p in points1)
            //{
            //    ip.DrawGreenFrame(ip.SourceImage.Mat, new Rectangle(new Point((int)p.Point.X, (int)p.Point.Y), new Size(shape.Width, shape.Height)), 1);
            //    ip.DrawGreenFrame(ip.SourceImage.Mat, new Rectangle(new Point((int)p.Point.X, (int)p.Point.Y), new Size(shape.Width, shape.Height)), 1);
            //}

            DisplayImg(pbSourceImage, img.Mat);
            DisplayImg(pbFinalPreviewImage, img.Mat);
        }


    }
}

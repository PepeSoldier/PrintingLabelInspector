using Emgu.CV;
using Emgu.CV.CvEnum;
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
            tbImageName.Text = @"\IKEA\911076044.png";
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
    }
}

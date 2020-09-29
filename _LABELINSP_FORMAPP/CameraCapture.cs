using System;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace _LABELINSP_FORMAPP
{
    public class CameraCapture
    {
        private readonly PictureBox _imageBox;
        private readonly Emgu.CV.VideoCapture _camera;
        private bool _enable;
        private Thread _refreshThread;
        private Mat _frameImage;
        private readonly MethodInvoker _refreshMethodInvoker;
        private readonly MethodInvoker _invoker;
        private Delegate _setImage;
        public int Interval { set; get; }

        public CameraCapture(int index, Delegate setImage, int width, int height, int interval = 10)
        {
            _frameImage = new Mat();
            Interval = interval;
            _setImage = setImage;
            _camera = new Emgu.CV.VideoCapture(index);
            //_camera.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, width);
            //_camera.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, height);
            _refreshMethodInvoker = Refresh;
            _refreshThread = new Thread(CallBack);

        }

        public CameraCapture(int index, PictureBox imageBox, int width, int height, int interval = 10)
        {
            _frameImage = new Mat();
            Interval = interval;
            _imageBox = imageBox;
            _camera = new Emgu.CV.VideoCapture(index);
            //_camera.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, width);
            //_camera.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, height);
            _refreshMethodInvoker = Refresh;
            _refreshThread = new Thread(CallBack);

        }

        public void Refresh()
        {
            _imageBox.Image = _frameImage.ToBitmap();   
        }

        public void CallBack()
        {
            while (_enable)
            {
                 _enable = _camera.Read(_frameImage);

                if (_setImage != null)
                {
                    try
                    {
                        _setImage.DynamicInvoke(_frameImage.ToBitmap());
                    }
                    catch { }
                }
                else
                {
                    if (!_imageBox.InvokeRequired)
                    {
                        _imageBox.Image = _frameImage.ToBitmap();
                    }
                    else
                    {
                        _imageBox.Invoke(_refreshMethodInvoker);
                    }
                }
                Thread.Sleep(Interval);
            }
        }

        public void Start()
        {

            if (_enable)
            {
                return;
            }
            _enable = true;
            _refreshThread = new Thread(CallBack);
            _refreshThread.Start();

        }

        public void Stop(bool force)
        {

            _enable = false;
            if (force)
            {
                _refreshThread.Abort();
            }

        }
    }
}
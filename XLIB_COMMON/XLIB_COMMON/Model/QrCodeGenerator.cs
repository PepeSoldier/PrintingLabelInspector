using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using static QRCoder.PayloadGenerator;

namespace XLIB_COMMON.Model
{
    public class QrCodeGenerator
    {
        public static string GenerateSVG(string text)
        {
            //PayloadGenerator.WiFi wifiPayload = new PayloadGenerator.WiFi("MyWiFi-SSID", "MyWiFi-Pass", PayloadGenerator.WiFi.Authentication.WPA);
            //string payload = wifiPayload.ToString();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            SvgQRCode qrCode = new SvgQRCode(qrCodeData);
            string qrCodeAsSvg = qrCode.GetGraphic(2, "#000000", "#FFFFFF", true, SvgQRCode.SizingMode.ViewBoxAttribute);
            //string qrCodeAsSvg = qrCode.GetGraphic(new Size(50, 50), true, SvgQRCode.SizingMode.ViewBoxAttribute);
            
            return qrCodeAsSvg;
        }

        //public static Bitmap GenerateBitmap(string text)
        //{
        //    //var qr = QrCode.EncodeText(text, QrCode.Ecc.Medium);
        //    //return qr.ToBitmap(2,2);
        //}

    }
}
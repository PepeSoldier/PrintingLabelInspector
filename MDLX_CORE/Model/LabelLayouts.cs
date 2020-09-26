using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using XLIB_COMMON.Model;

namespace MDLX_CORE.Model
{
    public class LabelLayouts
    {
        private LabelData labelData;

        public LabelLayouts(LabelData labelData)
        {
            this.labelData = labelData;
        }

        public string GetLayout(int layoutNo)
        {
            switch (layoutNo)
            {
                case 1: return Layout1_CAB_ELUX();
                case 2: return Layout2_ZEBRA_GH();
                case 3: return Layout3_CAB_ELUX_QTY();
                case 4: return PLB_ZEBRA_CUMULATIVE();
                case 5: return PLB_ZEBRA_QTY_REPRINT();
                default: return Layout();
            }
        }

        private string PLB_ZEBRA_CUMULATIVE()
        {
            StringBuilder labelText = new StringBuilder();
            labelText.Clear();
            return labelText.ToString();
        }

        private string PLB_ZEBRA_QTY_REPRINT()
        {
            StringBuilder labelText = new StringBuilder();
            labelText.Clear();

            labelText.Append("^XA");
            labelText.Append("^CFA,60");
            labelText.Append("^FO 10,50^FDILOSC^FS");
            labelText.Append("^CF0,100");
            labelText.Append("^FO 10,150^FD" + labelData.Qty + "^FS");
            labelText.Append("^CFA,60");
            labelText.Append("^FO 10,280^FD Data^FS");
            labelText.Append("^CFA,50^FO10,380^FD" + labelData.PrintDateTime +"^FS");
            labelText.Append("^FO80,440^FD" + labelData.PrintTime + "^FS");
            labelText.Append("^FO40,525^BQN5,2,270");
            labelText.Append("^FD" + labelData.PrintDateTime + "&" + labelData.Code + "&" + labelData.PrintDateTime + " " + labelData.PrintTime);
            labelText.Append("&"+ labelData.Qty + "&" + labelData.SerialNumber + "^FS"); 
            labelText.Append("^XZ");

            return labelText.ToString();
        }

        private string Layout()
        {
            return "Please select layout";
        }
        private string Layout1_CAB_ELUX()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("m m"); //;definicja rozmiaru i orientacji
            sb.Append(System.Environment.NewLine);
            sb.Append("J");
            sb.Append(System.Environment.NewLine);
            sb.Append("O R");
            sb.Append(System.Environment.NewLine);
            sb.Append("D -1,0");
            sb.Append(System.Environment.NewLine);
            sb.Append("H 100,0,T");
            sb.Append(System.Environment.NewLine);
            sb.Append("S l1;0,0,25,28,60");
            sb.Append(System.Environment.NewLine);
            //; dzielimy wynik
            sb.Append("T:RES1;25,5,0,5,pt11;" + labelData.SerialNumber + "[I]");   //Serial
            sb.Append(System.Environment.NewLine);
            sb.Append("T:RES2;25,10,0,5,pt11;" + labelData.MachineNumber + "[I]");        //Linia
            sb.Append(System.Environment.NewLine);
            sb.Append("T:RES3;25,15,0,5,pt11;" + labelData.PrintDateTime + "[I]"); //Data
            sb.Append(System.Environment.NewLine);
            //;przygotowujemy pola do wyswietlania
            //; ZMIENNA RESA - numer seyjny uzupelniany 0 do 10 znakow
            sb.Append("T:RESA;25,5,0,5,pt11;[+:0,RES1][C:0][D:10,0][I]");
            sb.Append(System.Environment.NewLine);
            //; ZMIENNA RESB -numer linii uzupelniany 0 do 2 znakow
            sb.Append("T:RESB;25,10,0,5,pt11;[+:0,RES2][C:0][D:2,0][I]");
            sb.Append(System.Environment.NewLine);
            //;ZMIENNA RESD - skladamy w jeden string date, godzine,nr zbiornika,nr seryjny
            sb.Append("T:RESD;5,20,0,5,pt8;" + labelData.Code + "[RES2][RESA][RES3][I]");
            sb.Append(System.Environment.NewLine);
            //;WYDRUK - kod datamatrix
            sb.Append("B 3,2,0,DATAMATRIX,1;[RESD]");
            sb.Append(System.Environment.NewLine);
            //;ZMIENNA RESY - nr linii
            sb.Append("T:RESY;30,10,0,5,pt11;[RESB][I]");
            sb.Append(System.Environment.NewLine);

            //;WYDRUK - drukujemy stale i zmienne
            sb.Append("T 25,5,0,3,pt9;[J:c24]DATA:");
            sb.Append(System.Environment.NewLine);
            sb.Append("T 25,9,0,3,pt9;[J:c24][RES3]");
            sb.Append(System.Environment.NewLine);
            sb.Append("G 25,11,0;L:24,0.3");
            sb.Append(System.Environment.NewLine);
            sb.Append("T 25,15,0,5,pt9;[J:c24]NR L: [RESB]");
            sb.Append(System.Environment.NewLine);
            sb.Append("T 25,19,0,5,pt9;[J:c24]SN: [RES1]");
            sb.Append(System.Environment.NewLine);
            sb.Append("G 51,23,90;L:21,0.3");
            sb.Append(System.Environment.NewLine);
            sb.Append("T 58,23,90,3,pt20;[J:c21]" + (labelData.Code.Length >= 3? labelData.Code.Substring(labelData.Code.Length - 3, 3) : labelData.Code));
            sb.Append(System.Environment.NewLine);
            //;ZMIENNA RESX - nowy nr seryjny - poprzedni +1
            sb.Append("T:RESX;30,5,0,5,pt11;[+:RES1,1][D:0,0][I]");
            sb.Append(System.Environment.NewLine);
            sb.Append("A 1");
            sb.Append(System.Environment.NewLine);

            //printLabelModel.Label = sb.ToString();
            return sb.ToString();
        }
        private string Layout2_ZEBRA_GH()
        {
            StringBuilder labelText = new StringBuilder();
            labelText.Clear();

            labelText.Append("^XA");

            labelText.Append("^A2N,40,20,B:CYRI_UB.FNT");
            labelText.Append("^FO 10,30");
            labelText.Append("^FD" + labelData.ClientName + "^FS");

            labelText.Append("^A2N,25,15,B:CYRI_UB.FNT");
            labelText.Append("^FO 730,30");
            labelText.Append("^FD 1/2 ^FS");

            labelText.Append("^A2N,15,15,B:CYRI_UB.FNT");
            labelText.Append("^FO 10,70");
            labelText.Append("^FD Date of production:" + labelData.PrintDateTime + "^FS");

            labelText.Append("^A2N,15,15");
            labelText.Append("^FO 10,130");
            labelText.Append("^FD Order no.:" + labelData.OrderNo + "^FS");

            labelText.Append("^A2N,40,25");
            labelText.Append("^FO 0,180");
            labelText.Append("^FD" + labelData.SerialNumber + "^FS");

            labelText.Append("^FO 30,230^BY4");
            labelText.Append("^B3N,N,50,N,N");
            labelText.Append("^FD" + labelData.Barcode + "^FS");

            labelText.Append("^XZ");

            return labelText.ToString();
        }
        private string Layout3_CAB_ELUX_QTY()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("m m"); //;definicja rozmiaru i orientacji
            sb.Append(System.Environment.NewLine);
            sb.Append("J");
            sb.Append(System.Environment.NewLine);
            sb.Append("O R");
            sb.Append(System.Environment.NewLine);
            sb.Append("D -1,0");
            sb.Append(System.Environment.NewLine);
            sb.Append("H 100,0,T");
            sb.Append(System.Environment.NewLine);
            sb.Append("S l1;0,0,25,28,60");
            sb.Append(System.Environment.NewLine);
            //; dzielimy wynik
            sb.Append("T:RES1;25,5,0,5,pt11;" + labelData.SerialNumber + "[I]");   //Serial
            sb.Append(System.Environment.NewLine);
            sb.Append("T:RES2;25,10,0,5,pt11;" + labelData.MachineNumber + "[I]");        //Linia
            sb.Append(System.Environment.NewLine);
            sb.Append("T:RES3;25,15,0,5,pt11;" + labelData.PrintDateTime + "[I]"); //Data
            sb.Append(System.Environment.NewLine);
            sb.Append("T:RES4;25,20,0,5,pt11;" + labelData.Qty + "[I]"); //Qty
            sb.Append(System.Environment.NewLine);
            //;przygotowujemy pola do wyswietlania
            //; ZMIENNA RESA - numer seyjny uzupelniany 0 do 10 znakow
            sb.Append("T:RESA;25,5,0,5,pt11;[+:0,RES1][C:0][D:10,0][I]");
            sb.Append(System.Environment.NewLine);
            //; ZMIENNA RESB -numer linii uzupelniany 0 do 2 znakow
            sb.Append("T:RESB;25,10,0,5,pt11;[+:0,RES2][C:0][D:2,0][I]");
            sb.Append(System.Environment.NewLine);
            //;ZMIENNA RESD - skladamy w jeden string date, godzine,nr zbiornika,nr seryjny
            sb.Append("T:RESD;5,20,0,5,pt8;" + labelData.Code + "[RES2][RESA][RES3][I]");
            sb.Append(System.Environment.NewLine);
            //;WYDRUK - kod datamatrix
            sb.Append("B 3,2,0,DATAMATRIX,1;[RESD]");
            sb.Append(System.Environment.NewLine);
            //;ZMIENNA RESY - nr linii
            sb.Append("T:RESY;30,10,0,5,pt11;[RESB][I]");
            sb.Append(System.Environment.NewLine);

            //;WYDRUK - drukujemy stale i zmienne
            sb.Append("T 25,5,0,3,pt9;[J:c24]DATA:");
            sb.Append(System.Environment.NewLine);
            sb.Append("T 25,9,0,3,pt9;[J:c24][RES3]");
            sb.Append(System.Environment.NewLine);
            sb.Append("G 25,11,0;L:24,0.3");
            sb.Append(System.Environment.NewLine);
            sb.Append("T 25,14,0,5,pt9;[J:c24]NR L: [RESB]");
            sb.Append(System.Environment.NewLine);
            sb.Append("T 25,17,0,5,pt9;[J:c24]SN: [RES1]");
            sb.Append(System.Environment.NewLine);
            sb.Append("T 25,21,0,5,pt9;[J:c24]SZT: [RES4]");
            sb.Append(System.Environment.NewLine);
            sb.Append("G 51,23,90;L:21,0.3");
            sb.Append(System.Environment.NewLine);
            sb.Append("T 58,23,90,3,pt20;[J:c21]" + (labelData.Code.Length >= 3 ? labelData.Code.Substring(labelData.Code.Length - 3, 3) : labelData.Code));
            sb.Append(System.Environment.NewLine);
            //;ZMIENNA RESX - nowy nr seryjny - poprzedni +1
            sb.Append("T:RESX;30,5,0,5,pt11;[+:RES1,1][D:0,0][I]");
            sb.Append(System.Environment.NewLine);
            sb.Append("A 1");
            sb.Append(System.Environment.NewLine);

            //printLabelModel.Label = sb.ToString();
            return sb.ToString();
        }
    }
}
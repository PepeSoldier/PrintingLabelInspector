namespace _LABELINSP_FORMAPP
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbSourceImage = new System.Windows.Forms.PictureBox();
            this.pbPrcessedImageStep1 = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbTeschold = new System.Windows.Forms.Label();
            this.lbMaxValue = new System.Windows.Forms.Label();
            this.pbExtractedImage = new System.Windows.Forms.PictureBox();
            this.trackBar4 = new System.Windows.Forms.TrackBar();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.lbB = new System.Windows.Forms.Label();
            this.lbA = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbImageName = new System.Windows.Forms.TextBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOCR = new System.Windows.Forms.Button();
            this.btnProdCode = new System.Windows.Forms.Button();
            this.pbPrcessedImageStep2 = new System.Windows.Forms.PictureBox();
            this.pbPrcessedImageStep3 = new System.Windows.Forms.PictureBox();
            this.pbPrcessedImageStep4 = new System.Windows.Forms.PictureBox();
            this.pbFinalPreviewImage = new System.Windows.Forms.PictureBox();
            this.pbPrcessedImageStep5 = new System.Windows.Forms.PictureBox();
            this.pbPrcessedImageStep6 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnBarcodeSmall = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbSourceImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrcessedImageStep1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExtractedImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrcessedImageStep2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrcessedImageStep3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrcessedImageStep4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFinalPreviewImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrcessedImageStep5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrcessedImageStep6)).BeginInit();
            this.SuspendLayout();
            // 
            // pbSourceImage
            // 
            this.pbSourceImage.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pbSourceImage.Location = new System.Drawing.Point(12, 101);
            this.pbSourceImage.Name = "pbSourceImage";
            this.pbSourceImage.Size = new System.Drawing.Size(490, 600);
            this.pbSourceImage.TabIndex = 0;
            this.pbSourceImage.TabStop = false;
            // 
            // pbPrcessedImageStep1
            // 
            this.pbPrcessedImageStep1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pbPrcessedImageStep1.Location = new System.Drawing.Point(12, 731);
            this.pbPrcessedImageStep1.Name = "pbPrcessedImageStep1";
            this.pbPrcessedImageStep1.Size = new System.Drawing.Size(245, 300);
            this.pbPrcessedImageStep1.TabIndex = 1;
            this.pbPrcessedImageStep1.TabStop = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(520, 525);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(490, 176);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 8;
            this.trackBar1.Location = new System.Drawing.Point(41, 42);
            this.trackBar1.Maximum = 255;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(189, 45);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // trackBar2
            // 
            this.trackBar2.LargeChange = 8;
            this.trackBar2.Location = new System.Drawing.Point(252, 42);
            this.trackBar2.Maximum = 255;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(186, 45);
            this.trackBar2.TabIndex = 5;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Treschold";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "MaxValue";
            // 
            // lbTeschold
            // 
            this.lbTeschold.AutoSize = true;
            this.lbTeschold.Location = new System.Drawing.Point(160, 13);
            this.lbTeschold.Name = "lbTeschold";
            this.lbTeschold.Size = new System.Drawing.Size(13, 13);
            this.lbTeschold.TabIndex = 8;
            this.lbTeschold.Text = "?";
            // 
            // lbMaxValue
            // 
            this.lbMaxValue.AutoSize = true;
            this.lbMaxValue.Location = new System.Drawing.Point(389, 13);
            this.lbMaxValue.Name = "lbMaxValue";
            this.lbMaxValue.Size = new System.Drawing.Size(13, 13);
            this.lbMaxValue.TabIndex = 9;
            this.lbMaxValue.Text = "?";
            // 
            // pbExtractedImage
            // 
            this.pbExtractedImage.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pbExtractedImage.Location = new System.Drawing.Point(520, 101);
            this.pbExtractedImage.Name = "pbExtractedImage";
            this.pbExtractedImage.Size = new System.Drawing.Size(490, 418);
            this.pbExtractedImage.TabIndex = 10;
            this.pbExtractedImage.TabStop = false;
            // 
            // trackBar4
            // 
            this.trackBar4.LargeChange = 8;
            this.trackBar4.Location = new System.Drawing.Point(667, 42);
            this.trackBar4.Maximum = 255;
            this.trackBar4.Name = "trackBar4";
            this.trackBar4.Size = new System.Drawing.Size(186, 45);
            this.trackBar4.TabIndex = 12;
            this.trackBar4.Scroll += new System.EventHandler(this.trackBar4_Scroll);
            // 
            // trackBar3
            // 
            this.trackBar3.LargeChange = 8;
            this.trackBar3.Location = new System.Drawing.Point(456, 42);
            this.trackBar3.Maximum = 255;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new System.Drawing.Size(189, 45);
            this.trackBar3.TabIndex = 11;
            this.trackBar3.Scroll += new System.EventHandler(this.trackBar3_Scroll);
            // 
            // lbB
            // 
            this.lbB.AutoSize = true;
            this.lbB.Location = new System.Drawing.Point(810, 13);
            this.lbB.Name = "lbB";
            this.lbB.Size = new System.Drawing.Size(13, 13);
            this.lbB.TabIndex = 16;
            this.lbB.Text = "?";
            // 
            // lbA
            // 
            this.lbA.AutoSize = true;
            this.lbA.Location = new System.Drawing.Point(581, 13);
            this.lbA.Name = "lbA";
            this.lbA.Size = new System.Drawing.Size(13, 13);
            this.lbA.TabIndex = 15;
            this.lbA.Text = "?";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(682, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "B";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(462, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "A";
            // 
            // tbImageName
            // 
            this.tbImageName.Location = new System.Drawing.Point(910, 13);
            this.tbImageName.Name = "tbImageName";
            this.tbImageName.Size = new System.Drawing.Size(285, 20);
            this.tbImageName.TabIndex = 17;
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(910, 48);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(97, 23);
            this.btnProcess.TabIndex = 18;
            this.btnProcess.Text = "Read Barcode";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1355, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Camera";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // btnOCR
            // 
            this.btnOCR.Location = new System.Drawing.Point(1142, 48);
            this.btnOCR.Name = "btnOCR";
            this.btnOCR.Size = new System.Drawing.Size(75, 23);
            this.btnOCR.TabIndex = 20;
            this.btnOCR.Text = "Prod. Name";
            this.btnOCR.UseVisualStyleBackColor = true;
            this.btnOCR.Click += new System.EventHandler(this.btnOCR_Click);
            // 
            // btnProdCode
            // 
            this.btnProdCode.Location = new System.Drawing.Point(1223, 48);
            this.btnProdCode.Name = "btnProdCode";
            this.btnProdCode.Size = new System.Drawing.Size(75, 23);
            this.btnProdCode.TabIndex = 21;
            this.btnProdCode.Text = "Prod.Code";
            this.btnProdCode.UseVisualStyleBackColor = true;
            this.btnProdCode.Click += new System.EventHandler(this.btnProdCode_Click);
            // 
            // pbPrcessedImageStep2
            // 
            this.pbPrcessedImageStep2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pbPrcessedImageStep2.Location = new System.Drawing.Point(263, 731);
            this.pbPrcessedImageStep2.Name = "pbPrcessedImageStep2";
            this.pbPrcessedImageStep2.Size = new System.Drawing.Size(245, 300);
            this.pbPrcessedImageStep2.TabIndex = 22;
            this.pbPrcessedImageStep2.TabStop = false;
            // 
            // pbPrcessedImageStep3
            // 
            this.pbPrcessedImageStep3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pbPrcessedImageStep3.Location = new System.Drawing.Point(514, 731);
            this.pbPrcessedImageStep3.Name = "pbPrcessedImageStep3";
            this.pbPrcessedImageStep3.Size = new System.Drawing.Size(245, 300);
            this.pbPrcessedImageStep3.TabIndex = 23;
            this.pbPrcessedImageStep3.TabStop = false;
            // 
            // pbPrcessedImageStep4
            // 
            this.pbPrcessedImageStep4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pbPrcessedImageStep4.Location = new System.Drawing.Point(765, 731);
            this.pbPrcessedImageStep4.Name = "pbPrcessedImageStep4";
            this.pbPrcessedImageStep4.Size = new System.Drawing.Size(245, 300);
            this.pbPrcessedImageStep4.TabIndex = 24;
            this.pbPrcessedImageStep4.TabStop = false;
            // 
            // pbFinalPreviewImage
            // 
            this.pbFinalPreviewImage.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pbFinalPreviewImage.Location = new System.Drawing.Point(1027, 101);
            this.pbFinalPreviewImage.Name = "pbFinalPreviewImage";
            this.pbFinalPreviewImage.Size = new System.Drawing.Size(490, 600);
            this.pbFinalPreviewImage.TabIndex = 25;
            this.pbFinalPreviewImage.TabStop = false;
            // 
            // pbPrcessedImageStep5
            // 
            this.pbPrcessedImageStep5.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pbPrcessedImageStep5.Location = new System.Drawing.Point(1016, 731);
            this.pbPrcessedImageStep5.Name = "pbPrcessedImageStep5";
            this.pbPrcessedImageStep5.Size = new System.Drawing.Size(245, 300);
            this.pbPrcessedImageStep5.TabIndex = 26;
            this.pbPrcessedImageStep5.TabStop = false;
            // 
            // pbPrcessedImageStep6
            // 
            this.pbPrcessedImageStep6.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pbPrcessedImageStep6.Location = new System.Drawing.Point(1272, 731);
            this.pbPrcessedImageStep6.Name = "pbPrcessedImageStep6";
            this.pbPrcessedImageStep6.Size = new System.Drawing.Size(245, 300);
            this.pbPrcessedImageStep6.TabIndex = 27;
            this.pbPrcessedImageStep6.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Source Image";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(520, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Extracted Image";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1029, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Final Preview Image";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 715);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(123, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Processed Image Step 1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(261, 715);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(123, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "Processed Image Step 2";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(517, 715);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(123, 13);
            this.label10.TabIndex = 33;
            this.label10.Text = "Processed Image Step 3";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(762, 715);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(123, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "Processed Image Step 4";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1013, 715);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(123, 13);
            this.label12.TabIndex = 35;
            this.label12.Text = "Processed Image Step 5";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1269, 715);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(123, 13);
            this.label13.TabIndex = 36;
            this.label13.Text = "Processed Image Step 6";
            // 
            // btnBarcodeSmall
            // 
            this.btnBarcodeSmall.Location = new System.Drawing.Point(1013, 48);
            this.btnBarcodeSmall.Name = "btnBarcodeSmall";
            this.btnBarcodeSmall.Size = new System.Drawing.Size(123, 23);
            this.btnBarcodeSmall.TabIndex = 37;
            this.btnBarcodeSmall.Text = "Read Barcode Small";
            this.btnBarcodeSmall.UseVisualStyleBackColor = true;
            this.btnBarcodeSmall.Click += new System.EventHandler(this.btnBarcodeSmall_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1534, 1048);
            this.Controls.Add(this.btnBarcodeSmall);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pbPrcessedImageStep6);
            this.Controls.Add(this.pbPrcessedImageStep5);
            this.Controls.Add(this.pbFinalPreviewImage);
            this.Controls.Add(this.pbPrcessedImageStep4);
            this.Controls.Add(this.pbPrcessedImageStep3);
            this.Controls.Add(this.pbPrcessedImageStep2);
            this.Controls.Add(this.btnProdCode);
            this.Controls.Add(this.btnOCR);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.tbImageName);
            this.Controls.Add(this.lbB);
            this.Controls.Add(this.lbA);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.trackBar4);
            this.Controls.Add(this.trackBar3);
            this.Controls.Add(this.pbExtractedImage);
            this.Controls.Add(this.lbMaxValue);
            this.Controls.Add(this.lbTeschold);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.pbPrcessedImageStep1);
            this.Controls.Add(this.pbSourceImage);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pbSourceImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrcessedImageStep1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExtractedImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrcessedImageStep2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrcessedImageStep3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrcessedImageStep4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFinalPreviewImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrcessedImageStep5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrcessedImageStep6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbSourceImage;
        private System.Windows.Forms.PictureBox pbPrcessedImageStep1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbTeschold;
        private System.Windows.Forms.Label lbMaxValue;
        private System.Windows.Forms.PictureBox pbExtractedImage;
        private System.Windows.Forms.TrackBar trackBar4;
        private System.Windows.Forms.TrackBar trackBar3;
        private System.Windows.Forms.Label lbB;
        private System.Windows.Forms.Label lbA;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbImageName;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnOCR;
        private System.Windows.Forms.Button btnProdCode;
        private System.Windows.Forms.PictureBox pbPrcessedImageStep2;
        private System.Windows.Forms.PictureBox pbPrcessedImageStep3;
        private System.Windows.Forms.PictureBox pbPrcessedImageStep4;
        private System.Windows.Forms.PictureBox pbFinalPreviewImage;
        private System.Windows.Forms.PictureBox pbPrcessedImageStep5;
        private System.Windows.Forms.PictureBox pbPrcessedImageStep6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnBarcodeSmall;
    }
}


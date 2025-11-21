namespace Elsanatlari
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
			this.BtnUrunEkle = new System.Windows.Forms.Button();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.TxtUrunAd = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.TxtMevcutStok = new System.Windows.Forms.TextBox();
			this.TxtBirimFiyat = new System.Windows.Forms.TextBox();
			this.TxtStok = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cmbUrunSec = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.cmbHareketTuru = new System.Windows.Forms.ComboBox();
			this.dataGridView2 = new System.Windows.Forms.DataGridView();
			this.BtnHareketEkle = new System.Windows.Forms.Button();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
			this.label11 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// BtnUrunEkle
			// 
			this.BtnUrunEkle.Location = new System.Drawing.Point(173, 255);
			this.BtnUrunEkle.Name = "BtnUrunEkle";
			this.BtnUrunEkle.Size = new System.Drawing.Size(163, 47);
			this.BtnUrunEkle.TabIndex = 5;
			this.BtnUrunEkle.Text = "Ürün Ekle";
			this.BtnUrunEkle.UseVisualStyleBackColor = true;
			this.BtnUrunEkle.Click += new System.EventHandler(this.BtnUrunEkle_Click_1);
			// 
			// dataGridView1
			// 
			this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(381, 55);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersWidth = 51;
			this.dataGridView1.RowTemplate.Height = 24;
			this.dataGridView1.Size = new System.Drawing.Size(566, 194);
			this.dataGridView1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(61, 61);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(101, 28);
			this.label1.TabIndex = 2;
			this.label1.Text = "Ürün Adı:";
			// 
			// TxtUrunAd
			// 
			this.TxtUrunAd.Location = new System.Drawing.Point(159, 55);
			this.TxtUrunAd.Name = "TxtUrunAd";
			this.TxtUrunAd.Size = new System.Drawing.Size(204, 34);
			this.TxtUrunAd.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 218);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(137, 28);
			this.label2.TabIndex = 4;
			this.label2.Text = "Mevcut Stok:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(36, 174);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(126, 28);
			this.label3.TabIndex = 5;
			this.label3.Text = "Birim Fiyatı:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(48, 120);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(114, 28);
			this.label4.TabIndex = 6;
			this.label4.Text = "Stok Kodu:";
			// 
			// TxtMevcutStok
			// 
			this.TxtMevcutStok.Location = new System.Drawing.Point(159, 215);
			this.TxtMevcutStok.Name = "TxtMevcutStok";
			this.TxtMevcutStok.Size = new System.Drawing.Size(204, 34);
			this.TxtMevcutStok.TabIndex = 4;
			// 
			// TxtBirimFiyat
			// 
			this.TxtBirimFiyat.Location = new System.Drawing.Point(159, 168);
			this.TxtBirimFiyat.Name = "TxtBirimFiyat";
			this.TxtBirimFiyat.Size = new System.Drawing.Size(204, 34);
			this.TxtBirimFiyat.TabIndex = 3;
			// 
			// TxtStok
			// 
			this.TxtStok.Location = new System.Drawing.Point(159, 114);
			this.TxtStok.Name = "TxtStok";
			this.TxtStok.Size = new System.Drawing.Size(204, 34);
			this.TxtStok.TabIndex = 2;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			this.label5.Location = new System.Drawing.Point(125, 5);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(238, 38);
			this.label5.TabIndex = 10;
			this.label5.Text = "ÜRÜN İŞLEMLERİ";
			// 
			// cmbUrunSec
			// 
			this.cmbUrunSec.FormattingEnabled = true;
			this.cmbUrunSec.Items.AddRange(new object[] {
            "Vazo",
            "bakır çezve",
            "ahşap saat",
            "çini tabakları",
            "tıg işi",
            "lif"});
			this.cmbUrunSec.Location = new System.Drawing.Point(189, 400);
			this.cmbUrunSec.Name = "cmbUrunSec";
			this.cmbUrunSec.Size = new System.Drawing.Size(121, 36);
			this.cmbUrunSec.TabIndex = 6;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(81, 522);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(81, 28);
			this.label6.TabIndex = 12;
			this.label6.Text = "Miktar:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(21, 467);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(141, 28);
			this.label7.TabIndex = 13;
			this.label7.Text = "Hareket Türü:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(61, 408);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(101, 28);
			this.label8.TabIndex = 14;
			this.label8.Text = "Ürün Seç:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(61, 659);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(81, 28);
			this.label9.TabIndex = 15;
			this.label9.Text = "Miktar:";
			// 
			// cmbHareketTuru
			// 
			this.cmbHareketTuru.FormattingEnabled = true;
			this.cmbHareketTuru.Items.AddRange(new object[] {
            "GİRİŞ",
            "ÇIKIŞ"});
			this.cmbHareketTuru.Location = new System.Drawing.Point(189, 464);
			this.cmbHareketTuru.Name = "cmbHareketTuru";
			this.cmbHareketTuru.Size = new System.Drawing.Size(121, 36);
			this.cmbHareketTuru.TabIndex = 17;
			// 
			// dataGridView2
			// 
			this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView2.Location = new System.Drawing.Point(381, 400);
			this.dataGridView2.Name = "dataGridView2";
			this.dataGridView2.RowHeadersWidth = 51;
			this.dataGridView2.RowTemplate.Height = 24;
			this.dataGridView2.Size = new System.Drawing.Size(566, 194);
			this.dataGridView2.TabIndex = 18;
			// 
			// BtnHareketEkle
			// 
			this.BtnHareketEkle.Location = new System.Drawing.Point(159, 640);
			this.BtnHareketEkle.Name = "BtnHareketEkle";
			this.BtnHareketEkle.Size = new System.Drawing.Size(163, 47);
			this.BtnHareketEkle.TabIndex = 19;
			this.BtnHareketEkle.Text = "Hareket Ekle";
			this.BtnHareketEkle.UseVisualStyleBackColor = true;
			this.BtnHareketEkle.Click += new System.EventHandler(this.BtnHareketEkle_Click_1);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(189, 522);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(121, 34);
			this.numericUpDown1.TabIndex = 20;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Segoe UI", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			this.label10.Location = new System.Drawing.Point(36, 359);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(274, 38);
			this.label10.TabIndex = 21;
			this.label10.Text = "STOK HAREKETLERİ";
			// 
			// maskedTextBox1
			// 
			this.maskedTextBox1.Location = new System.Drawing.Point(189, 575);
			this.maskedTextBox1.Mask = "00/00/0000 90:00";
			this.maskedTextBox1.Name = "maskedTextBox1";
			this.maskedTextBox1.Size = new System.Drawing.Size(121, 34);
			this.maskedTextBox1.TabIndex = 22;
			this.maskedTextBox1.ValidatingType = typeof(System.DateTime);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(51, 581);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(111, 28);
			this.label11.TabIndex = 23;
			this.label11.Text = "Tarih Saat:";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 28F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1518, 738);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.maskedTextBox1);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.BtnHareketEkle);
			this.Controls.Add(this.dataGridView2);
			this.Controls.Add(this.cmbHareketTuru);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.cmbUrunSec);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.TxtStok);
			this.Controls.Add(this.TxtBirimFiyat);
			this.Controls.Add(this.TxtMevcutStok);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.TxtUrunAd);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.BtnUrunEkle);
			this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button BtnUrunEkle;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TxtUrunAd;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox TxtMevcutStok;
		private System.Windows.Forms.TextBox TxtBirimFiyat;
		private System.Windows.Forms.TextBox TxtStok;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cmbUrunSec;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cmbHareketTuru;
		private System.Windows.Forms.DataGridView dataGridView2;
		private System.Windows.Forms.Button BtnHareketEkle;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.MaskedTextBox maskedTextBox1;
		private System.Windows.Forms.Label label11;
	}
}


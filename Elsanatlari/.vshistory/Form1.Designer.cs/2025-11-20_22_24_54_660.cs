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
			this.cmbHareketTuru = new System.Windows.Forms.ComboBox();
			this.dataGridView2 = new System.Windows.Forms.DataGridView();
			this.BtnHareketEkle = new System.Windows.Forms.Button();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.txtAra = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.BtnTemizle = new System.Windows.Forms.Button();
			this.BtnSİL = new System.Windows.Forms.Button();
			this.BtnGuncelle = new System.Windows.Forms.Button();
			this.CmbUrunFiltre = new System.Windows.Forms.ComboBox();
			this.label13 = new System.Windows.Forms.Label();
			this.cmbHareketFiltre = new System.Windows.Forms.ComboBox();
			this.label14 = new System.Windows.Forms.Label();
			this.dateTimePickerBaslangic = new System.Windows.Forms.DateTimePicker();
			this.dateTimePickerBitis = new System.Windows.Forms.DateTimePicker();
			this.CmbFiltre = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// BtnUrunEkle
			// 
			this.BtnUrunEkle.Location = new System.Drawing.Point(12, 255);
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
			this.dataGridView1.Size = new System.Drawing.Size(824, 247);
			this.dataGridView1.TabIndex = 1;
			this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
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
			this.cmbUrunSec.Location = new System.Drawing.Point(189, 416);
			this.cmbUrunSec.Name = "cmbUrunSec";
			this.cmbUrunSec.Size = new System.Drawing.Size(121, 36);
			this.cmbUrunSec.TabIndex = 6;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(81, 538);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(81, 28);
			this.label6.TabIndex = 12;
			this.label6.Text = "Miktar:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(21, 483);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(141, 28);
			this.label7.TabIndex = 13;
			this.label7.Text = "Hareket Türü:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(61, 424);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(101, 28);
			this.label8.TabIndex = 14;
			this.label8.Text = "Ürün Seç:";
			// 
			// cmbHareketTuru
			// 
			this.cmbHareketTuru.FormattingEnabled = true;
			this.cmbHareketTuru.Items.AddRange(new object[] {
            "GİRİŞ",
            "ÇIKIŞ"});
			this.cmbHareketTuru.Location = new System.Drawing.Point(189, 480);
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
			this.dataGridView2.Size = new System.Drawing.Size(824, 194);
			this.dataGridView2.TabIndex = 18;
			// 
			// BtnHareketEkle
			// 
			this.BtnHareketEkle.Location = new System.Drawing.Point(147, 689);
			this.BtnHareketEkle.Name = "BtnHareketEkle";
			this.BtnHareketEkle.Size = new System.Drawing.Size(163, 47);
			this.BtnHareketEkle.TabIndex = 19;
			this.BtnHareketEkle.Text = "Hareket Ekle";
			this.BtnHareketEkle.UseVisualStyleBackColor = true;
			this.BtnHareketEkle.Click += new System.EventHandler(this.BtnHareketEkle_Click_1);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(189, 538);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(121, 34);
			this.numericUpDown1.TabIndex = 20;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Segoe UI", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			this.label10.Location = new System.Drawing.Point(36, 375);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(274, 38);
			this.label10.TabIndex = 21;
			this.label10.Text = "STOK HAREKETLERİ";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(184, 586);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(111, 28);
			this.label11.TabIndex = 23;
			this.label11.Text = "Tarih Saat:";
			// 
			// txtAra
			// 
			this.txtAra.Location = new System.Drawing.Point(1001, 12);
			this.txtAra.Name = "txtAra";
			this.txtAra.Size = new System.Drawing.Size(204, 34);
			this.txtAra.TabIndex = 24;
			this.txtAra.TextChanged += new System.EventHandler(this.txtAra_TextChanged);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(390, 20);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(99, 28);
			this.label12.TabIndex = 25;
			this.label12.Text = "FİLTRELE:";
			// 
			// BtnTemizle
			// 
			this.BtnTemizle.Location = new System.Drawing.Point(200, 309);
			this.BtnTemizle.Name = "BtnTemizle";
			this.BtnTemizle.Size = new System.Drawing.Size(163, 47);
			this.BtnTemizle.TabIndex = 26;
			this.BtnTemizle.Text = "Temizle";
			this.BtnTemizle.UseVisualStyleBackColor = true;
			this.BtnTemizle.Click += new System.EventHandler(this.BtnTemizle_Click);
			// 
			// BtnSİL
			// 
			this.BtnSİL.Location = new System.Drawing.Point(12, 309);
			this.BtnSİL.Name = "BtnSİL";
			this.BtnSİL.Size = new System.Drawing.Size(163, 47);
			this.BtnSİL.TabIndex = 27;
			this.BtnSİL.Text = "SİL";
			this.BtnSİL.UseVisualStyleBackColor = true;
			this.BtnSİL.Click += new System.EventHandler(this.BtnSİL_Click);
			// 
			// BtnGuncelle
			// 
			this.BtnGuncelle.Location = new System.Drawing.Point(200, 255);
			this.BtnGuncelle.Name = "BtnGuncelle";
			this.BtnGuncelle.Size = new System.Drawing.Size(163, 47);
			this.BtnGuncelle.TabIndex = 28;
			this.BtnGuncelle.Text = "GÜNCELLE";
			this.BtnGuncelle.UseVisualStyleBackColor = true;
			this.BtnGuncelle.Click += new System.EventHandler(this.BtnGuncelle_Click);
			// 
			// CmbUrunFiltre
			// 
			this.CmbUrunFiltre.FormattingEnabled = true;
			this.CmbUrunFiltre.Location = new System.Drawing.Point(495, 12);
			this.CmbUrunFiltre.Name = "CmbUrunFiltre";
			this.CmbUrunFiltre.Size = new System.Drawing.Size(183, 36);
			this.CmbUrunFiltre.TabIndex = 29;
			this.CmbUrunFiltre.SelectedIndexChanged += new System.EventHandler(this.CmbUrunFiltre_SelectedIndexChanged);
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(938, 15);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(58, 28);
			this.label13.TabIndex = 30;
			this.label13.Text = "ARA:";
			// 
			// cmbHareketFiltre
			// 
			this.cmbHareketFiltre.FormattingEnabled = true;
			this.cmbHareketFiltre.Items.AddRange(new object[] {
            "ALFABETİK",
            "TARİH SAAT",
            "FİYAT"});
			this.cmbHareketFiltre.Location = new System.Drawing.Point(668, 351);
			this.cmbHareketFiltre.Name = "cmbHareketFiltre";
			this.cmbHareketFiltre.Size = new System.Drawing.Size(183, 36);
			this.cmbHareketFiltre.TabIndex = 31;
			this.cmbHareketFiltre.SelectedIndexChanged += new System.EventHandler(this.cmbHareketFiltre_SelectedIndexChanged_1);
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(550, 359);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(99, 28);
			this.label14.TabIndex = 32;
			this.label14.Text = "FİLTRELE:";
			// 
			// dateTimePickerBaslangic
			// 
			this.dateTimePickerBaslangic.Location = new System.Drawing.Point(43, 616);
			this.dateTimePickerBaslangic.Name = "dateTimePickerBaslangic";
			this.dateTimePickerBaslangic.Size = new System.Drawing.Size(289, 34);
			this.dateTimePickerBaslangic.TabIndex = 33;
			// 
			// dateTimePickerBitis
			// 
			this.dateTimePickerBitis.Location = new System.Drawing.Point(41, 649);
			this.dateTimePickerBitis.Name = "dateTimePickerBitis";
			this.dateTimePickerBitis.Size = new System.Drawing.Size(289, 34);
			this.dateTimePickerBitis.TabIndex = 34;
			// 
			// CmbFiltre
			// 
			this.CmbFiltre.FormattingEnabled = true;
			this.CmbFiltre.Location = new System.Drawing.Point(696, 12);
			this.CmbFiltre.Name = "CmbFiltre";
			this.CmbFiltre.Size = new System.Drawing.Size(183, 36);
			this.CmbFiltre.TabIndex = 35;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 28F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1518, 738);
			this.Controls.Add(this.CmbFiltre);
			this.Controls.Add(this.dateTimePickerBitis);
			this.Controls.Add(this.dateTimePickerBaslangic);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.cmbHareketFiltre);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.CmbUrunFiltre);
			this.Controls.Add(this.BtnGuncelle);
			this.Controls.Add(this.BtnSİL);
			this.Controls.Add(this.BtnTemizle);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.txtAra);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.BtnHareketEkle);
			this.Controls.Add(this.dataGridView2);
			this.Controls.Add(this.cmbHareketTuru);
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
		private System.Windows.Forms.ComboBox cmbHareketTuru;
		private System.Windows.Forms.DataGridView dataGridView2;
		private System.Windows.Forms.Button BtnHareketEkle;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox txtAra;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Button BtnTemizle;
		private System.Windows.Forms.Button BtnSİL;
		private System.Windows.Forms.Button BtnGuncelle;
		private System.Windows.Forms.ComboBox CmbUrunFiltre;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.ComboBox cmbHareketFiltre;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.DateTimePicker dateTimePickerBaslangic;
		private System.Windows.Forms.DateTimePicker dateTimePickerBitis;
		private System.Windows.Forms.ComboBox CmbFiltre;
	}
}


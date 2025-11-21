using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.Globalization; // TarihSaat formatı için

namespace Elsanatlari
{
	public partial class Form1 : Form
	{
		string connectionString = "Data Source=Elsanatlari.db";

		public Form1()
		{
			InitializeComponent();
			VeritabaniOlustur();
			FiltreleriDoldur(); // Yeni metot
			UrunleriListele();
			HareketleriListele();
		}

		private void VeritabaniOlustur()
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				var cmd1 = connection.CreateCommand();
				cmd1.CommandText =
				@"CREATE TABLE IF NOT EXISTS Urunler (
					Id INTEGER PRIMARY KEY AUTOINCREMENT,
					UrunAdi TEXT NOT NULL,
					StokKodu TEXT NOT NULL,
					BirimFiyat REAL NOT NULL,
					MevcutStok INTEGER NOT NULL DEFAULT 0
				);";
				cmd1.ExecuteNonQuery();

				var cmd2 = connection.CreateCommand();
				cmd2.CommandText =
				@"CREATE TABLE IF NOT EXISTS StokHareketleri (
					Id INTEGER PRIMARY KEY AUTOINCREMENT,
					UrunId INTEGER NOT NULL,
					HareketTuru TEXT NOT NULL,
					Miktar INTEGER NOT NULL,
					TarihSaat TEXT NOT NULL,
					FOREIGN KEY(UrunId) REFERENCES Urunler(Id)
				);";
				cmd2.ExecuteNonQuery();
			}
		}

		// Filtre Combobox'larını doldurmak için yeni metot
		private void FiltreleriDoldur()
		{
			// Ürün İşlemleri Filtresi
			CmbUrunFiltre.Items.Clear();
			CmbUrunFiltre.Items.Add("Ürün Adı (A-Z)"); // Varsayılan
			CmbUrunFiltre.Items.Add("Ürün Adı (Z-A)");
			CmbUrunFiltre.Items.Add("Fiyat (Düşükten Yükseğe)");
			CmbUrunFiltre.Items.Add("Fiyat (Yüksekten Düşüğe)");
			CmbUrunFiltre.Items.Add("Stok (Azdan Çoğa)");
			CmbUrunFiltre.Items.Add("Stok (Çoktan Aza)");
			CmbUrunFiltre.SelectedIndex = 0; // Başlangıçta Ürün Adı A-Z seçili

			// Stok Hareketleri Filtresi
			cmbHareketFiltre.Items.Clear(); // Yeni eklenen combobox
			cmbHareketFiltre.Items.Add("Tümü");
			cmbHareketFiltre.Items.Add("GİRİŞ");
			cmbHareketFiltre.Items.Add("ÇIKIŞ");
			cmbHareketFiltre.SelectedIndex = 0;

			// Başlangıç ve bitiş tarihlerini güncel tarihle doldur
			dateTimePickerBaslangic.Value = DateTime.Now.AddMonths(-1);
			dateTimePickerBitis.Value = DateTime.Now;
		}

		private void UrunleriListele()
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				// Stok Hareketleri combobox'ı için ürünleri al
				var cmdUrunler = connection.CreateCommand();
				cmdUrunler.CommandText = "SELECT Id, UrunAdi FROM Urunler ORDER BY UrunAdi;";
				var reader = cmdUrunler.ExecuteReader();

				DataTable dtUrunler = new DataTable();
				dtUrunler.Load(reader);

				cmbUrunSec.Items.Clear();
				CmbUrunFiltre.Items.Clear(); // Yeni Stok Hareketleri Ürün Filtresi

				CmbUrunFiltre.Items.Add(new ComboboxItem { Text = "Tüm Ürünler", Value = 0 }); // Tümü seçeneği

				foreach (DataRow row in dtUrunler.Rows)
				{
					var item = new ComboboxItem
					{
						Text = row["UrunAdi"].ToString(),
						Value = Convert.ToInt32(row["Id"])
					};
					cmbUrunSec.Items.Add(item);
					CmbUrunFiltre.Items.Add(item);
				}
				CmbUrunFiltre.SelectedIndex = 0;

				// Ana Ürün Listesini (dataGridView1) doldurmak için UrunFiltrele çağrılır
				UrunFiltrele();
			}
		}

		private void HareketleriListele()
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				string hareketTuru = cmbHareketFiltre.SelectedItem.ToString();
				int urunId = ((ComboboxItem)CmbUrunFiltre.SelectedItem).Value;
				string baslangicTarihi = dateTimePickerBaslangic.Value.ToString("yyyy-MM-dd 00:00:00");
				string bitisTarihi = dateTimePickerBitis.Value.ToString("yyyy-MM-dd 23:59:59");


				string sql =
				@"SELECT h.Id, u.UrunAdi, h.HareketTuru, h.Miktar, h.TarihSaat
				  FROM StokHareketleri h
				  JOIN Urunler u ON u.Id = h.UrunId
				  WHERE h.TarihSaat BETWEEN $baslangic AND $bitis";

				// Hareket Türü filtresi
				if (hareketTuru != "Tümü")
				{
					sql += " AND h.HareketTuru = $tur";
				}

				// Ürün filtresi
				if (urunId != 0)
				{
					sql += " AND h.UrunId = $urunId";
				}

				sql += " ORDER BY h.TarihSaat DESC;";

				var cmd = connection.CreateCommand();
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("$baslangic", baslangicTarihi);
				cmd.Parameters.AddWithValue("$bitis", bitisTarihi);
				if (hareketTuru != "Tümü") cmd.Parameters.AddWithValue("$tur", hareketTuru);
				if (urunId != 0) cmd.Parameters.AddWithValue("$urunId", urunId);


				var reader = cmd.ExecuteReader();
				DataTable dt = new DataTable();
				dt.Load(reader);

				dataGridView2.DataSource = dt;
			}
		}

		private void BtnUrunEkle_Click_1(object sender, EventArgs e)
		{
			// Basit bir kontrol ekleyelim
			if (string.IsNullOrWhiteSpace(TxtUrunAd.Text) || string.IsNullOrWhiteSpace(TxtStok.Text) || string.IsNullOrWhiteSpace(TxtBirimFiyat.Text))
			{
				MessageBox.Show("Ürün Adı, Stok Kodu ve Birim Fiyat alanları boş bırakılamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// İlk ekleme sırasında Mevcut Stok alanındaki değeri kullanıyoruz
			int ilkStok = 0;
			if (!int.TryParse(TxtMevcutStok.Text, out ilkStok) || ilkStok < 0)
			{
				MessageBox.Show("Mevcut Stok geçerli bir sayı olmalıdır (0 veya pozitif).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			double birimFiyat = 0;
			if (!double.TryParse(TxtBirimFiyat.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out birimFiyat))
			{
				MessageBox.Show("Birim Fiyat geçerli bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}


			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				var cmd = connection.CreateCommand();
				cmd.CommandText =
				@"INSERT INTO Urunler (UrunAdi, StokKodu, BirimFiyat, MevcutStok)
					VALUES ($a, $s, $b, $m);";

				cmd.Parameters.AddWithValue("$a", TxtUrunAd.Text);
				cmd.Parameters.AddWithValue("$s", TxtStok.Text);
				cmd.Parameters.AddWithValue("$b", birimFiyat);
				cmd.Parameters.AddWithValue("$m", ilkStok);

				cmd.ExecuteNonQuery();
			}

			// İşlem bittikten sonra listeleri ve kutuları temizle
			UrunleriListele();
			TxtUrunAd.Clear();
			TxtStok.Clear();
			TxtBirimFiyat.Clear();
			TxtMevcutStok.Clear(); // Eklenen yeni ürünün stoğu artık buradaki değerden başlatıldığı için temizlenecek.
		}

		private void BtnHareketEkle_Click_1(object sender, EventArgs e)
		{
			if (cmbUrunSec.SelectedItem == null || cmbHareketTuru.SelectedItem == null)
			{
				MessageBox.Show("Ürün ve Hareket Türü seçimi yapılmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			int hareketMiktari = Convert.ToInt32(numericUpDown1.Value); // numericUpDown'un Value'su daha güvenli
			if (hareketMiktari <= 0)
			{
				MessageBox.Show("Miktar sıfırdan büyük olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var secilen = (ComboboxItem)cmbUrunSec.SelectedItem;
			string hareketTuru = cmbHareketTuru.SelectedItem.ToString();


			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				var trans = connection.BeginTransaction();

				try
				{
					// NEGATİF STOK KONTROLÜ
					if (hareketTuru == "ÇIKIŞ")
					{
						var cmdCheck = connection.CreateCommand();
						cmdCheck.Transaction = trans;
						cmdCheck.CommandText = "SELECT MevcutStok FROM Urunler WHERE Id = $id;";
						cmdCheck.Parameters.AddWithValue("$id", secilen.Value);

						int mevcutStok = Convert.ToInt32(cmdCheck.ExecuteScalar());

						if (mevcutStok < hareketMiktari)
						{
							// İşlemi iptal et ve uyarı ver
							trans.Rollback();
							MessageBox.Show($"Yetersiz stok! Mevcut Stok: {mevcutStok}. Çıkış Miktarı: {hareketMiktari}. İşlem iptal edildi.", "Stok Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
					}
					// Kontrol başarılı/GİRİŞ işlemi

					// 1. Hareket ekle
					var cmdH = connection.CreateCommand();
					cmdH.Transaction = trans;
					cmdH.CommandText =
					@"INSERT INTO StokHareketleri (UrunId, HareketTuru, Miktar, TarihSaat)
						VALUES ($id, $tur, $mik, $tarih);";

					cmdH.Parameters.AddWithValue("$id", secilen.Value);
					cmdH.Parameters.AddWithValue("$tur", hareketTuru);
					cmdH.Parameters.AddWithValue("$mik", hareketMiktari);
					// Tarih saat formatı
					cmdH.Parameters.AddWithValue("$tarih", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

					cmdH.ExecuteNonQuery();

					// 2. Stok güncelle
					var cmdS = connection.CreateCommand();
					cmdS.Transaction = trans;

					if (hareketTuru == "GİRİŞ") // Büyük harf olarak değişti
					{
						cmdS.CommandText = "UPDATE Urunler SET MevcutStok = MevcutStok + $m WHERE Id = $id;";
					}
					else // "ÇIKIŞ"
					{
						cmdS.CommandText = "UPDATE Urunler SET MevcutStok = MevcutStok - $m WHERE Id = $id;";
					}

					cmdS.Parameters.AddWithValue("$m", hareketMiktari);
					cmdS.Parameters.AddWithValue("$id", secilen.Value);
					cmdS.ExecuteNonQuery();

					trans.Commit();
				}
				catch (Exception ex)
				{
					trans.Rollback();
					MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			HareketleriListele();
			UrunleriListele();
		}

		// ÜRÜN İŞLEMLERİ LİSTESİ FİLTRE VE SIRALAMASI
		private void UrunFiltrele()
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				string secilenSiralama = CmbUrunFiltre.SelectedItem?.ToString() ?? "Ürün Adı (A-Z)";
				string ara = txtAra.Text.Trim();

				string sql = "SELECT * FROM Urunler WHERE 1=1";
				string orderBy = "UrunAdi ASC";

				// Arama Filtresi
				if (!string.IsNullOrEmpty(ara))
					sql += $" AND (UrunAdi LIKE '%{ara}%' OR StokKodu LIKE '%{ara}%')";

				// Sıralama Filtreleri
				switch (secilenSiralama)
				{
					case "Ürün Adı (A-Z)":
						orderBy = "UrunAdi ASC";
						break;
					case "Ürün Adı (Z-A)":
						orderBy = "UrunAdi DESC";
						break;
					case "Fiyat (Düşükten Yükseğe)":
						orderBy = "BirimFiyat ASC";
						break;
					case "Fiyat (Yüksekten Düşüğe)":
						orderBy = "BirimFiyat DESC";
						break;
					case "Stok (Azdan Çoğa)":
						orderBy = "MevcutStok ASC";
						break;
					case "Stok (Çoktan Aza)":
						orderBy = "MevcutStok DESC";
						break;
				}

				sql += $" ORDER BY {orderBy}";

				var cmd = connection.CreateCommand();
				cmd.CommandText = sql;

				DataTable dt = new DataTable();
				dt.Load(cmd.ExecuteReader());
				dataGridView1.DataSource = dt;
			}
		}

		private void BtnSİL_Click(object sender, EventArgs e)
		{
			if (dataGridView1.SelectedRows.Count == 0)
			{
				MessageBox.Show("Silmek için ürün seç.");
				return;
			}

			int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				// önce hareket var mı kontrol
				var cmdCheck = connection.CreateCommand();
				cmdCheck.CommandText = "SELECT COUNT(*) FROM StokHareketleri WHERE UrunId=$id";
				cmdCheck.Parameters.AddWithValue("$id", id);

				int sayi = Convert.ToInt32(cmdCheck.ExecuteScalar());

				if (sayi > 0)
				{
					MessageBox.Show("Bu ürünün stok hareketi var. Silinemez! Önce hareketleri silin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				// silme işlemi
				var cmd = connection.CreateCommand();
				cmd.CommandText = "DELETE FROM Urunler WHERE Id=$id";
				cmd.Parameters.AddWithValue("$id", id);
				cmd.ExecuteNonQuery();
			}

			UrunleriListele();
			MessageBox.Show("Ürün silindi.");
		}

		private void BtnGuncelle_Click(object sender, EventArgs e)
		{
			if (dataGridView1.SelectedRows.Count == 0)
			{
				MessageBox.Show("Güncellemek için ürün seç.");
				return;
			}

			// Birim Fiyat kontrolü
			double birimFiyat = 0;
			if (!double.TryParse(TxtBirimFiyat.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out birimFiyat))
			{
				MessageBox.Show("Birim Fiyat geçerli bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Mevcut Stok kontrolü (doğrudan manuel güncellenmesini engellemek için koddan çıkarıldı, sadece bilgi amaçlı görünmeli)
			// Ancak eski koddaki Id'yi almalıyız
			int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);


			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				var cmd = connection.CreateCommand();
				cmd.CommandText =
				@"UPDATE Urunler SET
					UrunAdi = $a,
					StokKodu = $s,
					BirimFiyat = $b
				  WHERE Id = $id";

				cmd.Parameters.AddWithValue("$a", TxtUrunAd.Text);
				cmd.Parameters.AddWithValue("$s", TxtStok.Text);
				cmd.Parameters.AddWithValue("$b", birimFiyat);
				cmd.Parameters.AddWithValue("$id", id);

				cmd.ExecuteNonQuery();
			}

			UrunleriListele();
			MessageBox.Show("Ürün güncellendi.");
		}

		// *** Event Handler'lar Güncellendi / Eklendi ***

		private void CmbUrunFiltre_SelectedIndexChanged(object sender, EventArgs e)
		{
			UrunFiltrele();
		}

		private void txtAra_TextChanged(object sender, EventArgs e)
		{
			UrunFiltrele();
		}

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
				TxtUrunAd.Text = row.Cells["UrunAdi"].Value.ToString();
				TxtStok.Text = row.Cells["StokKodu"].Value.ToString();
				TxtBirimFiyat.Text = row.Cells["BirimFiyat"].Value.ToString();
				// Mevcut Stok bilgisini sadece göstermek için dolduruyoruz, güncelleme butonu ile değiştirilemez
				TxtMevcutStok.Text = row.Cells["MevcutStok"].Value.ToString();

				// DatagridView'deki satırı seçilen ürün hareketleri listesinde seçili hale getir
				if (cmbUrunSec.Items.Count > 0)
				{
					string urunAdi = row.Cells["UrunAdi"].Value.ToString();
					for (int i = 0; i < cmbUrunSec.Items.Count; i++)
					{
						if (cmbUrunSec.Items[i].ToString() == urunAdi)
						{
							cmbUrunSec.SelectedIndex = i;
							break;
						}
					}
				}
			}
		}

		// Stok Hareketleri Filtreleri için yeni event handler'lar
		private void CmbHareketFiltre_SelectedIndexChanged(object sender, EventArgs e)
		{
			HareketleriListele();
		}

		private void cmbUrunFiltre_SelectedIndexChanged(object sender, EventArgs e)
		{
			HareketleriListele();
		}

		private void dateTimePicker_ValueChanged(object sender, EventArgs e)
		{
			HareketleriListele();
		}

		private void cmbHareketFiltre_SelectedIndexChanged_1(object sender, EventArgs e)
		{
			HareketleriListele();
		}
	}

	public class ComboboxItem
	{
		public string Text { get; set; }
		public int Value { get; set; }
		public override string ToString() => Text;
	}
}
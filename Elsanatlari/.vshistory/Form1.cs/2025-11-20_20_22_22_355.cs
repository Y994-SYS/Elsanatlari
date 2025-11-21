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
			FiltreleriDoldur(); // Combobox seçeneklerini doldurur
			UrunleriListele(); // Ürün listesini ve stok hareketleri combobox'larını doldurur
			HareketleriListele(); // Stok hareketlerini filtrelerle listeler
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

		// Filtre Combobox'larını doldurmak için metot
		private void FiltreleriDoldur()
		{
			// *** ÜRÜN İŞLEMLERİ SIRALAMA FİLTRESİ SİLİNDİ ***
			// (CmbFiltre/CmbUrunFiltre'ye string ekleyen kod kaldırıldı)

			// *** STOK HAREKETLERİ FİLTRESİ (Hareket Türü) (cmbHareketFiltre) ***
			cmbHareketFiltre.Items.Clear();
			cmbHareketFiltre.Items.Add("Tümü");
			cmbHareketFiltre.Items.Add("GİRİŞ");
			cmbHareketFiltre.Items.Add("ÇIKIŞ");
			cmbHareketFiltre.SelectedIndex = 0;

			// Tarih ayarları
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
				// *** STOK HAREKETLERİ ÜRÜN FİLTRESİ (CmbUrunFiltre) ***
				CmbUrunFiltre.Items.Clear();

				CmbUrunFiltre.Items.Add(new ComboboxItem { Text = "Tüm Ürünler", Value = 0 }); // Tümü seçeneği

				foreach (DataRow row in dtUrunler.Rows)
				{
					var item = new ComboboxItem
					{
						Text = row["UrunAdi"].ToString(),
						Value = Convert.ToInt32(row["Id"])
					};
					cmbUrunSec.Items.Add(item); // Hareket eklemek için kullanılan combobox
					CmbUrunFiltre.Items.Add(item); // Stok Hareketlerini filtrelemek için kullanılan combobox
				}
				CmbUrunFiltre.SelectedIndex = 0; // "Tüm Ürünler" seçili

				// Ana Ürün Listesini (dataGridView1) doldurmak için UrunFiltrele çağrılır
				UrunFiltrele();
			}
		}

		private void HareketleriListele()
		{
			if (cmbHareketFiltre.SelectedItem == null)
			{
				// Önce öğe kontrolü: cmbHareketFiltre'de en az bir öğe ("Tümü") olmalı.
				if (cmbHareketFiltre.Items.Count > 0)
				{
					cmbHareketFiltre.SelectedIndex = 0;
				}
			}

			if (CmbUrunFiltre.SelectedItem == null)
			{
				// Öğe kontrolü: CmbUrunFiltre'de en az bir öğe ("Tüm Ürünler") olmalı.
				if (CmbUrunFiltre.Items.Count > 0)
				{
					CmbUrunFiltre.SelectedIndex = 0;
				}
			}

			// Seçili öğeler null ise (veya öğe sayısı 0 ise) bu noktadan sonra kod hata vermemelidir.
			// Ancak öğe sayısı 0 ise listeleme boş dönecektir, ki bu zaten beklenen davranış.

			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				// Öğeler olası "0" durumda bile, .SelectedItem yine de null olabilir.
				// Bu yüzden güvenli değer ataması yapılmalıdır.

				string hareketTuru = cmbHareketFiltre.SelectedItem?.ToString() ?? "Tümü"; // Null ise "Tümü" yap

				// CmbUrunFiltre'de seçili öğe yoksa (Count=0), burası hala hata verebilir. 
				// Ancak Count > 0 kontrolünü yukarıda yaptığımız için varsayılan olarak 0. indexi seçmiş olmalıyız.

				// HATA KONTROLÜ İÇİN DAHA GÜVENLİ BİR YÖNTEM:
				int urunId = 0; // Varsayılan ID = 0 (Tüm Ürünler)
				if (CmbUrunFiltre.SelectedItem != null)
				{
					urunId = ((ComboboxItem)CmbUrunFiltre.SelectedItem).Value;
				}
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
			// CultureInfo.InvariantCulture, ondalık ayırıcı olarak noktayı kabul etmek için eklendi.
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
			TxtMevcutStok.Clear();
		}

		private void BtnHareketEkle_Click_1(object sender, EventArgs e)
		{
			if (cmbUrunSec.SelectedItem == null || cmbHareketTuru.SelectedItem == null)
			{
				MessageBox.Show("Ürün ve Hareket Türü seçimi yapılmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// numericUpDown'ın Value'su int olarak alınır.
			int hareketMiktari = Convert.ToInt32(numericUpDown1.Value);
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
					// *** NEGATİF STOK KONTROLÜ ***
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

					if (hareketTuru == "GİRİŞ")
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

		// *** ÜRÜN İŞLEMLERİ LİSTESİ FİLTRE VE SIRALAMASI ***
		private void UrunFiltrele()
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				// Urun İşlemleri Listesinde sıralama/filtreleme artık sadece varsayılan olarak Ürün Adı ASC yapılır.
				// Eğer arayüzde bir sıralama ComboBox'ı (CmbFiltre gibi) eklerseniz, bu kodu tekrar güncelleyebiliriz.
				string ara = txtAra.Text.Trim();

				string sql = "SELECT * FROM Urunler WHERE 1=1";
				string orderBy = "UrunAdi ASC";

				// Arama Filtresi
				if (!string.IsNullOrEmpty(ara))
					sql += $" AND (UrunAdi LIKE '%{ara}%' OR StokKodu LIKE '%{ara}%')";

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

		// *** Event Handler'lar ***

		// Ürün İşlemleri Sıralama Filtresi (CmbFiltre yok, bu event handler kaldırıldı/yok sayıldı)
		/* private void CmbFiltre_SelectedIndexChanged(object sender, EventArgs e)
		{
			UrunFiltrele();
		} */

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

		// Stok Hareketleri Filtreleri için event handler'lar

		// Hareket Türü Filtresi
		private void cmbHareketFiltre_SelectedIndexChanged_1(object sender, EventArgs e)
		{
			HareketleriListele();
		}

		// Ürün Filtresi (Stok Hareketleri)
		private void CmbUrunFiltre_SelectedIndexChanged(object sender, EventArgs e)
		{
			HareketleriListele();
		}

		// Tarih Filtresi
		private void dateTimePicker_ValueChanged(object sender, EventArgs e)
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
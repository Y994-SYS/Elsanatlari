using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.Globalization;
using Elsanatlari.DAL; // Veri Erişim Katmanını dahil ettik

namespace Elsanatlari
{
	public partial class Form1 : Form
	{
		// Veritabanı işlemlerini yönetecek olan DAL nesnesi
		private SQLiteDataAccess _db = new SQLiteDataAccess();

		public Form1()
		{
			InitializeComponent();

			// Veritabanı oluşturma işlemini DAL'a devrettik
			_db.VeritabaniOlustur();

			FiltreleriDoldur();
			UrunleriListele();
			HareketleriListele();
		}

		// Veritabanı Oluşturma metodu KALDIRILDI! (Artık DAL'da)

		// Filtre Combobox'larını doldurmak için metot
		private void FiltreleriDoldur()
		{
			// *** 1. ÜRÜN İŞLEMLERİ SIRALAMA FİLTRESİ (CmbFiltre) ***
			CmbFiltre.Items.Clear();
			CmbFiltre.Items.Add("Ürün Adı (A-Z)");
			CmbFiltre.Items.Add("Ürün Adı (Z-A)");
			CmbFiltre.Items.Add("Fiyat (Düşükten Yükseğe)");
			CmbFiltre.Items.Add("Fiyat (Yüksekten Düşüğe)");
			CmbFiltre.Items.Add("Stok (Azdan Çoğa)");
			CmbFiltre.Items.Add("Stok (Çoktan Aza)");
			CmbFiltre.SelectedIndex = 0;

			// *** 2. STOK HAREKETLERİ FİLTRESİ (Hareket Türü) (cmbHareketFiltre) ***
			cmbHareketFiltre.Items.Clear();
			cmbHareketFiltre.Items.Add("Tümü");
			cmbHareketFiltre.Items.Add("GİRİŞ");
			cmbHareketFiltre.Items.Add("ÇIKIŞ");
			cmbHareketFiltre.SelectedIndex = 0;

			// Tarih ayarları (Sadece Başlangıç Filtresi kaldı)
			dateTimePickerBaslangic.Value = DateTime.Now.AddMonths(-1);
		}

		private void UrunleriListele()
		{
			// Tüm ürünleri çekmek için DAL'dan DataTable alırız.
			DataTable dtUrunler = _db.GetAllUrunlerForCombobox();

			cmbUrunSec.Items.Clear();

			foreach (DataRow row in dtUrunler.Rows)
			{
				var item = new ComboboxItem
				{
					Text = row["UrunAdi"].ToString(),
					Value = Convert.ToInt32(row["Id"])
				};
				cmbUrunSec.Items.Add(item); // Hareket eklemek için kullanılan combobox
			}

			// Ana Ürün Listesini (dataGridView1) doldurmak için UrunFiltrele çağrılır
			UrunFiltrele();
		}

		private void HareketleriListele()
		{
			// Seçim yoksa seç
			if (cmbHareketFiltre.SelectedIndex < 0 && cmbHareketFiltre.Items.Count > 0)
				cmbHareketFiltre.SelectedIndex = 0;

			// Hâlâ null ise çık (Form yüklenirken olabilir)
			if (cmbHareketFiltre.SelectedItem == null)
				return;

			string hareketTuru = cmbHareketFiltre.SelectedItem.ToString();
			string baslangicTarihi = dateTimePickerBaslangic.Value.ToString("yyyy-MM-dd 00:00:00");
			string bitisTarihi = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");

			// Veriyi DAL'dan alıp arayüze yüklüyoruz.
			dataGridView2.DataSource = _db.GetStokHareketleri(hareketTuru, baslangicTarihi, bitisTarihi);
		}

		private void BtnUrunEkle_Click_1(object sender, EventArgs e)
		{
			// Kontroller
			if (string.IsNullOrWhiteSpace(TxtUrunAd.Text) || string.IsNullOrWhiteSpace(TxtStok.Text) || string.IsNullOrWhiteSpace(TxtBirimFiyat.Text))
			{
				MessageBox.Show("Ürün Adı, Stok Kodu ve Birim Fiyat alanları boş bırakılamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
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

			// Veri ekleme işlemini DAL'a devrettik
			_db.InsertUrun(TxtUrunAd.Text, TxtStok.Text, birimFiyat, ilkStok);

			UrunleriListele();
			TxtUrunAd.Clear();
			TxtStok.Clear();
			TxtBirimFiyat.Clear();
			TxtMevcutStok.Clear();
		}

		private void BtnHareketEkle_Click_1(object sender, EventArgs e)
		{
			// Güçlendirilmiş Null Kontrolü
			if (cmbUrunSec.SelectedItem == null || cmbHareketTuru.SelectedItem == null)
			{
				MessageBox.Show("Ürün ve Hareket Türü seçimi yapılmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var secilen = (ComboboxItem)cmbUrunSec.SelectedItem;

			// Ürün Id'sinin 0 olup olmadığını kontrol et
			if (secilen.Value == 0)
			{
				MessageBox.Show("Geçerli bir ürün seçimi yapılmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			int hareketMiktari = Convert.ToInt32(numericUpDown1.Value);
			if (hareketMiktari <= 0)
			{
				MessageBox.Show("Miktar sıfırdan büyük olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			string hareketTuru = cmbHareketTuru.SelectedItem.ToString();
			int urunId = secilen.Value;

			// İşlemi DAL'a devrettik
			string hataMesaji = _db.AddStokHareket(urunId, hareketTuru, hareketMiktari);

			if (hataMesaji != null)
			{
				MessageBox.Show(hataMesaji, "İşlem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			HareketleriListele();
			UrunleriListele();
		}

		private void BtnHareketSil_Click(object sender, EventArgs e)
		{
			// 1. Seçim kontrolü
			if (dataGridView2.SelectedRows.Count == 0)
			{
				MessageBox.Show("Silinecek bir stok hareketi seçilmelidir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// Seçilen hareketin verilerini al
			int hareketId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["Id"].Value);

			// İşlemi DAL'a devrettik
			string hataMesaji = _db.DeleteStokHareket(hareketId);

			if (hataMesaji != null)
			{
				MessageBox.Show(hataMesaji, "İşlem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			MessageBox.Show("Stok hareketi başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

			HareketleriListele();
			UrunleriListele();
		}

		private void BtnHareketGuncelle_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Stok hareketlerinde miktar/tür değişikliği, karmaşık stok hesaplaması gerektirir. Güvenliğiniz için hareketi **SİLİP YENİDEN EKLEYİNİZ**.", "Güncelleme İptal Edildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}


		// *** ÜRÜN İŞLEMLERİ LİSTESİ FİLTRE VE SIRALAMASI ***
		private void UrunFiltrele()
		{
			string secilenSiralama = CmbFiltre.SelectedItem?.ToString() ?? "Ürün Adı (A-Z)";
			string ara = txtAra.Text.Trim();
			string orderByColumn = "UrunAdi";
			string orderDirection = "ASC";

			// Seçime göre SQL ORDER BY ifadesini ayarlar.
			switch (secilenSiralama)
			{
				case "Ürün Adı (A-Z)":
					orderByColumn = "UrunAdi"; orderDirection = "ASC";
					break;
				case "Ürün Adı (Z-A)":
					orderByColumn = "UrunAdi"; orderDirection = "DESC";
					break;
				case "Fiyat (Düşükten Yükseğe)":
					orderByColumn = "BirimFiyat"; orderDirection = "ASC";
					break;
				case "Fiyat (Yüksekten Düşüğe)":
					orderByColumn = "BirimFiyat"; orderDirection = "DESC";
					break;
				case "Stok (Azdan Çoğa)":
					orderByColumn = "MevcutStok"; orderDirection = "ASC";
					break;
				case "Stok (Çoktan Aza)":
					orderByColumn = "MevcutStok"; orderDirection = "DESC";
					break;
			}

			string orderBy = $"{orderByColumn} {orderDirection}";

			// Veriyi DAL'dan alıp arayüze yüklüyoruz.
			dataGridView1.DataSource = _db.GetUrunler(orderBy, ara);
		}

		private void BtnSİL_Click(object sender, EventArgs e)
		{
			if (dataGridView1.SelectedRows.Count == 0)
			{
				MessageBox.Show("Silmek için ürün seç.");
				return;
			}

			int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

			// İşlemi DAL'a devrettik
			string hataMesaji = _db.DeleteUrun(id);

			if (hataMesaji != null)
			{
				MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
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

			// Kontroller
			double birimFiyat = 0;
			if (!double.TryParse(TxtBirimFiyat.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out birimFiyat))
			{
				MessageBox.Show("Birim Fiyat geçerli bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			int mevcutStokGuncel = 0;
			if (!int.TryParse(TxtMevcutStok.Text, out mevcutStokGuncel) || mevcutStokGuncel < 0)
			{
				MessageBox.Show("Mevcut Stok geçerli bir sayı olmalıdır (0 veya pozitif).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

			// İşlemi DAL'a devrettik
			_db.UpdateUrun(id, TxtUrunAd.Text, TxtStok.Text, birimFiyat, mevcutStokGuncel);

			UrunleriListele();
			MessageBox.Show("Ürün güncellendi.");
		}

		// *** Event Handler'lar ***

		private void CmbFiltre_SelectedIndexChanged(object sender, EventArgs e)
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
				// Tıklanan satırı seçili hale getiriyoruz.
				dataGridView1.ClearSelection();
				dataGridView1.Rows[e.RowIndex].Selected = true;

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
		private void cmbHareketFiltre_SelectedIndexChanged_1(object sender, EventArgs e)
		{
			HareketleriListele();
		}

		private void dateTimePicker_ValueChanged(object sender, EventArgs e)
		{
			HareketleriListele();
		}

		private void BtnTemizle_Click(object sender, EventArgs e)
		{
			TxtUrunAd.Clear();
			TxtStok.Clear();
			TxtBirimFiyat.Clear();
			TxtMevcutStok.Clear();

		}

		private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				// Tıklanan satırı seçili hale getiriyorusz.
				dataGridView2.ClearSelection();
				dataGridView2.Rows[e.RowIndex].Selected = true;

				DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
				string urunAdi = row.Cells["UrunAdi"].Value.ToString();
				for (int i = 0; i < cmbUrunSec.Items.Count; i++)
				{
					if (cmbUrunSec.Items[i].ToString() == urunAdi)
					{
						cmbUrunSec.SelectedIndex = i;
						break;
					}
				}
				string hareketTuru = row.Cells["HareketTuru"].Value.ToString();
				for (int i = 0; i < cmbHareketTuru.Items.Count; i++)
				{
					if (cmbHareketTuru.Items[i].ToString() == hareketTuru)
					{
						cmbHareketTuru.SelectedIndex = i;
						break;
					}
				}
				numericUpDown1.Value = Convert.ToInt32(row.Cells["Miktar"].Value);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// UI'da "Hareket Sil" butonu olarak varsayılmıştır.
			BtnHareketSil_Click(sender, e);
		}

		public class ComboboxItem
		{
			public string Text { get; set; }
			public int Value { get; set; }
			public override string ToString() => Text;
		}
	}
}
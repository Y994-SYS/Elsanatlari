using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace Elsanatlari.DAL
{
	public class SQLiteDataAccess
	{
		private string connectionString = "Data Source=Elsanatlari.db";

		public void VeritabaniOlustur()
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

		// --- READ METOTLARI ---

		// Tüm ürünleri combobox için çeker (Id ve UrunAdi)
		public DataTable GetAllUrunlerForCombobox()
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();
				var cmd = connection.CreateCommand();
				cmd.CommandText = "SELECT Id, UrunAdi FROM Urunler ORDER BY UrunAdi;";

				DataTable dt = new DataTable();
				dt.Load(cmd.ExecuteReader());
				return dt;
			}
		}

		// Ürün listesini sıralama ve arama filtrelerine göre çeker
		public DataTable GetUrunler(string orderBy, string aramaKelimesi)
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				string sql = "SELECT Id, UrunAdi, StokKodu, BirimFiyat, MevcutStok FROM Urunler WHERE 1=1";

				if (!string.IsNullOrEmpty(aramaKelimesi))
					sql += $" AND (UrunAdi LIKE '%{aramaKelimesi}%' OR StokKodu LIKE '%{aramaKelimesi}%')";

				sql += $" ORDER BY {orderBy}";

				var cmd = connection.CreateCommand();
				cmd.CommandText = sql;

				DataTable dt = new DataTable();
				dt.Load(cmd.ExecuteReader());
				return dt;
			}
		}

		// Stok hareketlerini filtrelerle çeker
		public DataTable GetStokHareketleri(string hareketTuru, string baslangicTarihi, string bitisTarihi)
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				string sql =
				@"SELECT h.Id, u.UrunAdi, h.HareketTuru, h.Miktar, h.TarihSaat
                  FROM StokHareketleri h
                  JOIN Urunler u ON u.Id = h.UrunId
                  WHERE h.TarihSaat BETWEEN $baslangic AND $bitis";

				if (hareketTuru != "Tümü")
					sql += " AND h.HareketTuru = $tur";

				sql += " ORDER BY h.TarihSaat DESC";

				var cmd = connection.CreateCommand();
				cmd.CommandText = sql;

				cmd.Parameters.AddWithValue("$baslangic", baslangicTarihi);
				cmd.Parameters.AddWithValue("$bitis", bitisTarihi);

				if (hareketTuru != "Tümü")
					cmd.Parameters.AddWithValue("$tur", hareketTuru);

				DataTable dt = new DataTable();
				dt.Load(cmd.ExecuteReader());
				return dt;
			}
		}

		// --- CRUD METOTLARI ---

		// Ürün ekleme
		public void InsertUrun(string urunAdi, string stokKodu, double birimFiyat, int mevcutStok)
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();
				var cmd = connection.CreateCommand();
				cmd.CommandText =
				@"INSERT INTO Urunler (UrunAdi, StokKodu, BirimFiyat, MevcutStok)
					VALUES ($a, $s, $b, $m);";

				cmd.Parameters.AddWithValue("$a", urunAdi);
				cmd.Parameters.AddWithValue("$s", stokKodu);
				cmd.Parameters.AddWithValue("$b", birimFiyat);
				cmd.Parameters.AddWithValue("$m", mevcutStok);

				cmd.ExecuteNonQuery();
			}
		}

		// Ürün güncelleme
		public void UpdateUrun(int id, string urunAdi, string stokKodu, double birimFiyat, int mevcutStok)
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();
				var cmd = connection.CreateCommand();
				cmd.CommandText =
				@"UPDATE Urunler SET
					UrunAdi = $a,
					StokKodu = $s,
					BirimFiyat = $b,
					MevcutStok = $m
				  WHERE Id = $id";

				cmd.Parameters.AddWithValue("$a", urunAdi);
				cmd.Parameters.AddWithValue("$s", stokKodu);
				cmd.Parameters.AddWithValue("$b", birimFiyat);
				cmd.Parameters.AddWithValue("$m", mevcutStok);
				cmd.Parameters.AddWithValue("$id", id);

				cmd.ExecuteNonQuery();
			}
		}

		// Ürün silme
		public string DeleteUrun(int id)
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				// Önce hareket var mı kontrol
				var cmdCheck = connection.CreateCommand();
				cmdCheck.CommandText = "SELECT COUNT(*) FROM StokHareketleri WHERE UrunId=$id";
				cmdCheck.Parameters.AddWithValue("$id", id);

				int sayi = Convert.ToInt32(cmdCheck.ExecuteScalar());

				if (sayi > 0)
				{
					return "Bu ürünün stok hareketi var. Silinemez! Önce hareketleri silin.";
				}

				// silme işlemi
				var cmd = connection.CreateCommand();
				cmd.CommandText = "DELETE FROM Urunler WHERE Id=$id";
				cmd.Parameters.AddWithValue("$id", id);
				cmd.ExecuteNonQuery();
				return null; // Başarılı
			}
		}

		// Stok Hareketi Ekleme (İşlem kontrolü içerir)
		public string AddStokHareket(int urunId, string hareketTuru, int hareketMiktari)
		{
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
						cmdCheck.Parameters.AddWithValue("$id", urunId);

						int mevcutStok = Convert.ToInt32(cmdCheck.ExecuteScalar());

						if (mevcutStok < hareketMiktari)
						{
							trans.Rollback();
							return $"Yetersiz stok! Mevcut Stok: {mevcutStok}. Çıkış Miktarı: {hareketMiktari}. İşlem iptal edildi.";
						}
					}

					// 1. Hareket ekle
					var cmdH = connection.CreateCommand();
					cmdH.Transaction = trans;
					cmdH.CommandText =
					@"INSERT INTO StokHareketleri (UrunId, HareketTuru, Miktar, TarihSaat)
						VALUES ($id, $tur, $mik, $tarih);";

					cmdH.Parameters.AddWithValue("$id", urunId);
					cmdH.Parameters.AddWithValue("$tur", hareketTuru);
					cmdH.Parameters.AddWithValue("$mik", hareketMiktari);
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
					cmdS.Parameters.AddWithValue("$id", urunId);
					cmdS.ExecuteNonQuery();

					trans.Commit();
					return null; // Başarılı
				}
				catch (Exception ex)
				{
					trans.Rollback();
					return "Hata oluştu: " + ex.Message;
				}
			}
		}

		// Stok Hareketi Silme (İşlem kontrolü içerir)
		public string DeleteStokHareket(int hareketId)
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();
				var trans = connection.BeginTransaction();
				try
				{
					// 1. Ürün ID'sini, türünü ve miktarını bul
					var cmdGetInfo = connection.CreateCommand();
					cmdGetInfo.Transaction = trans;
					cmdGetInfo.CommandText = "SELECT UrunId, HareketTuru, Miktar FROM StokHareketleri WHERE Id = $hid;";
					cmdGetInfo.Parameters.AddWithValue("$hid", hareketId);

					using (var reader = cmdGetInfo.ExecuteReader())
					{
						if (!reader.Read())
						{
							trans.Rollback();
							return "Hata: Silinecek hareket bulunamadı.";
						}
						int urunId = reader.GetInt32(0);
						string hareketTuru = reader.GetString(1);
						int hareketMiktari = reader.GetInt32(2);

						// 2. Stok Dengelemesi yap (Ters İşlem)
						var cmdStokGuncelle = connection.CreateCommand();
						cmdStokGuncelle.Transaction = trans;

						if (hareketTuru == "GİRİŞ")
						{
							// Giriş silinirse stoktan düş
							cmdStokGuncelle.CommandText = "UPDATE Urunler SET MevcutStok = MevcutStok - $m WHERE Id = $urunId;";
						}
						else // "ÇIKIŞ"
						{
							// Çıkış silinirse stoğa ekle
							cmdStokGuncelle.CommandText = "UPDATE Urunler SET MevcutStok = MevcutStok + $m WHERE Id = $urunId;";
						}

						cmdStokGuncelle.Parameters.AddWithValue("$m", hareketMiktari);
						cmdStokGuncelle.Parameters.AddWithValue("$urunId", urunId);
						cmdStokGuncelle.ExecuteNonQuery();

						// 3. Hareketi Sil
						var cmdSil = connection.CreateCommand();
						cmdSil.Transaction = trans;
						cmdSil.CommandText = "DELETE FROM StokHareketleri WHERE Id=$hid";
						cmdSil.Parameters.AddWithValue("$hid", hareketId);
						cmdSil.ExecuteNonQuery();
					}

					trans.Commit();
					return null; // Başarılı

				}
				catch (Exception ex)
				{
					trans.Rollback();
					return "Silme sırasında hata oluştu: " + ex.Message;
				}
			}
		}
	}
}
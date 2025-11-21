using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace Elsanatlari
{
	public partial class Form1 : Form
	{
		string connectionString = "Data Source=Elsanatlari.db";

		public Form1()
		{
			InitializeComponent();
			VeritabaniOlustur();
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

		private void UrunleriListele()
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				var cmd = connection.CreateCommand();
				cmd.CommandText = "SELECT * FROM Urunler ORDER BY UrunAdi;";
				var reader = cmd.ExecuteReader();

				DataTable dt = new DataTable();
				dt.Load(reader);

				dataGridView1.DataSource = dt;

				cmbUrunSec.Items.Clear();
				foreach (DataRow row in dt.Rows)
				{
					cmbUrunSec.Items.Add(
						new ComboboxItem
						{
							Text = row["UrunAdi"].ToString(),
							Value = Convert.ToInt32(row["Id"])
						}
					);
				}
			}
		}

		private void HareketleriListele()
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				var cmd = connection.CreateCommand();
				cmd.CommandText =
				@"SELECT h.Id, u.UrunAdi, h.HareketTuru, h.Miktar, h.TarihSaat
                  FROM StokHareketleri h
                  JOIN Urunler u ON u.Id = h.UrunId
                  ORDER BY h.Id DESC;";

				var reader = cmd.ExecuteReader();
				DataTable dt = new DataTable();
				dt.Load(reader);

				dataGridView2.DataSource = dt;
			}
		}

		

		

		private void BtnUrunEkle_Click_1(object sender, EventArgs e)
		{


			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				var cmd = connection.CreateCommand();
				cmd.CommandText =
				@"INSERT INTO Urunler (UrunAdi, StokKodu, BirimFiyat, MevcutStok)
                  VALUES ($a, $s, $b, $m);";

				cmd.Parameters.AddWithValue("$a", TxtUrunAd.Text);
				cmd.Parameters.AddWithValue("$s", TxtStok.Text);
				cmd.Parameters.AddWithValue("$b", Convert.ToDouble(TxtBirimFiyat.Text));
				cmd.Parameters.AddWithValue("$m", Convert.ToInt32(numericUpDown1.Text));

				cmd.ExecuteNonQuery();
			}

			UrunleriListele();
			TxtUrunAd.Clear();
			TxtStok.Clear();
			TxtBirimFiyat.Clear();
			TxtMevcutStok.Clear();
		}

		private void BtnHareketEkle_Click_1(object sender, EventArgs e)
		{
			if (cmbUrunSec.SelectedItem == null)
			{
				MessageBox.Show("Ürün seç.");
				return;
			}

			var secilen = (ComboboxItem)cmbUrunSec.SelectedItem;

			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				var trans = connection.BeginTransaction();

				try
				{
					// hareket ekle
					var cmdH = connection.CreateCommand();
					cmdH.Transaction = trans;
					cmdH.CommandText =
					@"INSERT INTO StokHareketleri (UrunId, HareketTuru, Miktar, TarihSaat)
                      VALUES ($id, $tur, $mik, $tarih);";

					cmdH.Parameters.AddWithValue("$id", secilen.Value);
					cmdH.Parameters.AddWithValue("$tur", cmbHareketTuru.SelectedItem.ToString());
					cmdH.Parameters.AddWithValue("$mik", Convert.ToInt32(numericUpDown1.Text));
					cmdH.Parameters.AddWithValue("$tarih", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

					cmdH.ExecuteNonQuery();

					// stok güncelle
					var cmdS = connection.CreateCommand();
					cmdS.Transaction = trans;

					if (cmbHareketTuru.SelectedItem.ToString() == "Giriş")
					{
						cmdS.CommandText = "UPDATE Urunler SET MevcutStok = MevcutStok + $m WHERE Id = $id;";
					}
					else
					{
						cmdS.CommandText = "UPDATE Urunler SET MevcutStok = MevcutStok - $m WHERE Id = $id;";
					}

					cmdS.Parameters.AddWithValue("$m", Convert.ToInt32(numericUpDown1.Text));
					cmdS.Parameters.AddWithValue("$id", secilen.Value);
					cmdS.ExecuteNonQuery();

					trans.Commit();
				}
				catch
				{
					trans.Rollback();
					MessageBox.Show("Hata oluştu.");
				}
			}

			HareketleriListele();
			UrunleriListele();
		}
	}

	public class ComboboxItem
	{
		public string Text { get; set; }
		public int Value { get; set; }
		public override string ToString() => Text;
	}
}

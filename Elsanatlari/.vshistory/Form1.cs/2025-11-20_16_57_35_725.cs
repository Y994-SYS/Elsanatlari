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
		private void UrunFiltrele()
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				string filtre = CmbFiltre.SelectedItem?.ToString() ?? "";
				string ara = txtAra.Text.Trim();

				string sql = "SELECT * FROM Urunler WHERE 1=1";

				if (!string.IsNullOrEmpty(ara))
					sql += $" AND (UrunAdi LIKE '%{ara}%' OR StokKodu LIKE '%{ara}%')";

				if (filtre == "Stok 0 Olanlar")
					sql += " AND MevcutStok = 0";

				if (filtre == "Düşük Stok (<10)")
					sql += " AND MevcutStok < 10";

				sql += " ORDER BY UrunAdi";

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
					MessageBox.Show("Bu ürünün stok hareketi var. Silinemez!");
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
				cmd.Parameters.AddWithValue("$b", Convert.ToDouble(TxtBirimFiyat.Text));
				cmd.Parameters.AddWithValue("$id", id);

				cmd.ExecuteNonQuery();
			}

			UrunleriListele();
			MessageBox.Show("Ürün güncellendi.");
		}

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
				DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
				TxtUrunAd.Text = row.Cells["UrunAdi"].Value.ToString();
				TxtStok.Text = row.Cells["StokKodu"].Value.ToString();
				TxtBirimFiyat.Text = row.Cells["BirimFiyat"].Value.ToString();
				TxtMevcutStok.Text = row.Cells["MevcutStok"].Value.ToString();
			}
		}
	}

	public class ComboboxItem
	{
		public string Text { get; set; }
		public int Value { get; set; }
		public override string ToString() => Text;
	}
	

}

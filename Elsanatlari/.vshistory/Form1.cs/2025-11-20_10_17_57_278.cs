using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		}
		private void VeritabaniOlustur()
		{
			using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString))
			{
				connection.Open();
				var CmdUrun = connection.CreateCommand();
				CmdUrun.CommandText =
				@"CREATE TABLE IF NOT EXISTS Urunler (
					Id INTEGER PRIMARY KEY AUTOINCREMENT,
					UrunAdi TEXT NOT NULL,
                    StokKodu TEXT NOT NULL,
					BirimFiyat REAL NOT NULL,
                    MevcutStok INTEGER NOT NULL DEFAULT 0
				);";
				CmdUrun.ExecuteNonQuery();


				var CmdHareket = connection.CreateCommand();
				CmdHareket.CommandText =
					@"CREATE TABLE IF NOT EXISTS StokHareketleri (
					Id INTEGER PRIMARY KEY AUTOINCREMENT,
					UrunId INTEGER NOT NULL,
					IslemTuru TEXT NOT NULL,
					Miktar INTEGER NOT NULL,
					Tarih DATETIME NOT NULL,
					FOREIGN KEY(UrunId) REFERENCES Urunler(Id)
				);";
			}
		}
		private void BtnUrunEkle_Click(object sender, EventArgs e)
		{

		}
	}
}

using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Globalization;

namespace veritabani
{
    public partial class Form2 : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localhost; port=5432; Database=yurtyonetimi; user Id=postgres; password=123456");
        private Form1 form1;
        public Form2(Form1 form)
        {
            InitializeComponent();
            form1 = form;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Şifreyi byte dizisine dönüştür
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                // Hash hesapla
                byte[] hash = sha256.ComputeHash(bytes);

                // Hash'i hexadecimal formata çevir ve döndür
                StringBuilder result = new StringBuilder();
                foreach (byte b in hash)
                {
                    result.Append(b.ToString("x2"));
                }
                return result.ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanıcıdan alınan verileri TextBox'lardan alıyoruz
                string tcNo = textBox1.Text.Trim();
                string isim = textBox2.Text.Trim();
                string soyisim = textBox3.Text.Trim();
                string telNo = textBox4.Text.Trim();
                string mail = textBox5.Text.Trim();
                int odaNo = int.Parse(textBox6.Text.Trim()); // OdaNo'yu int olarak alıyoruz
                int girisYili = int.Parse(textBox7.Text.Trim()); // Giriş Yılı
                string bolum = textBox8.Text.Trim();
                int sinif = int.Parse(textBox9.Text.Trim()); // Sınıf
                string dogumYiliText = textBox10.Text.Trim(); // Doğum Yılı (string olarak alıyoruz)
                string password = textBox11.Text.Trim(); // Şifreyi alıyoruz

                // Şifreyi hashle
                string hashedPassword = HashPassword(password);

                // Doğum yılı tarih formatını kontrol et ve uygun formata çevir
                DateTime dogumYili;
                if (!DateTime.TryParseExact(dogumYiliText, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dogumYili))
                {
                    MessageBox.Show("Lütfen geçerli bir doğum tarihi girin (gg.aa.yyyy formatında).");
                    return;
                }

                // Veritabanı bağlantısını aç
                baglanti.Open();

                // TC numarasının zaten var olup olmadığını kontrol et (hem student hem de staff tablosunda)
                string tcCheckQuery = "SELECT check_tc_exists(@tcNo)";
                NpgsqlCommand tcCheckCommand = new NpgsqlCommand(tcCheckQuery, baglanti);
                tcCheckCommand.Parameters.AddWithValue("@tcNo", tcNo);

                bool tcExists = Convert.ToBoolean(tcCheckCommand.ExecuteScalar());

                if (tcExists)
                {
                    MessageBox.Show("Bu TC numarasına sahip bir öğrenci veya personel zaten mevcut. Lütfen farklı bir TC numarası girin.");
                    return;
                }

                // SQL sorgusunu hazırlıyoruz
                string sorgu = "INSERT INTO STUDENT (TCNumber, Name, Surname, PhoneNumber, Email, RoomNo, EntryYear, Department, Class, BirthDay, Password) " +
                               "VALUES (@tcNo, @isim, @soyisim, @telNo, @mail, @odaNo, @girisYili, @bolum, @sinif, @dogumYili, @passwordHash)";

                // Parametreli sorgu
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@tcNo", tcNo);
                komut.Parameters.AddWithValue("@isim", isim);
                komut.Parameters.AddWithValue("@soyisim", soyisim);
                komut.Parameters.AddWithValue("@telNo", telNo);
                komut.Parameters.AddWithValue("@mail", mail);
                komut.Parameters.AddWithValue("@odaNo", odaNo);
                komut.Parameters.AddWithValue("@girisYili", girisYili);
                komut.Parameters.AddWithValue("@bolum", bolum);
                komut.Parameters.AddWithValue("@sinif", sinif);
                komut.Parameters.AddWithValue("@dogumYili", dogumYili); // Doğum yılı tarih olarak ekleniyor
                komut.Parameters.AddWithValue("@passwordHash", hashedPassword); // Hashlenmiş şifre ekleniyor

                // Veriyi ekleyip kaydederiz
                komut.ExecuteNonQuery();

                // Başarı mesajı göster
                MessageBox.Show("Öğrenci kaydı başarıyla tamamlandı!");

                // DataGridView'i güncelle
                form1.UpdateStudentGrid();

                // Formu temizle
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
                textBox9.Clear();
                textBox10.Clear();
                textBox11.Clear();
            }
            catch (PostgresException pgEx)
            {
                // Tetikleyici hatalarını burada yakalayabilirsiniz
                if (pgEx.SqlState == "P0001") // Özel tetikleyici hatası
                {
                    MessageBox.Show($"Bir hata oluştu: {pgEx.MessageText}");
                }
                else
                {
                    MessageBox.Show($"Veritabanı hatası: {pgEx.Message}");
                }
            }
            catch (Exception ex)
            {
                // Genel hataları yakala
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Veritabanı bağlantısını kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanıcıdan alınan verileri TextBox'lardan alıyoruz
                string tcNo = textBox1.Text.Trim();
                string isim = textBox2.Text.Trim();
                string soyisim = textBox3.Text.Trim();
                string telNo = textBox4.Text.Trim();
                string mail = textBox5.Text.Trim();
                int odaNo = int.Parse(textBox6.Text.Trim()); // OdaNo'yu int olarak alıyoruz
                int girisYili = int.Parse(textBox7.Text.Trim()); // Giriş Yılı
                string bolum = textBox8.Text.Trim();
                int sinif = int.Parse(textBox9.Text.Trim()); // Sınıf
                string dogumYiliText = textBox10.Text.Trim(); // Doğum Yılı (string olarak alıyoruz)

                // Doğum yılı tarih formatını kontrol et ve uygun formata çevir
                DateTime dogumYili;
                if (!DateTime.TryParseExact(dogumYiliText, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dogumYili))
                {
                    MessageBox.Show("Lütfen geçerli bir doğum tarihi girin (gg.aa.yyyy formatında).");
                    return;
                }

                // Veritabanı bağlantısını aç
                baglanti.Open();

                // TC numarasının veritabanında var olup olmadığını kontrol et
                string tcCheckQuery = "SELECT COUNT(*) FROM STUDENT WHERE TCNumber = @tcNo";
                NpgsqlCommand tcCheckCommand = new NpgsqlCommand(tcCheckQuery, baglanti);
                tcCheckCommand.Parameters.AddWithValue("@tcNo", tcNo);
                int tcCount = Convert.ToInt32(tcCheckCommand.ExecuteScalar());

                if (tcCount == 0)
                {
                    MessageBox.Show("Bu TC numarasına sahip öğrenci bulunamadı.");
                    return;
                }

                // SQL sorgusunu hazırlıyoruz (UPDATE sorgusu)
                string sorgu = "UPDATE STUDENT SET Name = @isim, Surname = @soyisim, PhoneNumber = @telNo, Email = @mail, RoomNo = @odaNo, " +
                               "EntryYear = @girisYili, Department = @bolum, Class = @sinif, BirthDay = @dogumYili WHERE TCNumber = @tcNo";

                // Parametreli sorgu
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@tcNo", tcNo);
                komut.Parameters.AddWithValue("@isim", isim);
                komut.Parameters.AddWithValue("@soyisim", soyisim);
                komut.Parameters.AddWithValue("@telNo", telNo);
                komut.Parameters.AddWithValue("@mail", mail);
                komut.Parameters.AddWithValue("@odaNo", odaNo);
                komut.Parameters.AddWithValue("@girisYili", girisYili);
                komut.Parameters.AddWithValue("@bolum", bolum);
                komut.Parameters.AddWithValue("@sinif", sinif);
                komut.Parameters.AddWithValue("@dogumYili", dogumYili); // Doğum yılı tarih olarak ekleniyor

                // Veriyi güncelleyip kaydederiz
                komut.ExecuteNonQuery();

                // Başarı mesajı göster
                MessageBox.Show("Öğrenci bilgileri başarıyla güncellendi!");

                // DataGridView'i güncelle
                form1.UpdateStudentGrid();

                // Formu temizle
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
                textBox9.Clear();
                textBox10.Clear();
            }
            catch (Exception ex)
            {
                // Genel hataları yakala
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Veritabanı bağlantısını kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Get TC number from textBox1
                string tcNo = textBox1.Text.Trim();

                // Veritabanı bağlantısını aç
                baglanti.Open();

                // Veritabanından öğrenci bilgilerini almak için sorgu
                string sorgu = "SELECT Name, Surname, PhoneNumber, Email, RoomNo, EntryYear, Department, Class, BirthDay FROM STUDENT WHERE TCNumber = @tcNo";

                // Parametreli sorgu
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@tcNo", tcNo);

                // Sorguyu çalıştır ve verileri al
                NpgsqlDataReader reader = komut.ExecuteReader();

                // Eğer öğrenci bulunduysa, bilgileri textboxlara aktar
                if (reader.Read())
                {
                    textBox2.Text = reader["Name"].ToString();
                    textBox3.Text = reader["Surname"].ToString();
                    textBox4.Text = reader["PhoneNumber"].ToString();
                    textBox5.Text = reader["Email"].ToString();
                    textBox6.Text = reader["RoomNo"].ToString();
                    textBox7.Text = reader["EntryYear"].ToString();
                    textBox8.Text = reader["Department"].ToString();
                    textBox9.Text = reader["Class"].ToString();
                    textBox10.Text = Convert.ToDateTime(reader["BirthDay"]).ToString("dd.MM.yyyy");
                }
                else
                {
                    MessageBox.Show("Bu TC numarasına sahip öğrenci bulunamadı.");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Genel hataları yakala
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Veritabanı bağlantısını kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
    }
}

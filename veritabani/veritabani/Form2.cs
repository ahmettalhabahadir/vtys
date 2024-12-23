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
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localhost; port=5432; Database=yurtyonetim; user Id=postgres; password=123456");
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
                // 1. Personel tablosuna veri ekle
                string insertPersonQuery = "INSERT INTO PERSON (TCNumber, Name, Surname, PhoneNumber, Email) " +
                                           "VALUES (@tcNo, @isim, @soyisim, @telNo, @mail)";

                NpgsqlCommand insertPersonCommand = new NpgsqlCommand(insertPersonQuery, baglanti);
                insertPersonCommand.Parameters.AddWithValue("@tcNo", tcNo);
                insertPersonCommand.Parameters.AddWithValue("@isim", isim);
                insertPersonCommand.Parameters.AddWithValue("@soyisim", soyisim);
                insertPersonCommand.Parameters.AddWithValue("@telNo", telNo);
                insertPersonCommand.Parameters.AddWithValue("@mail", mail);
                // Doğum tarihini ekliyoruz

                // Personel tablosuna veri ekleyelim
                insertPersonCommand.ExecuteNonQuery();

                // 2. Öğrenci tablosuna veri ekle
                string insertStudentQuery = "INSERT INTO STUDENT (TCNumber, RoomNo, EntryYear,Birthday , Department, Class, Password) " +
                                           "VALUES (@tcNo, @odaNo,  @girisYili,@dogumYili, @bolum, @sinif,@hashedpassword)";

                NpgsqlCommand insertStudentCommand = new NpgsqlCommand(insertStudentQuery, baglanti);
                insertStudentCommand.Parameters.AddWithValue("@tcNo", tcNo);
                insertStudentCommand.Parameters.AddWithValue("@odaNo", odaNo);
                insertStudentCommand.Parameters.AddWithValue("@girisYili", girisYili);
                insertStudentCommand.Parameters.AddWithValue("@bolum", bolum);
                insertStudentCommand.Parameters.AddWithValue("@sinif", sinif);
                insertStudentCommand.Parameters.AddWithValue("@dogumYili", dogumYili);
                insertStudentCommand.Parameters.AddWithValue("@hashedpassword", hashedPassword);
                // Öğrenci tablosuna veri ekleyelim
                insertStudentCommand.ExecuteNonQuery();

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
                // Veritabanı bağlantısını aç
                baglanti.Open();

                // Transaction başlat
                using (var transaction = baglanti.BeginTransaction())
                {
                    try
                    {
                        // Kullanıcıdan alınan verileri TextBox'lardan alıyoruz
                        string tcNo = textBox1.Text.Trim();
                        string isim = textBox2.Text.Trim();
                        string soyisim = textBox3.Text.Trim();
                        string telNo = textBox4.Text.Trim();
                        string mail = textBox5.Text.Trim();
                        int odaNo = int.Parse(textBox6.Text.Trim());
                        int girisYili = int.Parse(textBox7.Text.Trim());
                        string bolum = textBox8.Text.Trim();
                        int sinif = int.Parse(textBox9.Text.Trim());
                        string dogumYiliText = textBox10.Text.Trim();

                        // Doğum yılı tarih formatını kontrol et ve uygun formata çevir
                        DateTime dogumYili;
                        if (!DateTime.TryParseExact(dogumYiliText, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dogumYili))
                        {
                            MessageBox.Show("Lütfen geçerli bir doğum tarihi girin (gg.aa.yyyy formatında).");
                            return;
                        }

                        // TC numarasının varlığını kontrol et
                        string tcCheckQuery = "SELECT COUNT(*) FROM STUDENT WHERE TCNumber = @tcNo";
                        NpgsqlCommand tcCheckCommand = new NpgsqlCommand(tcCheckQuery, baglanti);
                        tcCheckCommand.Parameters.AddWithValue("@tcNo", tcNo);
                        int tcCount = Convert.ToInt32(tcCheckCommand.ExecuteScalar());

                        if (tcCount == 0)
                        {
                            MessageBox.Show("Bu TC numarasına sahip öğrenci bulunamadı.");
                            return;
                        }

                        // PERSON tablosunu güncelle
                        string updatePersonQuery = "UPDATE PERSON SET Name = @isim, Surname = @soyisim, PhoneNumber = @telNo, Email = @mail WHERE TCNumber = @tcNo";
                        NpgsqlCommand updatePersonCommand = new NpgsqlCommand(updatePersonQuery, baglanti);
                        updatePersonCommand.Parameters.AddWithValue("@tcNo", tcNo);
                        updatePersonCommand.Parameters.AddWithValue("@isim", isim);
                        updatePersonCommand.Parameters.AddWithValue("@soyisim", soyisim);
                        updatePersonCommand.Parameters.AddWithValue("@telNo", telNo);
                        updatePersonCommand.Parameters.AddWithValue("@mail", mail);
                        updatePersonCommand.ExecuteNonQuery();

                        // STUDENT tablosunu güncelle
                        string updateStudentQuery = "UPDATE STUDENT SET RoomNo = @odaNo, EntryYear = @girisYili, Department = @bolum, Class = @sinif, BirthDay = @dogumYili WHERE TCNumber = @tcNo";
                        NpgsqlCommand updateStudentCommand = new NpgsqlCommand(updateStudentQuery, baglanti);
                        updateStudentCommand.Parameters.AddWithValue("@tcNo", tcNo);
                        updateStudentCommand.Parameters.AddWithValue("@odaNo", odaNo);
                        updateStudentCommand.Parameters.AddWithValue("@girisYili", girisYili);
                        updateStudentCommand.Parameters.AddWithValue("@bolum", bolum);
                        updateStudentCommand.Parameters.AddWithValue("@sinif", sinif);
                        updateStudentCommand.Parameters.AddWithValue("@dogumYili", dogumYili);
                        updateStudentCommand.ExecuteNonQuery();

                        // İşlem başarılı olursa transaction'u onayla
                        transaction.Commit();

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
                    catch (Exception)
                    {
                        // Hata durumunda yapılan tüm işlemleri geri al
                        transaction.Rollback();
                        throw; // Hata mesajını bir üst bloğa ilet
                    }
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

        private void button3_Click(object sender, EventArgs e)
        {
            textBox11.Visible = false;
            label12.Visible = false;
            try
            {
                // Get TC number from textBox1
                string tcNo = textBox1.Text.Trim();

                // Veritabanı bağlantısını aç
                baglanti.Open();

                // Veritabanından personel bilgilerini almak için sorgu
                string personQuery = "SELECT Name, Surname, PhoneNumber, Email FROM PERSON WHERE TCNumber = @tcNo";

                // Parametreli sorgu
                NpgsqlCommand personCommand = new NpgsqlCommand(personQuery, baglanti);
                personCommand.Parameters.AddWithValue("@tcNo", tcNo);

                // Personel verilerini al
                NpgsqlDataReader personReader = personCommand.ExecuteReader();

                // Eğer personel bulunduysa, bilgileri textboxlara aktar
                if (personReader.Read())
                {
                    textBox2.Text = personReader["Name"].ToString();
                    textBox3.Text = personReader["Surname"].ToString();
                    textBox4.Text = personReader["PhoneNumber"].ToString();
                    textBox5.Text = personReader["Email"].ToString();
                }
                else
                {
                    MessageBox.Show("Bu TC numarasına sahip personel bulunamadı.");
                    personReader.Close();
                    return;
                }

                personReader.Close();

                // Veritabanından öğrenci bilgilerini almak için sorgu
                string studentQuery = "SELECT RoomNo, EntryYear, Department, Class, BirthDay FROM STUDENT WHERE TCNumber = @tcNo";

                // Parametreli sorgu
                NpgsqlCommand studentCommand = new NpgsqlCommand(studentQuery, baglanti);
                studentCommand.Parameters.AddWithValue("@tcNo", tcNo);

                // Öğrenci verilerini al
                NpgsqlDataReader studentReader = studentCommand.ExecuteReader();

                // Eğer öğrenci bulunduysa, bilgileri textboxlara aktar
                if (studentReader.Read())
                {
                    textBox6.Text = studentReader["RoomNo"].ToString();
                    textBox7.Text = studentReader["EntryYear"].ToString();
                    textBox8.Text = studentReader["Department"].ToString();
                    textBox9.Text = studentReader["Class"].ToString();
                    textBox10.Text = Convert.ToDateTime(studentReader["BirthDay"]).ToString("dd.MM.yyyy");
                }
                else
                {
                    MessageBox.Show("Bu TC numarasına ait öğrenci bilgisi bulunamadı.");
                }

                studentReader.Close();
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

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox11.Visible = true;
            label12.Visible = true;
        }
    }
}

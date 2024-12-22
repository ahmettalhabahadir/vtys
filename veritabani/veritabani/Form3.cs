using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace veritabani
{
    public partial class Form3 : Form
    {
        private Form1 form1;
        public Form3(Form1 form)
        {
            InitializeComponent();
            form1 = form;
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=yurtyonetimi; user Id=postgres; password=123456");

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanıcıdan alınan bilgileri TextBox'lardan alıyoruz
                string tcNumber = textBox1.Text.Trim();
                string name = textBox2.Text.Trim();
                string surname = textBox3.Text.Trim();
                string role = textBox4.Text.Trim();
                string phoneNumber = textBox5.Text.Trim();
                int entryYear;

                // EntryYear'ı kontrol ediyoruz (int’e çevrilebilecek mi)
                if (!int.TryParse(textBox6.Text.Trim(), out entryYear))
                {
                    MessageBox.Show("Giriş yılı geçerli bir sayı olmalıdır.");
                    return;
                }

                // Veritabanı bağlantısını açıyoruz
                baglanti.Open();

                // TC numarasının öğrenci veya personel tablosunda var olup olmadığını kontrol ediyoruz
                string tcCheckQuery = "SELECT check_tc_exists(@tcNo)";
                NpgsqlCommand tcCheckCommand = new NpgsqlCommand(tcCheckQuery, baglanti);
                tcCheckCommand.Parameters.AddWithValue("@tcNo", tcNumber);

                // Fonksiyonu çağırıyoruz ve sonucu kontrol ediyoruz
                bool tcExists = Convert.ToBoolean(tcCheckCommand.ExecuteScalar());

                if (tcExists)
                {
                    MessageBox.Show("Bu TC numarasına sahip bir öğrenci veya personel zaten mevcut.");
                    return;
                }

                // Eğer rol "Yurt Müdürü" ise, zaten bir müdür olup olmadığını kontrol ediyoruz
                if (role.Equals("Yurt Müdürü", StringComparison.OrdinalIgnoreCase))
                {
                    string query = "SELECT COUNT(*) FROM STAFF WHERE Role = 'Yurt Müdürü'";
                    NpgsqlCommand checkCommand = new NpgsqlCommand(query, baglanti);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Zaten bir Yurt Müdürü mevcut. Başka bir müdür atanamaz.");
                        return;
                    }
                }

                // Personel veritabanına ekleme sorgusunu oluşturuyoruz
                string insertQuery = "INSERT INTO STAFF (tcNumber, Name, Surname, Role, PhoneNumber, EntryYear) " +
                                     "VALUES (@tcNumber, @name, @surname, @role, @phoneNumber, @entryYear)";

                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);
                insertCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                insertCommand.Parameters.AddWithValue("@name", name);
                insertCommand.Parameters.AddWithValue("@surname", surname);
                insertCommand.Parameters.AddWithValue("@role", role);
                insertCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                insertCommand.Parameters.AddWithValue("@entryYear", entryYear);

                // Veriyi ekleyip kaydediyoruz
                insertCommand.ExecuteNonQuery();

                MessageBox.Show("Personel başarıyla kaydedildi.");
                form1.UpdateStaffGrid();
                // Formdaki TextBox'ları temizliyoruz
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya mesaj gösteriyoruz
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Bağlantıyı kapatıyoruz
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
                // TC numarasını alıyoruz
                string tcNumber = textBox1.Text.Trim();

                // Veritabanı bağlantısını açıyoruz
                baglanti.Open();

                // Personel bilgilerini almak için sorgu
                string query = "SELECT Name, Surname, Role, PhoneNumber, EntryYear FROM STAFF WHERE tcNumber = @tcNumber";
                NpgsqlCommand command = new NpgsqlCommand(query, baglanti);
                command.Parameters.AddWithValue("@tcNumber", tcNumber);

                NpgsqlDataReader reader = command.ExecuteReader();

                // Eğer personel bulunduysa, bilgileri TextBox'lara aktar
                if (reader.Read())
                {
                    textBox2.Text = reader["Name"].ToString();
                    textBox3.Text = reader["Surname"].ToString();
                    textBox4.Text = reader["Role"].ToString();
                    textBox5.Text = reader["PhoneNumber"].ToString();
                    textBox6.Text = reader["EntryYear"].ToString();
                }
                else
                {
                    MessageBox.Show("Bu TC numarasına sahip personel bulunamadı.");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya mesaj gösteriyoruz
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Bağlantıyı kapatıyoruz
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanıcıdan alınan bilgileri TextBox'lardan alıyoruz
                string tcNumber = textBox1.Text.Trim();
                string name = textBox2.Text.Trim();
                string surname = textBox3.Text.Trim();
                string role = textBox4.Text.Trim();
                string phoneNumber = textBox5.Text.Trim();
                int entryYear;

                // EntryYear'ı kontrol ediyoruz (int’e çevrilebilecek mi)
                if (!int.TryParse(textBox6.Text.Trim(), out entryYear))
                {
                    MessageBox.Show("Giriş yılı geçerli bir sayı olmalıdır.");
                    return;
                }

                // Veritabanı bağlantısını açıyoruz
                baglanti.Open();

                // TC numarasının personel tablosunda var olup olmadığını kontrol ediyoruz
                string tcCheckQuery = "SELECT COUNT(*) FROM STAFF WHERE tcNumber = @tcNumber";
                NpgsqlCommand tcCheckCommand = new NpgsqlCommand(tcCheckQuery, baglanti);
                tcCheckCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                int count = Convert.ToInt32(tcCheckCommand.ExecuteScalar());

                if (count == 0)
                {
                    MessageBox.Show("Bu TC numarasına sahip personel bulunamadı.");
                    return;
                }

                // Personel veritabanını güncelleme sorgusunu oluşturuyoruz
                string updateQuery = "UPDATE STAFF SET Name = @name, Surname = @surname, Role = @role, PhoneNumber = @phoneNumber, EntryYear = @entryYear " +
                                     "WHERE tcNumber = @tcNumber";

                NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, baglanti);
                updateCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                updateCommand.Parameters.AddWithValue("@name", name);
                updateCommand.Parameters.AddWithValue("@surname", surname);
                updateCommand.Parameters.AddWithValue("@role", role);
                updateCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                updateCommand.Parameters.AddWithValue("@entryYear", entryYear);

                // Veriyi güncelleyip kaydediyoruz
                updateCommand.ExecuteNonQuery();

                MessageBox.Show("Personel bilgileri başarıyla güncellendi.");
                form1.UpdateStaffGrid();
                // Formdaki TextBox'ları temizliyoruz
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya mesaj gösteriyoruz
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Bağlantıyı kapatıyoruz
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
    }
}

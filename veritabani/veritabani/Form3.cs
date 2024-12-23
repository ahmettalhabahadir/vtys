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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localhost; port=5432; Database=yurtyonetim; user Id=postgres; password=123456");

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
                string email = textBox6.Text.Trim();

                // Veritabanı bağlantısını açıyoruz
                baglanti.Open();

                // Transaction başlat
                using (var transaction = baglanti.BeginTransaction())
                {
                    try
                    {
                        // TC numarasının öğrenci veya personel tablosunda var olup olmadığını kontrol ediyoruz
                        string tcCheckQuery = "SELECT check_tc_exists(@tcNo)";
                        NpgsqlCommand tcCheckCommand = new NpgsqlCommand(tcCheckQuery, baglanti);
                        tcCheckCommand.Parameters.AddWithValue("@tcNo", tcNumber);

                        // Fonksiyonu çağırıyoruz ve sonucu kontrol ediyoruz
                        bool tcExists = Convert.ToBoolean(tcCheckCommand.ExecuteScalar());

                        if (tcExists)
                        {
                            MessageBox.Show("Bu TC numarasına sahip bir öğrenci veya personel zaten mevcut.");
                            transaction.Rollback();
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
                                transaction.Rollback();
                                return;
                            }
                        }

                        // 1. PERSON tablosuna ekleme veya güncelleme sorgusu
                        string updatePersonQuery = "INSERT INTO PERSON (tcNumber, Name, Surname, PhoneNumber, Email) " +
                                                   "VALUES (@tcNumber, @name, @surname, @phoneNumber, @email) " +
                                                   "ON CONFLICT (tcNumber) DO UPDATE SET Name = @name, Surname = @surname, PhoneNumber = @phoneNumber, Email = @email";

                        NpgsqlCommand updatePersonCommand = new NpgsqlCommand(updatePersonQuery, baglanti);
                        updatePersonCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                        updatePersonCommand.Parameters.AddWithValue("@name", name);
                        updatePersonCommand.Parameters.AddWithValue("@surname", surname);
                        updatePersonCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        updatePersonCommand.Parameters.AddWithValue("@email", email);

                        // Personel bilgilerini ekle/güncelle
                        updatePersonCommand.ExecuteNonQuery();

                        // 2. STAFF tablosuna ekleme sorgusu
                        string insertStaffQuery = "INSERT INTO STAFF (tcNumber, Role) " +
                                                  "VALUES (@tcNumber, @role)";

                        NpgsqlCommand insertStaffCommand = new NpgsqlCommand(insertStaffQuery, baglanti);
                        insertStaffCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                        insertStaffCommand.Parameters.AddWithValue("@role", role);

                        // STAFF tablosuna ekle
                        insertStaffCommand.ExecuteNonQuery();

                        // Transaction'ı başarıyla tamamla
                        transaction.Commit();

                        MessageBox.Show("Personel başarıyla kaydedildi.");
                        form1.UpdateStaffGrid();

                        // Formdaki TextBox'ları temizle
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                        textBox6.Clear();
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda işlemleri geri al
                        transaction.Rollback();
                        MessageBox.Show($"Bir hata oluştu ve işlemler geri alındı: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Genel hata durumu
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

                // Personel bilgilerini almak için STAFF tablosundan sorgu
                string staffQuery = "SELECT Role FROM STAFF WHERE TCNumber = @tcNumber";
                NpgsqlCommand staffCommand = new NpgsqlCommand(staffQuery, baglanti);
                staffCommand.Parameters.AddWithValue("@tcNumber", tcNumber);

                NpgsqlDataReader staffReader = staffCommand.ExecuteReader();

                // Eğer personel STAFF tablosunda bulunduysa, bilgileri TextBox'lara aktar
                if (staffReader.Read())
                {
                    textBox4.Text = staffReader["Role"].ToString();
                }
                else
                {
                    MessageBox.Show("Bu TC numarasına sahip personel STAFF tablosunda bulunamadı.");
                    return; // Çıkış yapıyoruz, çünkü personel bulunamadı.
                }

                staffReader.Close();

                // Ayrıca, personel bilgilerini PERSON tablosundan da alıyoruz
                string personQuery = "SELECT Name, Surname, PhoneNumber, Email FROM PERSON WHERE TCNumber = @tcNumber";
                NpgsqlCommand personCommand = new NpgsqlCommand(personQuery, baglanti);
                personCommand.Parameters.AddWithValue("@tcNumber", tcNumber);

                NpgsqlDataReader personReader = personCommand.ExecuteReader();

                // Eğer personel PERSON tablosunda bulunduysa, bilgileri TextBox'lara aktar
                if (personReader.Read())
                {
                    textBox2.Text = personReader["Name"].ToString();
                    textBox3.Text = personReader["Surname"].ToString();
                    textBox5.Text = personReader["PhoneNumber"].ToString();
                    textBox6.Text = personReader["Email"].ToString(); // Yeni Email bilgisi
                }
                else
                {
                    MessageBox.Show("Person tablosunda bu TC numarasına ait kişisel bilgiler bulunamadı.");
                }

                personReader.Close();
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
                string email = textBox6.Text.Trim();  // Yeni Email bilgisi
         

               

                // Veritabanı bağlantısını açıyoruz
                baglanti.Open();

                // TC numarasının personel tablosunda var olup olmadığını kontrol ediyoruz
                string tcCheckQuery = "SELECT COUNT(*) FROM STAFF WHERE TCNumber = @tcNumber";
                NpgsqlCommand tcCheckCommand = new NpgsqlCommand(tcCheckQuery, baglanti);
                tcCheckCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                int count = Convert.ToInt32(tcCheckCommand.ExecuteScalar());

                if (count == 0)
                {
                    MessageBox.Show("Bu TC numarasına sahip personel bulunamadı.");
                    return;
                }

                // STAFF tablosundaki personel verisini güncelleme sorgusu
                string updateStaffQuery = "UPDATE STAFF SET Role = @role WHERE TCNumber = @tcNumber";
                NpgsqlCommand updateStaffCommand = new NpgsqlCommand(updateStaffQuery, baglanti);
                updateStaffCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                updateStaffCommand.Parameters.AddWithValue("@role", role);

                // STAFF tablosundaki veriyi güncelliyoruz
                updateStaffCommand.ExecuteNonQuery();

                // person tablosundaki kişisel bilgileri güncelleme sorgusu
                string updatePersonQuery = "UPDATE PERSON SET Name = @name, Surname = @surname, PhoneNumber = @phoneNumber, Email = @email " +
                                           "WHERE TCNumber = @tcNumber";

                NpgsqlCommand updatePersonCommand = new NpgsqlCommand(updatePersonQuery, baglanti);
                updatePersonCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                updatePersonCommand.Parameters.AddWithValue("@name", name);
                updatePersonCommand.Parameters.AddWithValue("@surname", surname);
                updatePersonCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                updatePersonCommand.Parameters.AddWithValue("@email", email);

                // PERSON tablosundaki veriyi güncelliyoruz
                updatePersonCommand.ExecuteNonQuery();

                // Başarı mesajı
                MessageBox.Show("Personel bilgileri başarıyla güncellendi.");

                // Formdaki DataGridView'i güncelleyerek değişiklikleri yansıtıyoruz
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
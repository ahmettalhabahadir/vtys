using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace veritabani
{
    public partial class studentpanel : Form
    {
        public string TcNo { get; set; }
        private signup signupForm;
        public studentpanel(signup signup)
        {
            InitializeComponent();
            signupForm = signup;
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=yurtyonetim; user Id=postgres; password=123456");
        private void UpdateFoodMenuGrid()
        {
            try
            {
                // Veritabanı bağlantısını aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Belirli bir öğrenciye ait yemekleri almak için SQL sorgusu
                string query = "SELECT date AS \"Tarih\", breakfast AS \"Kahvaltı\", dinner AS \"Akşam Yemeği\" FROM food_menu WHERE tcnumber = @tcnumber ORDER BY date ASC";


                // Komutu oluştur ve parametre ekle
                NpgsqlCommand command = new NpgsqlCommand(query, baglanti);
                command.Parameters.AddWithValue("@tcnumber", TcNo); // `TcNo` giriş yapan öğrencinin TC numarasıdır

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView1'e verileri bağla
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya mesaj göster
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Bağlantıyı kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

        }
        private void UpdateLaundryGrid()
        {
            try
            {
                // Veritabanı bağlantısını aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Öğrencinin randevularını almak için sorgu
                string query = "SELECT laundryid AS \"Makine\", date AS \"Tarih\", time AS \"Saat\" FROM laundry WHERE tcnumber = @tcNo ORDER BY date, time";

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@tcNo", TcNo); // Giriş yapan öğrencinin TC numarası

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView3'e verileri bağla
                dataGridView3.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya mesaj göster
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Bağlantıyı kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
        private void UpdatePaymentGrid()
        {
            try
            {
                // Bağlantıyı aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Ödeme bilgilerini al
                string query = "SELECT tcnumber AS \"TC No\", amount AS \"Tutar\", duedate AS \"Ödeme Tarihi\", status AS \"Durum\" " +
                                    "FROM payment WHERE tcnumber = @tcNo ORDER BY duedate DESC";
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // TC numarasına göre filtrele
                dataAdapter.SelectCommand.Parameters.AddWithValue("@tcNo", TcNo);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView'e bağla
                dataGridView2.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Bağlantıyı kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
        private void UpdateEventsGrid()
        {
            try
            {
                // Veritabanı bağlantısını kontrol et ve aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Etkinlik tablosundan verileri tarihe göre sıralı al
                string query = "SELECT eventid AS \"Etkinlik ID\", name AS \"Etkinlik Adı\", description AS \"Açıklama\", date AS \"Tarih\", organizer AS \"Organizatör\", time AS \"Saat\" " +
                 "FROM events ORDER BY date ASC";

                // DataAdapter ile verileri al
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView4'e verileri bağla
                dataGridView9.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi ver
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
        private void UpdateEventsGrid1()
        {
            try
            {
                // Veritabanı bağlantısını kontrol et ve aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Öğrencinin TC numarasını al
                string tcNo = TcNo;  // Burada TcNo'yu Form'dan veya başka bir yerden alabilirsiniz

                // SQL sorgusu: student_events ve events tablolarını inner join ile birleştir, TC numarasına göre filtrele
                string query = "SELECT se.tcnumber AS \"Öğrenci Tc No\", e.eventid AS \"Etkinlik ID\", e.name AS \"Etkinlik Adı\", " +
                               "e.description AS \"Açıklama\", e.date AS \"Tarih\", e.organizer AS \"Organizatör\", e.time AS \"Saat\" " +
                               "FROM student_events se " +
                               "INNER JOIN events e ON se.EventID = e.EventID " +
                               "WHERE se.tcnumber = @tcNo " +  // TC numarasına göre filtreleme
                               "ORDER BY e.date ASC";

                // DataAdapter ile verileri al
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@tcNo", tcNo);  // Parametreyi ekle

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView4'e verileri bağla
                dataGridView4.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi ver
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
        private void UpdatePermissionGrid()
        {
            try
            {
                // Veritabanı bağlantısını aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Öğrencinin izin bilgilerini almak için sorgu
                string query = "SELECT permissionid AS \"İzin No\", startdate AS \"Başlangıç Tarihi\", enddate AS \"Bitiş Tarihi\", reason AS \"Açıklama\" " +
                               "FROM permission WHERE tcnumber = @tcnumber ORDER BY startdate DESC";

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@tcnumber", TcNo); // Öğrencinin TC numarası

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView5'e verileri bağla
                dataGridView5.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Bağlantıyı kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
        private void UpdateRulesGrid()
        {
            try
            {
                // Veritabanı bağlantısını aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Kurallar tablosundan verileri al ve sıralı getir
                string query = "SELECT ruletext AS \"Kural Metni\", effectivedate AS \"Yürürlük Tarihi\" FROM rules ORDER BY effectivedate ASC";

                // DataAdapter ile verileri al
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView6'ya verileri bağla
                dataGridView6.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi ver
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
        private void UpdatecomplaintGrid()
        {
            try
            {
                // Veritabanı bağlantısını aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Öğrencinin şikayetlerini almak için sorgu
                string query = "SELECT complaintdate AS \"Tarih\", description AS \"Açıklama\", status AS \"Durum\" " +
                               "FROM complaint WHERE tcnumber = @tcnumber ORDER BY complaintdate DESC";

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@tcnumber", TcNo); // Öğrencinin TC numarası

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView7'ye verileri bağla
                dataGridView7.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Bağlantıyı kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
        private void UpdateStudentInfoGrid()
        {
            try
            {
                // Veritabanı bağlantısını aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Öğrencinin tüm bilgilerini al
                string query = "SELECT s.tcnumber AS \"TC No\", p.name AS \"Ad\", p.surname AS \"Soyad\", p.phonenumber AS \"Telefon\", p.email AS \"E-posta\", " +
                               "s.roomno AS \"Oda No\", s.entryyear AS \"Giriş Yılı\", s.birthday AS \"Doğum Tarihi\", s.department AS \"Bölüm\", s.class AS \"Sınıf\" " +
                               "FROM student s " +
                               "INNER JOIN person p ON s.tcnumber = p.tcnumber " +
                               "WHERE p.tcnumber = @tcnumber";

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@tcnumber", TcNo);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView8'e verileri bağla
                dataGridView8.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

        }

        private void studentpanel_Load(object sender, EventArgs e)
        {
            UpdateFoodMenuGrid();
            UpdateLaundryGrid();
            UpdatePaymentGrid();
            UpdateEventsGrid();
            UpdateRulesGrid();
            UpdatecomplaintGrid();
            UpdateStudentInfoGrid();
            UpdateEventsGrid1();
        }

        private void studentpanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            signupForm.Close();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Randevu için gerekli bilgileri al
                int laundryId = int.Parse(comboBox1.SelectedItem.ToString()); // Çamaşır makinesi ID'si
                string time = comboBox2.SelectedItem.ToString(); // Saat seçimi
                DateTime date = dateTimePicker1.Value.Date; // Tarih seçimi

                // Eğer gerekli alanlar boşsa hata mesajı göster
                if (laundryId == 0 || string.IsNullOrEmpty(time))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun.");
                    return;
                }

                // Veritabanı bağlantısını aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Aynı tarih, saat ve çamaşır makinesi için randevu var mı kontrol et
                string checkQuery = "SELECT check_laundry_appointment(@laundryId, @date, @time)";
                NpgsqlCommand checkCommand = new NpgsqlCommand(checkQuery, baglanti);
                checkCommand.Parameters.AddWithValue("@laundryId", laundryId);
                checkCommand.Parameters.AddWithValue("@date", date);
                checkCommand.Parameters.AddWithValue("@time", time);

                bool appointmentExists = (bool)checkCommand.ExecuteScalar();

                if (appointmentExists)
                {
                    MessageBox.Show("Bu tarih, saat ve çamaşır makinesi için zaten bir randevu var.");
                    return;
                }

                // Randevuyu ekleme sorgusu
                string insertQuery = "INSERT INTO laundry (laundryid, tcnumber, date, time) VALUES (@laundryId, @tcNo, @date, @time)";
                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);
                insertCommand.Parameters.AddWithValue("@laundryId", laundryId);
                insertCommand.Parameters.AddWithValue("@tcNo", TcNo); // Giriş yapan öğrencinin TC numarası
                insertCommand.Parameters.AddWithValue("@date", date);
                insertCommand.Parameters.AddWithValue("@time", time);  // Saat değerini text olarak ekliyoruz

                insertCommand.ExecuteNonQuery();

                MessageBox.Show("Randevunuz başarıyla alındı!");

                // DataGridView3'ü güncelle
                UpdateLaundryGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Bağlantıyı kapat
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
                // Seçilen bilgileri al
                int laundryId = int.Parse(comboBox1.SelectedItem.ToString()); // Çamaşır makinesi ID'si
                string time = comboBox2.SelectedItem.ToString(); // Seçilen saat
                DateTime date = dateTimePicker1.Value.Date; // Seçilen tarih

                // Eğer gerekli alanlar boşsa hata mesajı göster
                if (laundryId == 0 || string.IsNullOrEmpty(time))
                {
                    MessageBox.Show("Lütfen makine, tarih ve saat bilgilerini seçin.");
                    return;
                }

                // Veritabanı bağlantısını aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Randevuyu silme sorgusu
                string deleteQuery = "DELETE FROM laundry WHERE laundryid = @laundryId AND date = @date AND time = @time AND tcnumber = @tcNo";
                NpgsqlCommand deleteCommand = new NpgsqlCommand(deleteQuery, baglanti);
                deleteCommand.Parameters.AddWithValue("@laundryId", laundryId);
                deleteCommand.Parameters.AddWithValue("@date", date);
                deleteCommand.Parameters.AddWithValue("@time", time);
                deleteCommand.Parameters.AddWithValue("@tcNo", TcNo); // Öğrencinin TC numarasına göre silme

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                // İşlem sonucuna göre bilgi ver
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Randevu başarıyla iptal edildi.");
                }
                else
                {
                    MessageBox.Show("Seçilen kriterlere uygun bir randevu bulunamadı.");
                }

                // DataGridView3'ü güncelle
                UpdateLaundryGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Bağlantıyı kapat
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
                // Ödeme bilgileri
                decimal amount = 1000.00m; // Ödeme miktarı
                DateTime currentDate = DateTime.Now; // Mevcut tarih
                string paymentStatus = "Paid"; // Ödeme durumu (başlangıçta 'Paid' olarak kabul ediliyor, ancak trigger bunu kontrol eder)

                // Bağlantıyı aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Aynı ay içinde ödeme yapılmış mı kontrol et (Stored Function kullanımı)
                string checkQuery = "SELECT check_payment_status(@tcNo, @year, @month)";
                NpgsqlCommand checkCommand = new NpgsqlCommand(checkQuery, baglanti);
                checkCommand.Parameters.AddWithValue("@tcNo", TcNo); // Öğrenciye ait TC
                checkCommand.Parameters.AddWithValue("@year", currentDate.Year); // Geçerli yıl
                checkCommand.Parameters.AddWithValue("@month", currentDate.Month); // Geçerli ay

                bool hasPaid = Convert.ToBoolean(checkCommand.ExecuteScalar());

                if (hasPaid)
                {
                    // Aynı ay içinde ödeme yapılmış
                    MessageBox.Show("Zaten bu ay içinde ödeme yapılmış.");
                }
                else
                {
                    // Yeni ödeme ekle
                    string insertQuery = "INSERT INTO payment (tcnumber, amount, duedate, status) VALUES (@tcNo, @amount, @duedate, @status)";
                    NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);
                    insertCommand.Parameters.AddWithValue("@tcNo", TcNo);
                    insertCommand.Parameters.AddWithValue("@amount", amount);
                    insertCommand.Parameters.AddWithValue("@duedate", currentDate); // Bugünün tarihi
                    insertCommand.Parameters.AddWithValue("@status", paymentStatus); // Trigger otomatik olarak durumu belirleyecek

                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Ödeme başarıyla yapıldı.");
                    }
                    else
                    {
                        MessageBox.Show("Ödeme sırasında bir hata oluştu.");
                    }
                }

                // Ödeme listesini güncelle
                UpdatePaymentGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Bağlantıyı kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Tarih bilgilerini ve açıklamayı al
                DateTime startDate = dateTimePicker2.Value.Date; // Başlangıç tarihi
                DateTime endDate = dateTimePicker3.Value.Date;   // Bitiş tarihi
                string reason = textBox1.Text.Trim();           // Açıklama

                // Tarih kontrolü: Başlangıç tarihi bitiş tarihinden büyükse hata ver
                if (startDate > endDate)
                {
                    MessageBox.Show("Başlangıç tarihi bitiş tarihinden büyük olamaz.");
                    return;
                }

                // Eğer açıklama boşsa kullanıcıyı uyar
                if (string.IsNullOrEmpty(reason))
                {
                    MessageBox.Show("Lütfen bir açıklama girin.");
                    return;
                }

                // Veritabanı bağlantısını aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // İzin ekleme sorgusu
                string insertQuery = "INSERT INTO permission (tcnumber, startdate, enddate, reason) VALUES (@tcnumber, @startdate, @enddate, @reason)";
                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);

                // Parametreleri ekle
                insertCommand.Parameters.AddWithValue("@tcnumber", TcNo);      // Öğrencinin TC numarası
                insertCommand.Parameters.AddWithValue("@startdate", startDate); // Başlangıç tarihi
                insertCommand.Parameters.AddWithValue("@enddate", endDate);     // Bitiş tarihi
                insertCommand.Parameters.AddWithValue("@reason", reason);       // Açıklama

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("İzin başarıyla kaydedildi.");
                }
                else
                {
                    MessageBox.Show("İzin kaydedilirken bir hata oluştu.");
                }

                // DataGridView5'i güncelle
                UpdatePermissionGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                // Bağlantıyı kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanıcının TextBox2'ye girdiği izin ID'sini al
                if (!int.TryParse(textBox2.Text.Trim(), out int permissionId))
                {
                    MessageBox.Show("Lütfen geçerli bir izin ID'si girin.");
                    return;
                }

                // Veritabanı bağlantısını aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // İzin ID'sine göre silme sorgusu
                string deleteQuery = "DELETE FROM permission WHERE permissionid = @permissionId AND tcnumber = @tcnumber";
                NpgsqlCommand deleteCommand = new NpgsqlCommand(deleteQuery, baglanti);

                // Parametreleri ekle
                deleteCommand.Parameters.AddWithValue("@permissionId", permissionId);
                deleteCommand.Parameters.AddWithValue("@tcnumber", TcNo); // Giriş yapan öğrencinin TC numarası

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("İzin başarıyla iptal edildi.");
                }
                else
                {
                    MessageBox.Show("Bu izin ID'sine ait bir kayıt bulunamadı veya izin size ait değil.");
                }

                // İzin listesini güncelle
                UpdatePermissionGrid();
            }
            catch (Exception ex)
            {
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

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                // RichTextBox'tan açıklamayı al
                string description = richTextBox1.Text.Trim();

                // Eğer açıklama boşsa kullanıcıyı uyar
                if (string.IsNullOrEmpty(description))
                {
                    MessageBox.Show("Lütfen bir şikayet metni girin.");
                    return;
                }

                // Veritabanı bağlantısını aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Şikayet ekleme sorgusu
                string insertQuery = "INSERT INTO complaint (tcnumber, complaintdate, description, status) " +
                                     "VALUES (@tcnumber, @complaintdate, @description, @status)";
                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);

                // Parametreleri ekle
                insertCommand.Parameters.AddWithValue("@tcnumber", TcNo);               // Öğrencinin TC numarası
                insertCommand.Parameters.AddWithValue("@complaintdate", DateTime.Now); // Şikayet tarihi (şu anki tarih)
                insertCommand.Parameters.AddWithValue("@description", description);    // Şikayet açıklaması
                insertCommand.Parameters.AddWithValue("@status", "Pending");          // Başlangıçta "Bekliyor" durumu

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Şikayet başarıyla kaydedildi.");
                }
                else
                {
                    MessageBox.Show("Şikayet kaydedilirken bir hata oluştu.");
                }

                // Şikayet listesini güncelle
                UpdatecomplaintGrid();

                // RichTextBox'ı temizle
                richTextBox1.Clear();
            }
            catch (Exception ex)
            {
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

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                // Yeni telefon numarası ve e-posta adresi
                string newPhoneNumber = textBox3.Text.Trim();
                string newEmail = textBox4.Text.Trim();

                // Alanların boş olup olmadığını kontrol et
                if (string.IsNullOrEmpty(newPhoneNumber) || string.IsNullOrEmpty(newEmail))
                {
                    MessageBox.Show("Lütfen hem telefon numarasını hem de e-posta adresini girin.");
                    return;
                }

                // Veritabanı bağlantısını aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Güncelleme sorgusu: `person` tablosundaki telefon numarası ve e-posta adresini güncelliyoruz
                string updateQuery = "UPDATE person SET phonenumber = @newPhoneNumber, email = @newEmail WHERE tcnumber = @tcnumber";
                NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, baglanti);

                // Parametreleri ekle
                updateCommand.Parameters.AddWithValue("@newPhoneNumber", newPhoneNumber);
                updateCommand.Parameters.AddWithValue("@newEmail", newEmail);
                updateCommand.Parameters.AddWithValue("@tcnumber", TcNo); // Öğrencinin TC numarası

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("İletişim bilgileri başarıyla güncellendi.");
                }
                else
                {
                    MessageBox.Show("Bilgiler güncellenirken bir hata oluştu.");
                }

                // Öğrenci bilgilerini güncelle
                UpdateStudentInfoGrid(); // Öğrencinin bilgilerini grid üzerinde güncellemeyi unutma
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                // Mevcut ve yeni şifre
                string currentPassword = textBox5.Text.Trim();
                string newPassword = textBox6.Text.Trim();

                // Alanların boş olup olmadığını kontrol et
                if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
                {
                    MessageBox.Show("Lütfen mevcut şifreyi ve yeni şifreyi girin.");
                    return;
                }

                // Mevcut şifreyi hashle
                string hashedCurrentPassword = HashPassword(currentPassword);

                // Veritabanı bağlantısını aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Mevcut şifrenin doğru olup olmadığını kontrol et
                string checkQuery = "SELECT COUNT(*) FROM student WHERE tcnumber = @tcnumber AND password = @currentPassword";
                NpgsqlCommand checkCommand = new NpgsqlCommand(checkQuery, baglanti);
                checkCommand.Parameters.AddWithValue("@tcnumber", TcNo); // TC numarası
                checkCommand.Parameters.AddWithValue("@currentPassword", hashedCurrentPassword);

                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count == 0)
                {
                    MessageBox.Show("Mevcut şifre hatalı.");
                    return;
                }

                // Yeni şifreyi hashle
                string hashedNewPassword = HashPassword(newPassword);

                // Şifre güncelleme sorgusu
                string updateQuery = "UPDATE student SET password = @newPassword WHERE tcnumber = @tcnumber";
                NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, baglanti);
                updateCommand.Parameters.AddWithValue("@newPassword", hashedNewPassword);
                updateCommand.Parameters.AddWithValue("@tcnumber", TcNo);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Şifre başarıyla güncellendi.");
                }
                else
                {
                    MessageBox.Show("Şifre güncellenirken bir hata oluştu.");
                }

                // Alanları temizle
                textBox5.Clear();
                textBox6.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
        private string HashPassword(string password)
        {
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                // Veritabanı bağlantısını kontrol et ve aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // TextBox7'den etkinlik ID'sini al
                int eventId;
                if (!int.TryParse(textBox7.Text.Trim(), out eventId))
                {
                    MessageBox.Show("Lütfen geçerli bir etkinlik ID'si girin.");
                    return;
                }

                // Öğrencinin TC numarasını al (örneğin, formdan veya login işleminden alınabilir)
                string tcNumber = TcNo;  // TcNo burada öğrenci TC'si olmalıdır, formdan alınabilir

                // Etkinlik ID'sinin events tablosunda geçerli olup olmadığını kontrol et
                string eventCheckQuery = "SELECT COUNT(*) FROM events WHERE eventid = @eventId";
                NpgsqlCommand eventCheckCommand = new NpgsqlCommand(eventCheckQuery, baglanti);
                eventCheckCommand.Parameters.AddWithValue("@eventId", eventId);

                int eventExists = Convert.ToInt32(eventCheckCommand.ExecuteScalar());

                if (eventExists == 0)
                {
                    MessageBox.Show("Girilen etkinlik ID'si geçerli değil.");
                    return;
                }

                // Öğrencinin zaten etkinliğe katılıp katılmadığını kontrol et
                string checkQuery = "SELECT COUNT(*) FROM student_events WHERE tcnumber = @tcNumber AND eventid = @eventId";
                NpgsqlCommand checkCommand = new NpgsqlCommand(checkQuery, baglanti);
                checkCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                checkCommand.Parameters.AddWithValue("@eventId", eventId);

                int existingRecordCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (existingRecordCount > 0)
                {
                    MessageBox.Show("Bu öğrenci zaten bu etkinliğe katıldı.");
                    return;
                }

                // Etkinliğe katılımı student_events tablosuna kaydedecek SQL sorgusu
                string insertQuery = "INSERT INTO student_events (tcnumber, eventid) VALUES (@tcNumber, @eventId)";

                // SQL komutunu oluştur
                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);
                insertCommand.Parameters.AddWithValue("@tcNumber", tcNumber); // Öğrenci TC numarasını ekle
                insertCommand.Parameters.AddWithValue("@eventId", eventId);   // Etkinlik ID'sini ekle

                // Veriyi ekle
                insertCommand.ExecuteNonQuery();

                MessageBox.Show("Öğrenci etkinliğe katıldı.");
                UpdateEventsGrid1(); // Etkinlikler gridini güncelle

                // Gerekirse ekleme işleminden sonra TextBox7'yi temizleyebilirsiniz
                textBox7.Clear();
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi ver
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

using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace veritabani
{
    public partial class Form1 : Form
    {
        private signup signupForm;
        public string TcNo { get; set; }
        public Form1(signup signup)
        {
            InitializeComponent();
            signupForm = signup;

        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=yurtyonetim; user Id=postgres; password=123456");


        public void UpdateStudentGrid()
        {
            try
            {
                // Baðlantýyý aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Öðrenciler ve personel bilgilerini birleþtiren sorgu
                string sorgu = "SELECT s.tcnumber AS \"TC Kimlik No\", p.name AS \"Ad\", p.surname AS \"Soyad\", p.phonenumber AS \"Tel No\", p.email, s.birthday AS \"Doðum Günü\", s.department AS \"Bölüm\", s.class AS \"Sýnýf\", s.roomno AS \"Oda No\", s.entryyear AS \"Giriþ Yýlý\" FROM student s JOIN person p ON s.tcnumber = p.tcnumber";


                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sorgu, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView'e verileri baðla
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Baðlantýyý kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }


        public void UpdateStaffGrid()
        {
            try
            {
                // Baðlantýyý aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Personel bilgileri ve personel bilgilerini birleþtiren sorgu
                string sorgu = "SELECT s.tcnumber AS \"Personel TC\", p.name AS \"Ad\", p.surname AS \"Soyad\", s.role AS \"Görev\", p.phonenumber AS \"Telefon No\" FROM staff s JOIN person p ON s.tcnumber = p.tcnumber";


                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sorgu, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView2'ye verileri baðla
                dataGridView2.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Baðlantýyý kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void UpdateFoodMenuGrid()
        {
            try
            {
                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Yemek menüsü bilgilerini öðrenci bilgisiyle birlikte al
                string sorgu = "SELECT date AS \"Tarih\", breakfast AS \"Kahvaltý\", dinner AS \"Akþam Yemeði\", tcnumber AS \"T.C. Numarasý\" FROM food_menu ORDER BY date ASC";


                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sorgu, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView3'e verileri baðla
                dataGridView3.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya mesaj göster
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Baðlantýyý kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
        private void UpdateLaundryAppointmentsGrid()
        {
            try
            {
                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Laundry tablosundan verileri al ve takma adlarla sütunlarý yeniden adlandýr
                string query = "SELECT laundryid AS \"Makine ID\", tcnumber AS \"TC Numarasý\", date AS \"Tarih\", time AS \"Saat\" " +
                               "FROM laundry ORDER BY date, time";

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView4'e verileri baðla
                dataGridView4.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya mesaj göster
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Baðlantýyý kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
        private void UpdatePaymentGridForDataGridView5()
        {
            try
            {
                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Payment tablosundan verileri çek ve sütunlara takma ad ver
                string query = "SELECT paymentid AS \"Ödeme ID\", tcnumber AS \"TC Numarasý\", amount AS \"Tutar\", status AS \"Durum\" " +
                               "FROM payment ORDER BY paymentid DESC";

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView5'e verileri baðla
                dataGridView5.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya bilgi ver
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
        private void UpdateMaintenanceGrid()
        {
            try
            {
                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Maintenance taleplerini al ve sütunlara takma ad ver
                string query = "SELECT requestid AS \"Talep ID\", roomno AS \"Oda No\", requestdate AS \"Tarih\", status AS \"Durum\" " +
                               "FROM maintenance_request ORDER BY requestid ASC";

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView6'ya verileri baðla
                dataGridView6.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya bilgi ver
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Baðlantýyý kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
        private void UpdateDisciplinaryActionsGrid()
        {
            try
            {
                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Disiplin kayýtlarýný al ve sütunlara takma ad ver
                string query = "SELECT actionid AS \"Eylem ID\", tcnumber AS \"TC Numarasý\", actiondate AS \"Eylem Tarihi\", " +
                               "description AS \"Açýklama\", status AS \"Durum\" " +
                               "FROM disciplinary_actions ORDER BY actiondate DESC";

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView7'ye verileri baðla
                dataGridView7.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya bilgi ver
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Baðlantýyý kapat
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
                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Kurallar tablosunu al
                string query = "SELECT ruleid AS \"Kural ID\", ruletext AS \"Kural Metni\", effectivedate AS \"Geçerlilik Tarihi\" " +
                               "FROM rules ORDER BY effectivedate DESC";

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView8'e verileri baðla
                dataGridView8.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
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
                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Þikayet tablosunu al ve sütunlara takma ad ver
                string query = "SELECT complaintid AS \"Þikayet ID\", tcnumber AS \"TC Numarasý\", complaintdate AS \"Þikayet Tarihi\", description AS \"Açýklama\", status AS \"Durum\" " +
                               "FROM complaint ORDER BY complaintdate DESC";

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView9'a verileri baðla
                dataGridView9.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya mesaj göster
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
        private void UpdateVisitorGrid()
        {
            try
            {
                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Visitor tablosundan verileri al ve sütunlara takma adlar ver
                string query = "SELECT visitorid AS \"Ziyaretçi ID\", name AS \"Ad\", surname AS \"Soyad\", visitdate AS \"Ziyaret Tarihi\", " +
                               "tcnumber AS \"TC Numarasý\", reason AS \"Ziyaret Nedeni\" FROM visitor ORDER BY visitdate DESC";

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView10'a verileri baðla
                dataGridView10.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya mesaj göster
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
        private void UpdateWeeklyVisitorCount()
        {
            try
            {
                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Haftalýk ziyaretçi sayýsýný getiren fonksiyon çaðrýsý
                string functionCallQuery = "SELECT count_weekly_visitors()";
                NpgsqlCommand functionCallCommand = new NpgsqlCommand(functionCallQuery, baglanti);
                int weeklyVisitors = Convert.ToInt32(functionCallCommand.ExecuteScalar());

                // Haftalýk ziyaretçi sayýsýný Label'a yazdýr
                label23.Text = $"{weeklyVisitors}";
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya mesaj göster
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
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
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                string query = "SELECT name AS \"Etkinlik Adý\", description AS \"Açýklama\", date AS \"Tarih\", organizer AS \"Organizatör\", time AS \"Saat\" " +
                               "FROM events ORDER BY date ASC";

                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataGridView11.DataSource = dataTable; // DataGridView11'e baðlanýr
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
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
                // Veritabaný baðlantýsýný kontrol et ve aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: student_events ve events tablolarýný inner join ile birleþtir, TC numarasýna göre filtreleme yapýlmaz
                string query = "SELECT se.tcnumber AS \"Öðrenci Tc No\", e.eventid AS \"Etkinlik ID\", e.name AS \"Etkinlik Adý\", " +
                               "e.description AS \"Açýklama\", e.date AS \"Tarih\", e.organizer AS \"Organizatör\", e.time AS \"Saat\" " +
                               "FROM student_events se " +
                               "INNER JOIN events e ON se.EventID = e.EventID " +
                               "ORDER BY e.date ASC";  // TC numarasýna göre filtreleme kaldýrýldý

                // DataAdapter ile verileri al
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView4'e verileri baðla
                dataGridView12.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya bilgi ver
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

        }
        private void UpdateFoodMenuGrid1()
        {
            try
            {
                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Ayný tarih için yemekleri birleþtir ve sýrala
                string sorgu = "SELECT date AS \"Tarih\", breakfast AS \"Kahvaltý\", dinner AS \"Akþam Yemeði\" FROM food_menu GROUP BY date, breakfast, dinner ORDER BY date ASC";

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sorgu, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView13'e verileri baðla
                dataGridView13.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya mesaj göster
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0; // Ýlk elemaný seç
            }
            UpdateStudentGrid();
            UpdateStaffGrid();
            UpdateFoodMenuGrid();
            UpdateLaundryAppointmentsGrid();
            UpdatePaymentGridForDataGridView5();
            UpdateMaintenanceGrid();
            UpdateDisciplinaryActionsGrid();
            UpdateRulesGrid();
            UpdatecomplaintGrid();
            UpdateVisitorGrid();
            UpdateWeeklyVisitorCount();
            UpdateEventsGrid();
            UpdateEventsGrid1();
            UpdateFoodMenuGrid1();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Baðlantýyý aç
                baglanti.Open();

                // textBox1'deki TC numarasýný al
                string tcNumber = textBox1.Text.Trim();

                // Eðer TC numarasý girilmemiþse hata mesajý göster
                if (string.IsNullOrEmpty(tcNumber))
                {
                    MessageBox.Show("Lütfen bir TC numarasý girin.");
                    return;
                }

                // SQL sorgusu: Girilen TC numarasýna göre öðrenci bilgilerini al
                string sorgu = "SELECT * FROM STUDENT WHERE TCNumber = @tc";

                // Sorguyu hazýrlayýn
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@tc", tcNumber);

                // Verileri almak için DataAdapter kullan
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(komut);

                // Verileri DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Eðer sonuç yoksa uyarý göster
                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Bu TC numarasýna ait öðrenci bulunamadý.");
                }

                // DataGridView'e verileri baðla
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj göster
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Baðlantýyý kapat
                baglanti.Close();
            }
            UpdateStudentGrid();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Baðlantýyý aç
                baglanti.Open();

                // comboBox1'deki seçimi kontrol et
                string selectedOption = comboBox1.SelectedItem.ToString();
                string query = ""; // Baþlangýçta boþ bir sorgu

                // Seçilen seçeneðe göre SQL sorgusunu oluþtur
                if (selectedOption == "Ýsim-Soyisim")
                {
                    string nameSurname = textBox2.Text.Trim();
                    // Ýsim soyisim girildiyse sorgu oluþtur
                    if (string.IsNullOrEmpty(nameSurname))
                    {
                        MessageBox.Show("Lütfen bir isim soyisim girin.");
                        return;
                    }

                    query = "SELECT p.TCNumber AS \"TC Kimlik No\", " +
                            "p.Name AS \"Ad\", " +
                            "p.Surname AS \"Soyad\", " +
                            "p.PhoneNumber AS \"Telefon\", " +
                            "p.Email AS \"E-posta\", " +
                            "s.RoomNo AS \"Oda No\", " +
                            "s.EntryYear AS \"Giriþ Yýlý\", " +
                            "s.Department AS \"Bölüm\", " +
                            "s.Class AS \"Sýnýf\", " +
                            "s.BirthDay AS \"Doðum Tarihi\" " +
                            "FROM PERSON p " +
                            "JOIN STUDENT s ON p.TCNumber = s.TCNumber " +
                            "WHERE p.Name || ' ' || p.Surname ILIKE @nameSurname";

                }
                else if (selectedOption == "Oda No")
                {
                    if (int.TryParse(textBox2.Text, out int roomNo))
                    {
                        // Hem PERSON hem de STUDENT tablosunu birleþtirerek sorgu
                        query = "SELECT p.TCNumber AS \"TC Kimlik No\", " +
                                "p.Name AS \"Ad\", " +
                                "p.Surname AS \"Soyad\", " +
                                "p.PhoneNumber AS \"Telefon\", " +
                                "p.Email AS \"E-posta\", " +
                                "s.RoomNo AS \"Oda No\", " +
                                "s.EntryYear AS \"Giriþ Yýlý\", " +
                                "s.Department AS \"Bölüm\", " +
                                "s.Class AS \"Sýnýf\", " +
                                "s.BirthDay AS \"Doðum Tarihi\" " +
                                "FROM PERSON p " +
                                "JOIN STUDENT s ON p.TCNumber = s.TCNumber " +
                                "WHERE s.RoomNo = @roomNo";
                    }
                    else
                    {
                        MessageBox.Show("Lütfen geçerli bir oda numarasý girin.");
                        return;
                    }
                }

                // Eðer geçerli bir sorgu oluþturulmuþsa çalýþtýr
                if (!string.IsNullOrEmpty(query))
                {
                    NpgsqlCommand komut = new NpgsqlCommand(query, baglanti);

                    // Parametreleri ekle
                    if (selectedOption == "Ýsim-Soyisim")
                    {
                        komut.Parameters.AddWithValue("@nameSurname", "%" + textBox2.Text.Trim() + "%"); // 'ILIKE' operatörü büyük/küçük harf duyarsýz arama yapar
                    }
                    else if (selectedOption == "Oda No")
                    {
                        komut.Parameters.AddWithValue("@roomNo", int.Parse(textBox2.Text));
                    }

                    // Verileri almak için DataAdapter kullan
                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(komut);

                    // Verileri DataTable'a doldur
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Eðer sonuç yoksa uyarý göster
                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Aradýðýnýz kriterlere uygun öðrenci bulunamadý.");
                    }

                    // DataGridView'e verileri baðla
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj göster
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Baðlantýyý kapat
                baglanti.Close();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpdateStudentGrid();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Yeni bir Form2 nesnesi oluþtur
            Form2 yeniForm = new Form2(this);

            // Form2'yi göster
            yeniForm.Show();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanýcýnýn girdiði TC numarasýný al
                string tcNumber = textBox3.Text.Trim();

                // Eðer TC numarasý girilmemiþse hata mesajý göster
                if (string.IsNullOrEmpty(tcNumber))
                {
                    MessageBox.Show("Lütfen silmek istediðiniz öðrencinin TC numarasýný girin.");
                    return;
                }

                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Ýþlem sýrasýný baþlat
                using (var transaction = baglanti.BeginTransaction())
                {
                    try
                    {
                        // 1. Öncelikle student_events tablosundan ilgili öðrenciye ait etkinlikleri sil
                        string deleteStudentEventsQuery = "DELETE FROM student_events WHERE tcnumber = @tc";
                        NpgsqlCommand deleteStudentEventsCommand = new NpgsqlCommand(deleteStudentEventsQuery, baglanti, transaction);
                        deleteStudentEventsCommand.Parameters.AddWithValue("@tc", tcNumber);
                        deleteStudentEventsCommand.ExecuteNonQuery();

                        // 2. Ardýndan STUDENT tablosundan öðrenci kaydýný sil
                        string deleteStudentQuery = "DELETE FROM STUDENT WHERE TCNumber = @tc";
                        NpgsqlCommand deleteStudentCommand = new NpgsqlCommand(deleteStudentQuery, baglanti, transaction);
                        deleteStudentCommand.Parameters.AddWithValue("@tc", tcNumber);
                        int rowsAffectedStudent = deleteStudentCommand.ExecuteNonQuery();

                        // 3. Son olarak PERSON tablosundan ilgili personel kaydýný sil
                        string deletePersonQuery = "DELETE FROM PERSON WHERE TCNumber = @tc";
                        NpgsqlCommand deletePersonCommand = new NpgsqlCommand(deletePersonQuery, baglanti, transaction);
                        deletePersonCommand.Parameters.AddWithValue("@tc", tcNumber);
                        int rowsAffectedPerson = deletePersonCommand.ExecuteNonQuery();

                        // Eðer hem öðrenci hem de personel baþarýyla silindiyse, iþlem tamamlanmýþ olur
                        if (rowsAffectedStudent > 0 && rowsAffectedPerson > 0)
                        {
                            // Transaction'u commit et
                            transaction.Commit();
                            MessageBox.Show("Öðrenci ve personel baþarýyla silindi.");
                        }
                        else
                        {
                            // Eðer öðrenci veya personel bulunamazsa, iþlem iptal edilir
                            transaction.Rollback();
                            MessageBox.Show("Bu TC numarasýna ait öðrenci veya personel bulunamadý.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda rollback yap
                        transaction.Rollback();
                        MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Veritabaný baðlantýsý sýrasýnda oluþan hatalarý yakala
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // Formu temizle ve tabloyu güncelle
            textBox3.Clear();
            UpdateStudentGrid();
            UpdateEventsGrid1();

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            signupForm.Close();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form3 yeniForm = new Form3(this);

            // Form2'yi göster
            yeniForm.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                // TextBox5'te girilen TC numarasýný al
                string tcNumber = textBox5.Text.Trim();

                // Eðer TC numarasý girilmemiþse hata mesajý göster
                if (string.IsNullOrEmpty(tcNumber))
                {
                    MessageBox.Show("Lütfen silmek istediðiniz personelin TC numarasýný girin.");
                    return;
                }

                // Baðlantýyý aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Ýþlem sýrasýný baþlat
                using (var transaction = baglanti.BeginTransaction())
                {
                    try
                    {
                        // 1. Öncelikle STAFF tablosundan personel kaydýný sil
                        string deleteStaffQuery = "DELETE FROM STAFF WHERE TCNumber = @tcNumber";
                        NpgsqlCommand deleteStaffCommand = new NpgsqlCommand(deleteStaffQuery, baglanti, transaction);
                        deleteStaffCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                        int rowsAffectedStaff = deleteStaffCommand.ExecuteNonQuery();

                        // 2. Ardýndan PERSON tablosundan ilgili personel kaydýný sil
                        string deletePersonQuery = "DELETE FROM PERSON WHERE TCNumber = @tcNumber";
                        NpgsqlCommand deletePersonCommand = new NpgsqlCommand(deletePersonQuery, baglanti, transaction);
                        deletePersonCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                        int rowsAffectedPerson = deletePersonCommand.ExecuteNonQuery();

                        // Eðer hem STAFF hem de PERSON tablosundan baþarýlý bir þekilde silinmiþse, iþlem tamamlanmýþ olur
                        if (rowsAffectedStaff > 0 && rowsAffectedPerson > 0)
                        {
                            // Transaction'u commit et
                            transaction.Commit();
                            MessageBox.Show("Personel baþarýyla silindi.");
                        }
                        else
                        {
                            // Eðer personel bulunamazsa, iþlem iptal edilir
                            transaction.Rollback();
                            MessageBox.Show("Bu TC numarasýna ait personel bulunamadý.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda rollback yap
                        transaction.Rollback();
                        MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Veritabaný baðlantýsý sýrasýnda oluþan hatalarý yakala
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox5'i temizle
            textBox5.Clear();

            // DataGridView2'yi güncelle (Personel tablosunu tekrar yükle)
            UpdateStaffGrid();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                // TextBox4'te girilen TC numarasýný al
                string tcNumber = textBox4.Text.Trim();

                // Eðer TC numarasý girilmemiþse hata mesajý göster
                if (string.IsNullOrEmpty(tcNumber))
                {
                    MessageBox.Show("Lütfen sorgulamak istediðiniz personelin TC numarasýný girin.");
                    return;
                }

                // Baðlantýyý aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: STAFF ve PERSON tablosundan TC numarasýna göre verileri al
                string sorgu = "SELECT s.*, p.* FROM STAFF s LEFT JOIN PERSON p ON s.tcNumber = p.tcNumber WHERE s.tcNumber = @tcNumber";
                // Parametreli sorgu için NpgsqlCommand oluþtur
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@tcNumber", tcNumber);

                // Verileri almak için DataAdapter kullan
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(komut);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Eðer sonuç yoksa uyarý göster
                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Bu TC numarasýna ait personel bulunamadý.");
                }

                // DataGridView2'ye verileri baðla
                dataGridView2.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya mesaj göster
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Baðlantýyý kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // DataGrid'i güncelle (isteðe baðlý, sadece tabloyu yüklemek istiyorsanýz)
            UpdateStaffGrid();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            UpdateStaffGrid();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanýcýdan alýnan bilgileri al
                string breakfast = textBox6.Text.Trim();
                string dinner = textBox7.Text.Trim();
                DateTime selectedDate = dateTimePicker1.Value.Date; // Tarih seçimi

                // Eðer kahvaltý veya akþam yemeði bilgisi girilmemiþse hata mesajý göster
                if (string.IsNullOrEmpty(breakfast) || string.IsNullOrEmpty(dinner))
                {
                    MessageBox.Show("Lütfen kahvaltý ve akþam yemeði bilgilerini girin.");
                    return;
                }

                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Öðrenci listesini çek
                string studentQuery = "SELECT tcnumber FROM student";
                NpgsqlCommand studentCommand = new NpgsqlCommand(studentQuery, baglanti);

                // Öðrenci T.C. numaralarýný al
                NpgsqlDataReader reader = studentCommand.ExecuteReader();
                List<string> studentTcNumbers = new List<string>();
                while (reader.Read())
                {
                    studentTcNumbers.Add(reader.GetString(0));
                }
                reader.Close();

                // Her öðrenci için yemek menüsü kontrolü ve ekleme
                foreach (string tcNumber in studentTcNumbers)
                {
                    // Ayný tarih için yemek menüsünün zaten var olup olmadýðýný kontrol et
                    string checkQuery = "SELECT COUNT(*) FROM food_menu WHERE date = @date AND tcnumber = @tcNumber";
                    NpgsqlCommand checkCommand = new NpgsqlCommand(checkQuery, baglanti);
                    checkCommand.Parameters.AddWithValue("@date", selectedDate);
                    checkCommand.Parameters.AddWithValue("@tcNumber", tcNumber);

                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count > 0)
                    {
                        // Zaten mevcut olan menüleri atla
                        continue;
                    }

                    // SQL sorgusu: Yemek menüsünü ekle
                    string insertQuery = "INSERT INTO food_menu (tcnumber, date, breakfast, dinner) VALUES (@tcNumber, @date, @breakfast, @dinner)";
                    NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);
                    insertCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                    insertCommand.Parameters.AddWithValue("@date", selectedDate);
                    insertCommand.Parameters.AddWithValue("@breakfast", breakfast);
                    insertCommand.Parameters.AddWithValue("@dinner", dinner);

                    // Sorguyu çalýþtýr
                    insertCommand.ExecuteNonQuery();
                }

                // Kullanýcýya baþarý mesajý göster
                MessageBox.Show("Yemek menüleri baþarýyla oluþturuldu!");

                // TextBox'larý temizle
                textBox6.Clear();
                textBox7.Clear();
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya mesaj göster
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // DataGridView'i güncelle
            UpdateFoodMenuGrid();
            UpdateFoodMenuGrid1();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Attendance tablosundan verileri al ve sütunlara takma ad ver
                string query = "SELECT attendanceid AS \"Yoklama ID\", tcnumber AS \"TC Numarasý\", date AS \"Tarih\", status AS \"Durum\" " +
                               "FROM attendance ORDER BY date DESC";

                // DataAdapter ile verileri çek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView1'e verileri baðla
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya bilgi ver
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Baðlantýyý kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanýcýdan alýnan bilgileri al
                DateTime selectedDate = dateTimePicker2.Value.Date;
                string roomNo = textBox8.Text.Trim();
                string status = "Pending";

                // Eðer oda numarasý boþsa kullanýcýyý uyar
                if (string.IsNullOrEmpty(roomNo))
                {
                    MessageBox.Show("Lütfen oda numarasýný girin.");
                    return;
                }

                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }
                int roomNumber = Convert.ToInt32(roomNo);
                // Bakým talebi ekleme sorgusu
                string insertQuery = "INSERT INTO maintenance_request (roomno, requestdate, status) VALUES (@roomno, @requestdate, @status)";
                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);

                // Parametreleri ekle
                insertCommand.Parameters.AddWithValue("@roomno", roomNumber);
                insertCommand.Parameters.AddWithValue("@requestdate", selectedDate);
                insertCommand.Parameters.AddWithValue("@status", status);

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Bakým talebi baþarýyla oluþturuldu.");
                }
                else
                {
                    MessageBox.Show("Bakým talebi oluþturulurken bir hata oluþtu.");
                }

                // DataGridView6'yý güncelle
                UpdateMaintenanceGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox'ý temizle
            textBox8.Clear();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanýcýdan alýnan bilgileri al
                DateTime selectedDate = dateTimePicker3.Value.Date;
                string roomNo = textBox9.Text.Trim();
                string status = "Completed";

                // Eðer oda numarasý boþsa kullanýcýyý uyar
                if (string.IsNullOrEmpty(roomNo))
                {
                    MessageBox.Show("Lütfen oda numarasýný girin.");
                    return;
                }

                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }
                int roomNumber = Convert.ToInt32(roomNo);
                // Bakým talebini güncelleme sorgusu
                string updateQuery = "UPDATE maintenance_request SET status = @status WHERE roomno = @roomno AND requestdate = @requestdate";
                NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, baglanti);

                // Parametreleri ekle
                updateCommand.Parameters.AddWithValue("@status", status);
                updateCommand.Parameters.AddWithValue("@roomno", roomNumber);
                updateCommand.Parameters.AddWithValue("@requestdate", selectedDate);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Bakým talebi baþarýyla tamamlandý.");
                }
                else
                {
                    MessageBox.Show("Girilen bilgilerle bakým talebi bulunamadý.");
                }

                // DataGridView6'yý güncelle
                UpdateMaintenanceGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox'ý temizle
            textBox9.Clear();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanýcýdan alýnan bilgileri al
                DateTime actionDate = dateTimePicker4.Value.Date;
                string tcNumber = textBox10.Text.Trim();
                string description = textBox11.Text.Trim();
                string status = "Deðerlendiriliyor";

                // Gerekli alanlarýn doldurulup doldurulmadýðýný kontrol et
                if (string.IsNullOrEmpty(tcNumber) || string.IsNullOrEmpty(description))
                {
                    MessageBox.Show("Lütfen tüm alanlarý doldurun.");
                    return;
                }

                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Disiplin kaydý ekleme sorgusu
                string insertQuery = "INSERT INTO disciplinary_actions (tcnumber, actiondate, description, status) " +
                                     "VALUES (@tcnumber, @actiondate, @description, @status)";
                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);

                // Parametreleri ekle
                insertCommand.Parameters.AddWithValue("@tcnumber", tcNumber);
                insertCommand.Parameters.AddWithValue("@actiondate", actionDate);
                insertCommand.Parameters.AddWithValue("@description", description);
                insertCommand.Parameters.AddWithValue("@status", status);

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Disiplin kaydý baþarýyla oluþturuldu.");
                }
                else
                {
                    MessageBox.Show("Disiplin kaydý oluþturulurken bir hata oluþtu.");
                }

                // DataGridView7'yi güncelle
                UpdateDisciplinaryActionsGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox'larý temizle
            textBox10.Clear();
            textBox11.Clear();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanýcýdan alýnan bilgileri al
                DateTime actionDate = dateTimePicker5.Value.Date;
                string tcNumber = textBox12.Text.Trim();
                string newStatus = textBox13.Text.Trim();

                // Gerekli alanlarýn doldurulup doldurulmadýðýný kontrol et
                if (string.IsNullOrEmpty(tcNumber) || string.IsNullOrEmpty(newStatus))
                {
                    MessageBox.Show("Lütfen tüm alanlarý doldurun.");
                    return;
                }

                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Disiplin kaydýný güncelleme sorgusu
                string updateQuery = "UPDATE disciplinary_actions SET status = @status " +
                                     "WHERE tcnumber = @tcnumber AND actiondate = @actiondate";
                NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, baglanti);

                // Parametreleri ekle
                updateCommand.Parameters.AddWithValue("@status", newStatus);
                updateCommand.Parameters.AddWithValue("@tcnumber", tcNumber);
                updateCommand.Parameters.AddWithValue("@actiondate", actionDate);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Disiplin kaydý baþarýyla güncellendi.");
                }
                else
                {
                    MessageBox.Show("Girilen bilgilerle eþleþen bir disiplin kaydý bulunamadý.");
                }

                // DataGridView7'yi güncelle
                UpdateDisciplinaryActionsGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox'larý temizle
            textBox12.Clear();
            textBox13.Clear();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanýcýnýn TextBox14'e girdiði kural metni
                string ruleText = textBox14.Text.Trim();

                // Eðer kural metni boþsa kullanýcýyý uyar
                if (string.IsNullOrEmpty(ruleText))
                {
                    MessageBox.Show("Lütfen bir kural metni girin.");
                    return;
                }

                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Þu anki tarihi al
                DateTime effectiveDate = DateTime.Now;

                // Kurallarý ekleme sorgusu
                string insertQuery = "INSERT INTO rules (ruletext, effectivedate) VALUES (@ruletext, @effectivedate)";
                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);

                // Parametreleri ekle
                insertCommand.Parameters.AddWithValue("@ruletext", ruleText);
                insertCommand.Parameters.AddWithValue("@effectivedate", effectiveDate);

                // Sorguyu çalýþtýr
                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Kural baþarýyla eklendi.");
                }
                else
                {
                    MessageBox.Show("Kural eklenirken bir hata oluþtu.");
                }

                // DataGridView8'i güncelle
                UpdateRulesGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox'ý temizle
            textBox14.Clear();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanýcýnýn TextBox15'e girdiði Þikayet ID'sini al
                if (!int.TryParse(textBox15.Text.Trim(), out int complaintId))
                {
                    MessageBox.Show("Lütfen geçerli bir Þikayet ID'si girin.");
                    return;
                }

                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Þikayet durumunu güncelleme sorgusu
                string updateQuery = "UPDATE complaint SET status = 'Completed' WHERE complaintid = @complaintId";
                NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, baglanti);

                // Parametreyi ekle
                updateCommand.Parameters.AddWithValue("@complaintId", complaintId);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                // Ýþlem sonucuna göre bilgi ver
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Þikayet durumu baþarýyla güncellendi.");
                }
                else
                {
                    MessageBox.Show("Girilen Þikayet ID'sine ait bir kayýt bulunamadý.");
                }

                // Þikayet tablosunu güncelle
                UpdatecomplaintGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox'ý temizle
            textBox15.Clear();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanýcýdan alýnan bilgileri al
                string name = textBox16.Text.Trim(); // Ziyaretçi adý
                string surname = textBox17.Text.Trim(); // Ziyaretçi soyadý
                string tcNumber = textBox18.Text.Trim(); // Öðrenci TC Numarasý
                string reason = textBox19.Text.Trim(); // Ziyaret nedeni
                DateTime visitDate = DateTime.Now; // Ziyaret tarihi (þu anki tarih)

                // Zorunlu alanlarýn boþ olup olmadýðýný kontrol et
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(tcNumber) || string.IsNullOrEmpty(reason))
                {
                    MessageBox.Show("Lütfen tüm alanlarý doldurun.");
                    return;
                }

                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // TC numarasýnýn STUDENT tablosunda olup olmadýðýný kontrol et
                string tcCheckQuery = "SELECT COUNT(*) FROM STUDENT WHERE TCNumber = @tcnumber";
                NpgsqlCommand tcCheckCommand = new NpgsqlCommand(tcCheckQuery, baglanti);
                tcCheckCommand.Parameters.AddWithValue("@tcnumber", tcNumber);

                // Sorgu sonucunu kontrol et
                int tcExists = Convert.ToInt32(tcCheckCommand.ExecuteScalar());

                if (tcExists == 0)
                {
                    MessageBox.Show("Bu TC numarasýna sahip bir öðrenci bulunamadý.");
                    return;
                }

                // Visitor tablosuna yeni ziyaretçi ekleme sorgusu
                string insertQuery = "INSERT INTO visitor (name, surname, visitdate, tcnumber, reason) " +
                                     "VALUES (@name, @surname, @visitdate, @tcnumber, @reason)";
                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);

                // Parametreleri ekle
                insertCommand.Parameters.AddWithValue("@name", name);
                insertCommand.Parameters.AddWithValue("@surname", surname);
                insertCommand.Parameters.AddWithValue("@visitdate", visitDate);
                insertCommand.Parameters.AddWithValue("@tcnumber", tcNumber);
                insertCommand.Parameters.AddWithValue("@reason", reason);

                // Sorguyu çalýþtýr
                int rowsAffected = insertCommand.ExecuteNonQuery();

                // Ýþlem sonucuna göre kullanýcýya bilgi ver
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Ziyaretçi baþarýyla kaydedildi!");
                }
                else
                {
                    MessageBox.Show("Ziyaretçi kaydedilirken bir hata oluþtu.");
                }

                UpdateWeeklyVisitorCount();

                // Formdaki alanlarý temizle
                textBox16.Clear();
                textBox17.Clear();
                textBox18.Clear();
                textBox19.Clear();

                // DataGridView10'u güncelle
                UpdateVisitorGrid();
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya mesaj göster
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanýcýdan alýnan veriler
                string eventName = textBox20.Text.Trim(); // Etkinlik adý
                string description = textBox21.Text.Trim(); // Açýklama
                DateTime eventDate = dateTimePicker6.Value.Date; // Tarih
                string organizer = textBox22.Text.Trim(); // Organizatör
                string eventTime = textBox23.Text.Trim(); // Saat

                // Zorunlu alanlarýn doldurulup doldurulmadýðýný kontrol et
                if (string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(description) ||
                    string.IsNullOrEmpty(organizer) || string.IsNullOrEmpty(eventTime))
                {
                    MessageBox.Show("Lütfen tüm alanlarý doldurun.");
                    return;
                }

                // Saat formatýný kontrol et (HH:mm)
                if (!TimeSpan.TryParse(eventTime, out _))
                {
                    MessageBox.Show("Lütfen geçerli bir saat formatý girin (örneðin: 14:30).");
                    return;
                }

                // Veritabaný baðlantýsýný aç
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Etkinlik ekleme sorgusu
                string insertQuery = "INSERT INTO events (name, description, date, organizer, time) " +
                                     "VALUES (@name, @description, @date, @organizer, @time)";
                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);

                // Parametreleri sorguya ekle
                insertCommand.Parameters.AddWithValue("@name", eventName);
                insertCommand.Parameters.AddWithValue("@description", description);
                insertCommand.Parameters.AddWithValue("@date", eventDate);
                insertCommand.Parameters.AddWithValue("@organizer", organizer);
                insertCommand.Parameters.AddWithValue("@time", eventTime);

                // Sorguyu çalýþtýr
                int rowsAffected = insertCommand.ExecuteNonQuery();

                // Ýþlem sonucuna göre mesaj göster
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Etkinlik baþarýyla kaydedildi!");
                    UpdateEventsGrid(); // Etkinlik tablosunu güncelle
                }
                else
                {
                    MessageBox.Show("Etkinlik kaydedilirken bir hata oluþtu.");
                }

                // Form alanlarýný temizle
                textBox20.Clear();
                textBox21.Clear();
                textBox22.Clear();
                textBox23.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluþtu: {ex.Message}");
            }
            finally
            {
                // Veritabaný baðlantýsýný kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

      
    }
}

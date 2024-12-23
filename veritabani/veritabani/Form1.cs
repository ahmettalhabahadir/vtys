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
                // Ba�lant�y� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // ��renciler ve personel bilgilerini birle�tiren sorgu
                string sorgu = "SELECT s.tcnumber AS \"TC Kimlik No\", p.name AS \"Ad\", p.surname AS \"Soyad\", p.phonenumber AS \"Tel No\", p.email, s.birthday AS \"Do�um G�n�\", s.department AS \"B�l�m\", s.class AS \"S�n�f\", s.roomno AS \"Oda No\", s.entryyear AS \"Giri� Y�l�\" FROM student s JOIN person p ON s.tcnumber = p.tcnumber";


                // DataAdapter ile verileri �ek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sorgu, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView'e verileri ba�la
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Ba�lant�y� kapat
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
                // Ba�lant�y� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Personel bilgileri ve personel bilgilerini birle�tiren sorgu
                string sorgu = "SELECT s.tcnumber AS \"Personel TC\", p.name AS \"Ad\", p.surname AS \"Soyad\", s.role AS \"G�rev\", p.phonenumber AS \"Telefon No\" FROM staff s JOIN person p ON s.tcnumber = p.tcnumber";


                // DataAdapter ile verileri �ek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sorgu, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView2'ye verileri ba�la
                dataGridView2.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Ba�lant�y� kapat
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
                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Yemek men�s� bilgilerini ��renci bilgisiyle birlikte al
                string sorgu = "SELECT date AS \"Tarih\", breakfast AS \"Kahvalt�\", dinner AS \"Ak�am Yeme�i\", tcnumber AS \"T.C. Numaras�\" FROM food_menu ORDER BY date ASC";


                // DataAdapter ile verileri �ek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sorgu, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView3'e verileri ba�la
                dataGridView3.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya mesaj g�ster
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Ba�lant�y� kapat
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
                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Laundry tablosundan verileri al ve takma adlarla s�tunlar� yeniden adland�r
                string query = "SELECT laundryid AS \"Makine ID\", tcnumber AS \"TC Numaras�\", date AS \"Tarih\", time AS \"Saat\" " +
                               "FROM laundry ORDER BY date, time";

                // DataAdapter ile verileri �ek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView4'e verileri ba�la
                dataGridView4.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya mesaj g�ster
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Ba�lant�y� kapat
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
                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Payment tablosundan verileri �ek ve s�tunlara takma ad ver
                string query = "SELECT paymentid AS \"�deme ID\", tcnumber AS \"TC Numaras�\", amount AS \"Tutar\", status AS \"Durum\" " +
                               "FROM payment ORDER BY paymentid DESC";

                // DataAdapter ile verileri �ek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView5'e verileri ba�la
                dataGridView5.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya bilgi ver
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
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
                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Maintenance taleplerini al ve s�tunlara takma ad ver
                string query = "SELECT requestid AS \"Talep ID\", roomno AS \"Oda No\", requestdate AS \"Tarih\", status AS \"Durum\" " +
                               "FROM maintenance_request ORDER BY requestid ASC";

                // DataAdapter ile verileri �ek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView6'ya verileri ba�la
                dataGridView6.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya bilgi ver
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Ba�lant�y� kapat
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
                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Disiplin kay�tlar�n� al ve s�tunlara takma ad ver
                string query = "SELECT actionid AS \"Eylem ID\", tcnumber AS \"TC Numaras�\", actiondate AS \"Eylem Tarihi\", " +
                               "description AS \"A��klama\", status AS \"Durum\" " +
                               "FROM disciplinary_actions ORDER BY actiondate DESC";

                // DataAdapter ile verileri �ek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView7'ye verileri ba�la
                dataGridView7.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya bilgi ver
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Ba�lant�y� kapat
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
                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Kurallar tablosunu al
                string query = "SELECT ruleid AS \"Kural ID\", ruletext AS \"Kural Metni\", effectivedate AS \"Ge�erlilik Tarihi\" " +
                               "FROM rules ORDER BY effectivedate DESC";

                // DataAdapter ile verileri �ek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView8'e verileri ba�la
                dataGridView8.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
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
                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // �ikayet tablosunu al ve s�tunlara takma ad ver
                string query = "SELECT complaintid AS \"�ikayet ID\", tcnumber AS \"TC Numaras�\", complaintdate AS \"�ikayet Tarihi\", description AS \"A��klama\", status AS \"Durum\" " +
                               "FROM complaint ORDER BY complaintdate DESC";

                // DataAdapter ile verileri �ek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView9'a verileri ba�la
                dataGridView9.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya mesaj g�ster
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
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
                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Visitor tablosundan verileri al ve s�tunlara takma adlar ver
                string query = "SELECT visitorid AS \"Ziyaret�i ID\", name AS \"Ad\", surname AS \"Soyad\", visitdate AS \"Ziyaret Tarihi\", " +
                               "tcnumber AS \"TC Numaras�\", reason AS \"Ziyaret Nedeni\" FROM visitor ORDER BY visitdate DESC";

                // DataAdapter ile verileri �ek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView10'a verileri ba�la
                dataGridView10.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya mesaj g�ster
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
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
                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Haftal�k ziyaret�i say�s�n� getiren fonksiyon �a�r�s�
                string functionCallQuery = "SELECT count_weekly_visitors()";
                NpgsqlCommand functionCallCommand = new NpgsqlCommand(functionCallQuery, baglanti);
                int weeklyVisitors = Convert.ToInt32(functionCallCommand.ExecuteScalar());

                // Haftal�k ziyaret�i say�s�n� Label'a yazd�r
                label23.Text = $"{weeklyVisitors}";
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya mesaj g�ster
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
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

                string query = "SELECT name AS \"Etkinlik Ad�\", description AS \"A��klama\", date AS \"Tarih\", organizer AS \"Organizat�r\", time AS \"Saat\" " +
                               "FROM events ORDER BY date ASC";

                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataGridView11.DataSource = dataTable; // DataGridView11'e ba�lan�r
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
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
                // Veritaban� ba�lant�s�n� kontrol et ve a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: student_events ve events tablolar�n� inner join ile birle�tir, TC numaras�na g�re filtreleme yap�lmaz
                string query = "SELECT se.tcnumber AS \"��renci Tc No\", e.eventid AS \"Etkinlik ID\", e.name AS \"Etkinlik Ad�\", " +
                               "e.description AS \"A��klama\", e.date AS \"Tarih\", e.organizer AS \"Organizat�r\", e.time AS \"Saat\" " +
                               "FROM student_events se " +
                               "INNER JOIN events e ON se.EventID = e.EventID " +
                               "ORDER BY e.date ASC";  // TC numaras�na g�re filtreleme kald�r�ld�

                // DataAdapter ile verileri al
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView4'e verileri ba�la
                dataGridView12.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya bilgi ver
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
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
                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Ayn� tarih i�in yemekleri birle�tir ve s�rala
                string sorgu = "SELECT date AS \"Tarih\", breakfast AS \"Kahvalt�\", dinner AS \"Ak�am Yeme�i\" FROM food_menu GROUP BY date, breakfast, dinner ORDER BY date ASC";

                // DataAdapter ile verileri �ek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sorgu, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView13'e verileri ba�la
                dataGridView13.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya mesaj g�ster
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
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
                comboBox1.SelectedIndex = 0; // �lk eleman� se�
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
                // Ba�lant�y� a�
                baglanti.Open();

                // textBox1'deki TC numaras�n� al
                string tcNumber = textBox1.Text.Trim();

                // E�er TC numaras� girilmemi�se hata mesaj� g�ster
                if (string.IsNullOrEmpty(tcNumber))
                {
                    MessageBox.Show("L�tfen bir TC numaras� girin.");
                    return;
                }

                // SQL sorgusu: Girilen TC numaras�na g�re ��renci bilgilerini al
                string sorgu = "SELECT * FROM STUDENT WHERE TCNumber = @tc";

                // Sorguyu haz�rlay�n
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@tc", tcNumber);

                // Verileri almak i�in DataAdapter kullan
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(komut);

                // Verileri DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // E�er sonu� yoksa uyar� g�ster
                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Bu TC numaras�na ait ��renci bulunamad�.");
                }

                // DataGridView'e verileri ba�la
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj g�ster
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Ba�lant�y� kapat
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
                // Ba�lant�y� a�
                baglanti.Open();

                // comboBox1'deki se�imi kontrol et
                string selectedOption = comboBox1.SelectedItem.ToString();
                string query = ""; // Ba�lang��ta bo� bir sorgu

                // Se�ilen se�ene�e g�re SQL sorgusunu olu�tur
                if (selectedOption == "�sim-Soyisim")
                {
                    string nameSurname = textBox2.Text.Trim();
                    // �sim soyisim girildiyse sorgu olu�tur
                    if (string.IsNullOrEmpty(nameSurname))
                    {
                        MessageBox.Show("L�tfen bir isim soyisim girin.");
                        return;
                    }

                    query = "SELECT p.TCNumber AS \"TC Kimlik No\", " +
                            "p.Name AS \"Ad\", " +
                            "p.Surname AS \"Soyad\", " +
                            "p.PhoneNumber AS \"Telefon\", " +
                            "p.Email AS \"E-posta\", " +
                            "s.RoomNo AS \"Oda No\", " +
                            "s.EntryYear AS \"Giri� Y�l�\", " +
                            "s.Department AS \"B�l�m\", " +
                            "s.Class AS \"S�n�f\", " +
                            "s.BirthDay AS \"Do�um Tarihi\" " +
                            "FROM PERSON p " +
                            "JOIN STUDENT s ON p.TCNumber = s.TCNumber " +
                            "WHERE p.Name || ' ' || p.Surname ILIKE @nameSurname";

                }
                else if (selectedOption == "Oda No")
                {
                    if (int.TryParse(textBox2.Text, out int roomNo))
                    {
                        // Hem PERSON hem de STUDENT tablosunu birle�tirerek sorgu
                        query = "SELECT p.TCNumber AS \"TC Kimlik No\", " +
                                "p.Name AS \"Ad\", " +
                                "p.Surname AS \"Soyad\", " +
                                "p.PhoneNumber AS \"Telefon\", " +
                                "p.Email AS \"E-posta\", " +
                                "s.RoomNo AS \"Oda No\", " +
                                "s.EntryYear AS \"Giri� Y�l�\", " +
                                "s.Department AS \"B�l�m\", " +
                                "s.Class AS \"S�n�f\", " +
                                "s.BirthDay AS \"Do�um Tarihi\" " +
                                "FROM PERSON p " +
                                "JOIN STUDENT s ON p.TCNumber = s.TCNumber " +
                                "WHERE s.RoomNo = @roomNo";
                    }
                    else
                    {
                        MessageBox.Show("L�tfen ge�erli bir oda numaras� girin.");
                        return;
                    }
                }

                // E�er ge�erli bir sorgu olu�turulmu�sa �al��t�r
                if (!string.IsNullOrEmpty(query))
                {
                    NpgsqlCommand komut = new NpgsqlCommand(query, baglanti);

                    // Parametreleri ekle
                    if (selectedOption == "�sim-Soyisim")
                    {
                        komut.Parameters.AddWithValue("@nameSurname", "%" + textBox2.Text.Trim() + "%"); // 'ILIKE' operat�r� b�y�k/k���k harf duyars�z arama yapar
                    }
                    else if (selectedOption == "Oda No")
                    {
                        komut.Parameters.AddWithValue("@roomNo", int.Parse(textBox2.Text));
                    }

                    // Verileri almak i�in DataAdapter kullan
                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(komut);

                    // Verileri DataTable'a doldur
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // E�er sonu� yoksa uyar� g�ster
                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Arad���n�z kriterlere uygun ��renci bulunamad�.");
                    }

                    // DataGridView'e verileri ba�la
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj g�ster
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Ba�lant�y� kapat
                baglanti.Close();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpdateStudentGrid();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Yeni bir Form2 nesnesi olu�tur
            Form2 yeniForm = new Form2(this);

            // Form2'yi g�ster
            yeniForm.Show();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullan�c�n�n girdi�i TC numaras�n� al
                string tcNumber = textBox3.Text.Trim();

                // E�er TC numaras� girilmemi�se hata mesaj� g�ster
                if (string.IsNullOrEmpty(tcNumber))
                {
                    MessageBox.Show("L�tfen silmek istedi�iniz ��rencinin TC numaras�n� girin.");
                    return;
                }

                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // ��lem s�ras�n� ba�lat
                using (var transaction = baglanti.BeginTransaction())
                {
                    try
                    {
                        // 1. �ncelikle student_events tablosundan ilgili ��renciye ait etkinlikleri sil
                        string deleteStudentEventsQuery = "DELETE FROM student_events WHERE tcnumber = @tc";
                        NpgsqlCommand deleteStudentEventsCommand = new NpgsqlCommand(deleteStudentEventsQuery, baglanti, transaction);
                        deleteStudentEventsCommand.Parameters.AddWithValue("@tc", tcNumber);
                        deleteStudentEventsCommand.ExecuteNonQuery();

                        // 2. Ard�ndan STUDENT tablosundan ��renci kayd�n� sil
                        string deleteStudentQuery = "DELETE FROM STUDENT WHERE TCNumber = @tc";
                        NpgsqlCommand deleteStudentCommand = new NpgsqlCommand(deleteStudentQuery, baglanti, transaction);
                        deleteStudentCommand.Parameters.AddWithValue("@tc", tcNumber);
                        int rowsAffectedStudent = deleteStudentCommand.ExecuteNonQuery();

                        // 3. Son olarak PERSON tablosundan ilgili personel kayd�n� sil
                        string deletePersonQuery = "DELETE FROM PERSON WHERE TCNumber = @tc";
                        NpgsqlCommand deletePersonCommand = new NpgsqlCommand(deletePersonQuery, baglanti, transaction);
                        deletePersonCommand.Parameters.AddWithValue("@tc", tcNumber);
                        int rowsAffectedPerson = deletePersonCommand.ExecuteNonQuery();

                        // E�er hem ��renci hem de personel ba�ar�yla silindiyse, i�lem tamamlanm�� olur
                        if (rowsAffectedStudent > 0 && rowsAffectedPerson > 0)
                        {
                            // Transaction'u commit et
                            transaction.Commit();
                            MessageBox.Show("��renci ve personel ba�ar�yla silindi.");
                        }
                        else
                        {
                            // E�er ��renci veya personel bulunamazsa, i�lem iptal edilir
                            transaction.Rollback();
                            MessageBox.Show("Bu TC numaras�na ait ��renci veya personel bulunamad�.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda rollback yap
                        transaction.Rollback();
                        MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Veritaban� ba�lant�s� s�ras�nda olu�an hatalar� yakala
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // Formu temizle ve tabloyu g�ncelle
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

            // Form2'yi g�ster
            yeniForm.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                // TextBox5'te girilen TC numaras�n� al
                string tcNumber = textBox5.Text.Trim();

                // E�er TC numaras� girilmemi�se hata mesaj� g�ster
                if (string.IsNullOrEmpty(tcNumber))
                {
                    MessageBox.Show("L�tfen silmek istedi�iniz personelin TC numaras�n� girin.");
                    return;
                }

                // Ba�lant�y� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // ��lem s�ras�n� ba�lat
                using (var transaction = baglanti.BeginTransaction())
                {
                    try
                    {
                        // 1. �ncelikle STAFF tablosundan personel kayd�n� sil
                        string deleteStaffQuery = "DELETE FROM STAFF WHERE TCNumber = @tcNumber";
                        NpgsqlCommand deleteStaffCommand = new NpgsqlCommand(deleteStaffQuery, baglanti, transaction);
                        deleteStaffCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                        int rowsAffectedStaff = deleteStaffCommand.ExecuteNonQuery();

                        // 2. Ard�ndan PERSON tablosundan ilgili personel kayd�n� sil
                        string deletePersonQuery = "DELETE FROM PERSON WHERE TCNumber = @tcNumber";
                        NpgsqlCommand deletePersonCommand = new NpgsqlCommand(deletePersonQuery, baglanti, transaction);
                        deletePersonCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                        int rowsAffectedPerson = deletePersonCommand.ExecuteNonQuery();

                        // E�er hem STAFF hem de PERSON tablosundan ba�ar�l� bir �ekilde silinmi�se, i�lem tamamlanm�� olur
                        if (rowsAffectedStaff > 0 && rowsAffectedPerson > 0)
                        {
                            // Transaction'u commit et
                            transaction.Commit();
                            MessageBox.Show("Personel ba�ar�yla silindi.");
                        }
                        else
                        {
                            // E�er personel bulunamazsa, i�lem iptal edilir
                            transaction.Rollback();
                            MessageBox.Show("Bu TC numaras�na ait personel bulunamad�.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda rollback yap
                        transaction.Rollback();
                        MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Veritaban� ba�lant�s� s�ras�nda olu�an hatalar� yakala
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox5'i temizle
            textBox5.Clear();

            // DataGridView2'yi g�ncelle (Personel tablosunu tekrar y�kle)
            UpdateStaffGrid();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                // TextBox4'te girilen TC numaras�n� al
                string tcNumber = textBox4.Text.Trim();

                // E�er TC numaras� girilmemi�se hata mesaj� g�ster
                if (string.IsNullOrEmpty(tcNumber))
                {
                    MessageBox.Show("L�tfen sorgulamak istedi�iniz personelin TC numaras�n� girin.");
                    return;
                }

                // Ba�lant�y� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: STAFF ve PERSON tablosundan TC numaras�na g�re verileri al
                string sorgu = "SELECT s.*, p.* FROM STAFF s LEFT JOIN PERSON p ON s.tcNumber = p.tcNumber WHERE s.tcNumber = @tcNumber";
                // Parametreli sorgu i�in NpgsqlCommand olu�tur
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@tcNumber", tcNumber);

                // Verileri almak i�in DataAdapter kullan
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(komut);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // E�er sonu� yoksa uyar� g�ster
                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Bu TC numaras�na ait personel bulunamad�.");
                }

                // DataGridView2'ye verileri ba�la
                dataGridView2.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya mesaj g�ster
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Ba�lant�y� kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // DataGrid'i g�ncelle (iste�e ba�l�, sadece tabloyu y�klemek istiyorsan�z)
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
                // Kullan�c�dan al�nan bilgileri al
                string breakfast = textBox6.Text.Trim();
                string dinner = textBox7.Text.Trim();
                DateTime selectedDate = dateTimePicker1.Value.Date; // Tarih se�imi

                // E�er kahvalt� veya ak�am yeme�i bilgisi girilmemi�se hata mesaj� g�ster
                if (string.IsNullOrEmpty(breakfast) || string.IsNullOrEmpty(dinner))
                {
                    MessageBox.Show("L�tfen kahvalt� ve ak�am yeme�i bilgilerini girin.");
                    return;
                }

                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // ��renci listesini �ek
                string studentQuery = "SELECT tcnumber FROM student";
                NpgsqlCommand studentCommand = new NpgsqlCommand(studentQuery, baglanti);

                // ��renci T.C. numaralar�n� al
                NpgsqlDataReader reader = studentCommand.ExecuteReader();
                List<string> studentTcNumbers = new List<string>();
                while (reader.Read())
                {
                    studentTcNumbers.Add(reader.GetString(0));
                }
                reader.Close();

                // Her ��renci i�in yemek men�s� kontrol� ve ekleme
                foreach (string tcNumber in studentTcNumbers)
                {
                    // Ayn� tarih i�in yemek men�s�n�n zaten var olup olmad���n� kontrol et
                    string checkQuery = "SELECT COUNT(*) FROM food_menu WHERE date = @date AND tcnumber = @tcNumber";
                    NpgsqlCommand checkCommand = new NpgsqlCommand(checkQuery, baglanti);
                    checkCommand.Parameters.AddWithValue("@date", selectedDate);
                    checkCommand.Parameters.AddWithValue("@tcNumber", tcNumber);

                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count > 0)
                    {
                        // Zaten mevcut olan men�leri atla
                        continue;
                    }

                    // SQL sorgusu: Yemek men�s�n� ekle
                    string insertQuery = "INSERT INTO food_menu (tcnumber, date, breakfast, dinner) VALUES (@tcNumber, @date, @breakfast, @dinner)";
                    NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);
                    insertCommand.Parameters.AddWithValue("@tcNumber", tcNumber);
                    insertCommand.Parameters.AddWithValue("@date", selectedDate);
                    insertCommand.Parameters.AddWithValue("@breakfast", breakfast);
                    insertCommand.Parameters.AddWithValue("@dinner", dinner);

                    // Sorguyu �al��t�r
                    insertCommand.ExecuteNonQuery();
                }

                // Kullan�c�ya ba�ar� mesaj� g�ster
                MessageBox.Show("Yemek men�leri ba�ar�yla olu�turuldu!");

                // TextBox'lar� temizle
                textBox6.Clear();
                textBox7.Clear();
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya mesaj g�ster
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // DataGridView'i g�ncelle
            UpdateFoodMenuGrid();
            UpdateFoodMenuGrid1();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // SQL sorgusu: Attendance tablosundan verileri al ve s�tunlara takma ad ver
                string query = "SELECT attendanceid AS \"Yoklama ID\", tcnumber AS \"TC Numaras�\", date AS \"Tarih\", status AS \"Durum\" " +
                               "FROM attendance ORDER BY date DESC";

                // DataAdapter ile verileri �ek
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, baglanti);

                // Verileri bir DataTable'a doldur
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView1'e verileri ba�la
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya bilgi ver
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Ba�lant�y� kapat
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
                // Kullan�c�dan al�nan bilgileri al
                DateTime selectedDate = dateTimePicker2.Value.Date;
                string roomNo = textBox8.Text.Trim();
                string status = "Pending";

                // E�er oda numaras� bo�sa kullan�c�y� uyar
                if (string.IsNullOrEmpty(roomNo))
                {
                    MessageBox.Show("L�tfen oda numaras�n� girin.");
                    return;
                }

                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }
                int roomNumber = Convert.ToInt32(roomNo);
                // Bak�m talebi ekleme sorgusu
                string insertQuery = "INSERT INTO maintenance_request (roomno, requestdate, status) VALUES (@roomno, @requestdate, @status)";
                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);

                // Parametreleri ekle
                insertCommand.Parameters.AddWithValue("@roomno", roomNumber);
                insertCommand.Parameters.AddWithValue("@requestdate", selectedDate);
                insertCommand.Parameters.AddWithValue("@status", status);

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Bak�m talebi ba�ar�yla olu�turuldu.");
                }
                else
                {
                    MessageBox.Show("Bak�m talebi olu�turulurken bir hata olu�tu.");
                }

                // DataGridView6'y� g�ncelle
                UpdateMaintenanceGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox'� temizle
            textBox8.Clear();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullan�c�dan al�nan bilgileri al
                DateTime selectedDate = dateTimePicker3.Value.Date;
                string roomNo = textBox9.Text.Trim();
                string status = "Completed";

                // E�er oda numaras� bo�sa kullan�c�y� uyar
                if (string.IsNullOrEmpty(roomNo))
                {
                    MessageBox.Show("L�tfen oda numaras�n� girin.");
                    return;
                }

                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }
                int roomNumber = Convert.ToInt32(roomNo);
                // Bak�m talebini g�ncelleme sorgusu
                string updateQuery = "UPDATE maintenance_request SET status = @status WHERE roomno = @roomno AND requestdate = @requestdate";
                NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, baglanti);

                // Parametreleri ekle
                updateCommand.Parameters.AddWithValue("@status", status);
                updateCommand.Parameters.AddWithValue("@roomno", roomNumber);
                updateCommand.Parameters.AddWithValue("@requestdate", selectedDate);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Bak�m talebi ba�ar�yla tamamland�.");
                }
                else
                {
                    MessageBox.Show("Girilen bilgilerle bak�m talebi bulunamad�.");
                }

                // DataGridView6'y� g�ncelle
                UpdateMaintenanceGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox'� temizle
            textBox9.Clear();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullan�c�dan al�nan bilgileri al
                DateTime actionDate = dateTimePicker4.Value.Date;
                string tcNumber = textBox10.Text.Trim();
                string description = textBox11.Text.Trim();
                string status = "De�erlendiriliyor";

                // Gerekli alanlar�n doldurulup doldurulmad���n� kontrol et
                if (string.IsNullOrEmpty(tcNumber) || string.IsNullOrEmpty(description))
                {
                    MessageBox.Show("L�tfen t�m alanlar� doldurun.");
                    return;
                }

                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Disiplin kayd� ekleme sorgusu
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
                    MessageBox.Show("Disiplin kayd� ba�ar�yla olu�turuldu.");
                }
                else
                {
                    MessageBox.Show("Disiplin kayd� olu�turulurken bir hata olu�tu.");
                }

                // DataGridView7'yi g�ncelle
                UpdateDisciplinaryActionsGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox'lar� temizle
            textBox10.Clear();
            textBox11.Clear();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullan�c�dan al�nan bilgileri al
                DateTime actionDate = dateTimePicker5.Value.Date;
                string tcNumber = textBox12.Text.Trim();
                string newStatus = textBox13.Text.Trim();

                // Gerekli alanlar�n doldurulup doldurulmad���n� kontrol et
                if (string.IsNullOrEmpty(tcNumber) || string.IsNullOrEmpty(newStatus))
                {
                    MessageBox.Show("L�tfen t�m alanlar� doldurun.");
                    return;
                }

                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // Disiplin kayd�n� g�ncelleme sorgusu
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
                    MessageBox.Show("Disiplin kayd� ba�ar�yla g�ncellendi.");
                }
                else
                {
                    MessageBox.Show("Girilen bilgilerle e�le�en bir disiplin kayd� bulunamad�.");
                }

                // DataGridView7'yi g�ncelle
                UpdateDisciplinaryActionsGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox'lar� temizle
            textBox12.Clear();
            textBox13.Clear();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullan�c�n�n TextBox14'e girdi�i kural metni
                string ruleText = textBox14.Text.Trim();

                // E�er kural metni bo�sa kullan�c�y� uyar
                if (string.IsNullOrEmpty(ruleText))
                {
                    MessageBox.Show("L�tfen bir kural metni girin.");
                    return;
                }

                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // �u anki tarihi al
                DateTime effectiveDate = DateTime.Now;

                // Kurallar� ekleme sorgusu
                string insertQuery = "INSERT INTO rules (ruletext, effectivedate) VALUES (@ruletext, @effectivedate)";
                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);

                // Parametreleri ekle
                insertCommand.Parameters.AddWithValue("@ruletext", ruleText);
                insertCommand.Parameters.AddWithValue("@effectivedate", effectiveDate);

                // Sorguyu �al��t�r
                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Kural ba�ar�yla eklendi.");
                }
                else
                {
                    MessageBox.Show("Kural eklenirken bir hata olu�tu.");
                }

                // DataGridView8'i g�ncelle
                UpdateRulesGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox'� temizle
            textBox14.Clear();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullan�c�n�n TextBox15'e girdi�i �ikayet ID'sini al
                if (!int.TryParse(textBox15.Text.Trim(), out int complaintId))
                {
                    MessageBox.Show("L�tfen ge�erli bir �ikayet ID'si girin.");
                    return;
                }

                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // �ikayet durumunu g�ncelleme sorgusu
                string updateQuery = "UPDATE complaint SET status = 'Completed' WHERE complaintid = @complaintId";
                NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, baglanti);

                // Parametreyi ekle
                updateCommand.Parameters.AddWithValue("@complaintId", complaintId);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                // ��lem sonucuna g�re bilgi ver
                if (rowsAffected > 0)
                {
                    MessageBox.Show("�ikayet durumu ba�ar�yla g�ncellendi.");
                }
                else
                {
                    MessageBox.Show("Girilen �ikayet ID'sine ait bir kay�t bulunamad�.");
                }

                // �ikayet tablosunu g�ncelle
                UpdatecomplaintGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            // TextBox'� temizle
            textBox15.Clear();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullan�c�dan al�nan bilgileri al
                string name = textBox16.Text.Trim(); // Ziyaret�i ad�
                string surname = textBox17.Text.Trim(); // Ziyaret�i soyad�
                string tcNumber = textBox18.Text.Trim(); // ��renci TC Numaras�
                string reason = textBox19.Text.Trim(); // Ziyaret nedeni
                DateTime visitDate = DateTime.Now; // Ziyaret tarihi (�u anki tarih)

                // Zorunlu alanlar�n bo� olup olmad���n� kontrol et
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(tcNumber) || string.IsNullOrEmpty(reason))
                {
                    MessageBox.Show("L�tfen t�m alanlar� doldurun.");
                    return;
                }

                // Veritaban� ba�lant�s�n� a�
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                // TC numaras�n�n STUDENT tablosunda olup olmad���n� kontrol et
                string tcCheckQuery = "SELECT COUNT(*) FROM STUDENT WHERE TCNumber = @tcnumber";
                NpgsqlCommand tcCheckCommand = new NpgsqlCommand(tcCheckQuery, baglanti);
                tcCheckCommand.Parameters.AddWithValue("@tcnumber", tcNumber);

                // Sorgu sonucunu kontrol et
                int tcExists = Convert.ToInt32(tcCheckCommand.ExecuteScalar());

                if (tcExists == 0)
                {
                    MessageBox.Show("Bu TC numaras�na sahip bir ��renci bulunamad�.");
                    return;
                }

                // Visitor tablosuna yeni ziyaret�i ekleme sorgusu
                string insertQuery = "INSERT INTO visitor (name, surname, visitdate, tcnumber, reason) " +
                                     "VALUES (@name, @surname, @visitdate, @tcnumber, @reason)";
                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, baglanti);

                // Parametreleri ekle
                insertCommand.Parameters.AddWithValue("@name", name);
                insertCommand.Parameters.AddWithValue("@surname", surname);
                insertCommand.Parameters.AddWithValue("@visitdate", visitDate);
                insertCommand.Parameters.AddWithValue("@tcnumber", tcNumber);
                insertCommand.Parameters.AddWithValue("@reason", reason);

                // Sorguyu �al��t�r
                int rowsAffected = insertCommand.ExecuteNonQuery();

                // ��lem sonucuna g�re kullan�c�ya bilgi ver
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Ziyaret�i ba�ar�yla kaydedildi!");
                }
                else
                {
                    MessageBox.Show("Ziyaret�i kaydedilirken bir hata olu�tu.");
                }

                UpdateWeeklyVisitorCount();

                // Formdaki alanlar� temizle
                textBox16.Clear();
                textBox17.Clear();
                textBox18.Clear();
                textBox19.Clear();

                // DataGridView10'u g�ncelle
                UpdateVisitorGrid();
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya mesaj g�ster
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
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
                // Kullan�c�dan al�nan veriler
                string eventName = textBox20.Text.Trim(); // Etkinlik ad�
                string description = textBox21.Text.Trim(); // A��klama
                DateTime eventDate = dateTimePicker6.Value.Date; // Tarih
                string organizer = textBox22.Text.Trim(); // Organizat�r
                string eventTime = textBox23.Text.Trim(); // Saat

                // Zorunlu alanlar�n doldurulup doldurulmad���n� kontrol et
                if (string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(description) ||
                    string.IsNullOrEmpty(organizer) || string.IsNullOrEmpty(eventTime))
                {
                    MessageBox.Show("L�tfen t�m alanlar� doldurun.");
                    return;
                }

                // Saat format�n� kontrol et (HH:mm)
                if (!TimeSpan.TryParse(eventTime, out _))
                {
                    MessageBox.Show("L�tfen ge�erli bir saat format� girin (�rne�in: 14:30).");
                    return;
                }

                // Veritaban� ba�lant�s�n� a�
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

                // Sorguyu �al��t�r
                int rowsAffected = insertCommand.ExecuteNonQuery();

                // ��lem sonucuna g�re mesaj g�ster
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Etkinlik ba�ar�yla kaydedildi!");
                    UpdateEventsGrid(); // Etkinlik tablosunu g�ncelle
                }
                else
                {
                    MessageBox.Show("Etkinlik kaydedilirken bir hata olu�tu.");
                }

                // Form alanlar�n� temizle
                textBox20.Clear();
                textBox21.Clear();
                textBox22.Clear();
                textBox23.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata olu�tu: {ex.Message}");
            }
            finally
            {
                // Veritaban� ba�lant�s�n� kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

      
    }
}

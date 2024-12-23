using Npgsql;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace veritabani
{
    public partial class signup : Form
    {
        public signup()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("server=localhost; port=5432; Database=yurtyonetim; user Id=postgres; password=123456");

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanıcıdan alınan T.C. ve şifre
                string tcNo = textBox1.Text.Trim();
                string password = textBox2.Text.Trim();

                // Eğer kullanıcı adı ve şifre "admin" ise, doğrudan giriş yap
                if (tcNo == "admin" && password == "admin")
                {
                    // Admin girişine özel işlemler
                    Form1 form1 = new Form1(this);  // Form1'e signup formunu gönder
                    form1.TcNo = tcNo;
                    form1.Show();
                    this.Hide();  // Signup formunu gizle
                    return;
                }

                // Şifreyi hash'le
                string hashedPassword = HashPassword(password);

                // Veritabanı bağlantısını aç
                using (baglanti)
                {
                    baglanti.Open();

                    // TC ve hashlenmiş şifreyi kontrol eden SQL sorgusu
                    string sorgu = "SELECT COUNT(*) FROM STUDENT WHERE TCNumber = @tc AND Password = @hashedPassword";

                    // Parametreli komut oluştur
                    NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                    komut.Parameters.AddWithValue("@tc", tcNo);
                    komut.Parameters.AddWithValue("@hashedPassword", hashedPassword);

                    // Sorguyu çalıştır ve sonucu al
                    int count = Convert.ToInt32(komut.ExecuteScalar());

                    if (count > 0)
                    {
                        // Doğrulama başarılı, studentpanel formunu aç
                        studentpanel studentpanel1 = new studentpanel(this);
                        studentpanel1.TcNo = tcNo;
                        studentpanel1.Show();
                        this.Hide();  // Signup formunu gizle
                    }
                    else
                    {
                        // Doğrulama başarısız
                        MessageBox.Show("Hatalı T.C. numarası veya şifre.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya mesaj gösteriyoruz
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
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
    }
}

using System;
using System.Windows;
using CurrencyExchange.WPF.ExchangeService;

namespace CurrencyExchange.WPF
{
    public partial class MainWindow : Window
    {
        Service1Client client = new Service1Client();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre giriniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int userId = client.LoginUser(username, password);

                if (userId > 0)
                {
                    MessageBox.Show("Giriş Başarılı!", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Dashboard'u aç ve kullanıcının ID'sini gönder
                    Dashboard dashboardWindow = new Dashboard(userId);
                    dashboardWindow.Show();

                    // Eski giriş ekranını kapat
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hatalı kullanıcı adı veya şifre!", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sunucuya bağlanılamadı: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre giriniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool isSuccess = client.RegisterUser(username, password);

                if (isSuccess)
                {
                    MessageBox.Show("Kayıt başarılı! Şimdi giriş yapabilirsiniz.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Bu kullanıcı adı zaten alınmış olabilir veya bir hata oluştu.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sunucuya bağlanılamadı: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
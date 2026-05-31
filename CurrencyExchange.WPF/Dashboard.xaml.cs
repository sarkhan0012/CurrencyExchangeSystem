using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using CurrencyExchange.WPF.ExchangeService;

namespace CurrencyExchange.WPF
{
    public partial class Dashboard : Window
    {
        private int currentUserId;
        Service1Client client = new Service1Client();

        // Kurları hafızada tutacağımız liste
        private List<NbpRate> currentRates = new List<NbpRate>();

        public Dashboard(int userId)
        {
            InitializeComponent();
            currentUserId = userId;

            // Pencere ekrana yüklendiğinde verileri çekme işlemini başlatır
            this.Loaded += Dashboard_Loaded;
        }

        private async void Dashboard_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUserData();          // Veritabanından Bakiye ve Geçmişi getir
            await LoadNbpRatesAsync(); // NBP API'den güncel kurları getir
        }

        // --- Bakiye ve İşlem Geçmişi Yükleme ---
        private void LoadUserData()
        {
            try
            {
                // WCF üzerinden bakiyeyi çek ve ekrana yaz
                decimal balance = client.GetBalance(currentUserId);
                txtBalance.Text = $"{balance:0.00} PLN";

                // WCF üzerinden işlem geçmişini çek ve tabloya bağla
                var history = client.GetTransactionHistory(currentUserId);
                dgHistory.ItemsSource = history;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kullanıcı verileri alınamadı: " + ex.Message, "Hata");
            }
        }

        // --- NBP API'den Döviz Kurlarını Yükleme (HTTPS ile) ---
        private async Task LoadNbpRatesAsync()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Polonya Merkez Bankası A Tablosu API Adresi
                    string apiUrl = "https://api.nbp.pl/api/exchangerates/tables/A/?format=json";

                    string jsonResponse = await httpClient.GetStringAsync(apiUrl);
                    var tables = JsonConvert.DeserializeObject<List<NbpTable>>(jsonResponse);

                    if (tables != null && tables.Count > 0)
                    {
                        currentRates = tables[0].rates;
                        dgRates.ItemsSource = currentRates;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("NBP API kurları yüklenemedi: " + ex.Message, "Hata");
            }
        }

        // --- Para Yükleme İşlemi ---
        private void btnTopUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool success = client.TopUpWallet(currentUserId, 1000m);
                if (success)
                {
                    MessageBox.Show("Cüzdanınıza 1000 PLN yüklendi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUserData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        // --- Döviz Satın Alma (BUY) İşlemi ---
        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            string code = txtCurrencyCode.Text.ToUpper();

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Lütfen Miktar kısmına sadece sayı giriniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var rateInfo = currentRates.Find(r => r.code == code);
            if (rateInfo == null)
            {
                MessageBox.Show("Döviz kodu bulunamadı! Lütfen listeden geçerli bir kod girin (Örn: USD).", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                bool success = client.ProcessTransaction(currentUserId, "BUY", code, amount, rateInfo.mid);
                if (success)
                {
                    MessageBox.Show($"{amount} {code} başarıyla SATIN ALINDI!", "İşlem Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUserData();
                }
                else
                {
                    MessageBox.Show("Yetersiz Bakiye! Lütfen önce para yükleyin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sunucu hatası: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- Döviz Satma (SELL) İşlemi ---
        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            string code = txtCurrencyCode.Text.ToUpper();

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Lütfen Miktar kısmına sadece sayı giriniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var rateInfo = currentRates.Find(r => r.code == code);
            if (rateInfo == null)
            {
                MessageBox.Show("Döviz kodu bulunamadı! Lütfen listeden geçerli bir kod girin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                bool success = client.ProcessTransaction(currentUserId, "SELL", code, amount, rateInfo.mid);
                if (success)
                {
                    MessageBox.Show($"{amount} {code} başarıyla SATILDI ve cüzdanınıza PLN eklendi!", "İşlem Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUserData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sunucu hatası: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // --- NBP API'den Gelen JSON Verisi İçin Model Sınıfları ---
    public class NbpTable
    {
        public string table { get; set; }
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public List<NbpRate> rates { get; set; }
    }

    public class NbpRate
    {
        public string currency { get; set; }
        public string code { get; set; }
        public decimal mid { get; set; }
    }
}
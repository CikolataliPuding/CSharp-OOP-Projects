using Microsoft.Win32;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Tesseract;

namespace MLMatematikCozucu;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // Kullanıcının seçtiği görselin dosya yolu burada tutulur.
    // Böylece "Analiz Et ve Çöz" dediğinde aynı dosyayı OCR'a gönderebiliriz.
    private string? _seciliGorselYolu;

    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// "Görsel Yükle" butonu: Kullanıcıdan jpg/png seçtirir ve önizleme gösterir.
    /// </summary>
    private void BtnGorselYukle_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Windows'un standart dosya seçme penceresi
            var dialog = new OpenFileDialog
            {
                Title = "Bir görsel seçin",
                Filter = "Görsel Dosyaları (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg",
                Multiselect = false
            };

            if (dialog.ShowDialog() != true)
            {
                return; // kullanıcı vazgeçti
            }

            _seciliGorselYolu = dialog.FileName;

            // WPF Image kontrolüne göstermek için BitmapImage kullanıyoruz.
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // dosyayı kilitlememek için
            bitmap.UriSource = new Uri(_seciliGorselYolu);
            bitmap.EndInit();

            ImgOnizleme.Source = bitmap;
            TxtOnizlemeBos.Visibility = Visibility.Collapsed;

            // UI alanlarını sıfırla
            TxtTespitEdilenIslem.Text = "—";
            TxtSonuc.Text = "—";
            TxtHamOcr.Text = "";
            TxtUyari.Text = "";
            TxtUyari.Visibility = Visibility.Collapsed;
            TxtDurum.Text = "Görsel yüklendi.";
        }
        catch (Exception ex)
        {
            // Hocaya sunarken: Gerçek uygulamalarda hata detayını loglamak iyi olur.
            // Öğrenci projesi için kullanıcıya Türkçe kısa bir mesaj göstermek yeterli.
            MesajGosterHata($"Görsel yüklenirken hata oluştu: {ex.Message}");
        }
    }

    /// <summary>
    /// "Analiz Et ve Çöz" butonu: OCR + temizleme + hesaplama yapar.
    /// </summary>
    private async void BtnAnalizEt_Click(object sender, RoutedEventArgs e)
    {
        // Basit "busy state": butonları kilitleyelim ki kullanıcı arka arkaya basmasın.
        BtnAnalizEt.IsEnabled = false;
        BtnGorselYukle.IsEnabled = false;
        TxtDurum.Text = "Analiz ediliyor...";
        TxtUyari.Visibility = Visibility.Collapsed;
        TxtUyari.Text = "";

        try
        {
            if (string.IsNullOrWhiteSpace(_seciliGorselYolu) || !File.Exists(_seciliGorselYolu))
            {
                MesajGosterUyari("Lütfen önce bir görsel yükleyin.");
                return;
            }

            // OCR ve hesaplamayı UI'ı kilitlememek için arka planda çalıştırıyoruz.
            var sonuc = await Task.Run(() => AnalizEtVeCoz(_seciliGorselYolu));

            // UI'ya sonucu yaz
            TxtHamOcr.Text = sonuc.HamOcrMetni;
            TxtTespitEdilenIslem.Text = string.IsNullOrWhiteSpace(sonuc.TemizIslem) ? "—" : sonuc.TemizIslem;
            TxtSonuc.Text = string.IsNullOrWhiteSpace(sonuc.SonucMetni) ? "—" : sonuc.SonucMetni;

            if (!string.IsNullOrWhiteSpace(sonuc.UyariMetni))
            {
                TxtUyari.Text = sonuc.UyariMetni;
                TxtUyari.Visibility = Visibility.Visible;
            }

            TxtDurum.Text = "Tamamlandı.";
        }
        catch (Exception ex)
        {
            // Beklenmeyen bir durum olursa burada yakalarız.
            MesajGosterHata($"Analiz sırasında beklenmeyen hata: {ex.Message}");
        }
        finally
        {
            BtnAnalizEt.IsEnabled = true;
            BtnGorselYukle.IsEnabled = true;
        }
    }

    /// <summary>
    /// OCR + temizleme + çözüm işlemlerini tek yerde topluyoruz (test etmesi daha kolay).
    /// </summary>
    private AnalizSonucu AnalizEtVeCoz(string gorselYolu)
    {
        // 1) OCR: Görselden metni oku
        var ham = OcrIleMetinOku(gorselYolu);

        // 2) Metni temizle: sadece rakam ve operatörler kalsın
        var temiz = TemizIslemMetniOlustur(ham);

        // 3) Hesapla
        if (string.IsNullOrWhiteSpace(temiz))
        {
            return new AnalizSonucu
            {
                HamOcrMetni = ham,
                TemizIslem = "",
                SonucMetni = "",
                UyariMetni = "Görselden işlem okunamadı. Lütfen daha net bir görsel deneyin."
            };
        }

        try
        {
            // DataTable.Compute temel 4 işlem ve parantezleri çözebilir.
            // Öğrenci projesi için pratik ve kolay bir yöntem.
            var dt = new DataTable();

            // Türkiye kültüründe ondalık ayırıcı genellikle virgül olduğu için
            // '.' gördüğümüzde ',' yapıyoruz. (OCR bazen nokta üretebilir)
            var computeIfadesi = temiz.Replace('.', ',');

            var deger = dt.Compute(computeIfadesi, "");

            return new AnalizSonucu
            {
                HamOcrMetni = ham,
                TemizIslem = temiz,
                SonucMetni = deger?.ToString() ?? "",
                UyariMetni = ""
            };
        }
        catch
        {
            // İfade bozuksa (örnek: "25++10") Compute hata verir.
            return new AnalizSonucu
            {
                HamOcrMetni = ham,
                TemizIslem = temiz,
                SonucMetni = "",
                UyariMetni = "İşlem çözülemedi. OCR hatası olabilir; görseli daha net çekmeyi deneyin."
            };
        }
    }

    /// <summary>
    /// Tesseract ile görselden metin okur.
    /// </summary>
    private static string OcrIleMetinOku(string gorselYolu)
    {
        // !!! ÖNEMLİ: tessdata klasörü mantığı
        // TesseractEngine, dil verilerini (traineddata) bulmak zorundadır.
        // Bu projede yol şu şekilde ayarlı:
        //   AppContext.BaseDirectory\tessdata\
        // Yani build sonrası örnek:
        //   bin\Debug\net8.0-windows\tessdata\eng.traineddata
        //
        // NOT: traineddata dosyalarını repo içinde paylaşmak yerine kullanıcı indirip ekler.
        var tessdataYolu = Path.Combine(AppContext.BaseDirectory, "tessdata");

        // Dil: Matematiksel ifadeler için "eng" çoğu zaman yeterli.
        // İstersen "eng+tur" da yapabilirsin; o zaman iki traineddata gerekir.
        const string dil = "eng";

        // Görsel dosyasını byte[] olarak okuyup Pix'e çeviriyoruz.
        // Böylece System.Drawing bağımlılığına girmeden WPF'te çalışır.
        var bytes = File.ReadAllBytes(gorselYolu);

        using var engine = new TesseractEngine(tessdataYolu, dil, EngineMode.Default);
        using var pix = Pix.LoadFromMemory(bytes);
        using var page = engine.Process(pix);

        return page.GetText() ?? "";
    }

    /// <summary>
    /// OCR çıktılarını matematik ifadesine çevirmek için basit bir "temizleme" uygular.
    /// Amaç: harf/boşluk vs. at, sadece rakam ve operatörleri bırak.
    /// </summary>
    private static string TemizIslemMetniOlustur(string hamOcrMetni)
    {
        if (string.IsNullOrWhiteSpace(hamOcrMetni))
            return "";

        var text = hamOcrMetni.Trim();

        // OCR bazen çarpma işaretini "x" gibi algılar, bunları '*' yapalım.
        // Ayrıca bazı tire karakterleri farklı Unicode olabilir.
        text = text
            .Replace("×", "*")
            .Replace("X", "*")
            .Replace("x", "*")
            .Replace("÷", "/")
            .Replace("–", "-")
            .Replace("—", "-");

        // Sadece izin verdiğimiz karakterleri tut:
        // 0-9 ve + - * / ( ) . ,
        // (virgül/nokta ondalık için; hesaplamada kültüre göre dönüştürüyoruz)
        text = Regex.Replace(text, @"[^0-9\+\-\*\/\(\)\.\,]", "");

        // Birden fazla satırdan kalan karakterler vb. olabilir; ekstra boşluk zaten yok.
        return text;
    }

    private void MesajGosterUyari(string mesaj)
    {
        TxtUyari.Text = mesaj;
        TxtUyari.Visibility = Visibility.Visible;
        TxtDurum.Text = "Uyarı";
    }

    private void MesajGosterHata(string mesaj)
    {
        TxtUyari.Text = mesaj;
        TxtUyari.Visibility = Visibility.Visible;
        TxtDurum.Text = "Hata";
    }

    /// <summary>
    /// Tek bir analiz çalışmasının sonucunu UI'a taşımak için küçük bir model.
    /// </summary>
    private sealed class AnalizSonucu
    {
        public string HamOcrMetni { get; init; } = "";
        public string TemizIslem { get; init; } = "";
        public string SonucMetni { get; init; } = "";
        public string UyariMetni { get; init; } = "";
    }
}
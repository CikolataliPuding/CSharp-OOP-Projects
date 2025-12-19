using System;
using System.IO;
using System.Windows.Forms;

namespace AntiVirusWin
{
    // OOP Örneği: Form, antivirus tarama için IScanner arayüzünü kullanan bir "istemci" rolünde.
    public partial class MainForm : Form
    {
        private readonly IScanner _scanner;

        public MainForm()
        {
            InitializeComponent();

            // OOP Örneği: Bağımlılık tersine çevirme (Dependency Injection'in basit hâli).
            // Arayüz üzerinden konuşuyoruz, somut sınıfa (FileScanner) sıkı sıkıya bağlı değiliz.
            _scanner = new FileScanner(new SimpleSignatureDatabase());
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Taranacak dosyayı seçin",
                Filter = "Metin Dosyaları (*.txt)|*.txt|Tüm Dosyalar (*.*)|*.*"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = dialog.FileName;
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            var path = txtFilePath.Text;
            txtLog.Clear();

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                AppendLog("Lütfen geçerli bir dosya yolu seçin.");
                return;
            }

            AppendLog($"Dosya taranıyor: {path}");

            // OOP Örneği: IScanner arayüzünü kullanarak polimorfik tarama.
            var result = _scanner.Scan(path);

            AppendLog($"Durum: {(result.IsInfected ? "BULAŞIK" : "TEMİZ")}");
            AppendLog($"Detay: {result.Message}");
            AppendLog($"Tespit edilen imza: {result.DetectedSignature ?? "-"}");
        }

        private void AppendLog(string message)
        {
            txtLog.AppendText(message + Environment.NewLine);
        }
    }
}



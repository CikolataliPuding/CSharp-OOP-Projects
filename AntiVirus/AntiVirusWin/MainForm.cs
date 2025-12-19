using System;
using System.IO;
using System.Windows.Forms;
using VirusSim;

namespace AntiVirusWin
{
    public partial class MainForm : Form
    {
        private readonly IScanner _scanner;

        public MainForm()
        {
            InitializeComponent();
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

            var result = _scanner.Scan(path);

            AppendLog($"Durum: {(result.IsInfected ? "BULAŞIK" : "TEMİZ")}");
            AppendLog($"Detay: {result.Message}");
            AppendLog($"Tespit edilen imza: {result.DetectedSignature ?? "-"}");
        }

        private void btnCreateTestVirus_Click(object sender, EventArgs e)
        {
            try
            {
                // Test dosyası oluştur
                string testFilePath = Path.Combine(Path.GetTempPath(), "test_dosya.txt");
                File.WriteAllText(testFilePath, "Bu bir test dosyasıdır.\nNormal içerik burada.\n");

                // Virus simülasyonu ile dosyaya bulaştır
                var virus = new SimpleFileVirus();
                virus.Infect(testFilePath);

                // Dosya yolunu textbox'a yaz
                txtFilePath.Text = testFilePath;

                AppendLog($"✓ Test dosyası oluşturuldu ve virus bulaştırıldı: {testFilePath}");
                AppendLog($"Şimdi 'Tara' butonuna basarak tarama yapabilirsiniz.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AppendLog(string message)
        {
            txtLog.AppendText(message + Environment.NewLine);
        }
    }
}




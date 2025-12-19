using System.IO;

namespace AntiVirusWin
{
    /// <summary>
    /// Farklı tarama algoritmaları için yeni sınıflar yazıp bu arayüzü implemente edebiliriz.
    /// </summary>
    public interface IScanner
    {
        ScanResult Scan(string filePath);
    }

    /// <summary>
    /// Tarama sonucu verisini temsil eden basit bir model sınıf.
    /// </summary>
    public class ScanResult
    {
        public bool IsInfected { get; }
        public string Message { get; }
        public string? DetectedSignature { get; }

        public ScanResult(bool isInfected, string message, string? detectedSignature)
        {
            IsInfected = isInfected;
            Message = message;
            DetectedSignature = detectedSignature;
        }
    }

    /// <summary>
    /// Gerekirse XML'den, veritabanından, dosyadan vs. okuyacak başka sınıflar yazılabilir.
    /// </summary>
    public interface ISignatureDatabase
    {
        string[] GetSignatures();
    }

    /// <summary>
    /// Basit, sabit (in-memory) imza veritabanı.
    /// </summary>
    public class SimpleSignatureDatabase : ISignatureDatabase
    {
        // Simülasyon: Gerçek hash veya karmaşık imzalar yerine düz string kullanıyoruz.
        private readonly string[] _signatures =
        {
            "[VIRUS:SIMPLE]",
            "ESU-VIRUS-SIGNATURE",
            "//infected-by-esu"
        };

        public string[] GetSignatures() => _signatures;
    }

    /// <summary>
    /// Gerçek tarama işini yapan sınıf.
    /// </summary>
    public class FileScanner : IScanner
    {
        private readonly ISignatureDatabase _signatureDatabase;

        public FileScanner(ISignatureDatabase signatureDatabase)
        {
            _signatureDatabase = signatureDatabase;
        }

        public ScanResult Scan(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new ScanResult(false, "Dosya bulunamadı.", null);
            }

            string content = File.ReadAllText(filePath);

            foreach (var sig in _signatureDatabase.GetSignatures())
            {
                if (content.Contains(sig))
                {
                    return new ScanResult(
                        isInfected: true,
                        message: "Dosyada bilinen bir virus imzası tespit edildi.",
                        detectedSignature: sig);
                }
            }

            return new ScanResult(
                isInfected: false,
                message: "Herhangi bir bilinen virus imzası bulunamadı.",
                detectedSignature: null);
        }
    }
}



using System.IO;

namespace VirusSim
{
    /// <summary>
    /// OOP Örneği: Tüm zararlı yazılımlar için ortak sözleşme.
    /// </summary>
    public interface IMalware
    {
        string Name { get; }
        void Infect(string filePath);
    }

    /// <summary>
    /// OOP Örneği: Ortak davranışları tutan temel (base) sınıf.
    /// Diğer virus türleri bu sınıftan kalıtım alabilir.
    /// </summary>
    public abstract class BaseVirus : IMalware
    {
        public string Name { get; }

        protected BaseVirus(string name)
        {
            Name = name;
        }

        public abstract void Infect(string filePath);

        protected void AppendLine(string filePath, string line)
        {
            File.AppendAllText(filePath, line + "\n");
        }
    }

    /// <summary>
    /// Basit bir dosya sonuna imza ekleyen virus simülasyonu.
    /// Gerçek sisteme zarar vermez, sadece metin dosyasına özel bir satır ekler.
    /// </summary>
    public class SimpleFileVirus : BaseVirus
    {
        public const string Signature = "[VIRUS:SIMPLE]";

        public SimpleFileVirus() : base("Basit Dosya Virüsü")
        {
        }

        public override void Infect(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Bulaşma için hedef dosya bulunamadı.", filePath);
            }

            // Simülasyon: Dosyanın sonuna hem bir açıklama hem de imza ekliyoruz.
            AppendLine(filePath, "//infected-by-esu");
            AppendLine(filePath, Signature);
        }
    }
}



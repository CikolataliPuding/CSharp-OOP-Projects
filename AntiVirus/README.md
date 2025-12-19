# Basit AntiVirus ve Virus Simülasyonu

Bu proje, Nesne Tabanlı Programlama (OOP) dersi için geliştirilmiş bir **WinForms AntiVirus uygulaması** ve **Virus Simülasyonu** içerir. Proje, C# programlama dili kullanılarak yazılmıştır ve temel OOP kavramlarını (Interface, Inheritance, Polymorphism, Encapsulation) uygulamalı olarak gösterir.

## 📋 İçindekiler

- [Özellikler](#özellikler)
- [Proje Yapısı](#proje-yapısı)
- [Kurulum](#kurulum)
- [Kullanım](#kullanım)
- [OOP Kavramları](#oop-kavramları)
- [Teknik Detaylar](#teknik-detaylar)

## ✨ Özellikler

### AntiVirus Uygulaması
- ✅ Dosya tarama (imza tabanlı)
- ✅ Tarama sonuçlarını loglama
- ✅ WinForms grafik arayüzü
- ✅ Test virus oluşturma özelliği

### Virus Simülasyonu
- ✅ Dosya bulaştırma simülasyonu
- ✅ Güvenli test ortamı (gerçek sisteme zarar vermez)
- ✅ OOP tasarım desenleri

## 📁 Proje Yapısı

```
AntiVirus/
├── AntiVirus.sln              # Visual Studio çözüm dosyası
├── AntiVirusWin/               # WinForms AntiVirus Uygulaması
│   ├── AntiVirusWin.csproj
│   ├── Program.cs              # Uygulama giriş noktası
│   ├── MainForm.cs             # Ana form ve olay yönetimi
│   ├── MainForm.Designer.cs    # Form tasarımı
│   └── ScannerCore.cs          # Tarama mantığı (IScanner, FileScanner, vb.)
└── VirusSim/                   # Virus Simülasyon Projesi
    ├── VirusSim.csproj
    └── SimpleVirus.cs          # Virus sınıfları (IMalware, BaseVirus, SimpleFileVirus)
```

## 🚀 Kurulum

### Gereksinimler
- **.NET 8.0 SDK** veya üzeri
- **Visual Studio 2022** (önerilen) veya **Visual Studio Code**
- **Windows** işletim sistemi (WinForms gereksinimi)

### Adımlar

1. **Projeyi İndirin**
   ```bash
   git clone <repository-url>
   cd AntiVirus
   ```

2. **Visual Studio ile Açın**
   - `AntiVirus.sln` dosyasını Visual Studio ile açın
   - Visual Studio otomatik olarak gerekli paketleri yükleyecektir

3. **Projeyi Derleyin**
   - `Build` → `Build Solution` (veya `Ctrl+Shift+B`)
   - Hata olmadan derlenmeli

4. **Çalıştırın**
   - `AntiVirusWin` projesini **Startup Project** olarak ayarlayın
   - `F5` tuşuna basın veya **Debug** → **Start Debugging**

## 💻 Kullanım

### Senaryo 1: Test Virus Oluşturma ve Tarama

1. Programı çalıştırın
2. **"Test Virus Oluştur"** butonuna tıklayın
   - Geçici klasörde bir test dosyası oluşturulur
   - Dosyaya virus simülasyonu bulaştırılır
   - Dosya yolu otomatik olarak yazılır
3. **"Tara"** butonuna tıklayın
4. Log alanında sonuçları görüntüleyin:
   - **Durum**: BULAŞIK veya TEMİZ
   - **Detay**: Tespit edilen imza bilgisi

### Senaryo 2: Kendi Dosyanızı Tarama

1. **"Gözat..."** butonuna tıklayın
2. Taranacak `.txt` dosyasını seçin
3. **"Tara"** butonuna tıklayın
4. Sonuçları log alanında görüntüleyin

### Test İçin Manuel Dosya Oluşturma

Virus tespiti için test dosyası oluşturmak isterseniz:

1. Not Defteri'nde yeni bir dosya oluşturun
2. İçine şu imzalardan birini ekleyin:
   ```
   [VIRUS:SIMPLE]
   ```
   veya
   ```
   ESU-VIRUS-SIGNATURE
   ```
   veya
   ```
   //infected-by-esu
   ```
3. Dosyayı `.txt` olarak kaydedin
4. Antivirus programında bu dosyayı seçip tarayın → **BULAŞIK** olarak görünecektir

> **Not**: Temiz bir dosya için bu imzalardan hiçbirini eklemeyin → **TEMİZ** olarak görünecektir.

## 🎓 OOP Kavramları

Bu proje, aşağıdaki Nesne Tabanlı Programlama kavramlarını uygulamalı olarak gösterir:

### 1. **Interface (Arayüz)**

**Kullanım Yerleri:**
- `IScanner`: Tüm tarayıcılar için ortak sözleşme
- `ISignatureDatabase`: İmza veritabanı için soyutlama
- `IMalware`: Zararlı yazılımlar için ortak arayüz

**Örnek:**
```csharp
public interface IScanner
{
    ScanResult Scan(string filePath);
}
```

**Faydası:** Farklı tarama algoritmaları yazılabilir (`FileScanner`, `NetworkScanner`, vb.) ve kod polimorfik olarak çalışır.

---

### 2. **Inheritance (Kalıtım)**

**Kullanım Yeri:**
- `BaseVirus` → `SimpleFileVirus`

**Örnek:**
```csharp
public abstract class BaseVirus : IMalware
{
    public string Name { get; }
    public abstract void Infect(string filePath);
    protected void AppendLine(string filePath, string line) { ... }
}

public class SimpleFileVirus : BaseVirus
{
    public override void Infect(string filePath) { ... }
}
```

**Faydası:** Ortak özellikler ve davranışlar `BaseVirus`'ta tanımlanır, özel davranışlar alt sınıflarda override edilir.

---

### 3. **Polymorphism (Çok Biçimlilik)**

**Kullanım Yeri:**
- `MainForm` sadece `IScanner` arayüzünü bilir
- Somut sınıf (`FileScanner`) runtime'da belirlenir

**Örnek:**
```csharp
private readonly IScanner _scanner;

public MainForm()
{
    _scanner = new FileScanner(new SimpleSignatureDatabase());
}

var result = _scanner.Scan(path); // Polimorfik çağrı
```

**Faydası:** Gelecekte farklı bir tarayıcı (`AdvancedScanner`) yazılsa bile, form kodu değişmeden çalışır.

---

### 4. **Encapsulation (Kapsülleme)**

**Kullanım Yeri:**
- `ScanResult`: Tarama sonucu verilerini tek bir nesnede toplar
- `FileScanner`: İmza kontrol mantığını içeride saklar

**Örnek:**
```csharp
public class ScanResult
{
    public bool IsInfected { get; }
    public string Message { get; }
    public string? DetectedSignature { get; }
}
```

**Faydası:** Veriler ve davranışlar bir arada tutulur, dışarıdan erişim kontrollü hale gelir.

---

### 5. **Dependency Injection (Bağımlılık Enjeksiyonu)**

**Kullanım Yeri:**
- `FileScanner` constructor'ında `ISignatureDatabase` parametresi

**Örnek:**
```csharp
public class FileScanner : IScanner
{
    private readonly ISignatureDatabase _signatureDatabase;

    public FileScanner(ISignatureDatabase signatureDatabase)
    {
        _signatureDatabase = signatureDatabase;
    }
}
```

**Faydası:** `FileScanner` somut bir veritabanı sınıfına bağlı değildir. İsterseniz `XmlSignatureDatabase` veya `DatabaseSignatureDatabase` yazıp aynı şekilde kullanabilirsiniz.

---

## 🔧 Teknik Detaylar

### Tarama Algoritması

1. Kullanıcı bir dosya seçer
2. `FileScanner.Scan()` metodu çağrılır
3. Dosya içeriği okunur (`File.ReadAllText`)
4. Her imza için `Contains()` kontrolü yapılır
5. İmza bulunursa → `ScanResult(IsInfected: true)` döner
6. İmza bulunamazsa → `ScanResult(IsInfected: false)` döner

### Virus Simülasyonu

1. `SimpleFileVirus.Infect()` metodu çağrılır
2. Dosya varlığı kontrol edilir
3. Dosyanın sonuna iki satır eklenir:
   - `//infected-by-esu`
   - `[VIRUS:SIMPLE]`
4. Bu imzalar, antivirus tarafından tespit edilebilir

> **Güvenlik Notu:** Bu proje sadece eğitim amaçlıdır. Gerçek sistemlere zarar vermez. Tüm işlemler simülasyon amaçlıdır.

---

## 📝 Lisans ve Notlar

Bu proje, **Nesne Tabanlı Programlama** dersi için geliştirilmiştir.

**Geliştirici:** [Adınızı Buraya Yazın]  
**Tarih:** 2024  
**Ders:** NTP (Nesne Tabanlı Programlama)

---

## 🤝 Katkıda Bulunma

Bu bir öğrenci projesidir. Önerileriniz için issue açabilirsiniz.

---

## 📞 İletişim

Sorularınız için: [E-posta adresiniz]

---

**Not:** Bu proje eğitim amaçlıdır ve gerçek bir antivirus yazılımı değildir. Gerçek sistemlerde kullanmayın.


# LLM ChatBot - C# ile OpenAI Entegrasyonu

Bu proje, C# ve OpenAI API kullanarak geliştirilmiş bir konsol tabanlı chatbot uygulamasıdır.

## 🚀 Özellikler

- OpenAI GPT-3.5-turbo modeli ile konuşma
- Konuşma geçmişi yönetimi
- Türkçe dil desteği
- Konsol tabanlı kullanıcı arayüzü
- Yapılandırma dosyası ile kolay API anahtarı yönetimi

## 📋 Gereksinimler

- .NET 8.0 SDK veya üzeri
- OpenAI API anahtarı ([OpenAI Platform](https://platform.openai.com/) üzerinden alabilirsiniz)

## 🔧 Kurulum

1. Projeyi klonlayın veya indirin
2. `appsettings.json` dosyasını açın ve OpenAI API anahtarınızı ekleyin:

```json
{
  "OpenAI": {
    "ApiKey": "sk-your-api-key-here"
  }
}
```

Alternatif olarak, environment variable olarak da ayarlayabilirsiniz:
```bash
# Windows PowerShell
$env:OPENAI_API_KEY="sk-your-api-key-here"

# Linux/Mac
export OPENAI_API_KEY="sk-your-api-key-here"
```

3. Projeyi derleyin:
```bash
dotnet build
```

4. Uygulamayı çalıştırın:
```bash
dotnet run
```

## 💻 Kullanım

Uygulamayı başlattıktan sonra:

1. Konsola mesajınızı yazın ve Enter'a basın
2. Bot'un cevabını bekleyin
3. Çıkmak için `çıkış`, `exit` veya `quit` yazın
4. Konuşma geçmişini temizlemek için `temizle` veya `clear` yazın

### Örnek Kullanım

```
🤖 LLM ChatBot'a Hoş Geldiniz!
Çıkmak için 'çıkış' veya 'exit' yazın.
Konuşma geçmişini temizlemek için 'temizle' yazın.

Sen: Merhaba, nasılsın?
Bot: Merhaba! Ben bir AI asistanıyım ve iyi durumdayım, teşekkür ederim. Size nasıl yardımcı olabilirim?

Sen: Bugün hava nasıl?
Bot: Maalesef gerçek zamanlı hava durumu bilgisine erişimim yok. Hava durumu bilgisi için bir hava durumu servisi veya uygulaması kullanmanızı öneririm.

Sen: çıkış
Görüşmek üzere! 👋
```

## 📁 Proje Yapısı

```
LLMChatBot/
├── Services/
│   └── ChatService.cs      # OpenAI API entegrasyonu ve chat mantığı
├── Program.cs              # Ana program ve kullanıcı arayüzü
├── appsettings.json        # Yapılandırma dosyası
└── LLMChatBot.csproj       # Proje dosyası
```

## 🔐 Güvenlik

- API anahtarınızı asla Git'e commit etmeyin
- `appsettings.json` dosyasını `.gitignore` dosyasına ekleyin
- Production ortamında environment variable kullanın

## 📝 Lisans

Bu proje eğitim amaçlıdır.

## 🤝 Katkıda Bulunma

Katkılarınızı bekliyoruz! Lütfen pull request gönderin.

## 📚 Kaynaklar

- [OpenAI API Dokümantasyonu](https://platform.openai.com/docs)
- [OpenAI-DotNet NuGet Paketi](https://www.nuget.org/packages/OpenAI-DotNet/)
- [.NET Dokümantasyonu](https://learn.microsoft.com/dotnet/)


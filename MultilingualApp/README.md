## MultilingualApp – Basit Çok Dilli Konsol Uygulaması

Bu proje, C# ve .NET 8 kullanarak yapılmış basit bir **çok dilli (tr/en/es)** konsol uygulamasıdır.  
Uygulama, `.resx` kaynak dosyaları ile metinleri farklı dillere çevirir ve seçilen kültüre göre tarih/para formatlarını gösterir.

### Nasıl Çalıştırılır?
- Gerekli: [.NET SDK](https://dotnet.microsoft.com/) (en az .NET 8)
- Konsolda proje klasörüne gir:

```bash
cd MultilingualApp
dotnet run
```

### Dil Seçimi
Uygulamayı iki şekilde kullanabilirsin:

- **Konsoldan dil seçerek**:
  - `dotnet run`
  - Program açılınca `tr`, `en` veya `es` yazıp ENTER’a bas.

- **Komut satırında argümanla**:
  - `dotnet run -- tr`
  - `dotnet run -- en`
  - `dotnet run -- es`

### Kısa Notlar
- Dil metinleri `Resources/Strings*.resx` dosyalarında tutulur.
- `Program.cs` içinde `ResourceManager` ve `CultureInfo` kullanılarak dil ve kültür ayarlanır.
- Daha detaylı ders notu ve açıklama için `DersNotu.md` dosyasına bakabilirsin.




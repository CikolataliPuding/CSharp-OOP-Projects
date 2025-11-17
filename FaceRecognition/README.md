# FaceRecognition (Emgu CV + OpenCV, WinForms, .NET 8)

Bu proje, Emgu CV kullanarak Windows Forms üzerinde yüz algılama ve LBPH ile basit yüz tanıma örneği içerir.

## Gereksinimler
- Windows 10/11
- .NET 8 SDK (dotnet --version ile kontrol edebilirsiniz)
- Çalışan bir web kamera
- Visual Studio 2022 (önerilir) veya yalnızca .NET CLI

Projede NuGet bağımlılıkları tanımlıdır ve otomatik indirilir:
- Emgu.CV
- Emgu.CV.runtime.windows
- Emgu.CV.UI
- System.Speech

## Kurulum ve Çalıştırma
### Visual Studio
1. `FaceRecognition.sln` dosyasını açın
2. Gerekirse NuGet paketlerini geri yükleyin
3. F5 ile çalıştırın

### Komut Satırı (PowerShell)
```powershell
cd .\FaceRecognition

dotnet restore

dotnet run
```
Alternatif:
```powershell
dotnet build
./bin/Debug/net8.0-windows/FaceRecognition.exe
```

## Kullanım
1. Uygulamada "Kamera Başlat" butonuna tıklayın
2. Yüzünüz kadrajdayken yeşil kutu görünür (yüz algılandı)
3. "Yüz Kaydet" ile örnekleri kaydedin (100x100 gri, faces klasörüne)
4. "Eğit / Tanı" ile LBPH modeli eğitin (egemen.yml oluşturulur)
5. Eğitim sonrası canlı akışta ID ve mesafe (confidence) görünür
6. Uygulama açılışında model dosyası varsa otomatik yüklenir

## Dosyalar
- `Form1.cs`: Kamera, algılama ve tanıma akışı
- `Form1.Designer.cs`: Arayüz tanımları
- `haarcascade_frontalface_default.xml`: Haar Cascade yüz algılama modeli
- `faces/`: Kayıtlı yüz görüntüleri (çalışma esnasında oluşur)
- `egemen.yml`: LBPH model dosyası (eğitimden sonra oluşur)

## Çoklu Kişi (öneri)
- `faces/<isim>/*.jpg` şeklinde klasörleme yapın
- Eğitimde klasör adını etikete eşleyip canlıda etiketten isme çevirin

## Sorun Giderme
- Kamera açılmıyor veya siyah ekran:
  - Kamerayı kullanan diğer uygulamaları kapatın, yeniden deneyin
  - Harici kamera ise varsayılan aygıtı kontrol edin
- Cascade dosyası bulunamadı:
  - `haarcascade_frontalface_default.xml` derleme çıktısına kopyalanır, bulunduğundan emin olun
- Yüz algılanmıyor:
  - Aydınlatmayı artırın, kameraya yaklaşın, parametreleri (scaleFactor, minNeighbors, minSize) ayarlayın
- Tanıma zayıf:
  - Daha çok ve çeşitli örnek kaydedin, LBPH parametrelerini deneyin (radius, neighbors, gridX, gridY)

## Gizlilik
- `faces` klasöründeki görüntüler düz dosyadır. Kişisel veriler için dikkatli olun.

## Lisans
- Eğitim amaçlı örnek. Kendi kullanımınıza göre uyarlayabilirsiniz.


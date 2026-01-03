TESSDATA KLASÖRÜ (ÖNEMLİ)

Bu klasör, Tesseract OCR motorunun dil verilerini (traineddata) bulduğu yerdir.

1) Bu projede kod, tessdata yolunu şu şekilde arar:
   - Uygulamanın çalıştığı klasörün içinde:  ...\bin\Debug\net8.0-windows\tessdata\

2) Bu proje dosyası (csproj) ayarı sayesinde:
   - Proje içindeki "tessdata" klasörünün içeriği build çıktısına otomatik kopyalanır.

3) Ne yapmalısın?
   - Tesseract "traineddata" dosyasını indirip bu klasöre koy:
     Örnek: eng.traineddata

4) Nereden bulunur?
   - Resmi tessdata deposu: https://github.com/tesseract-ocr/tessdata

Not:
- Matematik işlemleri için genelde "eng" yeterlidir (rakamlar ve operatörler).
- İstersen "tur.traineddata" da ekleyip dili "eng+tur" yapabilirsin.


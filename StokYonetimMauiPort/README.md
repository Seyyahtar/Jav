# Stok Yönetim MAUI Uygulaması

Bu klasör, stok yönetimi, vaka girişi ve kontrol listesi işlemlerini takip etmek için .NET 9 tabanlı bir .NET MAUI uygulaması içerir. Uygulama aşağıdaki teknolojileri kullanır:

- .NET MAUI (net9.0)
- CommunityToolkit.MVVM & CommunityToolkit.MAUI
- sqlite-net-pcl ile yerel SQLite veritabanı
- ClosedXML ile Excel'e veri aktarma desteği

## Başlangıç

> Not: Bu kod paketi çevrim içi çalışma ortamında oluşturulduğu için .NET SDK'sı bulunmuyor. Aşağıdaki adımlar yerel bilgisayarınızda .NET 9 SDK kurulu olduğu varsayılarak verilmiştir.

```bash
cd StokYonetimMauiPort
nuget restore StokYonetimMaui.sln
# veya dotnet restore da kullanılabilir

# Uygulamayı çalıştırma (ör. Android emülatörü için)
dotnet build StokYonetimMaui.sln -t:Run -f net9.0-android
```

Diğer platformlar için uygun hedef çerçevesini (`net9.0-ios`, `net9.0-maccatalyst`, `net9.0-windows10.0.19041`) kullanabilirsiniz. SVG tabanlı ikon ve splash ekran dosyaları derleme sürecinde ilgili platformlar için otomatik olarak üretildiğinden, repoda PNG veya benzeri ikili varlıklar yer almaz.

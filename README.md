# Stok Yönetim Projesi Kod Paketleri

Bu depo iki farklı uygulama barındırır:

- **Kaynak/** klasöründe, orijinal Vite + React tabanlı stok yönetimi arayüzü bulunur. Bu klasör önceki sürümde olduğu gibi korunmuştur ve `npm install` / `npm run dev` komutları ile çalıştırılabilir.
- **StokYonetimMauiPort/** klasöründe ise uygulamanın .NET 9.0 MAUI uyarlaması yer alır. MAUI çözümü, CommunityToolkit.Mvvm, CommunityToolkit.Maui, sqlite-net-pcl ve ClosedXML gibi NuGet paketlerini kullanır. Platformlara özgü ikon ve splash ekran dosyaları derleme sırasında SVG varlıklarından üretildiği için depoda ikili (binary) dosya tutulmaz.

Aşağıdaki alt başlıklarda her iki uygulamayı çalıştırmak için temel adımlar özetlenmiştir.

## Vite + React uygulaması (Kaynak/)

```bash
cd Kaynak
npm install
npm run dev
```

## .NET MAUI uygulaması (StokYonetimMauiPort/)

```bash
cd StokYonetimMauiPort
nuget restore StokYonetimMaui.sln
# Örnek: Android derlemesi
dotnet build StokYonetimMaui.sln -t:Run -f net9.0-android
```

Farklı hedefler için `dotnet build` komutunda ilgili hedef çerçeveyi (`net9.0-ios`, `net9.0-maccatalyst`, `net9.0-windows10.0.19041`) kullanabilirsiniz.

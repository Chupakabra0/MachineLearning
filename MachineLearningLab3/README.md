![banner](https://www.softwareone.com/-/media/global/social-media-and-blog/hero/implementing-artificial-intelligence-part-1-hero.jpg?rev=56ebf75efd06466786861433a1cae008&sc_lang=en-be&hash=705D747C1F39E295D2BFB19901067B5B)
## Лабораторная работа №3

### Постановка задачи

Выбрать произвольную задачу и продемонстрировать работу прикладной нейронной сети для её решения. Разрешено использовать различные библиотеки и языки программирования.

### Выполнение работы

#### Краткий обзор нейронной сети

Для выполнения работы была выбрана нейронная сеть, размещённая в открытом доступе в Интернете, которая решает задачу генерации изображений на основе лингвистического описания на английском языке, а именно [**Craiyon**](https://www.craiyon.com), ранее известная как DALL-E Mini.

Используемая модель основана на более крупной и недоступной для массового использования версии нейронной сети, также известная как DALL-E mega, и обучается с использованием Google TRC.

Сайт имеет минималистичный интерфейс. Нас интересует только текстовое поле, в которое следует ввести запрос, по которому нейронная сеть построит 9 картинок.

Обычно, генерация картинки занимает от 30 до 100 секунд.

![|center](https://i.imgur.com/yDKTe66.png)

![|center](https://i.imgur.com/EFE9wPu.png)

Как можно видеть, нейронная сеть не очень удачно справляется с лицами, однако в остальном она генерирует довольно таки достойные изображения:

![|center](https://i.imgur.com/1J6Kd2x.png)

![|center](https://i.imgur.com/oIPQ3VK.png)

![|center](https://i.imgur.com/0IhgAXN.png)

#### Написание приложения

Для того, чтобы воспользоваться этим сайтом в нашем приложении, достаточно правильно составить веб-запрос, в ответ на который мы получим 9 картинок по заданному тексту. Для языка программирования C# доступна библиотека `DotNetCraiyon`, которая с лёгкостью позволит решить эту задачу.

Создадим приложение на фреймворке WPF. Для него через менеджер пакетов NuGet следует поставить такие библиотеки:

- [`DotNetCraiyon`](https://github.com/sa111n111/Craiyon.Net) — .NET обёртка над веб-сервисом Craiyon AI;
- [`WPF-UI`](https://github.com/lepoco/wpfui) — простая библиотека, позволяющая придать WPF-приложению современный дизайн а-ля Windows 11;
- [`Fody.PropertyChanged`](https://github.com/Fody/PropertyChanged) — внедрение кода, вызывающего событие `PropertyChanged`, в сеттеры свойств классов, реализующих интерфейс `INotifyPropertyChanged` для реализации паттерна MVVM;
- [`JetBrains.Annotations`](https://www.jetbrains.com/help/resharper/Code_Analysis__Code_Annotations.html) — библиотека вспомогательных атрибутов.

Код команды, которая отправляет запрос веб-сайту и принимает 9 картинок приведён ниже:

```csharp
async void GenerateImage()
{
    var craiyonService = new CraiyonService();

    try
    {
        this.IsWorking = true;
        await craiyonService.DownloadGalleryAsync(this.InputText, $"{WindowViewModel.ImageFolderName}");

        this.IsImageExists         = false;
        this.ImageSourceCollection = new();

        for (var i = 0; i < WindowViewModel.MaxImages; ++i)
        {
            var newName = $"{Guid.NewGuid()}.jpg";
            FileSystem.RenameFile($"{WindowViewModel.ImageFolderName}/{i}.jpg", newName);

            this.ImageSourceCollection.Add(new TransformedBitmap(new BitmapImage(new Uri($"{WindowViewModel.ImageFolderName}/{newName}", UriKind.Relative)),new ScaleTransform(WindowViewModel.Scale, WindowViewModel.Scale)));
        }

        this.Index         = 0;
        this.IsImageExists = true;

        this.OnPropertyChanged(nameof(this.ImageSource));
    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
    }
    finally
    {
        this.IsWorking = false;
    }
}
```

#### Примеры работы приложения

Запустив приложение, мы видим, что по умолчанию оно использует тёмную тема и системный акцентный цвет (в данном примере это светлый фиолетово-красный).

![|center](https://i.imgur.com/YN6G2ei.png)

Нас просят "ввести что-то крутое". Введём же то, что нас попросили.

![|center](https://i.imgur.com/VaGmz33.png)

Кнопка, которая ранее была недоступна, теперь кликабельная. После нажатие на неё, она снова становится недоступной, а текст на кнопке изменится.

![](https://i.imgur.com/yRnrQNZ.png)

Подождав некоторое время, на экране появится одно из 9 изображений, сгенерированных нейросетью, и две кнопки, который позволят переключаться между ними.

![|center](https://i.imgur.com/iXSVPS7.png)

![|center](https://i.imgur.com/TonT9NI.png)

Также в папке с исполняемым файлом программы появится папка `images`, куда будут сохранены эти самые изображения. Также здесь можно будет найти результаты работы предыдущих запусков приложения.

![|center](https://i.imgur.com/jU3WNtV.png)

### Выводы

Была освоена и исследована нейронная сеть Craiyon, которая позволяет легко генерировать различные изображения по лингвистическому описанию. Также было написано приложение-клиент, позволяющее воспользоваться возможностями этой нейронной сети.

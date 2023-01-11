![banner](https://aqi.co.id/wp-content/uploads/2021/03/apa-itu-machine-learning.jpg)

## Лабораторная работа №1

### Постановка задачи

1. Создать текстовый файл с данными.
2. Считать его, исключив из него следующие знаки:
   ```
   !()-[]{};:'"\,<>./?@#$%^&*_~
   ```
3. Убрать из файла "неинтересные" слова для анализа:
   ```
   ["the", "a", "to", "if", "is", "it", "of", "and", "or", "an", "as", "i", "me", "my", "we", "our", "ours", "you", "your", "yours", "he", "she", "him", "his", "her", "hers", "its", "they", "them", "their", "what", "which", "who", "whom", "this", "that", "am", "are", "was", "were", "be", "been", "being", "have", "has", "had", "do", "does", "did", "but", "at", "by", "with", "from", "here", "when", "where", "how", "all", "any", "both", "each", "few", "more", "some", "such", "no", "nor", "too", "very", "can", "will", "just", "on", "in"]
   ``` 
4. Создать словарь из пар "слово-количество его вхождений в файле".
5. Отобразить "облако слов".

### Выполнение работы

Воспользуемся языком программирования C#. Для выполнения задачи необходимо установить несколько NuGet-пакетов, а именно:

- [`CommandLineParser`](https://github.com/commandlineparser/commandline) — библиотека, предоставляющая класс-анализатор аргументов командной строки на языке программирования C#;
- [`KnowledgePicker.WordCloud`](https://github.com/knowledgepicker/word-cloud) — библиотека для создания и рисования "облаков слов" (также известных как облака тегов или wordle).
- [`SkiaSharp`](https://github.com/mono/SkiaSharp) (*опционально, может быть уже установлена*) — кроссплатформенный API 2D-графики для платформ .NET на основе библиотеки Google Skia Graphics Library, необходим для построения изображения "облака слов".

Наиболее сложной подзадачей в данной работе является метод формирования словаря, на основе которого будет построено "облако слов". Предоставляю код этого алгоритма:

```csharp
public Dictionary<string, int> GetFileDictionary()
{
	var freq = new Dictionary<string, int>();

	while (true)
	{
		var line = this.textLineGetter_.GetTextLine();
		if (line is "")
		{
			continue;
		}
		if (line is null)
		{
			break;
		}
		line = line.GetRidOfRedundantSymbols();

		foreach (var word in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
		{
			if (!exceptionStrings_?.Contains(word) ?? true)
			{
				if (!freq.TryAdd(word, 1))
				{
					++freq[word];
				}
			}
		}
	}

	return freq;
}
```

Код для построения "облака слов" приведён ниже:

```csharp
private static void Run(Options options)
{
	var exceptions = new[]
	{
                "the", "a", "to", "if", "is", "it", "of", "and", "or", "an", "as", "i", "me", "my",
                "we", "our", "ours", "you", "your", "yours", "he", "she", "him", "his", "her", "hers",
                "its", "they", "them", "their", "what", "which", "who", "whom", "this", "that", "am",
                "are", "was", "were", "be", "been", "being", "have", "has", "had", "do", "does", "did",
                "but", "at", "by", "with", "from", "here", "when", "where", "how", "all", "any", "both",
                "each", "few", "more", "some", "such", "no", "nor", "too", "very", "can", "will", "just",
                "on", "in"
	};

	var fileDic = new TextDictionaryGenerator(new FileTextLineGetter(new FileInfo(options.InputPathStr)), exceptions);
	// TODO: modernize sorting comparator
	var freq = fileDic.GetFileDictionary().OrderBy(pair => pair.Value).Reverse();

	foreach (var (key, value) in freq)
	{
		Console.WriteLine($"{key} : {value}");
	}

	var wordCloud = new WordCloudInput(freq.Select(p => new WordCloudEntry(p.Key, p.Value)))
	{
		Width       = 2560,
		Height      = 1440,
		MinFontSize = 6,
		MaxFontSize = 48
	};

	var sizer     = new LogSizer(wordCloud);
	var engine    = new SkGraphicEngine(sizer, wordCloud);
	var layout    = new SpiralLayout(wordCloud);
	var colorizer = new RandomColorizer(); // optional
	var wcg       = new WordCloudGenerator<SKBitmap>(wordCloud, engine, layout, colorizer);

	// Draw the bitmap on black background
	var final  = new SKBitmap(wordCloud.Width, wordCloud.Height);
	var canvas = new SKCanvas(final);

	canvas.Clear(SKColors.Black);
	canvas.DrawBitmap(wcg.Draw(), 0, 0);

	// Save to PNG
	var data   = final.Encode(SKEncodedImageFormat.Png, 100);
	var writer = new FileInfo(options.OutputPathStr).Create();

	data.SaveTo(writer);
}
```

### Примеры

Запустив консольное приложение с флагом `--help` мы увидим следующий вывод на экран:

![](https://i.imgur.com/YeKmLoF.png)

Приложение имеет два обязательных аргумента: `--input` и `--output`, которые определяют пути к файлам на ввод и вывод соответственно. На вход приложение требует текстовый файл, на выходе — изображение формата `.png`.

Попробуем протестировать приложение сценарием [небезызвестного мультипликационного фильма](https://www.imdb.com/title/tt0126029/). Больше тестовых файлов размещены в [папке `tests/`](tests/).

![](https://i.imgur.com/2pUVi3p.png)

В консоль будут выведены все элементы словаря:

![](https://i.imgur.com/RZgt1wO.png)

Также по пути, указанному ранее, будет создан файл, который содержит "облако слов":

![](https://i.imgur.com/iFyfPHH.png)

### Выводы

Было создано консольное приложение для построения "облака слов" из произвольного текстового файла. Задача построения изображения "облака" оказалась тривиальной и решалась при помощи сторонних библиотек, в то время как построение словаря, на основе которого будет построено изображение, оказалось наиболее сложной задачей, которая была успешно решена.

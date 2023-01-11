![banner](https://media.springernature.com/w580h326/nature-cms/uploads/collections/AI_HERO-58306268c6f4b659459f5b7b2dd3e8a5.jpg)

## Лабораторная работа №2

### Постановка задачи

Создать искусственный интеллект в виде экспертной системы на основе алгоритмов нечёткой логики на произвольную тему. ИИ должен обрабатывать на вход не меньше трёх лингвистических переменных.

### Выполнение работы

#### Тематика экспертной системы и выбор лингвистических переменных

Создадим интеллектуальный бенчмарк для персональных компьютеров, который на основе алгоритмов нечёткой логики будет принимать решение, насколько тот или иной компьютер соответствует современным требованиям к вычислительным и игровым мощностям. Иными словами, он будет проводить примитивные расчёты *коэффициента производительности*.

![|center](https://i.imgur.com/3KDgXwr.png)

Простоты ради используем только три характеристики, которые и станут лингвистическими переменными:

- Рейтинг производительности ЦПУ;
- Рейтинг производительности ГПУ;
- Рейтинг производительности ОЗУ.

Выходной лингвистической переменной будет, очевидно, коэффициент производительности.

Все лингвистические переменные будут находится на промежутке $[0; 100]$.

Что касается определения рейтинга производительности ЦПУ и ГПУ, то рекомендуется пользоваться удобными сайтами:

- для ЦПУ — https://technical.city/ru/cpu/rating
- для ГПУ — https://technical.city/ru/video/rating

Корректность рейтинга ОЗУ остаётся на совести пользователя.  Рекомендуется обращать внимание на такие характеристики (выше — важнее):

- частота (выше — лучше);
- латентность/тайминги (ниже — лучше);
- пропускная способность (выше — лучше);
- объём (выше — лучше);

#### Система правил

Составим систему правил. Таблицы ниже будут её визуализировать.

Если ЦПУ: 👍

| $\dfrac{\text{ОЗУ}}{\text{ЦПУ}}$ | 👍  | 👌  | 👎  |
|:--------------------------------:|:---:|:---:|:---:|
|                👍                | 👍  | 👍  | 👌  |
|                👌                | 👍  | 👌  | 👌  |
|                👎                | 👌  | 👌  | 👌  |

Если ЦПУ: 👌

| $\dfrac{\text{ОЗУ}}{\text{ЦПУ}}$ | 👍  | 👌  | 👎  |
|:--------------------------------:|:---:|:---:|:---:|
|                👍                | 👍  | 👌  | 👌  |
|                👌                | 👌  | 👌  | 👎  |
|                👎                | 👌  | 👎  | 👎  |

Если ЦПУ: 👎

| $\dfrac{\text{ОЗУ}}{\text{ЦПУ}}$ | 👍  | 👌  | 👎  |
|:--------------------------------:|:---:|:---:|:---:|
|                👍                | 👌  | 👌  | 👎  |
|                👌                | 👌  | 👎  | 👎  |
|                👎                | 👎  | 👎  | 👎  |

#### Создание программы

Воспользуемся языком программирования C#. Для выполнения задачи необходимо установить пару NuGet-пакетов, а именно:

- [`CommandLineParser`](https://github.com/commandlineparser/commandline) — библиотека, предоставляющая класс-анализатор аргументов командной строки на языке программирования C#;
- [`FLS`](https://github.com/davidgrupp/Fuzzy-Logic-Sharp) — библиотека, реализующая простую в использовании систему нечёткой логики.

Разместим код создания `IFuzzyEngine`, в котором можно увидеть создание всех лингвистических переменных и систему правил:

```csharp
private static IFuzzyEngine CreateFuzzyEngine() {
    var fuzzyEngine = new FuzzyEngineFactory().Default();

    var centralProcessorLingusticVar = new LinguisticVariable("CPU");
    var centralProcessorBad          = centralProcessorLingusticVar.MembershipFunctions.AddTriangle("Bad CPU", 0, 7.5, 14);
    var centralProcessorGood         = centralProcessorLingusticVar.MembershipFunctions.AddTriangle("Good CPU", 10, 15, 20);
    var centralProcessorPerfect      = centralProcessorLingusticVar.MembershipFunctions.AddTriangle("Perfect CPU", 17.5, 40, 100);

    var videoCardLingusticVar = new LinguisticVariable("GPU");
    var videoCardBad          = videoCardLingusticVar.MembershipFunctions.AddTriangle("Bad GPU", 0, 10, 20);
    var videoCardGood         = videoCardLingusticVar.MembershipFunctions.AddTriangle("Good GPU", 15, 35, 55);
    var videoCardPerfect      = videoCardLingusticVar.MembershipFunctions.AddTriangle("Perfect GPU", 40, 100, 100);

    var memoryLingusticVar = new LinguisticVariable("RAM");
    var memoryBad          = memoryLingusticVar.MembershipFunctions.AddTriangle("Bad RAM", 0, 12.5, 30);
    var memoryGood         = memoryLingusticVar.MembershipFunctions.AddTriangle("Good RAM", 25, 40, 55);
    var memoryPerfect      = memoryLingusticVar.MembershipFunctions.AddTriangle("Perfect RAM", 45, 100, 100);

    var computerSpeedLingusticVar = new LinguisticVariable("PC Speed");
    var computerSpeedBad          = computerSpeedLingusticVar.MembershipFunctions.AddTrapezoid("Bad PC Speed", 0, 0, 15, 30);
    var computerSpeedGood         = computerSpeedLingusticVar.MembershipFunctions.AddTriangle("Good PC Speed", 20, 50, 75);
    var computerSpeedPerfect      = computerSpeedLingusticVar.MembershipFunctions.AddTrapezoid("Perfect PC Speed", 65, 85, 100, 100);

    var ruleList = new List<FuzzyRule> {
        // slow CPU cases
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorBad)
                .And(memoryLingusticVar.Is(memoryBad))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedBad)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorBad)
                .And(videoCardLingusticVar.Is(videoCardBad))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedBad)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorBad)
                .And(videoCardLingusticVar.Is(videoCardBad))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedBad)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorBad)
                .And(memoryLingusticVar.Is(memoryPerfect))
                .And(videoCardLingusticVar.IsNot(videoCardBad))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorBad)
                .And(memoryLingusticVar.Is(memoryGood))
                .And(videoCardLingusticVar.Is(videoCardPerfect))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorBad)
                .And(memoryLingusticVar.Is(memoryGood))
                .And(videoCardLingusticVar.Is(videoCardGood))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedBad)),

        // good CPU cases
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorGood)
                .And(memoryLingusticVar.Is(memoryPerfect))
                .And(videoCardLingusticVar.Is(videoCardPerfect))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedPerfect)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorGood)
                .And(memoryLingusticVar.IsNot(memoryPerfect))
                .And(videoCardLingusticVar.Is(videoCardPerfect))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorGood)
                .And(memoryLingusticVar.Is(memoryPerfect))
                .And(videoCardLingusticVar.IsNot(videoCardPerfect))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorGood)
                .And(memoryLingusticVar.Is(memoryBad))
                .And(videoCardLingusticVar.IsNot(videoCardPerfect))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedBad)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorGood)
                .And(memoryLingusticVar.Is(memoryGood))
                .And(videoCardLingusticVar.Is(videoCardGood))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorGood)
                .And(memoryLingusticVar.Is(memoryBad))
                .And(videoCardLingusticVar.Is(videoCardBad))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedBad)),

        // perfect CPU cases
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorPerfect)
                .And(memoryLingusticVar.Is(memoryGood))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorPerfect)
                .And(videoCardLingusticVar.Is(videoCardGood))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorPerfect)
                .And(memoryLingusticVar.Is(memoryPerfect))
                .And(videoCardLingusticVar.IsNot(videoCardBad))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedPerfect)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorPerfect)
                .And(memoryLingusticVar.Is(memoryGood))
                .And(videoCardLingusticVar.Is(videoCardGood))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedGood)),
        Rule.If (
            centralProcessorLingusticVar.Is(centralProcessorPerfect)
                .And(memoryLingusticVar.Is(memoryGood))
                .And(videoCardLingusticVar.Is(videoCardPerfect))
        ).Then(computerSpeedLingusticVar.Is(computerSpeedPerfect))
    };

    fuzzyEngine.Rules.Add(ruleList.ToArray());

    return fuzzyEngine;
}
```

### Примеры

Запустив консольное приложение с флагом `--help` мы увидим следующий вывод на экран:

![|center](https://i.imgur.com/7WVb7Rb.png)

Приложение имеет три обязательных аргумента: `--cpu`, `--gpu` и `--ram`, которые определяют уровень производительности комплектующих.

Также приложение имеет секретный флаг `--test`. Если запустить приложение с ним, то будет запущены три тестовых случая, на которых можно убедится в работоспособности программы:

![|center](https://i.imgur.com/ya0P3Pp.png)

Введя пользовательские данные при помощи аргументов командной строки получаем следующий вывод:

![|center"](https://i.imgur.com/OJW01WZ.png)

### Выводы

Было создано консольное приложение с использованием алгоритмов нечёткой логики для анализа производительности персонального компьютера, основываясь на рейтингах производительность ЦПУ, ГПУ и ОЗУ. Наиболее сложной задачей оказалось в разработке $\pm$ адекватной системы правил, однако она была по итогу решена.

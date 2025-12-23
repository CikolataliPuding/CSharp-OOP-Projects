using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;

namespace MultilingualApp;

internal class Program
{
    private static readonly Dictionary<string, CultureInfo> SupportedCultures = new(StringComparer.OrdinalIgnoreCase)
    {
        { "tr", new CultureInfo("tr-TR") },
        { "en", new CultureInfo("en-US") },
        { "es", new CultureInfo("es-ES") }
    };

    private static readonly ResourceManager Strings = new("MultilingualApp.Resources.Strings", Assembly.GetExecutingAssembly());

    // Çevrilebilen ifade anahtarları (her dilde karşılığı .resx içinde tanımlı)
    private static readonly string[] PhraseKeys =
    {
        "Phrase_Hello",
        "Phrase_GoodMorning",
        "Phrase_ThankYou",
        "Phrase_Goodbye"
    };

    private static string GetString(string key, CultureInfo? culture = null) =>
        Strings.GetString(key, culture ?? CultureInfo.CurrentUICulture) ?? key;

    private static CultureInfo GetCultureFromArgs(string[] args)
    {
        if (args.Length > 0 && SupportedCultures.TryGetValue(args[0], out var culture))
        {
            return culture;
        }

        return CultureInfo.GetCultureInfo("en-US");
    }

    private static CultureInfo AskCulture(string promptKey)
    {
        Console.WriteLine(GetString(promptKey));

        foreach (var culture in SupportedCultures)
        {
            Console.WriteLine($" - {culture.Key} : {culture.Value.NativeName}");
        }

        Console.Write("> ");
        var selection = (Console.ReadLine() ?? string.Empty).Trim();

        if (SupportedCultures.TryGetValue(selection, out var cultureInfo))
        {
            return cultureInfo;
        }

        Console.WriteLine(GetString("InvalidLanguage"));
        return SupportedCultures["en"];
    }

    private static void ApplyCulture(CultureInfo culture)
    {
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }

    private static int AskPhraseIndex(CultureInfo sourceCulture)
    {
        Console.WriteLine();
        Console.WriteLine(GetString("PromptSelectPhrase", sourceCulture));

        for (var i = 0; i < PhraseKeys.Length; i++)
        {
            var phrase = GetString(PhraseKeys[i], sourceCulture);
            Console.WriteLine($" {i + 1}) {phrase}");
        }

        Console.WriteLine();
        Console.Write(GetString("PromptPhraseNumber", sourceCulture));
        var input = Console.ReadLine();

        if (int.TryParse(input, out var index))
        {
            index -= 1;
            if (index >= 0 && index < PhraseKeys.Length)
            {
                return index;
            }
        }

        Console.WriteLine(GetString("InvalidSelection", sourceCulture));
        return -1;
    }

    private static void RunTranslator(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Kaynak ve hedef dili al
        var sourceCulture = AskCulture("PromptSourceLanguage");
        var targetCulture = AskCulture("PromptTargetLanguage");

        Console.WriteLine();
        Console.WriteLine(string.Format(GetString("InfoLanguages"), sourceCulture.DisplayName, targetCulture.DisplayName));

        // İfade seçimi
        var phraseIndex = AskPhraseIndex(sourceCulture);
        if (phraseIndex < 0)
        {
            Console.WriteLine();
            Console.WriteLine(GetString("ExitMessage", sourceCulture));
            return;
        }

        var key = PhraseKeys[phraseIndex];
        var sourceText = GetString(key, sourceCulture);
        var targetText = GetString(key, targetCulture);

        Console.WriteLine();
        Console.WriteLine(new string('-', 40));
        Console.WriteLine(GetString("TranslationResultTitle", targetCulture));
        Console.WriteLine(string.Format(GetString("TranslationSource", targetCulture), sourceCulture.DisplayName, sourceText));
        Console.WriteLine(string.Format(GetString("TranslationTarget", targetCulture), targetCulture.DisplayName, targetText));
        Console.WriteLine(new string('-', 40));

        Console.WriteLine();
        Console.WriteLine(GetString("ExitMessage", targetCulture));
        Console.ReadLine();
    }

    private static void Main(string[] args)
    {
        RunTranslator(args);
    }
}

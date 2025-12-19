using LLMChatBot.Services;
using Microsoft.Extensions.Configuration;

namespace LLMChatBot;

class Program
{
    static async Task Main(string[] args)
    {
        // Yapılandırma dosyasını yükle
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // API anahtarını al (önce environment variable'dan, sonra appsettings.json'dan)
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") 
                     ?? configuration["OpenAI:ApiKey"];

        if (string.IsNullOrEmpty(apiKey) || apiKey == "BURAYA_API_ANAHTARINIZI_YAZIN")
        {
            Console.WriteLine("⚠️  UYARI: OpenAI API anahtarı bulunamadı!");
            Console.WriteLine("Lütfen aşağıdaki yöntemlerden birini kullanın:");
            Console.WriteLine("1. appsettings.json dosyasındaki 'OpenAI:ApiKey' değerini güncelleyin");
            Console.WriteLine("2. Veya environment variable olarak OPENAI_API_KEY ayarlayın");
            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
            Console.ReadKey();
            return;
        }

        // Chat servisini oluştur
        var chatService = new ChatService(apiKey);

        Console.WriteLine("🤖 LLM ChatBot'a Hoş Geldiniz!");
        Console.WriteLine("Çıkmak için 'çıkış' veya 'exit' yazın.");
        Console.WriteLine("Konuşma geçmişini temizlemek için 'temizle' yazın.\n");

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Sen: ");
            Console.ResetColor();

            var userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
                continue;

            // Çıkış komutları
            if (userInput.ToLower() == "çıkış" || userInput.ToLower() == "exit" || userInput.ToLower() == "quit")
            {
                Console.WriteLine("\nGörüşmek üzere! 👋");
                break;
            }

            // Geçmişi temizle
            if (userInput.ToLower() == "temizle" || userInput.ToLower() == "clear")
            {
                chatService.ClearHistory();
                Console.WriteLine("✅ Konuşma geçmişi temizlendi.\n");
                continue;
            }

            // Bot'a mesaj gönder ve cevabı göster
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Bot: ");
            Console.ResetColor();

            var response = await chatService.SendMessageAsync(userInput);
            Console.WriteLine(response);
            Console.WriteLine();
        }
    }
}

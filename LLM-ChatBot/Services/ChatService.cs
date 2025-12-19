using OpenAI;
using OpenAI.Chat;

namespace LLMChatBot.Services;

public class ChatService
{
    private readonly OpenAIClient _client;
    private readonly List<Message> _conversationHistory;

    public ChatService(string apiKey)
    {
        _client = new OpenAIClient(apiKey);
        _conversationHistory = new List<Message>();
        
        // Sistem mesajı - chatbot'un davranışını belirler
        _conversationHistory.Add(new Message(Role.System, "Sen yardımcı ve dostane bir asistan chatbot'sun. Türkçe konuşuyorsun ve kullanıcılara yardımcı olmaktan mutluluk duyuyorsun."));
    }

    public async Task<string> SendMessageAsync(string userMessage)
    {
        try
        {
            // Kullanıcı mesajını geçmişe ekle
            _conversationHistory.Add(new Message(Role.User, userMessage));

            // Chat completion isteği oluştur
            var chatRequest = new ChatRequest(_conversationHistory, model: "gpt-3.5-turbo", temperature: 0.7f);

            // API'ye istek gönder
            var response = await _client.ChatEndpoint.GetCompletionAsync(chatRequest);

            // Bot'un cevabını al
            var botResponse = response.FirstChoice.Message.Content;

            // Bot cevabını geçmişe ekle
            _conversationHistory.Add(new Message(Role.Assistant, botResponse ?? ""));

            return botResponse ?? "Üzgünüm, bir yanıt oluşturamadım.";
        }
        catch (Exception ex)
        {
            return $"Hata oluştu: {ex.Message}";
        }
    }

    public void ClearHistory()
    {
        _conversationHistory.Clear();
        _conversationHistory.Add(new Message(Role.System, "Sen yardımcı ve dostane bir asistan chatbot'sun. Türkçe konuşuyorsun ve kullanıcılara yardımcı olmaktan mutluluk duyuyorsun."));
    }
}


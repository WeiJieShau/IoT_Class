using System;
using System.Text;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


class Program
{
    static async Task Main()
    {
        //await SynthesizeUserInputAsyncTest();
        await StartSpeechRecognitionAsync();
    }

    public static async Task StartSpeechRecognitionAsync()
    {
        var config = SpeechConfig.FromSubscription("Key", "region"); // 修改金鑰及區域
        var language = "en-US"; // 語言

        using (var audioConfig = AudioConfig.FromDefaultMicrophoneInput())
        using (var recognizer = new SpeechRecognizer(config, language, audioConfig))
        {
            Console.WriteLine("請輸入 1 開始辨識語音，輸入 0 结束。");

            while (true)
            {
                var userInput = Console.ReadLine();

                if (userInput == "1")
                {
                    var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

                    if (result.Reason == ResultReason.RecognizedSpeech)
                    {
                        Console.WriteLine($"語音辨識結果: {result.Text}");

                        // 將語音辨識結果作為ChatGPT的輸入
                        await InvokeOpenAI(result.Text);
                    }
                    else
                    {
                        Console.WriteLine($"無法辨識: {result.Reason}");
                    }
                }
                else if (userInput == "0")
                {
                    Console.WriteLine("結束語音辨識。");
                    break;
                }
                else
                {
                    Console.WriteLine("輸入不正確，請再次輸入。");
                }
            }
        }
    }

    public static async Task InvokeOpenAI(string inputPrompt)
    {
        string apiKey = "OpenAI API Key";
        string endpoint = "https://api.openai.com/v1/chat/completions"; // 請求端點


        // 將 inputPrompt 包裝成 messages 數組
        var messages = new[]
        {
        new { role = "system", content = "想讓ChatGPT扮演的角色、想得到得回覆方式" },
        new { role = "user", content =  inputPrompt}
    };

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestData = new
            {
                model = "gpt-3.5-turbo",
                messages = messages
            };
            Console.WriteLine(messages[1]);
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, content);


            if (response.IsSuccessStatusCode)
            {
                string resultJson = await response.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<JObject>(resultJson);
                var resultcontent = resultObject["choices"]?[0]?["message"]?["content"]?.ToString();
                //處理結果，轉換 JSON 格式等...做進一步的功能
                Console.WriteLine(resultcontent);

                
            }
            else
            {
                Console.WriteLine($"请求失败：{response.StatusCode}");
            }
        }
    }

    

    


}

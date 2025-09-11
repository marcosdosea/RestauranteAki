using Core;
using System.Text.Json;

namespace Service
{
    public class ViaCepService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ViaCepService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<ViaCepResponse?> GetAddressByCepAsync(string cep)
        {

            var client = httpClientFactory.CreateClient();
            var requestUrl = $"https://viacep.com.br/ws/{cep}/json/";

            try
            {
                var response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ViaCepResponse>(jsonString);
                }
            }
            catch (HttpRequestException)
            {

            }

            return null;
        }
    }
}
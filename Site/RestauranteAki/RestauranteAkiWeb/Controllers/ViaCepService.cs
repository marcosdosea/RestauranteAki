using Core;
using Core.Exceptions;
using System.Text.Json;

namespace Service
{
    public class ViaCepService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ViaCepService> logger;

        public ViaCepService(IHttpClientFactory httpClientFactory, ILogger<ViaCepService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
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
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Erro ao consultar o serviço ViaCEP.{Cep}", cep);
                throw new CepServiceException("O serviço de busca de CEP está indisponível no momento.", ex);
            }

            return null;
        }
    }
}
using AutoMapper;
using Core.Exceptions;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace RestauranteAkiWeb.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RestauranteApiController : ControllerBase
    {

        private readonly IRestauranteService restauranteService;
        private readonly IMapper mapper;
        private readonly ViaCepService viaCepService;


        public RestauranteApiController(IRestauranteService restauranteService, IMapper mapper, ViaCepService viaCepService)
        {
            this.restauranteService = restauranteService;
            this.mapper = mapper;
            this.viaCepService = viaCepService;
        }


        [HttpGet("consultar-cep/{cep}")]
        public async Task<IActionResult> ConsultarCep(string cep)
        {
            try
            {
                var endereco = await viaCepService.GetAddressByCepAsync(cep);

                if (endereco == null)
                {
                    return NotFound("CEP não encontrado ou inválido.");
                }

                return Ok(endereco);
            }
            catch (CepServiceException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
            }

        }
    }
}
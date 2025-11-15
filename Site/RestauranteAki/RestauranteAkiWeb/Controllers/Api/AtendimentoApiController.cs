using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtendimentoApiController : ControllerBase
    {
        private readonly IPersonagemService personagemService;
        private readonly IPedidoService pedidoService;
        private readonly IMapper mapper;

        public AtendimentoApiController(IPersonagemService personagemService, IPedidoService pedidoService, IMapper mapper)
        {
            this.personagemService = personagemService;
            this.pedidoService = pedidoService;
            this.mapper = mapper;
        }

        [HttpGet("mesas/{idMesa}/personagens")]
        public async Task<ActionResult<IEnumerable<PersonagemViewModel>>> GetPersonagensByMesa(int idMesa)
        {
            var personagens = await personagemService.GetPersonagensByMesaAsync(idMesa);
            return Ok(mapper.Map<IEnumerable<PersonagemViewModel>>(personagens));
        }

        [HttpPost("contas/{idConta}/personagens")]
        public async Task<ActionResult<PersonagemViewModel>> AddPersonagem(int idConta)
        {
            var novoPersonagem = await personagemService.AddPersonagemAsync(idConta);

            var personagemViewModel = mapper.Map<PersonagemViewModel>(novoPersonagem);

            return Ok(personagemViewModel);
        }

        [HttpDelete("personagens/{id}")]
        public async Task<ActionResult> DeletePersonagem(int id)
        {
            await personagemService.DeleteAsync(id);
            return Ok();
        }
    }
}
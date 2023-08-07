using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RanqueDev.Services.Dto;
using RanqueDev.Domain.Entities;
using RanqueDev.Infra.Interfaces.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RanqueDev.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuestoesController : ControllerBase
    {
        private readonly IQuestaoRepository _questaoRepository;
        private readonly IMapper _mapper;
        public QuestoesController(IQuestaoRepository questaoRepository
            , IMapper mapper) {
        
            _questaoRepository = questaoRepository;
            _mapper = mapper;
        
        }

        [HttpGet]
        public async Task<IEnumerable<Questao>> BuscarQuestaoPorTag(string nomeTag)
        {
            return await _questaoRepository.BuscarQuestaoPorTag(nomeTag);
            
        }
        [HttpPost]
        public async Task<ActionResult<Questao>> CadastrarQuestao(CreateQuestao createQuestao)
        {
            var questao = _mapper.Map<Questao>(createQuestao);
            await _questaoRepository.CadastrarQuestao(questao);
            await _questaoRepository.CommitAsync();

            return questao;
        }

    }
}

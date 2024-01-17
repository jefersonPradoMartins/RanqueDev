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
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        public QuestoesController(IQuestionRepository questionRepository
            , IMapper mapper)
        {

            _questionRepository = questionRepository;
            _mapper = mapper;

        }

        [HttpGet("GetQuetionsByTagName")]
        public async Task<IEnumerable<Question>> GetQuetionsByTagName(string nomeTag)
        {
            return await _questionRepository.GetQuetionsByTagName(nomeTag);

        }
        [HttpPost]
        public async Task<ActionResult<Question>> CadastrarQuestao(CreateQuestao createQuestion)
        {
            var question = _mapper.Map<Question>(createQuestion);
            await _questionRepository.CreateQuestion(question);

            return question;
        }

    }
}

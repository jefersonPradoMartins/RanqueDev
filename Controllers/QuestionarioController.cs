using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RanqueDev.Domain.Entities;
using RanqueDev.Infra.Interfaces.Repository;

namespace RanqueDev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionarioController : ControllerBase
    {

        readonly IMapper _mapper;
        readonly IQuestionarioRepository _questaoRepository;

        public QuestionarioController(IMapper mapper, IQuestionarioRepository questionarioRepository ) 
        {
            _mapper = mapper;
            _questaoRepository = questionarioRepository;
        }


        [HttpGet]
        public IActionResult Get(ICollection<Tag> tags)
        {
            var result =  _questaoRepository.BuscarQuestao(tags);
            return Ok();
        }


    }
}

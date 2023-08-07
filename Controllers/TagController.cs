using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RanqueDev.Services.Dto;
using RanqueDev.Domain.Entities;
using RanqueDev.Infra.Interfaces.Repository;

namespace RanqueDev.Api.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ITagRepository _tagRepository;
        private readonly IQuestaoRepository _questaoRepository;

        public TagController(
              IMapper mapper
            , IQuestaoRepository questaoRepository
            , ITagRepository tagRepository
           )
        {
            _mapper = mapper;
            _tagRepository = tagRepository;
            _questaoRepository = questaoRepository;
        }



        [HttpPost]
        public async Task<ActionResult<Tag>> Post(CreateTagDto createTag)
        {
            try
            {
                var tag = _mapper.Map<Tag>(createTag);
                await _tagRepository.CadastrarTag(tag);
                await _tagRepository.CommitAsync();

                return tag;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _tagRepository.Rollback();

                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> Get()
        {
            var result = await _tagRepository.BuscarTodasTags();

            return Ok(result);
        }

        [HttpGet("{NomeTag}")]
        public async Task<IEnumerable<Tag>> BuscarTagPorNome(string NomeTag)
        {
            var result = await _tagRepository.BuscarTagsPorNome(NomeTag);
            return result;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int CodigoTag)
        {
            await _tagRepository.DeletarTag(CodigoTag);
            await _tagRepository.CommitAsync();
            return NoContent();
        }
    }
}

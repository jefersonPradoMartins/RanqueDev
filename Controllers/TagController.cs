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
        private readonly IQuestionRepository _questionRepository;

        public TagController(
              IMapper mapper
            , IQuestionRepository questionRepository
            , ITagRepository tagRepository
           )
        {
            _mapper = mapper;
            _tagRepository = tagRepository;
            _questionRepository = questionRepository;
        }



        [HttpPost("Create")]
        public async Task<ActionResult<Tag>> Post(CreateTagDto createTag)
        {
            try
            {
                var tag = _mapper.Map<Tag>(createTag);
                await _tagRepository.CreateTag(tag);
                return tag;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);


                return BadRequest();
            }
        }

        [HttpGet("GetAllTags")]
        public async Task<ActionResult<IEnumerable<Tag>>> Get()
        {
            var result = await _tagRepository.GetAllTags();

            return Ok(result);
        }

        [HttpGet("GetTagByName/{NomeTag}")]
        public async Task<IEnumerable<Tag>> GetTagByName(string NomeTag)
        {
            var result = await _tagRepository.GetTagsByName(NomeTag);
            return result;
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int CodigoTag)
        {
            await _tagRepository.DeleteTag(CodigoTag);

            return Ok();
        }
    }
}

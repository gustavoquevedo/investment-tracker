using Microsoft.AspNetCore.Mvc;
using InvestmentTracker.Domain.Interfaces;
using InvestmentTracker.Domain.Entities;
using InvestmentTracker.Api.DTOs;

namespace InvestmentTracker.Api.Controllers
{
    [ApiController]
    [Route("tags")]
    public class TagsController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;

        public TagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagResponse>>> GetAll()
        {
            var tags = await _tagRepository.GetAllAsync();
            return Ok(tags.Select(t => new TagResponse {
                Id = t.Id,
                Name = t.Name,
                ColorHex = t.ColorHex
            }));
        }

        [HttpPost]
        public async Task<ActionResult<TagResponse>> Create(CreateTagRequest request)
        {
            var tag = new Tag
            {
                Name = request.Name,
                ColorHex = request.ColorHex
            };

            var createdTag = await _tagRepository.AddAsync(tag);
            
            // Returns 201 Created with Location header pointing to the resource (even if GetById isn't exposed yet, the URI identifier exists conceptually)
            return Created($"/tags/{createdTag.Id}", new TagResponse {
                Id = createdTag.Id,
                Name = createdTag.Name,
                ColorHex = createdTag.ColorHex
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return NotFound();
            await _tagRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

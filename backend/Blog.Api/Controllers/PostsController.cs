using Blog.Api.Dtos;
using Blog.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<PostSummaryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var posts = await _postService.GetAllAsync(cancellationToken);
        return Ok(posts);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var post = await _postService.GetByIdAsync(id, cancellationToken);
        if (post is null)
        {
            return NotFound();
        }

        return Ok(post);
    }

    [HttpPost]
    public async Task<ActionResult<PostDetailDto>> Create(PostCreateDto dto, CancellationToken cancellationToken)
    {
        var created = await _postService.CreateAsync(dto, cancellationToken);
        if (created is null)
        {
            if (dto.BlogId.HasValue)
            {
                return NotFound($"Blog {dto.BlogId.Value} was not found.");
            }

            return BadRequest("Debe indicar un autor de blog válido o un identificador de blog existente.");
        }

        return CreatedAtAction(nameof(GetById), new { id = created.PostId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PostUpdateDto dto, CancellationToken cancellationToken)
    {
        var updated = await _postService.UpdateAsync(id, dto, cancellationToken);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _postService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}

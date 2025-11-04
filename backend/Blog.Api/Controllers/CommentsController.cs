using Blog.Api.Dtos;
using Blog.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/posts/{postId:int}/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CommentDto>>> GetAll(int postId, CancellationToken cancellationToken)
    {
        var comments = await _commentService.GetByPostIdAsync(postId, cancellationToken);
        return Ok(comments);
    }

    [HttpGet("{commentId:int}")]
    public async Task<ActionResult<CommentDto>> GetById(int postId, int commentId, CancellationToken cancellationToken)
    {
        var comment = await _commentService.GetByIdAsync(postId, commentId, cancellationToken);
        if (comment is null)
        {
            return NotFound();
        }

        return Ok(comment);
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create(int postId, CommentCreateDto dto, CancellationToken cancellationToken)
    {
        var comment = await _commentService.CreateAsync(postId, dto, cancellationToken);
        if (comment is null)
        {
            return NotFound();
        }

        return CreatedAtAction(nameof(GetById), new { postId, commentId = comment.Id }, comment);
    }

    [HttpDelete("{commentId:int}")]
    public async Task<IActionResult> Delete(int postId, int commentId, CancellationToken cancellationToken)
    {
        var deleted = await _commentService.DeleteAsync(postId, commentId, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}

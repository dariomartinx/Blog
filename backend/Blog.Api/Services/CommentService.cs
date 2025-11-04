using Blog.Api.Data;
using Blog.Api.Dtos;
using Blog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Services;

public class CommentService : ICommentService
{
    private readonly BlogDbContext _context;

    public CommentService(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<CommentDto>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(c => c.PostId == postId)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CommentDto(c.Id, c.PostId, c.AuthorName, c.Content, c.CreatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<CommentDto?> GetByIdAsync(int postId, int commentId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(c => c.PostId == postId && c.Id == commentId)
            .Select(c => new CommentDto(c.Id, c.PostId, c.AuthorName, c.Content, c.CreatedAt))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<CommentDto?> CreateAsync(int postId, CommentCreateDto dto, CancellationToken cancellationToken = default)
    {
        var postExists = await _context.Posts.AnyAsync(p => p.Id == postId, cancellationToken);
        if (!postExists)
        {
            return null;
        }

        var comment = new Comment
        {
            PostId = postId,
            AuthorName = dto.AuthorName.Trim(),
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        return new CommentDto(comment.Id, comment.PostId, comment.AuthorName, comment.Content, comment.CreatedAt);
    }

    public async Task<bool> DeleteAsync(int postId, int commentId, CancellationToken cancellationToken = default)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.PostId == postId && c.Id == commentId, cancellationToken);
        if (comment is null)
        {
            return false;
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

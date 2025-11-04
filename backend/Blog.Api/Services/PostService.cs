using Blog.Api.Data;
using Blog.Api.Dtos;
using Blog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Services;

public class PostService : IPostService
{
    private readonly BlogDbContext _context;

    public PostService(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<PostSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Posts
            .OrderByDescending(p => p.PublishedAt)
            .Select(p => new PostSummaryDto(p.Id, p.Title, p.AuthorName, p.PublishedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<PostDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Posts
            .Include(p => p.Comments)
            .Where(p => p.Id == id)
            .Select(p => new PostDetailDto(
                p.Id,
                p.Title,
                p.AuthorName,
                p.PublishedAt,
                p.Content,
                p.Comments
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => new CommentDto(c.Id, c.PostId, c.AuthorName, c.Content, c.CreatedAt))
                    .ToList()))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<PostDetailDto> CreateAsync(PostCreateDto dto, CancellationToken cancellationToken = default)
    {
        var post = new Post
        {
            Title = dto.Title.Trim(),
            AuthorName = dto.AuthorName.Trim(),
            Content = dto.Content,
            PublishedAt = dto.PublishedAt ?? DateTime.UtcNow
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(post.Id, cancellationToken) ??
               new PostDetailDto(post.Id, post.Title, post.AuthorName, post.PublishedAt, post.Content, Array.Empty<CommentDto>());
    }

    public async Task<bool> UpdateAsync(int id, PostUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var post = await _context.Posts.FindAsync(new object?[] { id }, cancellationToken);
        if (post is null)
        {
            return false;
        }

        post.Title = dto.Title.Trim();
        post.AuthorName = dto.AuthorName.Trim();
        post.Content = dto.Content;
        post.PublishedAt = dto.PublishedAt ?? post.PublishedAt;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var post = await _context.Posts.FindAsync(new object?[] { id }, cancellationToken);
        if (post is null)
        {
            return false;
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

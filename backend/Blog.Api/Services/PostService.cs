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
            .AsNoTracking()
            .Include(p => p.Blog)
            .OrderByDescending(p => p.PublishedAt)
            .Select(p => new PostSummaryDto(
                p.PostId,
                p.Title,
                p.BlogId,
                p.Blog != null ? p.Blog.Author : null,
                p.Blog != null ? p.Blog.Url : null,
                p.PublishedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<PostDetailDto?> GetByIdAsync(int postId, CancellationToken cancellationToken = default)
    {
        return await _context.Posts
            .AsNoTracking()
            .Include(p => p.Blog)
            .Include(p => p.Comments)
            .Where(p => p.PostId == postId)
            .Select(p => new PostDetailDto(
                p.PostId,
                p.Title,
                p.BlogId,
                p.Blog != null ? p.Blog.Author : null,
                p.Blog != null ? p.Blog.Url : null,
                p.PublishedAt,
                p.Content,
                p.Comments
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => new CommentDto(c.Id, c.PostId, c.AuthorName, c.Content, c.CreatedAt))
                    .ToList()))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<PostDetailDto?> CreateAsync(PostCreateDto dto, CancellationToken cancellationToken = default)
    {
        Blog? blog = null;

        if (dto.BlogId.HasValue)
        {
            var blogExists = await _context.Blogs
                .AsNoTracking()
                .AnyAsync(b => b.BlogId == dto.BlogId.Value, cancellationToken);
            if (!blogExists)
            {
                return null;
            }
        }
        else
        {
            if (string.IsNullOrWhiteSpace(dto.BlogAuthor))
            {
                return null;
            }

            blog = new Blog
            {
                Author = dto.BlogAuthor.Trim(),
                Url = NormalizeOptional(dto.BlogUrl)
            };

            _context.Blogs.Add(blog);
        }

        var post = new Post
        {
            Title = dto.Title.Trim(),
            Content = dto.Content,
            PublishedAt = dto.PublishedAt ?? DateTime.UtcNow
        };

        if (dto.BlogId.HasValue)
        {
            post.BlogId = dto.BlogId.Value;
        }
        else
        {
            post.Blog = blog;
        }

        _context.Posts.Add(post);
        await _context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(post.PostId, cancellationToken);
    }

    public async Task<bool> UpdateAsync(int postId, PostUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var post = await _context.Posts
            .Include(p => p.Blog)
            .FirstOrDefaultAsync(p => p.PostId == postId, cancellationToken);
        if (post is null)
        {
            return false;
        }

        if (dto.BlogId.HasValue)
        {
            var newBlog = await _context.Blogs.FindAsync(new object?[] { dto.BlogId.Value }, cancellationToken);
            if (newBlog is null)
            {
                return false;
            }

            post.BlogId = newBlog.BlogId;
            post.Blog = newBlog;
        }
        else if (!string.IsNullOrWhiteSpace(dto.BlogAuthor))
        {
            if (post.Blog is null)
            {
                post.Blog = new Blog
                {
                    Author = dto.BlogAuthor.Trim(),
                    Url = NormalizeOptional(dto.BlogUrl)
                };
            }
            else
            {
                post.Blog.Author = dto.BlogAuthor.Trim();
                if (dto.BlogUrl is not null)
                {
                    post.Blog.Url = NormalizeOptional(dto.BlogUrl);
                }
            }
        }
        else if (dto.BlogUrl is not null && post.Blog is not null)
        {
            post.Blog.Url = NormalizeOptional(dto.BlogUrl);
        }

        post.Title = dto.Title.Trim();
        post.Content = dto.Content;
        post.PublishedAt = dto.PublishedAt ?? post.PublishedAt;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int postId, CancellationToken cancellationToken = default)
    {
        var post = await _context.Posts.FindAsync(new object?[] { postId }, cancellationToken);
        if (post is null)
        {
            return false;
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static string? NormalizeOptional(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

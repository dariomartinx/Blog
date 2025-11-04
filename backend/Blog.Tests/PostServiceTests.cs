using Blog.Api.Data;
using Blog.Api.Dtos;
using Blog.Api.Models;
using Blog.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blog.Tests;

public class PostServiceTests
{
    private static BlogDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new BlogDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_PersistsPost()
    {
        await using var context = CreateContext();
        var blog = new Blog
        {
            Author = "Test Author",
            Url = "https://example.com"
        };

        context.Blogs.Add(blog);
        await context.SaveChangesAsync();

        var service = new PostService(context);
        var dto = new PostCreateDto
        {
            Title = "Hello",
            Content = "Sample content",
            PublishedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            BlogId = blog.BlogId
        };

        var created = await service.CreateAsync(dto);

        Assert.NotNull(created);
        Assert.Equal("Hello", created!.Title);
        Assert.Equal(blog.BlogId, created.BlogId);
        Assert.Equal("Test Author", created.BlogAuthor);
        Assert.Equal("https://example.com", created.BlogUrl);
        Assert.Equal(new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), created.PublishedAt);
        Assert.Single(context.Posts);
    }

    [Fact]
    public async Task CreateAsync_CreatesBlog_WhenNoBlogIdProvided()
    {
        await using var context = CreateContext();
        var service = new PostService(context);
        var dto = new PostCreateDto
        {
            Title = "New",
            Content = "Content",
            BlogAuthor = "Another Author",
            BlogUrl = "https://another.example.com"
        };

        var created = await service.CreateAsync(dto);

        Assert.NotNull(created);
        Assert.Equal("Another Author", created!.BlogAuthor);
        Assert.Equal("https://another.example.com", created.BlogUrl);
        Assert.Single(context.Blogs);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenPostDoesNotExist()
    {
        await using var context = CreateContext();
        var service = new PostService(context);
        var update = new PostUpdateDto
        {
            Title = "Updated",
            Content = "Updated content",
            PublishedAt = DateTime.UtcNow,
            BlogId = 1
        };

        var result = await service.UpdateAsync(42, update);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_RemovesPost()
    {
        await using var context = CreateContext();
        var blog = new Blog
        {
            Author = "Author",
            Url = "https://blog.example.com"
        };

        context.Blogs.Add(blog);
        await context.SaveChangesAsync();

        var service = new PostService(context);
        var post = await service.CreateAsync(new PostCreateDto
        {
            Title = "Post",
            Content = "Content",
            PublishedAt = DateTime.UtcNow,
            BlogId = blog.BlogId
        });

        Assert.NotNull(post);

        var deleted = await service.DeleteAsync(post!.PostId);

        Assert.True(deleted);
        Assert.Empty(context.Posts);
    }
}

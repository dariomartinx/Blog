using Blog.Api.Data;
using Blog.Api.Dtos;
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
        var service = new PostService(context);
        var dto = new PostCreateDto
        {
            Title = "Hello",
            AuthorName = "Test",
            Content = "Sample content",
            PublishedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        var created = await service.CreateAsync(dto);

        Assert.Equal("Hello", created.Title);
        Assert.Equal("Test", created.AuthorName);
        Assert.Equal(new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), created.PublishedAt);
        Assert.Single(context.Posts);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenPostDoesNotExist()
    {
        await using var context = CreateContext();
        var service = new PostService(context);
        var update = new PostUpdateDto
        {
            Title = "Updated",
            AuthorName = "Author",
            Content = "Updated content",
            PublishedAt = DateTime.UtcNow
        };

        var result = await service.UpdateAsync(42, update);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_RemovesPost()
    {
        await using var context = CreateContext();
        var service = new PostService(context);
        var post = await service.CreateAsync(new PostCreateDto
        {
            Title = "Post",
            AuthorName = "Author",
            Content = "Content",
            PublishedAt = DateTime.UtcNow
        });

        var deleted = await service.DeleteAsync(post.Id);

        Assert.True(deleted);
        Assert.Empty(context.Posts);
    }
}

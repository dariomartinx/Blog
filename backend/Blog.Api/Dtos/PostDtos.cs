using System.ComponentModel.DataAnnotations;

namespace Blog.Api.Dtos;

public record PostSummaryDto(int PostId, string Title, int BlogId, string? BlogAuthor, string? BlogUrl, DateTime PublishedAt);

public record PostDetailDto(int PostId, string Title, int BlogId, string? BlogAuthor, string? BlogUrl, DateTime PublishedAt,
    string Content, IReadOnlyCollection<CommentDto> Comments);

public record PostCreateDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; init; } = string.Empty;

    [Required]
    public string Content { get; init; } = string.Empty;

    public DateTime? PublishedAt { get; init; }

    public int? BlogId { get; init; }

    [MaxLength(100)]
    public string? BlogAuthor { get; init; }

    [Url]
    public string? BlogUrl { get; init; }
}

public record PostUpdateDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; init; } = string.Empty;

    [Required]
    public string Content { get; init; } = string.Empty;

    public DateTime? PublishedAt { get; init; }

    public int? BlogId { get; init; }

    [MaxLength(100)]
    public string? BlogAuthor { get; init; }

    [Url]
    public string? BlogUrl { get; init; }
}

public record CommentDto(int Id, int PostId, string AuthorName, string Content, DateTime CreatedAt);

public record CommentCreateDto
{
    [Required]
    [MaxLength(100)]
    public string AuthorName { get; init; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Content { get; init; } = string.Empty;
}

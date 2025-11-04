using System.ComponentModel.DataAnnotations;

namespace Blog.Api.Dtos;

public record PostSummaryDto(int Id, string Title, string AuthorName, DateTime PublishedAt);

public record PostDetailDto(int Id, string Title, string AuthorName, DateTime PublishedAt, string Content,
    IReadOnlyCollection<CommentDto> Comments);

public record PostCreateDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string AuthorName { get; init; } = string.Empty;

    [Required]
    public string Content { get; init; } = string.Empty;

    public DateTime? PublishedAt { get; init; }
}

public record PostUpdateDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string AuthorName { get; init; } = string.Empty;

    [Required]
    public string Content { get; init; } = string.Empty;

    public DateTime? PublishedAt { get; init; }
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

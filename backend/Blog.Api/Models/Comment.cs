using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Api.Models;

public class Comment
{
    public int Id { get; set; }

    [ForeignKey(nameof(Post))]
    public int PostId { get; set; }

    public Post? Post { get; set; }

    [Required]
    [MaxLength(100)]
    public string AuthorName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

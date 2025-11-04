using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Api.Models;

public class Post
{
    [Key]
    public int PostId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(Blog))]
    public int BlogId { get; set; }

    public Blog? Blog { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}

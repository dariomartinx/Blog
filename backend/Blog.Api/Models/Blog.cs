using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Api.Models;

public class Blog
{
    [Key]
    public int BlogId { get; set; }

    [Url]
    public string? Url { get; set; }

    [MaxLength(100)]
    public string? Author { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Post> Posts { get; set; } = new();
}

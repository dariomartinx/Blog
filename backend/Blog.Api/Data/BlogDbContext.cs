using Blog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Posts");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Title).IsRequired().HasMaxLength(200);
            entity.Property(p => p.AuthorName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Content).IsRequired();
            entity.Property(p => p.PublishedAt).IsRequired();
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comments");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.AuthorName).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Content).IsRequired().HasMaxLength(500);
            entity.Property(c => c.CreatedAt).IsRequired();
            entity.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

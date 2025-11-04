using System;
using Blog.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Blog.Api.Migrations
{
    [DbContext(typeof(BlogDbContext))]
    partial class BlogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("Blog.Api.Models.Comment", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("AuthorName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("TEXT");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("TEXT");

                b.Property<string>("Content")
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnType("TEXT");

                b.Property<int>("PostId")
                    .HasColumnType("INTEGER");

                b.HasKey("Id");

                b.HasIndex("PostId");

                b.ToTable("Comments");
            });

            modelBuilder.Entity("Blog.Api.Models.Post", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("AuthorName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("TEXT");

                b.Property<string>("Content")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<DateTime>("PublishedAt")
                    .HasColumnType("TEXT");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("Posts");
            });

            modelBuilder.Entity("Blog.Api.Models.Comment", b =>
            {
                b.HasOne("Blog.Api.Models.Post", "Post")
                    .WithMany("Comments")
                    .HasForeignKey("PostId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Post");
            });

            modelBuilder.Entity("Blog.Api.Models.Post", b =>
            {
                b.Navigation("Comments");
            });
#pragma warning restore 612, 618
        }
    }
}

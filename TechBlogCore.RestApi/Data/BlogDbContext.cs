using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechBlogCore.RestApi.Entities;
using System.Reflection.Emit;

namespace TechBlogCore.RestApi.Data
{
    public class BlogDbContext : IdentityDbContext<IdentityUser>
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }
        public DbSet<Blog_Article> Blog_Articles { get; set; }
        public DbSet<Blog_Category> Blog_Categories { get; set; }
        public DbSet<Blog_Tag> Blog_Tags { get; set; }
        public DbSet<Blog_Comment> Blog_Comments { get; set; }
        public DbSet<Blog_ArticleTags> Blog_ArticleTags { get; set; }
        public DbSet<Chat_Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Blog_Article>()
                .Property(u => u.State)
                .HasConversion<int>();
            builder.Entity<Blog_Article>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.CategoryId);
            builder.Entity<Blog_Comment>()
                .HasOne<Blog_Comment>(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Blog_Comment>()
                .HasOne(c => c.Article)
                .WithMany(a => a.Comments)
                .HasForeignKey(a => a.ArticleId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Blog_Comment>()
                .HasOne(c => c.User)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.Blog_UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // modelBuilder.Entity<Article_Tags>()
            //     .HasKey(at => new { at.ArticleId, at.TagId });
            builder.Entity<Blog_Article>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Blog_Article>()
                .HasMany(a => a.Tags)
                .WithMany(t => t.Articles)
                .UsingEntity<Blog_ArticleTags>(
                    o => o.HasOne(o => o.Tag).WithMany().HasForeignKey(o => o.TagId).OnDelete(DeleteBehavior.NoAction),
                    o => o.HasOne(o => o.Article).WithMany().HasForeignKey(o => o.ArticleId).OnDelete(DeleteBehavior.NoAction),
                    o => o.HasKey(t => new { t.ArticleId, t.TagId })
                );
            builder.Entity<Chat_Message>()
                .HasOne(c => c.User)
                .WithMany(c => c.Messages)
                .HasForeignKey(c => c.Blog_UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

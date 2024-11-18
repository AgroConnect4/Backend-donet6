using agroApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agroApp.Infra.Data.Context;

namespace agroApp.Infra.Data.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly AppDbContext _context;

        public BlogPostRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<BlogPost> GetAll()
        {
            return _context.BlogPosts.ToList();
        }

        public BlogPost GetById(int id)
        {
            return _context.BlogPosts.Find(id);
        }

        public void Add(BlogPost blogPost)
        {
            _context.BlogPosts.Add(blogPost);
            _context.SaveChanges();
        }

        public void Remove(BlogPost blogPost)
        {
            _context.BlogPosts.Remove(blogPost);
            _context.SaveChanges();
        }
    }
}

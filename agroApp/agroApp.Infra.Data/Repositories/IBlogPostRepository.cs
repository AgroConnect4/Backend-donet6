using agroApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agroApp.Infra.Data.Repositories
{
    public interface IBlogPostRepository
    {
        IEnumerable<BlogPost> GetAll();
        BlogPost GetById(int id);
        void Add(BlogPost blogPost);
        void Remove(BlogPost blogPost);
    }
}

using StackUnderflow.Domain.Schema.Models;
using StackUnderflow.EF.Models;
using System.Collections.Generic;

namespace StackUnderflow.Domain.Core.Contexts.Questions
{
    public class QuestionsWriteContext
    {
        public ICollection<Post> Posts { get; }
        public ICollection<User> User { get; }
     

        public QuestionsWriteContext(ICollection<Post> posts,ICollection<User> user)
        {
            Posts = posts ?? new List<Post>(0);
            User = user ?? new List<User>(0);
        }

    }
}

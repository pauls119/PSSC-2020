using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Questions
{
    public class QuestionsReadContext
    {
        public QuestionsReadContext(IEnumerable<Post> questions)
        {
            Questions = questions;
        }

        public IEnumerable<Post> Questions { get; }

    }
}

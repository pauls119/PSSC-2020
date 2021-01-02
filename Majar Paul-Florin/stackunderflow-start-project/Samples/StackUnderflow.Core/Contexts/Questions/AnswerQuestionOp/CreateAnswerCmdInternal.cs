using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Schema.Questions.AnswerQuestionOp
{
    public struct CreateAnswerCmdInternal
    {
        public int TenantId { get; set; }
        public Guid UserId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public int PostId { get; set; }
        public User User { get; private set; }
        public string Title { get; private set; }

        public CreateAnswerCmdInternal(int tenantId, int postId, Guid userId, int questionId, string answer, User user, string title)
        {
            TenantId = tenantId;
            UserId = userId;
            QuestionId = questionId;
            Answer = answer;
            PostId = postId;
            User = user;
            Title = title;
        }
    }
}

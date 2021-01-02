using CSharp.Choices;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Schema.Questions.AnswerQuestionOp
{
    [AsChoice]
    public static partial class CreateAnswerResult
    {
        public interface ICreateAnswerResult { }

        public class UnvalidatedReply : ICreateAnswerResult
        {
            public int TenantId { get; private set; }
            public Guid UserId { get; private set; }
            public int QuestionId { get; private set; }
            public string Answer { get; private set; }

            public UnvalidatedReply(int tenantId, Guid userId, int questionId, string answer)
            {
                TenantId = tenantId;
                UserId = userId;
                QuestionId = questionId;
                Answer = answer;
            }
            
        }

        public class ReplyPublished : ICreateAnswerResult
        {
            /* public int TenantId { get; set; }
        public Guid UserId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public int PostId { get; set; }
        public User User { get; private set; }
        public string Title { get; private set; }*/
            public User User { get; private set; }
            public int TenantId { get; }
            public Guid UserId { get; }
            public int QuestionId { get; }
            public string Answer { get; }
            public bool IsPublished { get; }
            public string Title { get; set; }
            public int PostId { get; private set; }
            public ReplyPublished(User user, int tenantId, Guid userId, int questionId, string answer, bool isPublished)
            {
                User = user;
                TenantId = tenantId;
                UserId = userId;
                QuestionId = questionId;
                Answer = answer;
                IsPublished = isPublished;
            }
            public ReplyPublished(User user, int tenantId, Guid userId, int questionId, string answer, bool isPublished, string title, int postId)
            {
                User = user;
                TenantId = tenantId;
                UserId = userId;
                QuestionId = questionId;
                Answer = answer;
                IsPublished = isPublished;
                Title = title;
                PostId = postId;
            }
        }

       

        public class ValidationError : ICreateAnswerResult
        {
            public string Message { get; }

            public ValidationError(string message)
            {
                Message = message;
            }

        }
    }
}

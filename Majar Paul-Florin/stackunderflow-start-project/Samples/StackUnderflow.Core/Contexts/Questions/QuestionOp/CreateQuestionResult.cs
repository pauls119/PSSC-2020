using System;
using System.Collections.Generic;
using System.Text;
using CSharp.Choices;
using StackUnderflow.EF.Models;

namespace StackUnderflow.Domain.Core.Contexts.Questions.QuestionOp
{
    [AsChoice]
    public static partial class CreateQuestionResult
    {
        public interface ICreateQuestionResult { }

        public class UnvalidatedQuestion : ICreateQuestionResult
        {
            public string Title { get; set; }
            public string Body { get; private set; }
            public string Tag { get; private set; }
            public int TenantId { get; set; }
            public Guid UserId { get; set; }
            public UnvalidatedQuestion(string title, string body, string tag, int tenantId, Guid userId)
            {
                Title = title;
                Body = body;
                Tag = tag;
                TenantId = tenantId;
                UserId = userId;
            }

            public UnvalidatedQuestion() { }

        }

        public class QuestPublished : ICreateQuestionResult
        {
            public string Title { get; private set; }
            public string Body { get; private set; }
            public string Tag { get; private set; }
            public int TenantId { get; private set; }
            public Guid UserId { get; private set; }
            public TenantUser User { get; private set; }
            public bool IsPublished { get; private set; }
            public QuestPublished(string title, string body, string tag, int tenantId, Guid userId, TenantUser user, bool isPublished)
            {
                Title = title;
                Body = body;
                Tag = tag;
                TenantId = tenantId;
                UserId = userId;
                User = user;
                IsPublished = isPublished;
            }
        }



        public class ValidationError : ICreateQuestionResult
        {
            public string Message { get; }

            public ValidationError(string message)
            {
                Message = message;
            }

        }
    }
}

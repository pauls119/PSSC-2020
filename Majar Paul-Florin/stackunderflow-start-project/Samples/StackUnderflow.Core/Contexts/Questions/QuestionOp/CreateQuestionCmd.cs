using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Questions.QuestionOp
{
    class CreateQuestionCmd
    {

        public string Title { get; set; }
        public string Body { get; set; }
        public int TenantId { get; set; }
        public string Tag { get; set; } 
        public Guid UserId { get; set; }
        //   public User User { get; set; }
        public TenantUser TenantUser { get; set; }
        public CreateQuestionCmd(string title, string body, string tag, int tenantId, Guid userId, TenantUser tenantUser)
        {
            Title = title;
            Body = body;
            Tag = tag;
            TenantId = tenantId;
            UserId = userId;
            TenantUser = tenantUser;
        }
    }
}

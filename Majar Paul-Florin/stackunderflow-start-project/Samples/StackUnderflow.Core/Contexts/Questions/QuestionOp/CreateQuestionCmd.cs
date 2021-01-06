using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Questions.QuestionOp
{
    class CreateQuestionCmd
    {
        [Required(ErrorMessage = "Title is missing")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Title is not valid.")]
        [MinLength(10), MaxLength(200)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Body is missing")]
        [MinLength(15), MaxLength(10000)]
        public string Body { get; set; }

        [Required(ErrorMessage = "TenantId is missing")]
        public int TenantId { get; set; }

        [Required(ErrorMessage = "Please enter at least one tag; see a list of popular tags.")]
        [MinLength(1), MaxLength(100)]
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

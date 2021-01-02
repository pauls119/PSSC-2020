using LanguageExt;
using StackUnderflow.Domain.Schema.Questions.AckOwnerQuestOp;
using StackUnderflow.Domain.Schema.Questions.AckOwnerReplyOp;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Questions
{
    public class QuestionsDependency
    {
        public Func<string> GenerateConfirmationToken { get; set; }
        public Func<User, ConfirmLetter, TryAsync<AckOwnerQuestResult.AckOwnerQuestSend>> SendConfirmationEmail { get; set; }
    }
}

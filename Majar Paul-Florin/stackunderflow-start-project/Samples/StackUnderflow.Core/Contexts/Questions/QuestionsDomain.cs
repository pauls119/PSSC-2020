using Access.Primitives.IO;
using LanguageExt;
using StackUnderflow.Domain.Schema.Questions.AckOwnerQuestOp;
using StackUnderflow.Domain.Schema.Questions.AckOwnerReplyOp;
using StackUnderflow.Domain.Schema.Questions.AnswerQuestionOp;
using StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp;
using StackUnderflow.Domain.Schema.Questions.QuestionOp;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static PortExt;
using static StackUnderflow.Domain.Schema.Questions.AckOwnerQuestOp.AckOwnerQuestResult;
using static StackUnderflow.Domain.Schema.Questions.AckOwnerReplyOp.AckOwnerReplyResult;
using static StackUnderflow.Domain.Schema.Questions.AnswerQuestionOp.CreateAnswerResult;
using static StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp.InviteTenantAdminResult;
using static StackUnderflow.Domain.Schema.Questions.QuestionOp.CreateQuestionResult;

namespace StackUnderflow.Domain.Core.Contexts.FrontOffice
{
    public static class QuestionsDomain
    {
      /*  public static Port<ICreateQuestionResult> CreateQuestion(string title, string body, string tag, int tenantId, Guid userId, User user) =>
            NewPort<CreateQuestionCmd, ICreateQuestionResult>(new CreateQuestionCmd(title, body, tag, tenantId, userId, user)); //string title, string body, string tag, int tenantId, Guid userId, User user
*/
        public static Port<ICreateAnswerResult> CreateAnswer(int tenantId, int postId, Guid userId, int questionId, string answer, User user, string title) =>
            NewPort<CreateAnswerCmdInternal, ICreateAnswerResult>(new CreateAnswerCmdInternal(tenantId, postId, userId, questionId, answer, user, title)); //int tenantId, int postId, Guid userId, int questionId, string answer, User user, string title

        public static object AcknoledgeOwnerQuest(User ownerQuest, string answer) => 
            NewPort<AckOwnerQuestCmd, IAckOwnerQuestResult>(new AckOwnerQuestCmd(ownerQuest, answer));

        public static object AcknoledgeOwnerReply(User replyOwner, Tenant tenant, int questionId, string answer) =>
            NewPort<AckOwnerReplyCmd, IAckOwnerReplyResult>(new AckOwnerReplyCmd(replyOwner, tenant, questionId, answer));

        public static Port<ICreateQuestionResult> CreateQuestion(CreateQuestionCmd cmd) =>
           NewPort<CreateQuestionCmd, ICreateQuestionResult>(new CreateQuestionCmd(cmd.Title, cmd.Body, cmd.Tag, cmd.TenantId, cmd.UserId, cmd.TenantUser)); //string title, string body, string tag, int tenantId, Guid userId, User user

    }
}

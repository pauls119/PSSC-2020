using StackUnderflow.Domain.Core.Contexts.Questions;
using System;
using System.Collections.Generic;
using System.Text;
using StackUnderflow.Domain.Schema.Questions.AckOwnerQuestOp;
using Access.Primitives.IO;
using static StackUnderflow.Domain.Schema.Questions.AckOwnerQuestOp.AckOwnerQuestResult;
using System.Threading.Tasks;
using Access.Primitives.Extensions.ObjectExtensions;
using static StackUnderflow.Domain.Schema.Questions.QuestionOp.CreateQuestionResult;

namespace StackUnderflow.Domain.Schema.Questions.AckOwnerQuestOp
{
    public class AckOwnerQuestAdapter : Adapter<AckOwnerQuestCmd, IAckOwnerQuestResult, QuestionsWriteContext, QuestionsDependency>
    {
        public override Task PostConditions(AckOwnerQuestCmd cmd, IAckOwnerQuestResult result, QuestionsWriteContext state)
        {
            return Task.CompletedTask;
        }

        public async override Task<IAckOwnerQuestResult> Work(AckOwnerQuestCmd cmd, QuestionsWriteContext state, QuestionsDependency dependencies)
        {
            var workflow = from valid in state.TryValidate()
                           let t = AddQuestToUser(state, (AckOwnerQuestCmd)CreateQuestFromCmd(cmd))
                           select t;

            var result = await workflow.Match(
                Succ: r => r,
                Fail: ex => new InvalidRequest(ex.Message)
                );
            return result;
        }

        private IAckOwnerQuestResult AddQuestToUser(QuestionsWriteContext state, AckOwnerQuestCmd cmd)
        {
            return new AckOwnerQuestSend(cmd.OwnerQuest, cmd.Answer, new ConfirmLetter(cmd.OwnerQuest.Email, cmd.Answer, new Uri("uriString")));//ReplyPublished(1, 2, cmd.QuestionId, "My answer body", true);
        }

        private AckOwnerQuestCmd CreateQuestFromCmd(AckOwnerQuestCmd cmd)
        {
            return cmd;
        }
    }
}


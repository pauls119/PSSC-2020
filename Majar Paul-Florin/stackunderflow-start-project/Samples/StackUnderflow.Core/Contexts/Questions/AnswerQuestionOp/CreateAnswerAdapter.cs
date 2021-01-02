using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using StackUnderflow.Domain.Core.Contexts.Questions;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackUnderflow.Domain.Schema.Questions.AnswerQuestionOp.CreateAnswerResult;

namespace StackUnderflow.Domain.Schema.Questions.AnswerQuestionOp
{
    public class CreateAnswerAdapter : Adapter<CreateAnswerCmdInternal, ICreateAnswerResult, QuestionsWriteContext, QuestionsDependency>
    {
        public override Task PostConditions(CreateAnswerCmdInternal cmd, ICreateAnswerResult result, QuestionsWriteContext state)
        {
            return Task.CompletedTask;
        }

        public async override Task<ICreateAnswerResult> Work(CreateAnswerCmdInternal cmd, QuestionsWriteContext state, QuestionsDependency dependencies)
        {
            var workflow = from valid in cmd.TryValidate()
                           let t = AddAnswerIfMissing(state, CreateAnswerFromCmd(cmd))
                           select t;

            var result = await workflow.Match(
                Succ: r => r,
                Fail: ex => new ValidationError(ex.Message)
                ) ;
            return result;
        }

        public ICreateAnswerResult AddAnswerIfMissing(QuestionsWriteContext state, Post post)
        {
            if (state.Posts.Any(p => p.Title.Equals(post.Title) && p.TenantUser.Equals(post.TenantUser)))
                return new ValidationError("Answer exist !");

            if (state.Posts.All(p => !p.Title.Equals(post.Title) && p.TenantId != post.TenantId))
                state.Posts.Add(post);
            return new ReplyPublished(post.TenantUser.User, post.TenantId, post.TenantUser.UserId, post.PostId, "My answer", post.AcceptedAnswer);
        }

        private Post CreateAnswerFromCmd(CreateAnswerCmdInternal cmd)
        {
            var result = new Post()
            {

                TenantId = cmd.TenantId,
                PostId = cmd.PostId,
                ParentPostId = cmd.QuestionId,
                Title = cmd.Title,
            };
           /* result.TenantUser.Add(new TenantUser()
            {
                User = new User()
                {
                    UserId = cmd.UserId,
                    Name = cmd.User.Name,
                    Email = cmd.User.Email,
                    DisplayName = cmd.User.Name,
                    WorkspaceId = cmd.UserId
                }
            });*/

            return result;
        }
    }
}

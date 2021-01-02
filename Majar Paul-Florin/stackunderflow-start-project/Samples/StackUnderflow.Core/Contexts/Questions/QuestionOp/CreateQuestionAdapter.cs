using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using StackUnderflow.Domain.Schema.Questions.QuestionOp;
using StackUnderflow.EF.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackUnderflow.Domain.Schema.Questions.QuestionOp.CreateQuestionResult;

namespace StackUnderflow.Domain.Core.Contexts.Questions.QuestionOp
{
    public class CreateQuestionAdapter : Adapter<CreateQuestionCmd, ICreateQuestionResult, QuestionsWriteContext, QuestionsDependency>
    {
        public override Task PostConditions(CreateQuestionCmd cmd, ICreateQuestionResult result, QuestionsWriteContext state)
        {
            return Task.CompletedTask;
        }

        public async override Task<ICreateQuestionResult> Work(CreateQuestionCmd cmd, QuestionsWriteContext state, QuestionsDependency dependencies)
        {
            var workflow = from valid in cmd.TryValidate()
                           let t = AddQuestionIfMissing(state, CreateQuestionFromCmd(cmd))
                           select t;

            var result = await workflow.Match(
                Succ: r => r,
                Fail: ex => new ValidationError(ex.Message)
                );
            return result;
        }

        public ICreateQuestionResult AddQuestionIfMissing(QuestionsWriteContext state, Post post)
        {
            if (state.Posts.Any(p => p.Title.Equals(post.Title) && p.TenantUser.Equals(post.TenantUser)))
                return new ValidationError("Question exist !");

            if (state.Posts.All(p => !p.Title.Equals(post.Title) && p.TenantId != post.TenantId))
                state.Posts.Add(post);
            return new QuestPublished(post.Title, post.PostText, post.PostTag.ElementAt(0).T.Name, post.TenantId, post.TenantUser.UserId, post.TenantUser, post.AcceptedAnswer);
        }

        private Post CreateQuestionFromCmd(CreateQuestionCmd cmd)
        {

            Tag tag = new Tag();
            tag.Name = cmd.Tag;

            PostTag postTag = new PostTag();
            postTag.QuestionId = cmd.TenantId;
            postTag.T = tag;

            var result = new Post()
            {
                TenantId = cmd.TenantId,
                PostText = cmd.Body,
                Title = cmd.Title,
                TenantUser = cmd.TenantUser,
                AcceptedAnswer = true,
                PostTag = new List<PostTag>() { postTag }
            };

            return (Post)result;
        }
    }
}

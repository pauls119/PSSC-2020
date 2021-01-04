using System;
using System.Linq;
using System.Threading.Tasks;
using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;
using StackUnderflow.EF.Models;
using Access.Primitives.EFCore;
using StackUnderflow.Domain.Core.Contexts.FrontOffice;
using StackUnderflow.Domain.Core.Contexts.Questions;
using StackUnderflow.Domain.Schema.Questions.AnswerQuestionOp;
using Microsoft.Xrm.Sdk.Workflow.Activities;
using StackUnderflow.Domain.Schema.Questions.QuestionOp;
using StackUnderflow.Domain.Schema.Models;
using StackUnderflow.Domain.Schema.Questions.AckOwnerQuestOp;
using static StackUnderflow.Domain.Schema.Questions.QuestionOp.CreateQuestionResult;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Orleans;
using static StackUnderflow.Domain.Schema.Questions.AckOwnerQuestOp.AckOwnerQuestResult;
using GrainInterfaces;
using GrainImplementation;

namespace StackUnderflow.API.Rest.Controllers
{
    [ApiController]
    [Route("question")]
    public class QuestionController : ControllerBase
    {
        private readonly IInterpreterAsync _interpreter;
        private readonly StackUnderflowContext _dbContext;
        private readonly IClusterClient _client;

        public QuestionController(IInterpreterAsync interpreter, StackUnderflowContext dbContext, IClusterClient client)
        {
            _interpreter = interpreter;
            _dbContext = dbContext;
            _client = client;
            Console.WriteLine(_client.GetGrain<IEmailSender>(1));
        }

        [HttpPost()]
        public async Task<IActionResult> CreateQuestionAsync([FromBody] CreateQuestionCmd createQuestionCmd)
        {
            var ctx = new QuestionsWriteContext(new EFList<Post>(_dbContext.Post), new EFList<User>(_dbContext.User));

            var dependencies = new QuestionsDependency();
            dependencies.GenerateConfirmationToken = () => Guid.NewGuid().ToString();
            dependencies.SendConfirmationEmail = SendEmailToOwner;

            var expr = from createResult in QuestionsDomain.CreateQuestion(createQuestionCmd)
                       let title = createResult.SafeCast<CreateQuestionResult.QuestPublished>().Select(p => p.Title)
                       let body = createResult.SafeCast<CreateQuestionResult.QuestPublished>().Select(p => p.Body)
                       let tag = createResult.SafeCast<CreateQuestionResult.QuestPublished>().Select(p => p.Tag)
                       let tenantId = createResult.SafeCast<CreateQuestionResult.QuestPublished>().Select(p => p.TenantId)
                       let userId = createResult.SafeCast<CreateQuestionResult.QuestPublished>().Select(p => p.UserId)
                       let user = createResult.SafeCast<CreateQuestionResult.QuestPublished>().Select(p => p.User)
                       let confirmQuest = new CreateQuestionCmd((string)title, (string)body, (string)tag, (int)tenantId, (Guid)userId, (TenantUser)user)
                       from CreateQuestionResult in QuestionsDomain.CreateQuestion(confirmQuest)
                       select new { createResult, CreateQuestionResult };

            var r = await _interpreter.Interpret(expr, ctx, dependencies);
            _dbContext.SaveChanges();

            return r.createResult.Match(
                created => (IActionResult)Ok(created.Title),
                notCreated => StatusCode(StatusCodes.Status500InternalServerError, "Question could not be created."),
                invalidRequest => BadRequest("Invalid request."));
        }

        private TryAsync<AckOwnerQuestSend> SendEmailToOwner(User user, ConfirmLetter letter)
            => async () =>
        {
            var emailToSend = _client.GetGrain<IEmailSender>(0);
            await emailToSend.SendEmailAsync(letter.Letter);
            return new AckOwnerQuestSend(user, "Hello, you have posted a new Question !", letter);
        };

        [HttpPost("question/{questionId}")]
        public async Task<IActionResult> CreateReplyAsync(int questionId, [FromBody] CreateAnswerCmdInternal createAnswerCmd)
        {
              
              var ctx = new QuestionsWriteContext(new EFList<Post>(_dbContext.Post), new EFList<User>(_dbContext.User));

            var dependencies = new QuestionsDependency();
            dependencies.GenerateConfirmationToken = () => Guid.NewGuid().ToString();
            dependencies.SendConfirmationEmail = SendEmailToOwner;
            
            var expr = from createResult in QuestionsDomain.CreateAnswer(createAnswerCmd.TenantId, createAnswerCmd.PostId, createAnswerCmd.UserId, questionId, createAnswerCmd.Answer, createAnswerCmd.User, createAnswerCmd.Title)
                       let user = createResult.SafeCast<CreateAnswerResult.ReplyPublished>().Select(p => p.User)
                       let tenantId = createResult.SafeCast<CreateAnswerResult.ReplyPublished>().Select(p => p.TenantId)
                       let userId = createResult.SafeCast<CreateAnswerResult.ReplyPublished>().Select(p => p.UserId)
                       let QuestionId = createResult.SafeCast<CreateAnswerResult.ReplyPublished>().Select(p => p.QuestionId)
                       let answer = createResult.SafeCast<CreateAnswerResult.ReplyPublished>().Select(p => p.Answer)
                       let isPublished = createResult.SafeCast<CreateAnswerResult.ReplyPublished>().Select(p => p.IsPublished)
                       let postId = createResult.SafeCast<CreateAnswerResult.ReplyPublished>().Select(p => p.PostId)
                       let title = createResult.SafeCast<CreateAnswerResult.ReplyPublished>().Select(p => p.Title)
                       let confirmReply = new CreateAnswerCmdInternal((int)tenantId, (int)postId, (Guid)userId, (int)QuestionId, (string)answer, (User)user, (string)title)
                       from CreateAnswerResult in QuestionsDomain.CreateAnswer(((CreateAnswerCmdInternal)confirmReply).TenantId, ((CreateAnswerCmdInternal)confirmReply).PostId, ((CreateAnswerCmdInternal)confirmReply).UserId, ((CreateAnswerCmdInternal)confirmReply).QuestionId, ((CreateAnswerCmdInternal)confirmReply).Answer, ((CreateAnswerCmdInternal)confirmReply).User, ((CreateAnswerCmdInternal)confirmReply).Title)
                       select new { createResult, CreateAnswerResult };

            var r = await _interpreter.Interpret(expr, ctx, dependencies);
            _dbContext.SaveChanges();

            return r.createResult.Match(
                created => (IActionResult)Ok(created.Answer),
                notCreated => StatusCode(StatusCodes.Status500InternalServerError, "Question could not be created."),
                invalidRequest => BadRequest("Invalid request."));
        }

        [HttpGet("GetQuestion/{questionId}")]
        public async Task<IActionResult> GetQuestion(int questionId)
        {
            //get ref to question grain
            QuestionGrain grain = new QuestionGrain(questionId, _dbContext);
            var questionSummary = grain.GetQuestionWithReplys();
            //get replys by grain;
            return (IActionResult)questionSummary;/*.Match(
                 created => (IActionResult)Ok(created.Title),
                 notCreated => StatusCode(StatusCodes.Status500InternalServerError, "Question could not be find."),
                 invalidRequest => BadRequest("Invalid request."));*/
        }
    }
}

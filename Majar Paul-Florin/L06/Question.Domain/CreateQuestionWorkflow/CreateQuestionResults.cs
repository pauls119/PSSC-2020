using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Question.Domain.CreateQuestionWorkflow
{
    public static partial class CreateQuestionResult
    {
        public interface ICreateQuestionResult
        {
            bool form { get; set; }

            void Match(Func<QuestionCreated, ICreateQuestionResult> processQuestionCreated, Func<QuestionCreated.QuestionNotCreated, ICreateQuestionResult> processQuestionNotCreated, Func<QuestionCreated.QuestionValidationFailed, ICreateQuestionResult> processInvalidQuestion);
            void getVotes(int v);
        }

        public class QuestionCreated : ICreateQuestionResult
        {
            public Guid QuestionId { get; private set; }
            public string User { get; private set; }
            public string Question { get; private set; }
            public bool form { get; set; }


            public QuestionCreated(Guid questionId, string question, string user, bool form)
            {
                QuestionId = questionId;
                Question = question;
                User = user;
                form = form;
            }


            public class QuestionNotCreated : ICreateQuestionResult
            {
                public string Feedback { get; set; }
                public bool form { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

                public QuestionNotCreated(string feedback)
                {
                    Feedback = feedback;
                }

                public void Match(Func<QuestionCreated, ICreateQuestionResult> processQuestionCreated, Func<QuestionNotCreated, ICreateQuestionResult> processQuestionNotCreated, Func<QuestionValidationFailed, ICreateQuestionResult> processInvalidQuestion)
                {
                    throw new NotImplementedException();
                }

                public void getVotes(int v)
                {
                    throw new NotImplementedException();
                }
            }

            public class QuestionValidationFailed : ICreateQuestionResult
            {
                public IEnumerable<string> ValidationErrors { get; private set; }
                public bool form { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

                public QuestionValidationFailed(IEnumerable<string> errors)
                {
                    ValidationErrors = errors.AsEnumerable();
                }

                public void Match(Func<QuestionCreated, ICreateQuestionResult> processQuestionCreated, Func<QuestionNotCreated, ICreateQuestionResult> processQuestionNotCreated, Func<QuestionValidationFailed, ICreateQuestionResult> processInvalidQuestion)
                {
                    throw new NotImplementedException();
                }

                public void getVotes(int v)
                {
                    throw new NotImplementedException();
                }
            }

            void ICreateQuestionResult.Match(Func<QuestionCreated, ICreateQuestionResult> processQuestionCreated, Func<QuestionNotCreated, ICreateQuestionResult> processQuestionNotCreated, Func<QuestionValidationFailed, ICreateQuestionResult> processInvalidQuestion)
            {
                throw new NotImplementedException();
            }

            public void getVotes(int v)
            {
                throw new NotImplementedException();
            }
        }
    }
}

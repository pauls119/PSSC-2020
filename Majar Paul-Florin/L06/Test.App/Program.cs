﻿using LanguageExt;
using Profile.Domain.CreateProfileWorkflow;
using Question.Domain.CreateQuestionWorkflow;
using Reply.Domain.CreateReplyWorkflow;
using System;
using System.Collections.Generic;
using static Question.Domain.CreateQuestionWorkflow.CreateQuestionResult;
using static Question.Domain.CreateQuestionWorkflow.CreateQuestionResult.QuestionCreated;
using static Reply.Domain.CreateReplyWorkflow.Reply;

namespace Test.App
{
    class Program
    {
      
        static void Main(string[] args)
        {
            var replyResult = UnvalidatedReply.Create("Respond to How to use sockets in C#"
                , "I have tried for a lot of times to use sockets in C#  and I followed several tutorials");


            var cmd = new CreateQuestionCmd("How to use sockets in C#"
                , "I have tried for a lot of times to use sockets in C#  and I followed several tutorials "
                + "\nCan anyone to help me ? "
                , new string[] { "C#", "Sockets" }, "https://www.infoworld.com/article/3016922/how-to-work-with-sockets-in-c.html ");

            var result = CreateQuestion(cmd);

            result.Match(
                ProcessQuestionCreated,
                ProcessQuestionNotCreated,
                ProcessInvalidQuestion
            );


            replyResult.Match(
                    Succ: reply =>
                    {
                        SendValidityConfirmation(reply);

                        Console.WriteLine("Reply is valid. Reply posted");
                        return Unit.Default;
                    },
                    Fail: ex =>
                    {
                        Console.WriteLine($"Invalid reply. Reason: {ex.Message}");
                        return Unit.Default;
                    }
                );


            Console.ReadLine();
            if (result.form == true)
            {
                Console.WriteLine("Do you want to vote this ?");
                Console.WriteLine("Yes or No ? Y or N");
                string decision = Console.ReadLine();
                if (decision.Equals("Y"))
                {
                    Console.WriteLine("You can give a positive or negative vote: P or N ?");
                    string vote = Console.ReadLine();
                    if (vote.Equals("P"))
                        result.getVotes(1);
                    else if (vote.Equals("N"))
                        result.getVotes(-1);
                }
            }
        }

        private static void SendValidityConfirmation(UnvalidatedReply reply)
        {
            var verifiedReply = new ValidateReplyService().VerifyReply(reply);
            verifiedReply.Match(
                    verifiedReply =>
                    {
                        new RestReplyOwnerService().SendRestReplyOwnerAck(verifiedReply).Wait();
                        return Unit.Default;
                    },
                    ex =>
                    {
                        Console.WriteLine("Reply could not be verified");
                        return Unit.Default;
                    }
                );
        }

        private static ICreateQuestionResult ProcessQuestionNotCreated(QuestionNotCreated questionNotCreatedResult)
        {
            Console.WriteLine($"Question not created: {questionNotCreatedResult.Feedback}");
            return questionNotCreatedResult;
        }

        private static ICreateQuestionResult ProcessQuestionCreated(QuestionCreated question)
        {
            Console.WriteLine($"Question {question.QuestionId}");
            return question;
        }

        private static ICreateQuestionResult ProcessInvalidQuestion(QuestionValidationFailed validationErrors)
        {
            Console.WriteLine("Question validation failed: ");
            foreach (var error in validationErrors.ValidationErrors)
            {
                Console.WriteLine(error);
            }
            return validationErrors;
        }


        public static ICreateQuestionResult CreateQuestion(CreateQuestionCmd createQuestionCommand)
        {
            if (string.IsNullOrWhiteSpace(createQuestionCommand.Title))
            {
                var errors = new List<string>() { "Title is missing" };
                return new QuestionValidationFailed(errors);
            }

            if (createQuestionCommand.Title.Length < 10 && !string.IsNullOrWhiteSpace(createQuestionCommand.Title))
            {
                var errors = new List<string>() { "Title cannot be shorter than 10 characters." };
                return new QuestionValidationFailed(errors);
            }

            if (createQuestionCommand.Title.Length > 180)
            {
                var errors = new List<string>() { "Title cannot be longer than 200 characters." };
                return new QuestionValidationFailed(errors);
            }

            if (string.IsNullOrWhiteSpace(createQuestionCommand.Body))
            {
                var errors = new List<string>() { "Body is missing" };
                return new QuestionValidationFailed(errors);
            }

            if (createQuestionCommand.Body.Length < 15 && !string.IsNullOrWhiteSpace(createQuestionCommand.Title))
            {
                var errors = new List<string>() { "Body cannot be shorter than 15 characters." };
                return new QuestionValidationFailed(errors);
            }

            if (createQuestionCommand.Body.Length > 10000)
            {
                var errors = new List<string>() { "Body is limited to 10000 characters; you entered 10005." };
                return new QuestionValidationFailed(errors);
            }

            if (createQuestionCommand.Tags.Length < 1)
            {
                var errors = new List<string>() { "Please enter at least one tag; see a list of popular tags." };
                return new QuestionValidationFailed(errors);
            }

            if (createQuestionCommand.Tags.Length > 10)
            {
                var errors = new List<string>() { "You entered to much tags, you need to delete some of these" };
                return new QuestionValidationFailed(errors);
            }

            if (new Random().Next(10) > 1)
            {
                return new QuestionNotCreated("Question could not be verified");
            }

            var questionId = Guid.NewGuid();
            var result = new QuestionCreated(questionId, createQuestionCommand.Title, "paul.majar01@gmail.com", true);

            if (result.form)
            {
                Console.WriteLine(result.ToString());
            }
            else
            {
                QuestionNotCreated feedback = new QuestionNotCreated("Question was closed, can be created !");
            }

            return result;
        }
    }
}


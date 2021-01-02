using CSharp.Choices;
using StackUnderflow.Domain.Core.Contexts.Questions;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Schema.Questions.AckOwnerQuestOp
{
    [AsChoice]
    public static partial class AckOwnerQuestResult
    {
        public interface IAckOwnerQuestResult { }

        public class AckOwnerQuestSend : IAckOwnerQuestResult
        {
            public User QuestOwner { get; private set; }
            public string Reply { get; private set; }
            public ConfirmLetter Letter { get; private set; }
            public AckOwnerQuestSend(User questOwner, string reply, ConfirmLetter letter)
            {
                QuestOwner = questOwner;
                Reply = reply;
                Letter = letter;
            }
            ///TODO
        }

        public class AckOwnerQuestNotSend : IAckOwnerQuestResult
        {
            ///TODO
        }

        public class InvalidRequest : IAckOwnerQuestResult
        {
            public string Message { get; }

            public InvalidRequest(string message)
            {
                Message = message;
            }

        }
    }
}

using Access.Primitives.IO;
using EarlyPay.Primitives.ValidationAttributes;
using LanguageExt;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StackUnderflow.Domain.Schema.Questions.AckOwnerQuestOp
{
    public struct AckOwnerQuestCmd
    {
        [OptionValidator(typeof(RequiredAttribute))]
        public User OwnerQuest { get; }
        public string Answer { get; private set; }

        public AckOwnerQuestCmd(User ownerQuest, string answer)
        {
            OwnerQuest = ownerQuest;
            Answer = answer;
        }
    }

    public enum AckOwnerQuestCmdInput
    {
        Valid,
        UserIsNone
    }

    public class AckOwnerQuestInputGen : InputGenerator<AckOwnerQuestCmd, AckOwnerQuestCmdInput>
    {
        public AckOwnerQuestInputGen(string answer)
        {
            mappings.Add(AckOwnerQuestCmdInput.Valid, () =>
                new AckOwnerQuestCmd(
                    (User)Option<User>.Some(new User()
                    {
                        DisplayName = Guid.NewGuid().ToString(),
                        Email = $"{Guid.NewGuid()}@mailinator.com"
                    }), answer)
            );

            mappings.Add(AckOwnerQuestCmdInput.UserIsNone, () =>
                new AckOwnerQuestCmd(
                    (User)Option<User>.None
                    , answer)
            );
        }

      
    }
}


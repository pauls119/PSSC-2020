using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrainInterfaces;
using StackUnderflow.Domain.Schema.Models;
using StackUnderflow.EF.Models;

namespace GrainImplementation
{
    public class QuestionGrain : Orleans.Grain
    {
        private StackUnderflowContext _dbContext;
        private QuestionSummary state;
        private int questionId;

        public QuestionGrain(int questionId, StackUnderflowContext dbContext)
        {
            this.questionId = questionId;
            _dbContext = dbContext;
        }

        public override Task OnActivateAsync()
        {
            //read state from DB
            //where PostId= OR ParentId=
            state = (QuestionSummary)_dbContext.Set<QuestionSummary>().Where(element => element.QuestionId == questionId);
            
            //subscribe to replys stream
            GetStreamProvider(state.ToString());
            
            return base.OnActivateAsync();
        }

        public QuestionSummary GetQuestionWithReplys()
        {
            return state;
        }
    }
}

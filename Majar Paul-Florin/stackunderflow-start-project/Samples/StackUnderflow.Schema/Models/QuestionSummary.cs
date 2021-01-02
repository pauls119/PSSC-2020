using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Schema.Models
{
    public class QuestionSummary
    {
        public int QuestionId { get; set; }
        public int Votes { get; set; }
        public int Answers { get; set; }
        public int Views { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string LastUpdatedText { get; set; }

        public QuestionSummary(int questionId, int votes, int answers, int views, string title, string tags, string lastUpdatedText)
        {
            QuestionId = questionId;
            Votes = votes;
            Answers = answers;
            Views = views;
            Title = title;
            Tags = tags;
            LastUpdatedText = lastUpdatedText;
        }

    }
}

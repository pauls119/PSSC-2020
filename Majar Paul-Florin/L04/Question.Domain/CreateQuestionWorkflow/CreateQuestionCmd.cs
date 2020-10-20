﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Question.Domain.CreateQuestionWorkflow
{
    public struct CreateQuestionCmd
    {

        [Required(ErrorMessage = "Title is missing")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Title is not valid.")]
        [MinLength(10), MaxLength(200)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Body is missing")]
        [MinLength(15), MaxLength(10000)]
        public string Body { get; set; }

        [Required(ErrorMessage = "Please enter at least one tag; see a list of popular tags.")]
        [MinLength(1), MaxLength(10)]
        public string[] Tags { get; set; }

        public string Link { get; set; }

        public CreateQuestionCmd(string title, string body, string[] tags, string link)
        {
            this.Title = title;
            this.Body = body;
            this.Tags = tags;
            this.Link = link;
        }
    }
}

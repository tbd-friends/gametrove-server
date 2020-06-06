using System;
using System.Collections;
using System.Collections.Generic;

namespace GameTrove.Api.Models
{
    public class TitleModel
    {
        public string Name { get; set; }
        public string Subtitle { get; set; }
    }

    public class UpdateTitleModel : TitleModel
    {
        public Guid Id { get; set; }

        public IEnumerable<string> Genres { get; set; }
    }
}
using System;

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
    }
}
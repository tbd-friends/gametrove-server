using System;
using System.Collections;
using System.Collections.Generic;

namespace GameTrove.Application.ViewModels
{
    public class TitleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subtitle { get; set; }
        public IEnumerable<string> Genres { get; set; }
    }
}
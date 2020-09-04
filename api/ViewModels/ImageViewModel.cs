using System;

namespace GameTrove.Api.ViewModels
{
    public class ImageViewModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public bool IsCoverArt { get; set; }
    }
}
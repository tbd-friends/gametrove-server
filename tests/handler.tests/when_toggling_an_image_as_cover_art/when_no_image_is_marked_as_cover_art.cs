using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using handler.tests.Infrastructure;
using Xunit;

namespace handler.tests.when_toggling_an_image_as_cover_art
{
    public class when_no_image_is_marked_as_cover_art : InMemoryContext<GameTrackerContext>
    {
        private ToggleImageAsCoverArtHandler _subject;
        private readonly Guid _imageId = Guid.Parse("46CBEAD6-D9BE-4169-9C97-2392F117DCF2");

        public when_no_image_is_marked_as_cover_art()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _subject = new ToggleImageAsCoverArtHandler(Context);

            Context.PlatformGameImages.Add(new GameImage
            {
                Id = _imageId,
                GameId = new Guid("C132C142-84A8-43DA-A39A-1E190625E453"),
                FileName = string.Empty,
                IsCoverArt = false
            });

            Context.SaveChanges();
        }

        private void Act()
        {
            _subject.Handle(new ToggleImageAsCoverArt { ImageId = _imageId }, CancellationToken.None).GetAwaiter()
                .GetResult();
        }

        [Fact]
        public void image_is_marked_as_cover_art()
        {
            Context.PlatformGameImages.Single(gi => gi.Id == _imageId).IsCoverArt.Should().BeTrue();
        }
    }
}
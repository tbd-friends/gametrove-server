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
    public class when_image_is_marked_as_cover_art : InMemoryContext<GameTrackerContext>
    {
        private ToggleImageAsCoverArtHandler _subject;
        private readonly Guid _imageId = Guid.Parse("C8035653-2D83-4A26-9796-41AA8C2E6C5D");

        public when_image_is_marked_as_cover_art()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _subject = new ToggleImageAsCoverArtHandler(Context);

            Context.PlatformGameImages.Add(
                new GameImage
                {
                    Id = _imageId,
                    IsCoverArt = true,
                    FileName = string.Empty,
                    GameId = new Guid("1A282E1F-22A3-49C4-BEFC-48D96B36ACAD")
                });

            Context.SaveChanges();
        }

        private void Act()
        {
            _subject.Handle(new ToggleImageAsCoverArt { ImageId = _imageId }, CancellationToken.None).GetAwaiter()
                .GetResult();
        }

        [Fact]
        public void is_marked_as_not_cover_art()
        {
            Context.PlatformGameImages.Single(pgi => pgi.Id == _imageId).IsCoverArt.Should().BeFalse();
        }
    }
}
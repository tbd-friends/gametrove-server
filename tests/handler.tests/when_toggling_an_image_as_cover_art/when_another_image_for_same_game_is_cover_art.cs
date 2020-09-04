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
    public class when_another_image_for_same_game_is_cover_art : InMemoryContext<GameTrackerContext>
    {
        private ToggleImageAsCoverArtHandler _subject;
        private readonly Guid _imageId = Guid.Parse("1421658A-B220-4D95-9639-D238887BB841");
        private readonly Guid _otherImageId = Guid.Parse("228113D0-B391-41AB-A521-C950EF41C509");
        private readonly Guid _gameId = Guid.Parse("B477763F-8B9C-42E1-B2B4-6D302D8FD21C");

        public when_another_image_for_same_game_is_cover_art()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _subject = new ToggleImageAsCoverArtHandler(Context);

            Context.PlatformGameImages.Add(new GameImage
            {
                Id = _otherImageId,
                IsCoverArt = true,
                FileName = string.Empty,
                GameId = _gameId
            });

            Context.PlatformGameImages.Add(new GameImage
            {
                Id = _imageId,
                IsCoverArt = false,
                FileName = string.Empty,
                GameId = _gameId
            });

            Context.SaveChanges();
        }

        private void Act()
        {
            _subject.Handle(new ToggleImageAsCoverArt { ImageId = _imageId }, CancellationToken.None).GetAwaiter()
                .GetResult();
        }

        [Fact]
        public void selected_image_is_marked_as_cover_art()
        {
            Context.PlatformGameImages.Single(pgi => pgi.Id == _imageId).IsCoverArt.Should().BeTrue();
        }

        [Fact]
        public void previous_image_is_marked_as_not_cover_art()
        {
            Context.PlatformGameImages.Single(pgi => pgi.Id == _otherImageId).IsCoverArt.Should().BeFalse();
        }
    }
}
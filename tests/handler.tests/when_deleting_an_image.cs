using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using handler.tests.Infrastructure;
using MediatR;
using Moq;
using Xunit;

namespace handler.tests
{
    public class when_deleting_an_image : InMemoryContext<GameTrackerContext>
    {
        private DeleteImageHandler _subject;
        private Mock<IMediator> _mediator;
        private readonly Guid _idToDelete = Guid.Parse("B23CB9A8-42C1-4301-B1C2-C137E07E9F91");
        private readonly Guid _gameId = Guid.Parse("4F96DFCD-9E23-4332-A5D5-3FDA93D4E9BE");

        public when_deleting_an_image()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _subject = new DeleteImageHandler(_mediator.Object, Context);

            Context.PlatformGameImages.Add(new GameImage
            {
                Id = _idToDelete,
                FileName = "FileName",
                IsCoverArt = false,
                GameId = _gameId
            });

            Context.SaveChanges();
        }

        private void Act()
        {
            _subject.Handle(new DeleteImage
            {
                Id = _idToDelete
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void platform_game_image_is_deleted()
        {
            Context.PlatformGameImages.SingleOrDefault(pgi => pgi.Id == _idToDelete).Should().BeNull();
        }

        [Fact]
        public void command_to_remove_from_storage_is_issued()
        {
            _mediator.Verify(m =>
                    m.Send(It.Is<RemoveImageFromStorage>(c => c.GameId == _gameId && c.ImageId == _idToDelete),
                        CancellationToken.None),
                Times.Once);
        }
    }
}
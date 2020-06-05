using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Storage;
using handler.tests.Infrastructure;
using MediatR;
using Moq;
using Xunit;

namespace handler.tests.when_assigning_genres_to_a_title
{
    public class when_title_is_not_associated_with_genre : InMemoryContext<GameTrackerContext>
    {
        private AssignGenresToTitleHandler _subject;
        private Mock<IMediator> _mediator;

        private readonly Guid _titleId = new Guid("E9993679-C55F-4411-AA0A-6C1DF0BAAC1D");
        private readonly Guid _genreId = new Guid("56C1C00D-13A8-41FA-A539-9812011F6912");

        public when_title_is_not_associated_with_genre()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IMediator>();

            _mediator.Setup(mediator =>
                    mediator.Send(It.Is<RegisterGenre>(g => g.Name == "Genre1"), CancellationToken.None))
                .Returns(Task.FromResult(_genreId));

            _subject = new AssignGenresToTitleHandler(_mediator.Object, Context);
        }

        private void Act()
        {
            _subject.Handle(new AssignGenresToTitle
            {
                TitleId = _titleId,
                Genres = new[] { "Genre1" }
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void title_is_associated_with_genre()
        {
            Context.TitleGenres.Count(tg => tg.TitleId == _titleId && tg.GenreId == _genreId).Should().Be(1);
        }
    }
}
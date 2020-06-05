using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Storage;
using handler.tests.Infrastructure;
using MediatR;
using Moq;
using Xunit;

namespace handler.tests.when_assigning_genres_to_a_title
{
    public class when_duplicate_genre_labels_are_provided : InMemoryContext<GameTrackerContext>
    {
        private AssignGenresToTitleHandler _subject;
        private Mock<IMediator> _mediator;

        private readonly Guid _titleId = new Guid("81F440FB-0477-4918-8794-E667CCC8A464");
        private readonly Guid _genreId = new Guid("67B6A8E8-AE43-4A4F-85BF-EDE4056A83C1");
        private readonly IEnumerable<string> _genres = new[] { "Genre", "Genre", "Genre", "Genre" };

        public when_duplicate_genre_labels_are_provided()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IMediator>();

            _mediator.Setup(mediator =>
                    mediator.Send(It.Is<RegisterGenre>(g => g.Name == "Genre"), CancellationToken.None))
                .Returns(Task.FromResult(_genreId));

            _subject = new AssignGenresToTitleHandler(_mediator.Object, Context);
        }

        private void Act()
        {
            _subject.Handle(new AssignGenresToTitle
            {
                TitleId = _titleId,
                Genres = _genres
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void processes_unique_label()
        {
            _mediator.Verify(
                mediator => mediator.Send(It.Is<RegisterGenre>(g => g.Name == "Genre"), CancellationToken.None),
                Times.Once);
        }
    }
}
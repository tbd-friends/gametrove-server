using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using handler.tests.Infrastructure;
using MediatR;
using Moq;
using Xunit;

namespace handler.tests.when_assigning_genres_to_a_title
{
    public class when_title_is_associated_with_genre : InMemoryContext<GameTrackerContext>
    {
        private AssignGenresToTitleHandler _subject;
        private Mock<IMediator> _mediator;

        private readonly Guid _titleId = new Guid("8B55DF2F-224D-4453-AB39-576D9D0CFF35");
        private readonly Guid _genreId = new Guid("4F866F4C-9477-43AD-BB7A-5F0D4055E1E9");
        private readonly IEnumerable<string> _genres = new[] { "Genre1" };

        public when_title_is_associated_with_genre()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IMediator>();

            Context.TitleGenres.Add(new TitleGenre
            {
                TitleId = _titleId,
                GenreId = _genreId
            });

            Context.SaveChanges();

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
                Genres = _genres
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void genre_is_not_associated_with_title_multiple_times()
        {
            Context.TitleGenres.Count(tg => tg.TitleId == _titleId && tg.GenreId == _genreId).Should().Be(1);
        }
    }
}
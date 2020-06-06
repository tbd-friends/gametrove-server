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
    public class when_genre_is_no_longer_associated : InMemoryContext<GameTrackerContext>
    {
        private AssignGenresToTitleHandler _subject;
        private Mock<IMediator> _mediator;

        private readonly Guid _titleId = new Guid("E88C6A88-CD68-4568-A722-A8D539E33ECF");
        private readonly Guid _removeGenreId = new Guid("DDD8A244-932C-4540-B050-6C1B1C78B477");
        private readonly Guid _keepGenreId = new Guid("F5A2AD66-D13E-4157-B6B6-51EC99B3717B");
        private readonly string _keepGenre = "Genre1";

        public when_genre_is_no_longer_associated()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IMediator>();

            _subject = new AssignGenresToTitleHandler(_mediator.Object, Context);

            Context.TitleGenres.Add(new TitleGenre { TitleId = _titleId, GenreId = _keepGenreId });
            Context.TitleGenres.Add(new TitleGenre { TitleId = _titleId, GenreId = _removeGenreId });

            Context.SaveChanges();

            _mediator.Setup(mediator =>
                    mediator.Send(It.Is<RegisterGenre>(rg => rg.Name == _keepGenre), CancellationToken.None))
                .Returns(Task.FromResult(_keepGenreId));

            _mediator.Setup(mediator =>
                    mediator.Send(It.Is<RegisterGenre>(rg => rg.Name == "Genre2"), CancellationToken.None))
                .Returns(Task.FromResult(_removeGenreId));
        }

        private void Act()
        {
            _subject.Handle(new AssignGenresToTitle
            {
                TitleId = _titleId,
                Genres = new[] { _keepGenre }
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void association_is_removed()
        {
            Context.TitleGenres.Count(tg => tg.TitleId == _titleId && tg.GenreId == _removeGenreId).Should().Be(0);
        }

        [Fact]
        public void the_associated_genre_still_remains()
        {
            Context.TitleGenres.Count(tg => tg.TitleId == _titleId && tg.GenreId == _keepGenreId).Should().Be(1);
        }
    }
}
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

namespace handler.tests.when_registering_a_genre
{
    public class when_genre_does_exist : InMemoryContext<GameTrackerContext>
    {
        private RegisterGenreHandler _subject;
        private Guid _result;

        private readonly string _genreName = "GenreName";
        private readonly Guid _genreId = new Guid("27B45963-CB76-4CC6-B6DD-3DD3EF1F32FB");

        public when_genre_does_exist()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _subject = new RegisterGenreHandler(Context);

            Context.Genres.Add(new Genre
            {
                Id = _genreId,
                Name = _genreName
            });

            Context.SaveChanges();
        }

        private void Act()
        {
            _result = _subject.Handle(new RegisterGenre
            {
                Name = _genreName
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void genre_is_not_created()
        {
            Context.Genres.Count(g => g.Name == _genreName).Should().Be(1);
        }

        [Fact]
        public void genre_id_is_returned()
        {
            _genreId.Should().Be(_result);
        }
    }
}
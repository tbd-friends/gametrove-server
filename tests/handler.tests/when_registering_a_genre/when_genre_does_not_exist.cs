using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Storage;
using handler.tests.Infrastructure;
using Xunit;

namespace handler.tests.when_registering_a_genre
{
    public class when_genre_does_not_exist : InMemoryContext<GameTrackerContext>
    {
        private RegisterGenreHandler _subject;
        private Guid _result;

        private readonly string _genreName = "GenreName";

        public when_genre_does_not_exist()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _subject = new RegisterGenreHandler(Context);
        }

        private void Act()
        {
            _result = _subject.Handle(new RegisterGenre
            {
                Name = _genreName
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void genre_is_created()
        {
            Context.Genres.Count(g => g.Name == _genreName).Should().Be(1);
        }

        [Fact]
        public void genre_id_is_returned()
        {
            Context.Genres.Single(g => g.Name == _genreName).Id.Should().Be(_result);
        }
    }
}
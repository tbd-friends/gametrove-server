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

namespace handler.tests
{
    public class when_favoriting_a_game : InMemoryContext<GameTrackerContext>
    {
        private FavoriteGameHandler _subject;

        private readonly Guid _gameId = new Guid("5C16F086-EBA0-4218-9F15-46B07B4463D3");

        public when_favoriting_a_game()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Context.Games.Add(new Game
            {
                Id = _gameId
            });

            Context.SaveChanges();

            _subject = new FavoriteGameHandler(Context);
        }

        private void Act()
        {
            _subject.Handle(new FavoriteGame
            {
                GameId = _gameId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void game_is_marked_as_favorite()
        {
            Context.Games.Single(g => g.Id == _gameId).IsFavorite.Should().BeTrue();
        }
    }
}
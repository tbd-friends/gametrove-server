using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Storage;
using handler.tests.Infrastructure;
using Xunit;

namespace handler.tests.when_registering_a_copy
{
    public class when_only_game_id_is_provided : InMemoryContext<GameTrackerContext>
    {
        private RegisterCopyHandler _subject;
        private Guid _gameId = new Guid("43D7C3EF-A9A9-4D95-819E-1E995E407B4C");

        public when_only_game_id_is_provided()
        {
            _subject = new RegisterCopyHandler(Context);

            _subject.Handle(new RegisterCopy
            {
                GameId = _gameId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void copy_is_registered()
        {
            Context.Copies.Count().Should().Be(1);
        }
    }
}
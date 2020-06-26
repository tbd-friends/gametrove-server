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

namespace handler.tests.when_updating_a_copy
{
    public class when_was_wanted_and_is_now_owned : InMemoryContext<GameTrackerContext>
    {
        public UpdateCopyHandler _subject;

        private readonly Guid GameCopyId = new Guid("DDD61E0D-4E09-4077-A3A9-5E3257BCD413");

        public when_was_wanted_and_is_now_owned()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _subject = new UpdateCopyHandler(Context);

            Context.Copies.Add(new Copy
            {
                Id = GameCopyId,
                Tags = "",
                GameId = new Guid("799C783F-7D02-4C1A-AE98-E713DCC6D138"),
                Cost = null,
                Purchased = null,
                IsWanted = true
            });

            Context.SaveChanges();
        }

        private void Act()
        {
            _subject.Handle(new UpdateCopy
            {
                Id = GameCopyId,
                IsWanted = false
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void copy_is_marked_as_not_wanted()
        {
            Context.Copies.Single(cp => cp.Id == GameCopyId).IsWanted.Should().BeFalse();
        }
    }
}
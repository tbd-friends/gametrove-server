using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Storage;
using handler.tests.Infrastructure;
using MediatR;
using Moq;
using Xunit;

namespace handler.tests.when_adding_a_copy
{
    public class when_tags_are_provided : InMemoryContext<GameTrackerContext>
    {
        private AddCopyHandler _subject;
        private Mock<IMediator> _mediator;
        private Guid _gameId = new Guid("D90CCB13-5932-42EB-80F7-7DD61C70367B");
        private string[] _tags = { "Tag1", "Tag2" };

        public when_tags_are_provided()
        {
            _mediator = new Mock<IMediator>();

            _subject = new AddCopyHandler(Context, _mediator.Object);

            _subject.Handle(new AddCopy
            {
                GameId = _gameId,
                Tags = _tags,
                Email = "EmailAddress",
                Identifier = "Identifier"
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void tags_are_added()
        {
            var copy = Context.Copies.SingleOrDefault(c => c.GameId == _gameId);

            copy.Should().NotBeNull();
            copy.Tags.Should().Contain("Tag1");
            copy.Tags.Should().Contain("Tag2");
        }
    }
}
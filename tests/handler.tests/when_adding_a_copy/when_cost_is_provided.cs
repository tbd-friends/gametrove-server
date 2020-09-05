using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using handler.tests.Infrastructure;
using MediatR;
using Moq;
using Xunit;

namespace handler.tests.when_adding_a_copy
{
    public class when_cost_is_provided : InMemoryContext<GameTrackerContext>
    {
        private readonly AddCopyHandler _subject;
        private readonly Guid _gameId = new Guid("EA3B0EA5-D005-4D2F-95EF-9894132EC63E");
        private decimal _cost = 19.99m;
        private readonly Guid _userId = new Guid("381BEF14-35AF-47FC-8FE2-35132121EA3B");
        private readonly Guid _tenantId = new Guid("7CC736D5-C339-4D95-8192-5F4C29604EEA");

        public when_cost_is_provided()
        {
            _subject = new AddCopyHandler(Context);

            _subject.Handle(new AddCopy
            {
                GameId = _gameId,
                Cost = _cost,
                UserId = _userId,
                TenantId = _tenantId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void value_is_record()
        {
            var copy = Context.Copies.SingleOrDefault(c => c.GameId == _gameId);

            copy.Cost.Should().NotBeNull();
            copy.Cost.Should().Be(_cost);
        }
    }
}
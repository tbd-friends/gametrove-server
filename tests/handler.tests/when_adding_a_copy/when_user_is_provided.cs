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
    public class when_user_is_provided : InMemoryContext<GameTrackerContext>
    {
        private AddCopyHandler _subject;
        private readonly Guid _gameId = new Guid("EA3B0EA5-D005-4D2F-95EF-9894132EC63E");
        private readonly Guid _userId = new Guid("381BEF14-35AF-47FC-8FE2-35132121EA3B");
        private readonly Guid _tenantId = new Guid("7CC736D5-C339-4D95-8192-5F4C29604EEA");

        public when_user_is_provided()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _subject = new AddCopyHandler(Context);
        }

        private void Act()
        {
            _subject.Handle(new AddCopy
            {
                GameId = _gameId,
                UserId = _userId,
                TenantId = _tenantId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void copy_is_attached_to_the_tenant()
        {
            Context.Copies.Count(c => c.TenantId == _tenantId).Should().Be(1);
        }
    }
}
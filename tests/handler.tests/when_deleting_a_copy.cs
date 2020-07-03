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
    public class when_deleting_a_copy : InMemoryContext<GameTrackerContext>
    {
        private DeleteCopyHandler _subject;
        private readonly Guid _copyId = new Guid("07A50E6E-891A-47D8-9A99-8869D80ABEE3");
        private readonly Guid _tenantId = new Guid("077AC570-C104-4E68-B98A-C57DD55E9E9C");
        private readonly Guid _userId = new Guid("54F1622D-0CD5-4C13-8FA3-B82799CD0F86");
        private readonly Guid _gameId = new Guid("1B0BF30F-5B2A-4908-8785-C8F6B5A339BF");

        public when_deleting_a_copy()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Context.Copies.Add(
                new Copy
                {
                    Id = _copyId,
                    TenantId = _tenantId,
                    GameId = _gameId
                });

            Context.SaveChanges();

            _subject = new DeleteCopyHandler(Context);
        }

        private void Act()
        {
            _subject.Handle(new DeleteCopy
            {
                TenantId = _tenantId,
                UserId = _userId,
                CopyId = _copyId
            }, CancellationToken.None);
        }

        [Fact]
        public void copy_is_removed()
        {
            Context.Copies.SingleOrDefault(c => c.Id == _copyId).Should().BeNull();
        }
    }
}
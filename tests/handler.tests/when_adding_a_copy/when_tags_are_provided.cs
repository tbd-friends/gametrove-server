﻿using System;
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
    public class when_tags_are_provided : InMemoryContext<GameTrackerContext>
    {
        private AddCopyHandler _subject;
        private readonly Guid _gameId = new Guid("D90CCB13-5932-42EB-80F7-7DD61C70367B");
        private readonly Guid _userId = new Guid("381BEF14-35AF-47FC-8FE2-35132121EA3B");
        private readonly Guid _tenantId = new Guid("7CC736D5-C339-4D95-8192-5F4C29604EEA");
        private readonly string[] _tags = { "Tag1", "Tag2" };

        public when_tags_are_provided()
        {
            _subject = new AddCopyHandler(Context);

            _subject.Handle(new AddCopy
            {
                GameId = _gameId,
                Tags = _tags,
                UserId = _userId,
                TenantId = _tenantId
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
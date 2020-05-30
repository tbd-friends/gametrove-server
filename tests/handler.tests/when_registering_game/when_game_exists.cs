using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using handler.tests.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace handler.tests.when_registering_game
{
    public class when_game_exists : InMemoryContext<GameTrackerContext>
    {
        private RegisterGameHandler _subject;
        private Mock<IMediator> _mediator;

        private const string Code = "Code";
        private const string TitleName = "TitleName";
        private const string TitleSubtitle = "TitleSubtitle";
        private Guid TitleId = new Guid("3CD120FC-1238-450B-A4AB-CC510F4C9A43");
        private Guid PlatformId = new Guid("FB93718F-7BC2-45F7-A5F5-324CD8A991E4");

        public when_game_exists()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IMediator>();

            Context.Games.Add(new Game()
            {
                TitleId = TitleId,
                PlatformId = PlatformId,
                Code = Code,
                Registered = DateTime.UtcNow
            });

            Context.SaveChanges();

            _subject = new RegisterGameHandler(Context, _mediator.Object);
        }

        private void Act()
        {
            _subject.Handle(
                new RegisterGame
                {
                    Code = Code,
                    Subtitle = TitleSubtitle,
                    Name = TitleName,
                    Platform = PlatformId
                },
                CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void game_is_not_created()
        {
            Context.Games.Count(g => g.TitleId == TitleId && g.PlatformId == PlatformId).Should().Be(1);
        }

        [Fact]
        public void title_is_not_registered()
        {
            _mediator.Verify(md => md.Send(It.IsAny<RegisterTitle>(), CancellationToken.None), Times.Never);
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using handler.tests.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace handler.tests.when_registering_game
{
    public class when_game_does_not_exist : InMemoryContext<GameTrackerContext>
    {
        private RegisterGameHandler _subject;
        private Mock<IMediator> _mediator;
        private GameViewModel _result;

        private Guid TitleId = Guid.Parse("75D0D4E3-211C-4D48-9CAD-785C2E85B596");
        private Guid PlatformId = Guid.Parse("A9F4BE39-B6C8-4DD7-8F67-D6FF47850A80");
        private const string TitleName = "GameTitle";
        private const string TitleSubtitle = "GameSubtitle";
        private const string Code = "ScannedCode";
        private const string PlatformName = "Platform";

        public when_game_does_not_exist()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IMediator>();

            Context.Platforms.Add(new Platform()
            {
                Id = PlatformId,
                Name = PlatformName
            });

            Context.SaveChanges();

            _mediator.Setup(md =>
                    md.Send(
                        It.Is<RegisterTitle>(rg => rg.Name == TitleName && rg.Subtitle == TitleSubtitle),
                        CancellationToken.None)
                    ).Returns(
                Task.FromResult(new TitleViewModel
                {
                    Id = TitleId,
                    Name = TitleName,
                    Subtitle = TitleSubtitle
                }));

            _subject = new RegisterGameHandler(Context, _mediator.Object);
        }

        private void Act()
        {
            _result = _subject.Handle(new RegisterGame
            {
                Code = Code,
                Subtitle = TitleSubtitle,
                Name = TitleName,
                Platform = PlatformId
            },
            CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void game_is_added()
        {
            Context.Games.Single(g => g.TitleId == TitleId && g.PlatformId == PlatformId).Should().NotBeNull();
        }

        [Fact]
        public void game_view_model_is_returned()
        {
            _result.Should().NotBeNull();
            _result.Name.Should().Be(TitleName);
            _result.Description.Should().Be(TitleSubtitle);
            _result.Platform.Should().Be(PlatformName);
        }
    }
}
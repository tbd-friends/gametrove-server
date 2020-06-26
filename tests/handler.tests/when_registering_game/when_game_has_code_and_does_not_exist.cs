using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using handler.tests.Infrastructure;
using MediatR;
using Moq;
using Xunit;

namespace handler.tests.when_registering_game
{
    public class when_game_has_code_and_does_not_exist : InMemoryContext<GameTrackerContext>
    {
        private RegisterGameHandler _subject;
        private Mock<IAuthenticatedMediator> _mediator;
        private GameViewModel _result;
        private Game _game;

        private Guid TitleId = Guid.Parse("75D0D4E3-211C-4D48-9CAD-785C2E85B596");
        private Guid PlatformId = Guid.Parse("A9F4BE39-B6C8-4DD7-8F67-D6FF47850A80");
        private const string TitleName = "GameTitle";
        private const string TitleSubtitle = "GameSubtitle";
        private const string Code = "ScannedCode";
        private const string PlatformName = "Platform";

        public when_game_has_code_and_does_not_exist()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IAuthenticatedMediator>();

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

            _game = Context.Games.SingleOrDefault();
        }

        [Fact]
        public void game_is_added()
        {
            _game.Should().NotBeNull();

            _game.TitleId.Should().Be(TitleId);
            _game.PlatformId.Should().Be(PlatformId);
            _game.IsFavorite.Should().BeFalse();
            _game.Registered.Should().NotBe(default);
            _game.Registered.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public void game_view_model_is_returned()
        {
            _result.Should().NotBeNull();
            _result.Name.Should().Be(TitleName);
            _result.Subtitle.Should().Be(TitleSubtitle);
            _result.Platform.Should().Be(PlatformName);
        }

        [Fact]
        public void copy_is_registered_for_user()
        {
            _mediator.Verify(mediator =>
                mediator.Send(
                    It.Is<RegisterCopy>(cp => cp.GameId == _result.Id), CancellationToken.None), Times.Once);
        }
    }
}
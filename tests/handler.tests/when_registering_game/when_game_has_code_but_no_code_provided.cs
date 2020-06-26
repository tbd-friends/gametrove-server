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
    public class when_game_has_code_but_no_code_provided : InMemoryContext<GameTrackerContext>
    {
        private RegisterGameHandler _subject;
        private Mock<IAuthenticatedMediator> _mediator;
        private GameViewModel _result;

        private readonly string _code = "Code";
        private readonly string _titleName = "TitleName";
        private readonly string _titleSubtitle = "TitleSubtitle";
        private readonly Guid _titleId = new Guid("3CD120FC-1238-450B-A4AB-CC510F4C9A43");
        private readonly Guid _platformId = new Guid("FB93718F-7BC2-45F7-A5F5-324CD8A991E4");

        public when_game_has_code_but_no_code_provided()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IAuthenticatedMediator>();

            _mediator.Setup(md =>
                md.Send(
                    It.Is<RegisterTitle>(rg => rg.Name == _titleName && rg.Subtitle == _titleSubtitle),
                    CancellationToken.None)
            ).Returns(
                Task.FromResult(new TitleViewModel
                {
                    Id = _titleId,
                    Name = _titleName,
                    Subtitle = _titleSubtitle
                }));

            Context.Games.Add(new Game()
            {
                TitleId = _titleId,
                PlatformId = _platformId,
                Code = _code,
                Registered = DateTime.UtcNow
            });

            Context.Platforms.Add(new Platform
            {
                Id = _platformId,
                Name = "Platform"
            });

            Context.SaveChanges();

            _subject = new RegisterGameHandler(Context, _mediator.Object);
        }

        private void Act()
        {
            _result = _subject.Handle(
                new RegisterGame
                {
                    Subtitle = _titleSubtitle,
                    Name = _titleName,
                    Platform = _platformId
                },
                CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void game_is_not_created()
        {
            Context.Games.Count(g => g.TitleId == _titleId && g.PlatformId == _platformId).Should().Be(1);
        }

        [Fact]
        public void game_view_model_is_returned()
        {
            _result.Should().NotBeNull();
            _result.Name.Should().Be(_titleName);
            _result.Subtitle.Should().Be(_titleSubtitle);
            _result.Platform.Should().Be("Platform");
            _result.Code.Should().Be(_code);
        }

        [Fact]
        public void copy_is_registered_for_user()
        {
            _mediator.Verify(mediator =>
                mediator.Send(
                    It.Is<AddCopy>(cp => cp.GameId == _result.Id), CancellationToken.None), Times.Once);
        }
    }
}
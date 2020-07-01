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
using Moq;
using Xunit;

namespace handler.tests.when_registering_game
{
    public class when_game_exists_for_my_tenant : InMemoryContext<GameTrackerContext>
    {
        private RegisterGameHandler _subject;
        private Mock<IAuthenticatedMediator> _mediator;
        private GameViewModel _result;
        private Game _game;

        private readonly Guid _titleId = Guid.Parse("75D0D4E3-211C-4D48-9CAD-785C2E85B596");
        private readonly Guid _platformId = Guid.Parse("A9F4BE39-B6C8-4DD7-8F67-D6FF47850A80");
        private readonly string _titleName = "GameTitle";
        private readonly string _titleSubtitle = "GameSubtitle";
        private readonly string _code = "ScannedCode";
        private readonly string _platformName = "Platform";
        private readonly Guid _tenantId = new Guid("02E8831D-1C0A-41C8-9C48-428727EC1EE7");
        
        public when_game_exists_for_my_tenant()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IAuthenticatedMediator>();

            Context.Platforms.Add(new Platform()
            {
                Id = _platformId,
                Name = _platformName
            });

            Context.Games.Add(new Game()
            {
                TitleId = _titleId,
                PlatformId = _platformId,
                Code = _code,
                Registered = DateTime.UtcNow,
                TenantId = _tenantId
            });

            Context.SaveChanges();

            _mediator.Setup(md =>
                md.Send(
                    It.Is<RegisterTitle>(rg =>
                        rg.Name == _titleName && rg.Subtitle == _titleSubtitle),
                    CancellationToken.None)
            ).Returns(
                Task.FromResult(new TitleViewModel
                {
                    Id = _titleId,
                    Name = _titleName,
                    Subtitle = _titleSubtitle
                }));

            _subject = new RegisterGameHandler(Context, _mediator.Object);
        }

        private void Act()
        {
            _result = _subject.Handle(new RegisterGame
            {
                Code = _code,
                Name = _titleName,
                Subtitle = _titleSubtitle,
                Platform = _platformId,
                TenantId = _tenantId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void game_information_is_returned()
        {
            _result.Name.Should().Be(_titleName);
            _result.Subtitle.Should().Be(_titleSubtitle);
            _result.Code.Should().Be(_code);
        }

        [Fact]
        public void game_is_not_added()
        {
            Context.Games.Count().Should().Be(1);
        }
    }
}
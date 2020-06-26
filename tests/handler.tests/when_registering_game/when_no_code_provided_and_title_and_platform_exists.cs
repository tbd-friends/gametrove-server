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
    public class when_no_code_provided_and_title_and_platform_exists : InMemoryContext<GameTrackerContext>
    {
        private RegisterGameHandler _subject;
        private GameViewModel _result;

        private Mock<IAuthenticatedMediator> _mediator;
        private readonly Guid _platformId = new Guid("900583FE-1B2C-4D15-BA56-236F37459FE1");
        private readonly Guid _titleId = new Guid("6B2BA877-ADDA-49D0-828F-C27856A921E8");
        private readonly string _titleName = "TitleName";
        private readonly string _titleSubtitle = "TitleSubtitle";

        public when_no_code_provided_and_title_and_platform_exists()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IAuthenticatedMediator>();

            _subject = new RegisterGameHandler(Context, _mediator.Object);

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

            Context.Games.Add(new Game
            {
                Id = new Guid("0BD0B68C-1729-43CD-8C84-1C2E05A8C719"),
                TitleId = _titleId,
                PlatformId = _platformId,
                Code = null,
            });

            Context.Platforms.Add(new Platform
            {
                Id = _platformId,
                Name = "Platform"
            });

            Context.SaveChanges();
        }

        private void Act()
        {
            _result = _subject.Handle(new RegisterGame
            {
                Platform = _platformId,
                Name = "TitleName",
                Subtitle = "TitleSubtitle",
                Code = null
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void game_is_not_created()
        {
            var games = Context.Games.Where(g => g.TitleId == _titleId && g.PlatformId == _platformId);

            games.Count().Should().Be(1);
        }

        [Fact]
        public void game_view_model_is_returned()
        {
            _result.Should().NotBeNull();
            _result.Name.Should().Be(_titleName);
            _result.Subtitle.Should().Be(_titleSubtitle);
            _result.Platform.Should().Be("Platform");
            _result.Code.Should().Be(null);
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
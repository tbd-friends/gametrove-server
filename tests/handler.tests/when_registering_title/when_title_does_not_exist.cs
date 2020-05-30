using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using handler.tests.Infrastructure;
using Moq;
using Xunit;

namespace handler.tests.when_registering_title
{
    public class when_title_does_not_exist : InMemoryContext<GameTrackerContext>
    {
        private RegisterTitleHandler _subject;
        private TitleViewModel _result;

        private const string TitleName = "Game Title";
        private const string TitleSubtitle = "Subtitle for Game";

        public when_title_does_not_exist()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _subject = new RegisterTitleHandler(Context);
        }

        private void Act()
        {
            _result = _subject.Handle(new RegisterTitle
            {
                Name = TitleName,
                Subtitle = TitleSubtitle
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void title_is_returned()
        {
            _result.Should().NotBeNull();
            _result.Name.Should().Be(TitleName);
            _result.Subtitle.Should().Be(TitleSubtitle);
        }

        [Fact]
        public void title_is_created()
        {
            Context.Titles
                .SingleOrDefault(t => t.Name == TitleName && t.Subtitle == TitleSubtitle).Should()
                .NotBeNull();
        }
    }
}

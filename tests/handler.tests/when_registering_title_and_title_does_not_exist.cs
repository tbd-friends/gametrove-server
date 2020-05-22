using System;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Application.ViewModels;
using GameTrove.Storage.Contracts;
using GameTrove.Storage.Models;
using Moq;
using Xunit;

namespace handler.tests
{
    public class when_registering_title_and_title_does_not_exist
    {
        private RegisterTitleHandler _subject;
        private TitleViewModel _result;

        private const string TitleName = "Game Title";
        private const string TitleSubtitle = "Subtitle for Game";

        private Mock<ITitleRepository> _titleRepository;

        public when_registering_title_and_title_does_not_exist()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _titleRepository = new Mock<ITitleRepository>();

            _subject = new RegisterTitleHandler(_titleRepository.Object);
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
            _titleRepository.Verify(repo =>
                repo.Add(It.Is<Title>(x => x.Name == TitleName && x.Subtitle == TitleSubtitle)));
        }
    }
}

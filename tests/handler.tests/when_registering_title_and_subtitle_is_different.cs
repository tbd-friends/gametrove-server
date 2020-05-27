using System;
using System.Linq;
using System.Linq.Expressions;
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
    public class when_registering_title_and_subtitle_is_different
    {
        private RegisterTitleHandler _subject;
        private TitleViewModel _result;

        private const string TitleName = "TitleName";
        private const string TitleSubtitle = "SubtitleOfTitle";
        private const string OtherSubtitle = "OtherSubtitle";

        private Mock<ITitleRepository> _titleRepository;

        public when_registering_title_and_subtitle_is_different()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _titleRepository = new Mock<ITitleRepository>();

            _titleRepository.Setup(repo => repo.Query(It.IsAny<Expression<Func<Title, bool>>>()))
                .Returns((Expression<Func<Title, bool>> expr) =>
                    new[] { new Title { Name = TitleName, Subtitle = TitleSubtitle } }.AsQueryable().Where(expr));

            _titleRepository
                .Setup(repo =>
                    repo.AddTitle(TitleName, OtherSubtitle))
                .Returns(new Title { Id = Guid.NewGuid(), Name = TitleName, Subtitle = OtherSubtitle });

            _subject = new RegisterTitleHandler(_titleRepository.Object);
        }

        private void Act()
        {
            _result = _subject.Handle(new RegisterTitle
            {
                Name = TitleName,
                Subtitle = OtherSubtitle
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void title_is_returned()
        {
            _result.Should().NotBeNull();
            _result.Name.Should().Be(TitleName);
            _result.Subtitle.Should().Be(OtherSubtitle);
        }

        [Fact]
        public void new_title_is_created()
        {
            _titleRepository.Verify(repo =>
                repo.AddTitle(It.Is<string>(t => t == TitleName), It.Is<string>(t => t == OtherSubtitle)));
        }
    }
}
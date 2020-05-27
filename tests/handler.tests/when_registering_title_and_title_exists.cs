using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
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
    public class when_registering_title_and_title_exists
    {
        private RegisterTitleHandler _subject;
        private TitleViewModel _result;

        private const string TitleName = "TitleName";
        private const string TitleSubtitle = "SubtitleOfTitle";

        private Mock<ITitleRepository> _titleRepository;

        public when_registering_title_and_title_exists()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _titleRepository = new Mock<ITitleRepository>();

            _titleRepository.Setup(repo => repo.Query(It.IsAny<Expression<Func<Title, bool>>>())).Returns(new[]
            {
                new Title
                    {Id = Guid.NewGuid(), Name = TitleName, Subtitle = TitleSubtitle}
            }.AsQueryable());

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
        public void title_is_not_added()
        {
            _titleRepository.Verify(repo => repo.AddTitle(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
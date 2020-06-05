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
using MediatR;
using Moq;
using Xunit;

namespace handler.tests.when_updating_a_title
{
    public class when_new_title_name_is_provided : InMemoryContext<GameTrackerContext>
    {
        private UpdateTitleHandler _subject;
        private TitleViewModel _result;
        private Mock<IMediator> _mediator;

        private Guid _titleId = new Guid("006A571F-5BEB-4AB5-867E-9195C31AB4BA");
        private string TitleName = "TitleName";
        private string TitleSubtitle = "TitleSubtitle";

        public when_new_title_name_is_provided()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IMediator>();

            _subject = new UpdateTitleHandler(_mediator.Object, Context);

            Context.Titles.Add(new Title
            {
                Id = _titleId,
                Name = TitleName,
                Subtitle = TitleSubtitle
            });

            Context.SaveChanges();
        }

        private void Act()
        {
            _result = _subject.Handle(new UpdateTitle
            {
                TitleId = _titleId,
                Name = "NewTitleName",
                Subtitle = TitleSubtitle
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void title_name_is_updated()
        {
            var title = Context.Titles.Single(t => t.Id == _titleId);

            title.Name.Should().Be("NewTitleName");
            title.Subtitle.Should().Be(TitleSubtitle);
        }

        [Fact]
        public void new_title_details_are_returned()
        {
            _result.Id.Should().Be(_titleId);
            _result.Name.Should().Be("NewTitleName");
            _result.Subtitle.Should().Be(TitleSubtitle);
        }
    }
}
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
    public class when_null_title_name_is_provided : InMemoryContext<GameTrackerContext>
    {
        private UpdateTitleHandler _subject;
        private TitleViewModel _result;
        private Mock<IMediator> _mediator;

        private Guid _titleId = new Guid("8D535577-BA03-4B94-9272-71E1E527FF57");
        private string TitleName = "Name";
        private string TitleSubtitle = "Subtitle";

        public when_null_title_name_is_provided()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _subject = new UpdateTitleHandler(_mediator.Object, Context);

            Context.Titles.Add(new Title()
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
                Name = null,
                Subtitle = null
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void title_is_not_updated()
        {
            var title = Context.Titles.Single(t => t.Id == _titleId);

            title.Name.Should().Be(TitleName);
            title.Subtitle.Should().Be(TitleSubtitle);
        }

        [Fact]
        public void original_title_values_are_returned()
        {
            _result.Id.Should().Be(_titleId);
            _result.Name.Should().Be(TitleName);
            _result.Subtitle.Should().Be(TitleSubtitle);
        }
    }
}
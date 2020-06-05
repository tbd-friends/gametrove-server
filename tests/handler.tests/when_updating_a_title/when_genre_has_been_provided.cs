using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using handler.tests.Infrastructure;
using MediatR;
using Moq;
using Xunit;

namespace handler.tests.when_updating_a_title
{
    public class when_genre_has_been_provided : InMemoryContext<GameTrackerContext>
    {
        private UpdateTitleHandler _subject;
        private Mock<IMediator> _mediator;

        private readonly Guid _titleId = new Guid("2F1ADEF7-2E92-41FE-9A2C-D9E38AD068AC");
        private readonly string _titleName = "TitleName";
        private readonly string _titleSubtitle = "TitleSubtitle";
        private readonly IEnumerable<string> _genres = new[] { "Genre1" };

        public when_genre_has_been_provided()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            _mediator = new Mock<IMediator>();

            Context.Titles.Add(new Title
            {
                Id = _titleId,
                Name = _titleName,
                Subtitle = _titleSubtitle
            });

            Context.SaveChanges();

            _subject = new UpdateTitleHandler(_mediator.Object, Context);
        }

        private void Act()
        {
            _subject.Handle(new UpdateTitle
            {
                TitleId = _titleId,
                Name = _titleName,
                Subtitle = _titleSubtitle,
                Genres = _genres
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void register_genres_for_title_is_called()
        {
            _mediator.Verify(
                mediator => mediator.Send(
                    It.Is<AssignGenresToTitle>(n => n.TitleId == _titleId && n.Genres.All(g => _genres.Contains(g))),
                    CancellationToken.None),
                Times.Once);
        }
    }
}
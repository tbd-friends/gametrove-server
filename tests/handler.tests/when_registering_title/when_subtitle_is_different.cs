using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using FluentAssertions;
using GameTrove.Application.Commands;
using GameTrove.Application.Commands.Handlers;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using handler.tests.Infrastructure;
using Xunit;

namespace handler.tests.when_registering_title
{
    public class when_subtitle_is_different : InMemoryContext<GameTrackerContext>
    {
        private RegisterTitleHandler _subject;
        private TitleViewModel _result;
        
        private const string TitleName = "TitleName";
        private const string TitleSubtitle = "SubtitleOfTitle";
        private const string OtherSubtitle = "OtherSubtitle";

        public when_subtitle_is_different()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Context.Titles.Add(new Title() { Name = TitleName, Subtitle = TitleSubtitle });

            Context.SaveChanges();

            _subject = new RegisterTitleHandler(Context);
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
            Context.Titles.Count().Should().Be(2);
            Context.Titles
                .SingleOrDefault(t => t.Name == TitleName && t.Subtitle == OtherSubtitle).Should().NotBeNull();
        }
    }
}
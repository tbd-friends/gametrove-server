using System;
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
using Moq;
using Xunit;

namespace handler.tests.when_registering_title
{
    public class when_title_exists_for_current_tenant : InMemoryContext<GameTrackerContext>
    {
        private RegisterTitleHandler _subject;
        private TitleViewModel _result;

        private const string TitleName = "TitleName";
        private const string TitleSubtitle = "SubtitleOfTitle";
        private Guid _userId = new Guid("51F57AC3-0922-4501-9E1E-75444BCA7C20");
        private readonly Guid _tenantId = new Guid("2F35AE3E-AE6C-4E3F-8BD2-60538ACF1943");

        public when_title_exists_for_current_tenant()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Context.Titles.Add(new Title { Name = TitleName, Subtitle = TitleSubtitle, TenantId = _tenantId });
            Context.SaveChanges();

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
        public void title_is_not_added()
        {
            Context.Titles.Count(t => t.Name == TitleName && t.Subtitle == TitleSubtitle && t.TenantId == _tenantId).Should().Be(1);
        }
    }
}
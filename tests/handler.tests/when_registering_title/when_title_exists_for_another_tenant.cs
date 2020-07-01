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
using Xunit;

namespace handler.tests.when_registering_title
{
    public class when_title_exists_for_another_tenant : InMemoryContext<GameTrackerContext>
    {
        private RegisterTitleHandler _subject;
        private TitleViewModel _result;

        private const string TitleName = "TitleName";
        private const string TitleSubtitle = "SubtitleOfTitle";
        private readonly Guid _tenantId = new Guid("2F35AE3E-AE6C-4E3F-8BD2-60538ACF1943");
        private readonly Guid _otherUserTenantId = new Guid("703EF29D-0C34-4962-97BB-47060DB8E3DA");

        public when_title_exists_for_another_tenant()
        {
            Arrange();

            Act();
        }

        private void Arrange()
        {
            Context.Titles.Add(new Title { Name = TitleName, Subtitle = TitleSubtitle, TenantId = _otherUserTenantId });
            Context.SaveChanges();

            _subject = new RegisterTitleHandler(Context);
        }

        private void Act()
        {
            _result = _subject.Handle(new RegisterTitle
            {
                Name = TitleName,
                Subtitle = TitleSubtitle,
                TenantId = _tenantId
            }, CancellationToken.None).GetAwaiter().GetResult();
        }

        [Fact]
        public void title_information_is_returned()
        {
            _result.Name.Should().Be(TitleName);
            _result.Subtitle.Should().Be(TitleSubtitle);
        }

        [Fact]
        public void title_is_added()
        {
            Context.Titles
                .SingleOrDefault(t =>
                    t.Name == TitleName &&
                    t.Subtitle == TitleSubtitle &&
                    t.TenantId == _tenantId)
                .Should().NotBeNull();
        }
    }
}
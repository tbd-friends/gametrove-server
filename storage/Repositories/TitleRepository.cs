using GameTrove.Storage.Contracts;
using GameTrove.Storage.Models;

namespace GameTrove.Storage.Repositories
{
    public class TitleRepository : Repository<Title>, ITitleRepository
    {
        public TitleRepository(GameTrackerContext context)
            : base(context)
        {

        }

        public Title AddTitle(string name, string subtitle)
        {
            var title = new Title()
            {
                Name = name,
                Subtitle = subtitle
            };

            Context.Set<Title>().Add(title);
            
            Context.SaveChanges();

            return title;
        }
    }
}
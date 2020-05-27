using GameTrove.Storage.Models;

namespace GameTrove.Storage.Contracts
{
    public interface ITitleRepository : IRepository<Title>
    {
        Title AddTitle(string title, string subtitle);
    }
}
using InformationCards.DataLayer.Entities;

namespace InformationCards.DataLayer.Repositories.Interfaces
{
    public interface IInfoCardRepository
    {
        public void Add(InformationCard card);
        //public IAsyncEnumerable<InformationCard> Get();
        //public T Load<T>(string fileName) where T : class;
    }
}

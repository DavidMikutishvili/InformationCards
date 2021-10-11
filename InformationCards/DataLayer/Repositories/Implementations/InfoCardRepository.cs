using InformationCards.DataLayer.Entities;
using InformationCards.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.Json;

namespace InformationCards.DataLayer.Repositories.Implementations
{
    public class InfoCardRepository : IInfoCardRepository
    {
        private ApplicationContext _context;
        private readonly DbSet<InformationCard> _table;
        public InfoCardRepository(ApplicationContext context)
        {
            _context = context;
            _table = _context.Set<InformationCard>();
        }

        public void Add(InformationCard card)
        {
            using (FileStream fs = new FileStream("InformationCards.json", FileMode.OpenOrCreate))
            {
                InformationCard tom = card;
                JsonSerializer.SerializeAsync(fs, tom);
                //Console.WriteLine("Data has been saved to file");
                //_context.InformationCards.Add(tom);
                _context.SaveChanges();
            }

            //_context.Add(card);
            //_context.SaveChanges();
        }

    }
}

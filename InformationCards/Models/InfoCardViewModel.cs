using Microsoft.AspNetCore.Http;

namespace InformationCards.Models
{
    public class InfoCardViewModel
    {
        public IFormFile Image { get; set; }
        public string CardName { get; set; }
    }
}

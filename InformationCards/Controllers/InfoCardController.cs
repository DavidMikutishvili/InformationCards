using Elasticsearch.Net;
using InformationCards.DataLayer.Entities;
using InformationCards.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace InformationCards.Controllers
{
    [Route("api/[controller]")]
    public class InfoCardController : ControllerBase
    {
        //private readonly IInfoCardRepository _repository;

        //public InfoCardController(IInfoCardRepository repository)
        //{
        //    _repository = repository;
        //}

        //[HttpPost]
        //public void AddCard([FromBody] InformationCard card)
        //{
        //    _repository.Add(card);
        //}

        private const string INFO_CARDS_FILE_NAME = @"D:\C#\InformationCards\InfoCards.json";
        private List<InformationCard> _cards = new List<InformationCard>();
        private readonly IWebHostEnvironment _appEnvironment;

        public InfoCardController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public void ReadJson()
        {
            var read = System.IO.File.ReadAllText(INFO_CARDS_FILE_NAME);

            if (!String.IsNullOrEmpty(read))
            {
                _cards = JsonSerializer.Deserialize<List<InformationCard>>(read);
            }
        }

        public void WriteJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(_cards, options);
            System.IO.File.WriteAllText(INFO_CARDS_FILE_NAME, json);
        }

        [HttpPost]
        public IActionResult AddInfoCard(InfoCardViewModel model)
        {
            string pathCardImage; 

            if (model.Image != null)
            {
                pathCardImage = AddImage(model.Image);
            }
            else
            {
                return BadRequest("Please put an image");
            }

            if (!String.IsNullOrEmpty(model.CardName))
            {
                CreateInfoCard(model.CardName, pathCardImage);
            }
            else
            {
                return BadRequest("Please specify the name of the card");
            }

            return Ok();
        }

        public string AddImage(IFormFile file)
        {
            string pathCardImage;
            try
            {
                // path to the Images folder
                string path = "/Images/" + file.FileName;

                // save the file to the Images folder in the root directory of the application
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    pathCardImage = fileStream.Name;
                    file.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(HttpStatusCode.InternalServerError.ToString(), new Exception(ex.Message));
            }

            return pathCardImage;
        }

        public void CreateInfoCard(string name, string pathCardImage)
        {
            ReadJson();

            try
            {
                if (_cards.Count != 0)
                {
                    _cards.Add(new InformationCard
                    {
                        Id = GenerateId(_cards[_cards.Count - 1].Id),
                        CardName = name,
                        ImagePath = pathCardImage
                    }); ;
                }
                else
                {
                    _cards.Add(new InformationCard
                    {
                        Id = 1,
                        CardName = name,
                        ImagePath = pathCardImage
                    }); ;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(HttpStatusCode.InternalServerError.ToString(), new Exception(ex.Message));
            }

            WriteJson();
        }

        public int GenerateId(int id)
        {
            if (id != 0)
            {
                return id + 1;
            }
            else
            {
                return 1;
            }
        }
    }

}

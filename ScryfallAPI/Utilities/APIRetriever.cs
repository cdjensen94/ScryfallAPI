﻿using Newtonsoft.Json;
using System.Numerics;
namespace ScryfallAPI.Utilities
{
    public class APIRetriever
    {
        
        public APIRetriever() 
        {
               
        }

        public static async Task<List<ModelToParse>> GetData()
        {
            try
            {
                var url = $"https://api.scryfall.com/cards/search?q=b%3Ausg";
                ModelToParse model = new();
                List<ModelToParse> listModel = new();
                using var client = new HttpClient();
                using Stream stream = await client.GetStreamAsync(url);
                using StreamReader streamReader = new StreamReader(stream);
                using (JsonTextReader reader = new JsonTextReader(streamReader)) 
                {
                    var serializer = new JsonSerializer();
                    
                    while(reader.Read()) 
                    {
                        if(reader.TokenType == JsonToken.StartObject)
                        {
                            //Retrieving data and deserialzing it
                            ModelToParse dataRetrieved = serializer.Deserialize<ModelToParse>(reader);
                      
                            model.Object = dataRetrieved.Object;
                            model.has_more = dataRetrieved.has_more;
                            model.next_page = dataRetrieved.next_page;
                            model.total_cards = dataRetrieved.total_cards;
                            model.Data = dataRetrieved.Data.Where(p => p.penny_rank >= 8000)
                                                           .OrderByDescending(p => p.penny_rank)
                                                           .ToList();
                            listModel.Add(model);

                        }
                    }

                    return listModel ?? new List<ModelToParse>();

                    
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public class ModelToParse
        {

            public string? Object { get; set; }
            public int total_cards { get; set; }
            public bool has_more { get; set; }
            public string? next_page { get; set; }
            public List<Data>? Data { get; set; }
        }

        public class Data
        {
            public Guid? id { get; set; }
            public string? mtgo_id { get; set; }
            public string? mtgo_foil_Id { get; set; }
            public string? tcgplayer_Id { get; set; }
                         
            public string? name { get; set; }
            public string? lang { get; set; }
            public string? uri { get; set; }
            public string? scryfall_uri { get; set; }
            public string? layout { get; set; }
            public bool highres_image {  get; set; }      
            public string? released_at { get; set; }
            public int penny_rank { get; set; }
            
        }

    }

}

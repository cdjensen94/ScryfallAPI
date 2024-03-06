using Newtonsoft.Json;
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
                ModelToParse model1 = new();
                List<ModelToParse> listModel = new();
                using var client = new HttpClient();
                using Stream stream = await client.GetStreamAsync(url);
                using StreamReader streamReader = new StreamReader(stream);
                using (JsonTextReader reader = new JsonTextReader(streamReader)) 
                {
                    
                    //reader.SupportMultipleContent = true;
                    var serializer = new JsonSerializer();
                    while(reader.Read()) 
                    {
                        if(reader.TokenType == JsonToken.StartObject)
                        {
                            //Dictionary<string, object> dic2 = new();
                            ModelToParse model= serializer.Deserialize<ModelToParse>(reader);
                            //Dictionary<string, Dictionary<string, object>>dic = serializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(reader);

                            //foreach(var d in dic)
                            //{
                            //    if(d.Key.Equals("data"))
                            //    {
                            //        dic2.Add(d.Key, d.Value);
                            //    }
                            //}
                            //var filtered = model.Data.Where(p => p.penny_rank >= 8000).ToList();
                            //model.Data = filtered;
                            //datas.Add(model);


                            model1.Object = model.Object;
                            model1.has_more = model.has_more;
                            model1.next_page = model.next_page;
                            model1.total_cards = model.total_cards;
                            model1.Data = model.Data.Where(p => p.penny_rank >= 8000).ToList();
                            listModel.Add(model1);

                            //ModelToParse result = serializer.Deserialize<ModelToParse>(reader);

                            //model.TotalCards = result.TotalCards;
                            //model.Object = result.Object;
                            //model.HasMore = result.HasMore;
                            //model.NextPage = result.NextPage;
                            //  model.Data = result.Data;
                            //foreach(var item in result)
                            //{



                            //    //if (item.Key.Equals("data"))
                            //    //    list.Add((string)item.Value);
                            //    //    foreach (var item2 in list)
                            //    //    {

                            //    //        model.PennYRank.Add(item.Key, );
                            //    //    }
                            //}

                            //while(model.PennYRank.ContainsValue(5176))
                            //{
                            //    continue;
                            //}

                        }
                    }

                    return listModel ?? new List<ModelToParse>();

                    
                }
                //var response = await client.GetAsync(url);
               

                   // string responseContent = await response.Content.ReadAsStringAsync();
                    //return JsonSerializer.Deserialize<string>(responseContent);
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

        //public class DatasException : Exception
        //{
        //    public DatasException()
        //    {
        //        base.Message;
        //    }
        }


    }

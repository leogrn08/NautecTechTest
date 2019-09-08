using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace NautecTechTest
{
    /// <summary>
    /// Classe que representa cada um dos objetos formatados em json
    /// do arquivo "boxes.data"
    /// </summary>
    public class Box
    {
        public string id { get; set; }
        public string person_id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int p1x { get; set; }
        public int p1y { get; set; }
        public int p2x { get; set; }
        public int p2y { get; set; }


        public Box()
        {

        }

        /// <summary>
        /// Construtor da classe Box que recebe uma linha de json e converte nos
        /// atributos do objeto
        /// </summary>
        /// <param name="json"></param>
        public Box(string json)
        {
            JObject jObject = JObject.Parse(json);
            id = (string) jObject["id"];
            person_id = (string) jObject["person_id"];
            x = (int) jObject["x"];
            y = (int)jObject["y"];
            p1x = (int) jObject["p1x"];
            p1y = (int) jObject["p1y"];
            p2x = (int) jObject["p2x"];
            p2y = (int) jObject["p2y"];
        }
    }

}

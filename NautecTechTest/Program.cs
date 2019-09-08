using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NautecTechTest
{   

    /// <summary>
    /// Classe principal do programa que contem o método Main.
    /// </summary>
    class Program
    {
        /// <summary>
        /// O método main verifica se existem arquivos .data na pasta Input,
        /// caso existam ele passa arquivo por arquivo executando a função
        /// leArquivoData() para ler todos os objetos Box de cada um, cria uma
        /// lista de pessoas com cada lista de objetos Box executando a função
        /// criaListaPessoas() e depois escreve no arquivo output.data encontrado
        /// na pasta Output com a função escreveOutputData(), caso não existam
        /// arquivos na pasta Input ele simplesmente avisa o usuário disso.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string caminhoInput = Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\", @"Input"));
            string[] arquivosData = Directory.GetFiles(caminhoInput, "*.data", SearchOption.TopDirectoryOnly);
            if (arquivosData.Length != 0)
            {
                foreach (string arquivoData in arquivosData)
                {
                    List<Box> boxes = leArquivoData(arquivoData);
                    List<Person> persons = criaListaPessoas(boxes);
                    escreveOutputData(persons);
                }
                Console.WriteLine("Passagem dos arquivos da pasta Input para o arquivo output.data concluída");
            }
            else
            {
                Console.WriteLine("Nenhum arquivo .data foi identificado na pasta input");
            }

        }

        /// <summary>
        /// Função que lê o arquivo data e retorna uma lista de objetos
        /// do tipo Box com as informações encontradas nele.
        /// </summary>
        /// <returns></returns>
        public static List<Box> leArquivoData(string caminhoArquivo)
        {
            List<Box> boxes = new List<Box>();
            string[] lines = File.ReadAllLines(caminhoArquivo);
            foreach (string line in lines)
                boxes.Add(new Box(line));
            return boxes;
        }

        /// <summary>
        /// Função que recebe uma lista de objetos Box e retorna uma lista com todos
        /// os person_id existentes.
        /// </summary>
        /// <param name="boxes"></param>
        /// <returns></returns>
        public static List<string> identificaTodosPersonIds(List<Box> boxes)
        {
            List<string> personIds = new List<string>();
            foreach(Box box in boxes)
            {
                if (!personIds.Contains(box.person_id))
                {
                    personIds.Add(box.person_id);
                }
            }
            return personIds;
        }

        /// <summary>
        /// Função que analisa as listas de movimentos no eixo y
        /// de cada pessoa retorna sua ação principal.
        /// </summary>
        /// <param name="deltax"></param>
        /// <param name="deltay"></param>
        /// <returns></returns>
        public static string identificaAcaoPessoa(List<int> eixoY)
        {
            int deltay = eixoY[eixoY.Count - 1] - eixoY[0];
            Console.WriteLine("variação do eixo Y: " + deltay);
            string acao = "Ação não identificada";
            if (deltay < 100 && deltay >-100)
            {
                acao = "Passou na frente da loja";
                foreach(int y in eixoY)
                {
                    if(y > 400)
                    {
                        acao = "Passou por dentro da loja";
                    }
                }
            }else if(deltay > 150)
            {
                acao = "Entrou na loja";
            }else if(deltay < -150)
            {
                acao = "Saiu da loja";
            }
            Console.WriteLine("Ação principal: " + acao);
            return acao;
        }

        /// <summary>
        /// Função que recebe uma lista de objetos Box, utiliza a função 
        /// indentificaTodosIds()para obter os person_id existentes, guarda todas
        /// as posições y de um mesmo person_id em uma lista e depois a insere
        /// na função identificaAcaoPessoa() para obter sua ação, criando uma
        /// pessoa com o person_id e ação correspondentespara adicioná-la na
        /// lista de pessoas e após todos os person_id serem verificados 
        /// a lista de pessoas é retornada.
        /// </summary>
        /// <param name="boxes"></param>
        /// <returns></returns>
        public static List<Person> criaListaPessoas(List<Box> boxes)
        {
            List<Person> persons = new List<Person>();
            List<string> personIds = identificaTodosPersonIds(boxes);
            foreach (string personId in personIds) {
                Console.WriteLine("person_id: " + personId);
                List<int> y = new List<int>();
                foreach (Box box in boxes)
                {   
                    if(box.person_id == personId)
                    {
                        y.Add(box.y);
                    }
                }
                Person person = new Person(personId, identificaAcaoPessoa(y));
                persons.Add(person);
            }
            return persons;
        }

        /// <summary>
        /// Função que escreve as pessoas da lista de pessoas no arquivo 
        /// output.data em formato json.
        /// </summary>
        /// <param name="persons"></param>
        public static void escreveOutputData(List<Person> persons)
        {
            string caminhoOutput = Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\", @"Output\output.data"));
            foreach (Person person in persons)
            {
                JObject jo = (JObject)JToken.FromObject(person);
                if(!File.Exists(caminhoOutput))
                    File.WriteAllText(caminhoOutput, jo.ToString() + "\n");
                else
                    File.AppendAllText(caminhoOutput, jo.ToString() + "\n");
            }
        }
    }
}

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
        /// Função que analisa as variações do eixox e do eixoy recebidas e retorna a
        /// ação correspondente.
        /// </summary>
        /// <param name="deltax"></param>
        /// <param name="deltay"></param>
        /// <returns></returns>
        public static string identificaAcaoPessoa(int deltax, int deltay)
        {
            string acao = "Ação não identificada";
            if (deltay < 100 && deltay >-100)
            {
                if (deltax > 100 || deltax < -100)
                    acao = "Passou na frente da loja";
            }else if(deltay > 150)
            {
                acao = "Entrou na loja";
            }else if(deltay < -150)
            {
                acao = "Saiu da loja";
            }
            return acao;
        }

        /// <summary>
        /// Função que recebe uma lista de objetos Box, utiliza a função 
        /// indentificaTodosIds()para obter os person_id existentes, guarda todas
        /// as posições x e y de um mesmo person_id em listas e depois insere a 
        /// diferença do último valor de cada lista com o primeiro (respectivamente
        /// a última posição da pessoa e a primeira) na função identificaAcaoPessoa()
        /// para obter sua ação, criando uma pessoa com o person_id e ação correspondentes
        /// para adicioná-la na lista de pessoas e após todos os person_id serem 
        /// verificados a lista de pessoas é retornada.
        /// </summary>
        /// <param name="boxes"></param>
        /// <returns></returns>
        public static List<Person> criaListaPessoas(List<Box> boxes)
        {
            List<Person> persons = new List<Person>();
            List<string> personIds = identificaTodosPersonIds(boxes);
            foreach (string personId in personIds) {
                Console.WriteLine(personId);
                List<int> x = new List<int>(), y = new List<int>();
                foreach (Box box in boxes)
                {   
                    if(box.person_id == personId)
                    {
                        x.Add(box.x);
                        y.Add(box.y);
                    }
                }
                Console.WriteLine(x[x.Count - 1] - x[0]);
                Console.WriteLine(y[y.Count - 1] - y[0]);
                Person person = new Person(personId, identificaAcaoPessoa(x[x.Count-1] - x[0], y[y.Count-1] - y[0]));
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

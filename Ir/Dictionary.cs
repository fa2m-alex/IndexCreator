using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Ir
{
    class Dictionary
    {
        public Dictionary<string, int> Dict { get; }
        public Dictionary<int, List<int>> Collection { get; }

        private string filesPath;
        string[] files;
        //StreamWriter saver;

        public Dictionary(string filesPath)
        {
            this.filesPath = filesPath;
            files = Directory.GetFiles(filesPath);
            Dict = new Dictionary<string, int>();
            Collection = new Dictionary<int, List<int>>();
        }

        private void CreateDictionaryAndIndex()
        {
            
            int words_counter = 1;

            int doc_counter = 0;

            foreach (var file in files)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);

                XmlNodeList elemList = doc.GetElementsByTagName("p");

                foreach (XmlNode elem in elemList)
                {
                    foreach (var word in GetWords(elem.InnerText))
                    {

                        if (!Dict.ContainsKey(word.ToLower()))
                        {
                            //dictionary creation
                            Dict.Add(word.ToLower(), words_counter);
                            words_counter++;
                        }



                        //index creation
                        List<int> temp = new List<int>();
                        temp.Add(doc_counter);
                        int wordIndex = Dict[word.ToLower()];
                        if (!Collection.ContainsKey(wordIndex))
                            Collection.Add(wordIndex, temp);
                        else if (!Collection[wordIndex].Contains(doc_counter))
                            Collection[wordIndex].Add(doc_counter);
                    }
                }

                doc_counter++;
            }

        }

        

        public void SaveDictionary(string path)
        {
            StreamWriter saver = new StreamWriter(path);

            CreateDictionaryAndIndex();

            var keys = Dict.Keys.ToList();
            keys.Sort();

            foreach (var word in keys)
            {
                saver.WriteLine(word + " " + Dict[word]);
            }

            saver.Close();

            Console.WriteLine("Done");
        }

        public void SaveIndex(string path)
        {
            StreamWriter saver = new StreamWriter(path);

            CreateDictionaryAndIndex();

            foreach (var word_id in Collection.Keys)
            {
                saver.Write(word_id);

                foreach (var book_id in Collection[word_id].ToList())
                {
                    saver.Write(" " + book_id);
                }

                saver.Write(System.Environment.NewLine);
            }

            saver.Close();

            Console.WriteLine("Done");
        }

        private string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\w+");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select m.Value;

            return words.ToArray();
        }
    }
}

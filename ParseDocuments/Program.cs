using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ParseDocuments
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите путь к файлу: ");
            var path = Console.ReadLine();
            Console.Write("Введите куда сохранить результат: ");
            string writePath = Console.ReadLine();
            Console.Clear();
            var dict = new Parser().Parse(File.ReadAllText(path, Encoding.Default));
            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                foreach (var d in dict.Values)
                    sw.WriteLine(d);
            }
        }

        class Parser
        {
            public Dictionary<string, Document> Parse(string text)
            {
                var result = new Dictionary<string, Document>();
                var iLine = 0;
                Console.Write("Введите слово: ");
                string wor = Console.ReadLine();
                Console.Write("Введите регулярное выражение: ");
                string regular = Console.ReadLine();
                foreach (var line in text.ToLower().Split(new char[] { '\n' }, StringSplitOptions.None))
                {
                    iLine++;
                    foreach (Match m in Regex.Matches(line, wor + $@"{regular}"))
                    {
                        var word = m.Value;
                        Document d;
                        if (!result.TryGetValue(word, out d))
                            d = result[word] = new Document(word);
                        d.Add(iLine);
                        d.Added(line);
                    }
                }

                return result;
            }
        }

        class Document
        {
            public string Value { get; private set; }
            public int Count { get; private set; }
            public HashSet<int> Lines = new HashSet<int>();
            public string Str { get; private set; }

            public void Added(string line)
            {
                Str = line;
            }
            public Document(string value)
            {
                this.Value = value;
            }

            public void Add(int iLine)
            {
                Lines.Add(iLine);
                Count++;
            }

            public override string ToString()
            {
                return String.Format("По запросу {0} найдено {1} строк(и):\n{2} \nТекст строки: {3}", Value, Count, String.Join("\n", Lines), Str);
            }
        }
    }
}
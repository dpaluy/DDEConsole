using System;
using System.Xml;
using System.IO;

namespace DDEConsole
{
    public class DataReader
    {
        private XmlReader reader;
        public DataReader(String fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open);
            reader = new XmlTextReader(stream);
        }

        public void Read()
        {
            while (reader.Read())
            {
                Console.WriteLine(reader.Value);
            }
        }
    }
}

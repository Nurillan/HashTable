using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HashTable
{
    public interface IHashTableService
    {
        List<CarRecord> Load(string fileName);
        void Save(List<CarRecord> toys, string fileName);
    }

    public static class ServiceFactory
    {        
        public static IHashTableService getService(string ex)
        {   
            switch (ex)
            {
                case ".txt":
                    return new TxtService();

                case ".xml":
                    return new XmlService();

                case ".dat":
                    return new DatService();

                default: throw new Exception("Unknown file extension");
            }        
        } 
    }

    class TxtService: IHashTableService
    {
        private static CarRecord RecordFromString(string str)
        {
            int space = str.IndexOf(' ');
            int semicolon = str.IndexOf(';');
            string sBrand = str.Substring(0, space);
            CarBrand Brand = (CarBrand)Enum.Parse(typeof(CarBrand), sBrand);

            string sNumber = str.Substring(space + 1, semicolon - space);
            CarNumber Number = new CarNumber(sNumber);

            string sPerson = str.Substring(str.IndexOf(':') + 2);
            int firstSpace = sPerson.IndexOf(' ');
            int lastSpace = sPerson.LastIndexOf(' ');

            Person person = new Person(sPerson.Substring(0, firstSpace),
                                       sPerson.Substring(firstSpace + 1, lastSpace - firstSpace - 1),
                                       sPerson.Substring(lastSpace + 1, sPerson.Length - lastSpace - 2));

            return new CarRecord(Brand, person, Number);
        }

        public void Save(List<CarRecord> items, string fileName)
        {
            string text = "";
            foreach (CarRecord item in items)
                text += item.ToString() + Environment.NewLine;

            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                writer.Write(text);
            }
        }

        public List<CarRecord> Load(string fileName)
        {
            List<CarRecord> list = new List<CarRecord>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    list.Add(RecordFromString(str));
                }
            }
            return list;
        }
    }

    class XmlService: IHashTableService
    {
        public void Save(List<CarRecord> items, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CarRecord>));
            using (FileStream file = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(file, items);
            }
        }

        public List<CarRecord> Load(string fileName)
        {
            List<CarRecord> items = new List<CarRecord>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<CarRecord>));
            using (FileStream file = new FileStream(fileName, FileMode.Open))
            {
                items = (List<CarRecord>)serializer.Deserialize(file);
            }
            return items;
        }
    }

    class DatService: IHashTableService
    {
        public void Save(List<CarRecord> items, string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream file = new FileStream(fileName, FileMode.Create))
            {
                formatter.Serialize(file, items);
            }
        }

        public List<CarRecord> Load(string fileName)
        {
            List<CarRecord> items = new List<CarRecord>();
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream file = new FileStream(fileName, FileMode.Open))
            {
                items = (List<CarRecord>)formatter.Deserialize(file);
            }
            return items;
        }
    }
}

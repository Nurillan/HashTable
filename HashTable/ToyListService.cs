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
    public interface IToyListService
    {
        List<CarRecord> Load(string fileName);
        void Save(List<CarRecord> toys, string fileName);
    }

    public static class ServiceFactory
    {        
        public static IToyListService getService(string ex)
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

    class TxtService: IToyListService
    {
        private static CarRecord ToyFromString(string str) //string like "<type>, *<Age>*, *<Price>*"
        {
            string NumberFromString(string substring)
            {
                int i = 0;
                int len = substring.Length;
                string res = "";
                while ((i < len) && ((substring[i] > '9') || (substring[i] < '0')))
                    i++;
                while ((i < len) && (substring[i] <= '9') && (substring[i] >= '0'))
                {
                    res += substring[i];
                    i++;
                }
                return (res == "") ? null : res;
            }

            string sType = str.Substring(0, str.IndexOf(',')).Trim();
            CarBrand type = (CarBrand)Enum.Parse(typeof(CarBrand), sType);

            string temp = str.Substring(str.IndexOf(','), str.LastIndexOf(',')).Trim();
            string sAge = NumberFromString(temp);
            byte age = byte.Parse(sAge);

            temp = str.Substring(str.LastIndexOf(',')).Trim();
            string sPrice = NumberFromString(temp);
            double price = double.Parse(sPrice);

            return new CarRecord(CarBrand.Daihatsu, new Person("ss", "ss", "ss"), new CarNumber("ccc", "444", "44"));
        }

        public void Save(List<CarRecord> toys, string fileName)
        {
            string text = "";
            foreach (CarRecord toy in toys)
                text += toy.ToString() + Environment.NewLine;

            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                writer.Write(text);
            }
        }

        public List<CarRecord> Load(string fileName)
        {
            List<CarRecord> toys = new List<CarRecord>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    toys.Add(ToyFromString(str));
                }
            }
            return toys;
        }
    }

    class XmlService: IToyListService
    {
        public void Save(List<CarRecord> toys, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CarRecord>));
            using (FileStream file = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(file, toys);
            }
        }

        public List<CarRecord> Load(string fileName)
        {
            List<CarRecord> toys = new List<CarRecord>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<CarRecord>));
            using (FileStream file = new FileStream(fileName, FileMode.Open))
            {
                toys = (List<CarRecord>)serializer.Deserialize(file);
            }
            return toys;
        }
    }

    class DatService: IToyListService
    {
        public void Save(List<CarRecord> toys, string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream file = new FileStream(fileName, FileMode.Create))
            {
                formatter.Serialize(file, toys);
            }
        }

        public List<CarRecord> Load(string fileName)
        {
            List<CarRecord> toys = new List<CarRecord>();
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream file = new FileStream(fileName, FileMode.Open))
            {
                toys = (List<CarRecord>)formatter.Deserialize(file);
            }
            return toys;
        }
    }
}

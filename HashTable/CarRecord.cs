using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{

    public struct CarNumber
    {
        public string letters { get; private set; }
        public string num { get; private set; }
        public string region { get; private set; }

        public CarNumber(string letters, string num, string region)
        {
            this.letters = letters;
            this.num = num;
            this.region = region;
        }

        public override string ToString()
        {
            return letters[0] + num + letters.Substring(1) + " (" + region + ")";
        }

        public int CompareTo(CarNumber other)
        {
            return this.ToString().CompareTo(other.ToString());
        }
    }

    [Serializable]
    public class CarRecord : IComparable<CarRecord>
    {
        public CarBrand Brand { get; }
        public Person Person { get; private set; } 
        public CarNumber Number { get; private set; }      

        public CarRecord(CarBrand type, Person person, CarNumber number)
        {
            Brand = type;
            Person = person;
            Number = number;
        }

        public override string ToString()
        {
            string result = Brand.ToString() + " ";
            result += Number.ToString() + Environment.NewLine;
            result += "Owner: " + Person.ToString() + ".";
            return result;
        }

        public int CompareTo(CarRecord other)
        {
            int result = 0;
            result = this.Person.CompareTo(other.Person) * 10;
            result = this.Brand.CompareTo(other.Brand) * 10;
            result = this.Number.CompareTo(other.Number) * 10;
            return result;
        }

    }
}

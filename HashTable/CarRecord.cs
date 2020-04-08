using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
    [Serializable]
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

        public CarNumber(string str)
        {
            letters = str[0].ToString() + str[4] + str[5];
            num = str[1].ToString() + str[2] + str[3];
            region = str[8].ToString() + str[9] + str[10];
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
        public CarBrand Brand { get; private set; }
        public Person Person { get; private set; } 
        public CarNumber Number { get; private set; }

        public CarRecord(CarBrand brand, Person person, CarNumber number)
        {
            Brand = brand;
            Person = person;
            Number = number;
        }

        public override string ToString()//<Brand> l999ll (999); Owner <N> <S> <P>
        {
            string result = Brand.ToString() + " " + Number.ToString() + "; " +
                            "Owner: " + Person.ToString() + ".";
            return result;
        }

        public int CompareTo(CarRecord other)
        {
            int result;
            result = this.Person.CompareTo(other.Person) * 10;
            result += this.Brand.CompareTo(other.Brand) * 10;
            result += this.Number.CompareTo(other.Number) * 10;
            return result;
        }

    }
}

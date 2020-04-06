using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
    public class Person: IComparable<Person>
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Patronymic { get; private set; }

        public Person(string name, string surname, string patronymic)
        {
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
        }

        public override string ToString()
        {
            return Name + " " + Surname + " " + Patronymic;
        }

        public int CompareTo(Person other)
        {
            return this.ToString().CompareTo(other.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
метод отрытой адресации (линейное опробование);
Задан набор записей следующей структуры: номер автомобиля (запись), его
марка и ФИО владельца. По номеру автомобиля найти остальные сведения.
Должны быть реализованы следующие
возможности:
1)загрузка данных из файла (текстового или типизированного);
2)сохранение данных в файл (текстовый или типизированный);
3)добавление данных;
4)удаление данных;
5)поиск данных;
6)выполнение основной задачи, указанной в упражнении.
*/

namespace HashTable
{
    public enum CellState
    {
        free,
        full,
        del
    }

    public struct Cell
    {
        public CarRecord record;
        public CellState state;
    }

    public class HashTable
    {
        public static int N { get; set; }
        public static int step { get; set; }
        public int Count { get; private set; }
        public Cell[] Table { get; private set; }

        public delegate int TypeGetKey(CarRecord record);
        public TypeGetKey GetKey { get; set; }
        public delegate int TypeHashFunction(int key);
        public TypeHashFunction HashFunction{ get; set; }

        public HashTable(int N = 101, int step = 1)
        {
            HashTable.N = N;
            HashTable.step = step;
            GetKey = buidInGetKey;
            HashFunction = BuildInHashFunction;
            Table = new Cell[N];
            Count = 0;
            for(int i = 0; i < N; i++)
            {
                Table[i].state = CellState.free;
            }
        }

        public static int buidInGetKey(CarRecord record)
        {
            int i = int.Parse(record.Number.region);
            string s = "";
            foreach (char ch in record.Number.letters)
                s += ((int)ch).ToString();
            int j = int.Parse(s);
            int k = int.Parse(record.Number.num);

            return i * k % j;
        }

        int BuildInHashFunction(int key)
        {
            return key % N;
        }
        
        int NextCell(int a0, int i)
        {
            return (a0 + i * step) % N;
        }

        int IndexOf(int key)
        {
            int a0 = HashFunction(key);
            int i = 0;
            int a = a0;
            do
            {
                i++;
                switch (Table[a].state)
                {
                    case CellState.free:
                        return -1;
                    case CellState.del:
                        a = NextCell(a0, i);
                        break;
                    case CellState.full:
                        if (key == GetKey(Table[a].record))
                            return a;
                        else a = NextCell(a0, i);
                        break;
                }
            }
            while (i < N);
            return -1;
        }

        public void Clear()
        {
            for (int i = 0; i < N; i++)
            {
                Table[i].state = CellState.free;
                Table[i].record = null;
            }
            Count = 0;
        } 

        public bool Add(CarRecord record)
        {
            int key = GetKey(record);
            int index = IndexOf(key);
            if (Count == N || index != -1)
                return false;
            index = HashFunction(key);
            while (Table[index].state == CellState.full)
                index = NextCell(index, 1);

            Table[index].record = record;
            Table[index].state = CellState.full;
            Count++;            
            return true;
        }

        public bool Delete(int key)
        {
            int index = IndexOf(key);
            if (Count == 0 || index == -1)
                return false;
            
            Table[index].state = CellState.del;
            Count--;
            return true;
        }

        public CarRecord Find(int key)
        {
            int index = IndexOf(key);
            if (Count == 0 || index == 0)
                return null;
            return Table[index].record;
        }  

        public CarRecord Find(CarNumber number)
        {
            CarRecord temp = new CarRecord(CarBrand.Acura, new Person("a", "a", "a"), number);
            int key = GetKey(temp);
            int index = IndexOf(key);
            return index == -1 ? null : Table[index].record;                    
        }

        public void SaveToFile(FileInfo file)
        {
            List<CarRecord> list = GetRecordsToList();
            IHashTableService saver = ServiceFactory.getService(file.Extension);
            saver.Save(list, file.FullName);
        }

        public void LoadFromFile(FileInfo file)
        {            
            IHashTableService loader = ServiceFactory.getService(file.Extension);
            List<CarRecord> list = loader.Load(file.FullName);
            LoadRecordsFromList(list);
        }

        public List<CarRecord> GetRecordsToList()
        {
            List<CarRecord> result = new List<CarRecord>();
            foreach(Cell cell in Table)
            {
                if (cell.state == CellState.full)
                    result.Add(cell.record);
            }
            return result;
        }

        public void LoadRecordsFromList(List<CarRecord> list)
        {
            foreach (CarRecord record in list)
                Add(record);
        }
    }
}

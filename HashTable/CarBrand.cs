using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
    [Serializable]
    public enum CarBrand
    {
        Acura = 1,
        Daihatsu = 2,
        Datsun = 3,
        Honda = 4,
        Infiniti = 5,
        Isuzu = 6,
        Lexus = 7,
        Mazda = 8,
        Mitsubishi = 9,
        Mitsuoka = 10,
        Nissan = 11,
        Subaru = 12,
        Suzuki = 13,
        Toyota = 14,
        Audi,
        BMW,
        Opel,
        Porsche,
        Smart,
        Volkswagen,
        GAZ,
        Lada,
        UAZ,
        ZiL,
    }

    public static class CarBrandClass
    {
        public static Dictionary<int, CarBrand> CarBrandDict = new Dictionary<int, CarBrand>();

        public static void InitDict()
        {
            foreach (CarBrand make in Enum.GetValues(typeof(CarBrand)))
            {
                CarBrandDict.Add((int)(make), make);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenetikAlgoritma
{
    abstract class K_nnFonksiyonlar
    {
      
        public static K_nnFonksiyonlar fonksiyon_sec(string fonksiyonIsmi)
        {
            switch (fonksiyonIsmi)
            {
                case "Oklid": return new K_nnOklid();
                case "Mannathan": return new K_nnManhattan();
                
                default: return null;
            }
        }
        abstract public double uzaklikHesapla (VeriKayit t, VeriKayit e,List<Gen>x);
    }
}

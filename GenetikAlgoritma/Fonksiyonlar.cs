using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenetikAlgoritma
{
    abstract class Fonksiyonlar
    {
        int genSayisi;
        double altsinir, ustsinir, minimumDegeri;

        public Fonksiyonlar( double altsinir, double ustsinir,double minimumDegeri,int genSayisi)
        {
            this.Altsinir = altsinir;
            this.Ustsinir = ustsinir;
            this.MinimumDegeri = minimumDegeri;
            this.genSayisi = genSayisi;
        }

        public double Altsinir
        {
            get
            {
                return altsinir;
            }

            set
            {
                altsinir = value;
            }
        }

        public double MinimumDegeri
        {
            get
            {
                return minimumDegeri;
            }

            set
            {
                minimumDegeri = value;
            }
        }

        public double Ustsinir
        {
            get
            {
                return ustsinir;
            }

            set
            {
                ustsinir = value;
            }
        }

        public int GenSayisi
        {
            get
            {
                return genSayisi;
            }

            set
            {
                genSayisi = value;
            }
        }

        public static Fonksiyonlar fonksiyon_olustur (int genSayisi, int kDegeri, List<VeriKayit> egitimVerisiNesne, List<VeriKayit> testVerisiNesne, List<String> testVerisiEtiketListesi,string combofonksiyon, string combobitirme)
        {
            K_nn k_nn = new K_nn(genSayisi);
            k_nn.setDegerAta(kDegeri, egitimVerisiNesne, testVerisiNesne, testVerisiEtiketListesi, combofonksiyon,combobitirme);
            return k_nn;
        }
        abstract public double hesapla(List<Gen> x);
    }
}

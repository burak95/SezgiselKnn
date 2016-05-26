using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenetikAlgoritma
{
    class K_nnManhattan : K_nnFonksiyonlar
    {
        public override double uzaklikHesapla(VeriKayit t, VeriKayit e, List<Gen> x)
        {
            double uzaklik = 0;
            for (int i = 0; i < e.kolon.Count; i++)
            {
                uzaklik = uzaklik + x[i].Veri *( Math.Abs(((double)e.kolon[i] - (double)t.kolon[i])));
            }
            return uzaklik;
        }
    }
}

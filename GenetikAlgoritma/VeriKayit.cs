using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenetikAlgoritma
{
    class VeriKayit
    {
        public string etiket;
        public double uzaklik;
        public ArrayList kolon = new ArrayList();
        public VeriKayit(string etiket)
        {
            this.etiket = etiket;
        }
        public VeriKayit()
        {
        }
        public void setKolon(double a)
        {
            kolon.Add(a);
        }
    }
}

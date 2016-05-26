using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenetikAlgoritma
{
    class K_nn : Fonksiyonlar
    {


        List<VeriKayit> egitimVerisiNesne = new List<VeriKayit>();
        List<VeriKayit> testVerisiNesne = new List<VeriKayit>();
        List<EtiketTutma> etiketListesi = new List<EtiketTutma>();
        List<EtiketTutma> agirliklioylamaListesi = new List<EtiketTutma>();
        List<String> testVerisiEtiketListesi = new List<string>();
        Random rnd = new Random();
        int kDegeri;
        K_nnFonksiyonlar fonksiyon;
        string combofonksiyon, combobitirme;
        public K_nn(int genSayisi) : base(0, 1, 100, genSayisi)
        {
            
        }
        public void setDegerAta(int kDegeri, List<VeriKayit> egitimVerisiNesne, List<VeriKayit> testVerisiNesne, List<String> testVerisiEtiketListesi,string combofonksiyon,string combobitirme)
        {
            this.kDegeri = kDegeri;
            this.egitimVerisiNesne = egitimVerisiNesne;
            this.testVerisiNesne = testVerisiNesne;
            this.testVerisiEtiketListesi = testVerisiEtiketListesi;
            this.combobitirme = combobitirme;
            this.combofonksiyon = combofonksiyon;
        }
        public override double hesapla(List<Gen> x)
        {
            fonksiyon = K_nnFonksiyonlar.fonksiyon_sec(combofonksiyon);

            agirliklioylamaListesi.Clear();
            etiketListesi.Clear();
            for (int i = 0; i < testVerisiNesne.Count; i++)
            {

                for (int j = 0; j < egitimVerisiNesne.Count; j++)
                {
                    egitimVerisiNesne[j].uzaklik = fonksiyon.uzaklikHesapla(testVerisiNesne[i], egitimVerisiNesne[j],x);
                }
                siralama(egitimVerisiNesne);
                if (combobitirme == "Ağırlıklı Oylama")
                {
                    testVerisiNesne[i].etiket = etiketBelirleAgirlikliOylama();
                }
                else
                {
                    testVerisiNesne[i].etiket = etiketBelirle();
                }

            }

            double doğruSonuc = 0;
            for (int i = 0; i < testVerisiNesne.Count; i++)
            {
                if (testVerisiNesne[i].etiket.Equals(testVerisiEtiketListesi[i]))
                {
                    doğruSonuc++;
                }
            }
            double basariOrani = (doğruSonuc * 100) / testVerisiNesne.Count;
            //MessageBox.Show("Test Edilen Veri Sayısı : " + testVerisiNesne.Count + "\nDoğru Sonuç Sayısı : " + doğruSonuc + "\nBaşarı Oranı %" + basariOrani);

            return basariOrani;
        }
        
        string etiketBelirleAgirlikliOylama()
        {
            for (int i = 0; i < kDegeri; i++)
            {
                EtiketTutma a = new EtiketTutma();
                agirliklioylamaListesi.Add(a);
                agirliklioylamaListesi[i].etiketSayisi = 1 / (egitimVerisiNesne[i].uzaklik * egitimVerisiNesne[i].uzaklik);
                agirliklioylamaListesi[i].etiket = egitimVerisiNesne[i].etiket;
            }
            for (int i = 0; i < agirliklioylamaListesi.Count; i++)
            {
                for (int j = 0; j < agirliklioylamaListesi.Count; j++)
                {
                    if (agirliklioylamaListesi[i].etiketSayisi > agirliklioylamaListesi[j].etiketSayisi)
                    {
                        EtiketTutma temp = agirliklioylamaListesi[i];
                        agirliklioylamaListesi[i] = agirliklioylamaListesi[j];
                        agirliklioylamaListesi[j] = temp;

                    }
                }
            }
            return agirliklioylamaListesi[0].etiket;
        }

        string etiketBelirle()
        {
            etiketListesi.Clear();
            string e;
            etiketListesi = new List<EtiketTutma>();
            for (int i = 0; i < kDegeri; i++)
            {
                e = egitimVerisiNesne[i].etiket;
                int etiketSayisi = 0;

                for (int j = 0; j < kDegeri; j++)
                {
                    if (egitimVerisiNesne[j].etiket.Equals(e))
                    {
                        etiketSayisi++;
                    }

                }
                if (etiketKontrol(egitimVerisiNesne[i].etiket))
                {
                    etiketListesi.Add(new EtiketTutma(egitimVerisiNesne[i].etiket, etiketSayisi));
                }
            }

            string etiket = etiketListesi[0].etiket;
            double enbuyuk = etiketListesi[0].etiketSayisi;

            for (int j = 0; j < etiketListesi.Count && j < kDegeri; j++)
            {
                if (enbuyuk < etiketListesi[j].etiketSayisi)
                {
                    enbuyuk = etiketListesi[j].etiketSayisi;
                    etiket = etiketListesi[j].etiket;
                }
                if (enbuyuk == etiketListesi[j].etiketSayisi)
                {
                    int sayi = rnd.Next(0, 2);
                    if (sayi == 0)
                    {
                        enbuyuk = etiketListesi[j].etiketSayisi;
                        etiket = etiketListesi[j].etiket;
                    }
                }
            }
            return etiket;

        }

        Boolean etiketKontrol(string etiket)
        {
            for (int i = 0; i < etiketListesi.Count; i++)
            {
                if (etiketListesi[i].etiket.Equals(etiket))
                {
                    return false;
                }
            }
            return true;
        }
        void siralama(List<VeriKayit> k)
        {

            for (int i = 0; i < k.Count; i++)
            {
                for (int j = 0; j < k.Count; j++)
                {
                    if (k[i].uzaklik < k[j].uzaklik)
                    {
                        VeriKayit temp = k[i];
                        k[i] = k[j];
                        k[j] = temp;

                    }
                }
            }
        }

      

      
    }
    
    public class EtiketTutma
    {
        public string etiket;
        public double etiketSayisi;

        public EtiketTutma(string k, int l)
        {
            etiket = k;
            etiketSayisi = l;
        }
        public EtiketTutma()
        {
        }

    }


}
    

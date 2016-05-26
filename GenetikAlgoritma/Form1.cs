using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace GenetikAlgoritma
{
    public partial class Form1 : Form
    {
        Random rnd = new Random();
        double caprazlamaOrani, mutasyonOrani, mutasyonKatsayisi;
        int populasyonBuyuklugu, iterasyonSayısı = 0;
        string bireySecimi, caprazlamaCesiti, mutasyonSecimi;
        Fonksiyonlar fonksiyon;
        Kromozom kromozom;
        Populasyon populasyon = new Populasyon();
        BireySecimi bireySec;
        Caprazlama caprazlamaSec;
        Mutasyon mutasyon;
        List<Kromozom> caprazlamakromozomList = new List<Kromozom>();
        Kromozom birey1, birey2, secilenCocukKromozom;
        List<Kromozom> yedekPopulasyon = new List<Kromozom>();
        List<double> enIyiUygunluklar = new List<double>();
        int mutasyonsayisi;
        string bireyRandom;
        string mutasyonRandom;
        public static int index;
        int iterasyonSayisiText;





        string k_nnfonksiyon, k_nnbitirme;
        int kDegeri;
        List<VeriKayit> egitimVerisiNesne = new List<VeriKayit>();
        List<VeriKayit> testVerisiNesne = new List<VeriKayit>();
        List<String> testVerisiEtiketListesi = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "excel dosyası|*.xlsx|excel dosyası|*.xls";
            if (o.ShowDialog() == DialogResult.OK)
            {

                egitimVerisiNesne = new VeriSeti(o.FileName, true).getEgitimVeriSeti();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "excel dosyası|*.xlsx|excel dosyası|*.xls";
            if (o.ShowDialog() == DialogResult.OK)
            {

                testVerisiNesne = new VeriSeti(o.FileName, false).getEgitimVeriSeti();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "excel dosyası|*.xlsx|excel dosyası|*.xls";
            if (o.ShowDialog() == DialogResult.OK)
            {

                try
                {


                    Excel.Application xlApp = new Excel.Application();
                    Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(o.FileName);
                    Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                    Excel.Range xlRange = xlWorksheet.UsedRange;
                    int satir = xlRange.Rows.Count;
                    int sutun = xlRange.Columns.Count;
                    for (int i = 2; i <= satir; i++)
                    {
                        testVerisiEtiketListesi.Add(xlRange.Cells[i, sutun].Value2.ToString());
                    }
                }
                catch (Exception k)
                {

                    MessageBox.Show("Hata " + k.ToString());
                }
            }
        }
        
        public void bireyListele(DataGridView dgData)
        {
            dgData.Rows.Clear();
            dgData.Columns.Clear();

            int columnsCount = populasyon.KromozomList[0].GenListesi.Count;
            for (int i = 0; i < columnsCount; i++)
            {
                dgData.Columns.Add("", "");
            }
            dgData.Columns.Add("", "");
            for (int i = 0; i < populasyon.KromozomList.Count; i++)
            {
                dgData.Rows.Add();
                int j = 0;


                for (; j < columnsCount; j++)
                {

                    dgData.Columns[j].HeaderText = "Gen" + j;


                    dgData.Rows[i].Cells[j].Value = populasyon.KromozomList[i].GenListesi[j].Veri;

                }
                if (populasyon.KromozomList.Count - 1 == i)
                    dgData.Columns[j].HeaderText = "Uygunluk değeri";
                dgData.Rows[i].Cells[j].Value = populasyon.KromozomList[i].UygulukDegeri;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (egitimVerisiNesne.Count == 0 || testVerisiNesne.Count == 0 || testVerisiEtiketListesi.Count == 0 || textBox1.Text == "" || textBox2.Text == "" || textBox6.Text == "" || textBox5.Text == "" || textBox3.Text == "" || textBox4.Text == "" || comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox4.SelectedItem == null || comboBox5.SelectedItem == null || comboBox6.SelectedItem == null)
            {
                MessageBox.Show("Eksik veri girdiniz");
                return;
            }

            caprazlamaOrani = Convert.ToDouble(textBox3.Text);
            mutasyonOrani = Convert.ToDouble(textBox2.Text);
            populasyonBuyuklugu = Convert.ToInt16(textBox1.Text);
            mutasyonKatsayisi = Convert.ToDouble(textBox4.Text);
            bireySecimi = comboBox1.Text;
            caprazlamaCesiti = comboBox2.Text;
            mutasyonSecimi = comboBox4.Text;
            kDegeri = Convert.ToInt16(textBox5.Text);
            bireyRandom = bireySecimi;
            mutasyonRandom = mutasyonSecimi;
            mutasyonsayisi = 0;
            k_nnbitirme = comboBox6.Text;
            k_nnfonksiyon = comboBox5.Text;
            iterasyonSayısı = 0;
            populasyon = new Populasyon();
            caprazlamakromozomList.Clear();
            yedekPopulasyon.Clear();
            iterasyonSayisiText = Convert.ToInt16(textBox6.Text);

            ilk_populasyon_olustur();
            bireyListele(dgvIlkIterasyon);


           

            int programSonlanmaYuzdesi =0;
            while (sonlanma_durumu())
            {
                try
                {
                    this.Text = "" + (programSonlanmaYuzdesi * 100) / iterasyonSayisiText;
                }
              catch (Exception) { }
                programSonlanmaYuzdesi++;
                iterasyon();
            }
            this.Text = "100";
            bireyListele(dgvSonuc);
            MessageBox.Show("En iyi birey uygunluk değeri : "+populasyon.KromozomList[0].UygulukDegeri);

            GrafikCizdirme grafik = new GrafikCizdirme();
            grafik.cizGrafik1(enIyiUygunluklar, chart1, "populasyon degisimi", Color.Blue);

        }

        public void ilk_populasyon_olustur()
        {
          
            fonksiyon = Fonksiyonlar.fonksiyon_olustur(egitimVerisiNesne[0].kolon.Count,kDegeri,egitimVerisiNesne,testVerisiNesne,testVerisiEtiketListesi,k_nnfonksiyon,k_nnbitirme);

            for (int i = 0; i < populasyonBuyuklugu; i++)
            {
                kromozom = new Kromozom();
                for (int j = 0; j < fonksiyon.GenSayisi; j++)
                {
                    kromozom.genEkle(new Gen((rnd.NextDouble() * (fonksiyon.Ustsinir - fonksiyon.Altsinir) + fonksiyon.Altsinir)));
                }
                kromozom.UygulukDegeri = fonksiyon.hesapla(kromozom.GenListesi);
                populasyon.kromozomEkle(kromozom);
            }
            populasyon_sirala(populasyon.KromozomList);
            sonlanma_durumu();
            enIyiUygunluklar.Add(populasyon.KromozomList[0].UygulukDegeri);

        }
        public void iterasyon()
        {
           
            random_secimler();

            populasyon_sirala(populasyon.KromozomList);
            iterasyonSayısı++;

           

            bireySec = BireySecimi.birey_secimi_yontem_sec(bireySecimi);
            caprazlamaSec = Caprazlama.caprazlama_secimi_yontem_sec(caprazlamaCesiti);

            listeKopyala();
            birey1 = bireySec.birey_sec(yedekPopulasyon);
            yedekPopulasyon.Remove(yedekPopulasyon[Form1.index]);
            birey2 = bireySec.birey_sec(yedekPopulasyon);

            caprazlamakromozomList = caprazlamaSec.caprazlama_yap(birey1, birey2, caprazlamaOrani);

            mutasyonOrani *= 100;

            int a = rnd.Next(0, 101);

            if (a >= 0 && a < mutasyonOrani)
            {
                mutasyon = Mutasyon.mutasyon_yontem_sec(mutasyonSecimi);
                caprazlamakromozomList = mutasyon.mutasyon_yap(caprazlamakromozomList, mutasyonKatsayisi, fonksiyon.Altsinir, fonksiyon.Ustsinir);
                mutasyonsayisi++;
            }

            foreach (var item in caprazlamakromozomList)
            {
                item.UygulukDegeri = fonksiyon.hesapla(item.GenListesi);
            }

            populasyon_sirala(caprazlamakromozomList);
            secilenCocukKromozom = caprazlamakromozomList[0];

            
            populasyon_sirala(populasyon.KromozomList);
            populasyon.KromozomList.Remove(populasyon.KromozomList[populasyon.KromozomList.Count-1]);


            populasyon.kromozomEkle(secilenCocukKromozom);
            populasyon_sirala(populasyon.KromozomList);

            enIyiUygunluklar.Add(populasyon.KromozomList[0].UygulukDegeri);
        }
        void random_secimler()
        {

            if (bireyRandom == "Random")
            {
                int rastgelesayi = rnd.Next(0, 4);
                if (rastgelesayi == 0)
                {
                    bireySecimi = "Deterministik";
                }
                else if (rastgelesayi == 1)
                {
                    bireySecimi = "Turnuva";
                }
                else if (rastgelesayi == 2)
                {

                    bireySecimi = "Rulet Tekerleği";
                }
                else
                {
                    bireySecimi = "Rastgele";
                }
            }
            if (mutasyonRandom == "Random")
            {
                int rastgelesayi = rnd.Next(0, 2);
                if (rastgelesayi == 0)
                {
                    mutasyonSecimi = "Toplama";
                }
                else
                {
                    mutasyonSecimi = "Çıkarma";
                }

            }

        
        }
        void populasyon_sirala(List<Kromozom> list)
        {
            List<SiralamaUzaklıkHesapla> uzaklikNesnesi = new List<SiralamaUzaklıkHesapla>();
            for (int i = 0; i < list.Count; i++)
            {
                SiralamaUzaklıkHesapla a = new SiralamaUzaklıkHesapla(Math.Abs((double)(list[i].UygulukDegeri - fonksiyon.MinimumDegeri)),list[i].UygulukDegeri);
                uzaklikNesnesi.Add(a);
            }
            for (int i = 0; i < uzaklikNesnesi.Count; i++)
            {
                for (int j = 0; j < uzaklikNesnesi.Count; j++)
                {
                    if (uzaklikNesnesi[i].uzaklik < uzaklikNesnesi[j].uzaklik)
                    {
                        SiralamaUzaklıkHesapla temp = uzaklikNesnesi[i];
                        uzaklikNesnesi[i] = uzaklikNesnesi[j];
                        uzaklikNesnesi[j] = temp;
                    }
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i; j < list.Count; j++)
                {
                    if (uzaklikNesnesi[i].uygunluk == list[j].UygulukDegeri)
                    {
                        Kromozom temp = list[j];
                        list[j] = list[i];
                        list[i] = temp;
                    }
                }
                
            }
        }
        void listeKopyala()
        {
            yedekPopulasyon.Clear();
            for (int i = 0; i < populasyon.KromozomList.Count; i++)
            {
                yedekPopulasyon.Add(populasyon.KromozomList[i]);
            }
        }
        public bool sonlanma_durumu()
        {
            bool durum = true;
            for (int i = 0; i < populasyon.KromozomList.Count; i++)
            {
                if ((populasyon.KromozomList[i].UygulukDegeri> (fonksiyon.MinimumDegeri-1.5))||populasyon.KromozomList[i].UygulukDegeri == fonksiyon.MinimumDegeri)
                {
                    MessageBox.Show("uygunluk bitirdi iterasyon sayisi : " + iterasyonSayısı);
                    durum = false;
                }
                if (iterasyonSayısı == iterasyonSayisiText)
                {
                    durum = false;
                }
            }
            return durum;
        }

    }
    class SiralamaUzaklıkHesapla
    {
        public double uzaklik,uygunluk;
        public SiralamaUzaklıkHesapla(double uzaklik,double uygunluk)
        {
            this.uzaklik = uzaklik;
            this.uygunluk = uygunluk;
        }
    }
}

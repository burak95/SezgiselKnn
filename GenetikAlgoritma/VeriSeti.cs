using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace GenetikAlgoritma
{
    class VeriSeti
    {
        List<VeriKayit> egitimVerisiNesne = new List<VeriKayit>();
        public VeriSeti(string dosyaYolu, bool veriseti)
        {
            try
            {


                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(dosyaYolu);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                VeriKayit k1;
                int satir = xlRange.Rows.Count;
                int sutun = xlRange.Columns.Count;
                for (int i = 2; i <= satir; i++)
                {
                    if (veriseti)
                        k1 = new VeriKayit(xlRange.Cells[i, sutun].Value2.ToString());
                    else
                        k1 = new VeriKayit();

                    for (int j = 1; j <= sutun - 1; j++)
                    {

                        k1.setKolon(xlRange.Cells[i, j].Value2);


                    }
                    egitimVerisiNesne.Add(k1);
                }
            }
            catch (Exception e)
            {

                MessageBox.Show("Hata " + e.ToString());
            }
        }
        public List<VeriKayit> getEgitimVeriSeti()
        {
            return egitimVerisiNesne;
        }
    }
}

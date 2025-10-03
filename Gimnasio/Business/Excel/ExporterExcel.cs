using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Gimnasio.Business.Excel
{
    class ExporterExcel
    {

        public static void Export(DataGridView dgvLista){
            // Creating a Excel object. 
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = "Hoja1";

                bool banderaHeader = true;
                int cellRowIndex = 1;
                int cellColumnIndex = 1;

                //Loop through each row and read value from each column. 
                for (int i = 0; i < dgvLista.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvLista.Columns.Count; j++)
                    {
                        // si es primer vez ponemos los headers
                        if (banderaHeader)
                        {
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dgvLista.Columns[j].HeaderText;

                        }

                        worksheet.Cells[cellRowIndex + 1, cellColumnIndex] = dgvLista.Rows[i].Cells[j].Value.ToString();

                        cellColumnIndex++;
                    }

                    //la segunda vez ya no debe poner los headers
                    if (banderaHeader) banderaHeader = false;

                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                //Getting the location and file name of the excel to save from user. 
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveDialog.FilterIndex = 1;

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Exportación exitosa");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            } 
        }

        
    }
}

using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace GeneracionDeInformes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize the database with localdb
            DatabaseHelper.InitializeDatabase();
            
            cmbCategorias.SelectedIndex = 0; // "Todas"
            CargarInforme("Todas");
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CargarInforme(cmbCategorias.SelectedItem.ToString());
        }

        private void CargarInforme(string categoria)
        {
            // Fetch data
            DataTable dtProductos = DatabaseHelper.GetProductos(categoria);

            // Configure Dataset for RDLC
            ReportDataSource rds = new ReportDataSource("dsProductos", dtProductos);

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            // Configure Parameters
            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("CategoriaFiltro", categoria);
            
            reportViewer1.LocalReport.SetParameters(parameters);
            
            // Refresh
            reportViewer1.RefreshReport();
        }
    }
}

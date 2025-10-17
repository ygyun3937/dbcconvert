using Microsoft.Data.Analysis;
using ScottPlot;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudyDataFrame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Control.Update += Update_ProgressBar_UI;

            //Control_Test();
        }
        public void Init_UI()
        {
            FileProgressBar.Maximum = 100;
        }



        private void Update_ProgressBar_UI(int nValue)
        {
            FileProgressBar.Value = nValue;
        }

        private void btnDbcTest_Click(object sender, EventArgs e)
        {
            //2. 파일 불러오기
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Can Dbc(*.dbc) | *.dbc";
            openFileDialog.ShowDialog();
            string path = openFileDialog.FileName;

            tb_InputFile.Text = path;
            //4. 파일 로드
            Control control = new Control();

            string outputpath = "";

            control.DbcFileConvert(path, out outputpath);

            tb_OutputFile.Text = outputpath;




        }
    }
}

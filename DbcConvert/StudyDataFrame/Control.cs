using Microsoft.Data.Analysis;
using OfficeOpenXml;
using OpenTK.Audio.OpenAL;
using ScottPlot;
using StudyDataFrame.DataClass;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static OfficeOpenXml.ExcelErrorValue;

namespace StudyDataFrame
{


    public class Control
    {
        public delegate void StatusUpdate(int data);
        public static event StatusUpdate Update;

        Data_dbc data_Dbc;

        public Control()
        {
            data_Dbc = new Data_dbc();
        }


        public bool DbcFileConvert(string filename, out string outputfile)
        {
            try
            {
                bool nRet = true;

                outputfile = "";
                //1. File Load
                nRet = data_Dbc.LoadDbcFile(filename);
                if (nRet != true)
                    return false;
                if (Update != null)
                {
                    Update(80);
                }
                //2. File Save
                nRet = data_Dbc.excelSave(filename, out outputfile);
                if (nRet != true)
                    return false;
                if (Update != null)
                {
                    Update(100);
                }
                return true;

            }
            catch (Exception ex)
            {
                outputfile = "";

                return false;
            }
        }




    }
}

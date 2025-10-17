using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OfficeOpenXml.ExcelErrorValue;

namespace StudyDataFrame.DataClass
{
    public class Data_dbc
    {

        string[] dbcHeaders = new string[Enum.GetNames(typeof(dbcPrefix)).Count()];
        string[] dbcSignalHeaders = new string[Enum.GetNames(typeof(dbcSignalDataHeader)).Count()];
        string MessageID;
        string MessageName;
        string DLC;
        public List<SignalData> dbcData;
        public List<string> dbcData_Test;

        public Data_dbc()
        {
            //1. dbc Dictionary 생성
            dbcData = new List<SignalData>();
            dbcData_Test = new List<string>();

            //2. dbc Header 취득
            dbcHeaders = Enum.GetNames(typeof(dbcPrefix));
            dbcSignalHeaders = Enum.GetNames(typeof(dbcSignalDataHeader));
        }
        public bool LoadDbcFile(string filename)
        {
            try
            {
                var lines = new List<string>();

                StreamReader sr = new StreamReader(filename,Encoding.Default);
                //파일라인 수 취득
                double lineCount = File.ReadLines(filename).Count();


                //3. 현재 필드가 속한 Header가 무엇인지 확인하는 변수
                string curHeader = "";


                //4. 현재 라인 수 확인
                double nCurrentLine = 0;
                while (!sr.EndOfStream)
                {

                    string line = sr.ReadLine();
                    nCurrentLine++;

                    //몇앞 공백 제거
                    line = line.TrimStart();

                    if (line == "")
                        continue;
                    //5. 탭 구분
                    var data = line.Split(' ');


                    //6. 헤더 취득
                    //6-1.헤더 면 헤더 변경
                    int nHeader = Array.IndexOf(dbcHeaders, data[0]);
                    if (nHeader > -1)
                    {
                        curHeader = dbcHeaders[nHeader];
                    }


                    //7. 헤더가 BO, SG 인경우
                    if (curHeader.Equals(dbcHeaders[(int)dbcPrefix.BO_]) ||
                        curHeader.Equals(dbcHeaders[(int)dbcPrefix.SG_]))
                    {
                        convertData_Dbc(curHeader, line);
                    }


                    //int nValue = (int)(nCurrentLine / lineCount * 100);
                    //if (Update != null)
                    //{
                    //    Update(nValue);
                    //}                


                }

                return true;
            }
            catch (Exception ex)
            {
                //_logdata.Add(ex.ToString().Split('\t'));

                return false;
            }

        }
        public void convertData_Dbc(string Header, string line)
        {
            switch (Header)
            {
                case "BO_":
                    var MessageData = line.Split(':');
                    var MessageInfo = MessageData[0].Split(' ');
                    MessageID = MessageInfo[1].ToString();
                    MessageName = MessageInfo[2].ToString();
                    MessageInfo = MessageData[1].TrimStart().Split(' ');
                    DLC = MessageInfo[0].ToString();
                    break;
                case "SG_":
                    SignalData signalData = new SignalData();
                    var SignalData = line.Split(':');
                    var SignalName = SignalData[0].Split(' ');
                    signalData.signalName = SignalName[1].ToString();   //signal Name

                    var SignalInfo = SignalData[1].TrimStart().Replace("  ", " ").Replace("\"", "").Replace("\" ", "").Split(' ');


                    var bitInfo = SignalInfo[0].Trim().Split('|');
                    signalData.Startbit = bitInfo[0].ToString();
                    var byteOrder = bitInfo[1].Split('@');
                    signalData.Length = byteOrder[0].ToString();
                    if (byteOrder[1].Contains("1"))
                    {
                        signalData.ByteOrder = "intel";
                    }
                    else
                    {
                        signalData.ByteOrder = "motorola";
                    }
                    if (byteOrder[1].Contains("+"))
                    {
                        signalData.Unsigned = "unsigned";
                    }
                    else
                    {
                        signalData.Unsigned = "signed";
                    }

                    SignalInfo[1] = SignalInfo[1].Replace("(", "");
                    SignalInfo[1] = SignalInfo[1].Replace(")", "");

                    var Fatorinfo = SignalInfo[1].Split(',');
                    signalData.Factor = Fatorinfo[0].ToString();
                    signalData.Offset = Fatorinfo[1].ToString();

                    SignalInfo[2] = SignalInfo[2].Replace("[", "");
                    SignalInfo[2] = SignalInfo[2].Replace("]", "");

                    var MinMaxInfo = SignalInfo[2].Split('|');
                    signalData.Minmum = MinMaxInfo[0].ToString();
                    signalData.Maxmum = MinMaxInfo[1].ToString();

                    var unit = SignalInfo[3];
                    signalData.Unit = unit;

                    var receiver = SignalInfo[4];
                    signalData.Receiver = receiver;

                    byte[] byteID = BitConverter.GetBytes(Convert.ToInt64(MessageID) & 0xFFFF);

                    Array.Reverse(byteID);
                    string strID = BitConverter.ToString(byteID).Replace("00", "").Replace("-", "");
                    if (strID == "")
                    {
                        break;
                    }
                    signalData.MessageID = string.Format("0x{0}", strID);
                    signalData.MessageName = MessageName;
                    signalData.DLC = DLC;

                    dbcData.Add(signalData);
                    break;
            }
        }

        public bool excelSave(string filename, out string outputFileName)
        {
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                ExcelPackage package = new ExcelPackage();

                var worksheet = package.Workbook.Worksheets.Add("Test");


                for (int nindex = 0; nindex < dbcSignalHeaders.Count(); nindex++)
                {
                    worksheet.Cells[1, nindex + 1].Value = dbcSignalHeaders[nindex];
                }

                int nrow = 2;
                foreach (var data in dbcData)
                {
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.Version + 1].Value = data.Version;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.MessageID + 1].Value = data.MessageID;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.DLC + 1].Value = data.DLC;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.MessageName + 1].Value = data.MessageName;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.signalName + 1].Value = data.signalName;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.Startbit + 1].Value = data.Startbit;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.Length + 1].Value = data.Length;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.ByteOrder + 1].Value = data.ByteOrder;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.Unsigned + 1].Value = data.Unsigned;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.Factor + 1].Value = data.Factor;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.Offset + 1].Value = data.Offset;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.Minmum + 1].Value = data.Minmum;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.Maxmum + 1].Value = data.Maxmum;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.Unit + 1].Value = data.Unit;
                    worksheet.Cells[nrow, (int)dbcSignalDataHeader.Receiver + 1].Value = data.Receiver;

                    nrow++;
                }


                if (File.Exists(filename + "_Convert.xlsx"))
                {
                    File.Delete(filename + "_Convert.xlsx");
                }
                File.WriteAllBytes(filename + "_Convert.xlsx", package.GetAsByteArray());

                outputFileName = filename + "_Convert.xlsx";
                return true;
            }
            catch (Exception)
            {
                outputFileName = "fail";
                return false;
            }
        }


    }
}

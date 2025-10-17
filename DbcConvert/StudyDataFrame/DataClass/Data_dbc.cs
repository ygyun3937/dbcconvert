using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OfficeOpenXml.ExcelErrorValue;
using static OpenTK.Graphics.OpenGL.GL;

namespace StudyDataFrame.DataClass
{
    public enum dbcSignalDataHeader
    {
        Version,
        MessageID,
        DLC,
        MessageName,
        signalName,
        Startbit,
        Length,
        ByteOrder,
        Unsigned,
        Factor,
        Offset,
        Minmum,
        Maxmum,
        Unit,
        Receiver,
    }
    class CanMessage
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string DLC { get; set; }
        public string Transmitter { get; set; }
        public List<CanSignal> Signals { get; set; }

        public CanMessage()
        {
            ID = "";
            Name = "";
            DLC = "";
            Transmitter = "";
            Signals = new List<CanSignal>();
        }
    }

    class CanSignal
    {
        public string signalName { get; set; }
        public string Startbit { get; set; }
        public string Length { get; set; }
        public string ByteOrder { get; set; }
        public string Unsigned { get; set; }
        public string Factor { get; set; }
        public string Offset { get; set; }
        public string Minmum { get; set; }
        public string Maxmum { get; set; }
        public string Unit { get; set; }
        public string Receiver { get; set; }
    }

    public class Data_dbc
    {
        public Data_dbc()
        {

        }
        public bool ConvertDbcFile(string filename, out string outputFileName)
        {
            try
            {
                List<CanMessage> message = ParseDbcFile(filename);


                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                ExcelPackage package = new ExcelPackage();

                var worksheet = package.Workbook.Worksheets.Add("Test");

                string[] dbcSignalHeaders = Enum.GetNames(typeof(dbcSignalDataHeader));

                for (int nindex = 0; nindex < dbcSignalHeaders.Count(); nindex++)
                {
                    worksheet.Cells[1, nindex + 1].Value = dbcSignalHeaders[nindex];
                }

                int nrow = 2;
                foreach (var data in message)
                {
                    //ID가 공백인 경우는 예약 메세지
                    if (data.ID == "")
                        continue;

                    foreach (var signaldata in data.Signals)
                    {
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.MessageID + 1].Value = string.Format("0x{0}", data.ID);
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.DLC + 1].Value = data.DLC;
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.MessageName + 1].Value = data.Name;
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.signalName + 1].Value = signaldata.signalName;
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.Startbit + 1].Value = signaldata.Startbit;
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.Length + 1].Value = signaldata.Length;
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.ByteOrder + 1].Value = signaldata.ByteOrder;
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.Unsigned + 1].Value = signaldata.Unsigned;
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.Factor + 1].Value = signaldata.Factor;
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.Offset + 1].Value = signaldata.Offset;
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.Minmum + 1].Value = signaldata.Minmum;
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.Maxmum + 1].Value = signaldata.Maxmum;
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.Unit + 1].Value = signaldata.Unit;
                        worksheet.Cells[nrow, (int)dbcSignalDataHeader.Receiver + 1].Value = signaldata.Receiver;
                        nrow++;
                    }

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

        static List<CanMessage> ParseDbcFile(string path)
        {
            var messages = new List<CanMessage>();
            CanMessage currentMsg = null;

            foreach (var line in File.ReadLines(path, Encoding.Default))
            {
                if (line.StartsWith("BO_"))
                {
                    var match = Regex.Match(line, @"BO_\s+(\d+)\s+(\w+)\s*:\s*(\d+)\s+(\w+)");
                    if (match.Success)
                    {

                        currentMsg = new CanMessage
                        {
                            ID = match.Groups[1].Value,
                            Name = match.Groups[2].Value,
                            DLC = match.Groups[3].Value,
                            Transmitter = match.Groups[4].Value
                        };

                        byte[] byteID = BitConverter.GetBytes(Convert.ToInt64(currentMsg.ID) & 0xFFFF);

                        Array.Reverse(byteID);
                        string strID = BitConverter.ToString(byteID).Replace("-", "");
                        strID = strID.TrimStart('0');

                        currentMsg.ID = strID;
                        messages.Add(currentMsg);
                    }
                }
                //SIgnal Message 이면서 Message Info가 있는 경우
                else if (line.TrimStart().StartsWith("SG_") && currentMsg != null)
                {
                    // SG_ BMSRdy : 0|1@1+ (1,0) [0|1] \"\" Vector__XXX
                    //SG_ BMSRdy :                                  \s+(\w+)\s*:
                    //0 start bit                                   \s*(\d+)\s*
                    //|                                             \|
                    //1 Length                                      \s*(\d+)
                    //@                                             @
                    //1 byteorder  (1: intel, 0 : motorora)         (\d)\s*
                    //+ sign/unsign (+ : Unsigned -: signed)        ([+-])\s*
                    //(                                             \(
                    //1, Factor                                     ([^,]+),\s* //,을 제외한 문자
                    //0) Offset                                     ([^)]+)\)\s*      //)을 제외한 문자                   
                    //[
                    //0| Minmum                                     ([^\|]+)\|\s*       //\|을 제외한 문자
                    //1] Maxmum                                      ([^\]]+)\]\s*       //\]을 제외한 문자  
                    //"" Unit                                       ""([^""]*)""\s        //공백과 공백사이의 ""을 제외한 문자열
                    //Vector__XXX Receiver                          (\w+)   //문자열
                    //정규식
                    //* 0번이상 반복인
                    //+ 1번이상 반복인
                    //\s 공백
                    //\w 문자 (+붙이면 하나이상의 문자)
                    //\d 10진수 (+붙이면 하나이상의 10진수)
                    //\D 숫자가 아닌 문자(\d와 반대)
                    //[문자] 괄호안에 문자 추출
                    //[^...] 괄호안 문자외 문자 추출
                    var match = Regex.Match(line, @"SG_\s+(\w+)\s*:\s*(\d+)\s*\|\s*(\d+)@(\d)\s*([+-])\s*\(([^,]+),\s*([^)]+)\)\s*\[([^\|]+)\|\s*([^\]]+)\]\s*""([^""]*)""\s+(\w+)");

                    if (match.Success)
                    {
                        var signal = new CanSignal
                        {
                            signalName = match.Groups[1].Value,
                            Startbit = match.Groups[2].Value,
                            Length = match.Groups[3].Value,
                            ByteOrder = match.Groups[4].Value == "1" ? "intel" : "motoroa",
                            Unsigned = match.Groups[5].Value == "+" ? "unsigned" : "signed",
                            Factor = match.Groups[6].Value,
                            Offset = match.Groups[7].Value,
                            Minmum = match.Groups[8].Value,
                            Maxmum = match.Groups[9].Value,
                            Unit = match.Groups[10].Value,
                            Receiver = match.Groups[11].Value
                        };
                        currentMsg.Signals.Add(signal);
                    }
                }
            }
            return messages;
        }
    }
}

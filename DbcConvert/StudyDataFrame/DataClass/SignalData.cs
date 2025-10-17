using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyDataFrame
{
    public enum dbcPrefix
    {
        VERSION = 0, //Version
        NS_, //NewSymbol
        BS_, //Can 통신속도
        BU_, //ECU 정보
        BO_, //Message
        SG_, //Signal 
        CM_, //Comment
        VAL_,//Value Table , Signal 판정 테이블
    }
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
    public class SignalData
    {
        public string Version;
        public string MessageID;
        public string DLC;
        public string MessageName;

        public string signalName;
        public string Startbit;
        public string Length;
        public string ByteOrder;
        public string Unsigned;
        public string Factor;
        public string Offset;
        public string Minmum;
        public string Maxmum;
        public string Unit;
        public string Receiver;


        public void DataClear()
        {
            Version = null;
            MessageID = null;
            DLC = null;
            MessageName = null;

            signalName = null;
            Startbit = null;
            Length = null;
            ByteOrder = null;
            Unsigned = null;
            Factor = null;
            Offset = null;
            Minmum = null;
            Maxmum = null;
            Unit = null;
            Receiver = null;
        }
        public void DataCopy(SignalData signalData)
        {
            Version = signalData.Version;
            MessageID = signalData.MessageID;
            DLC = signalData.DLC;
            MessageName = signalData.MessageName;

            signalName = signalData.signalName;
            Startbit = signalData.Startbit;
            Length = signalData.Length;
            ByteOrder = signalData.ByteOrder;
            Unsigned = signalData.Unsigned; 
            Factor = signalData.Factor; 
            Offset = signalData.Offset; 
            Minmum = signalData.Minmum; 
            Maxmum = signalData.Maxmum; 
            Unit = signalData.Unit; 
            Receiver = signalData.Receiver;   
           
        }
    }
}

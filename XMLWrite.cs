using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Chatbot
{
    //情報をXML化するクラス
    class XMLEdit
    {
        //データを読み込む関数
        public List<Datas> noteRead(List<Datas> dataList)
        {
            //            dataList = new List<Datas>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Datas>));
            using (var fs = new FileStream("DataTogether.xml", FileMode.Open))
            {
                dataList = (List<Datas>)serializer.Deserialize(fs);
            }
            return dataList;
        }
        //データを書き込む関数
        public List<Datas> noteWrite(List<Datas> dataList)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Datas>));
            using (FileStream fs = new FileStream("DataTogether.xml", FileMode.Create))
            {
                serializer.Serialize(fs, dataList);
            }
            return dataList;
        }
    }
}

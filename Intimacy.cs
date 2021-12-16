using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot
{
    //女の子の好感度を編集するクラス
    class Intimacy
    {
        List<Datas> dataList = new List<Datas>();
        XMLEdit xmlEdit = new XMLEdit();
        Datas datas = new Datas();
        
        //女の子の好感度を計算する関数
        public void intimacyCalc(int emotion)
        {
            dataList.Add(datas);
            dataList=xmlEdit.noteRead(dataList);
            dataList[0].Intimacy += emotion;
            if (dataList[0].Intimacy < -50)
            {
                dataList[0].Intimacy = -50;
            }
            xmlEdit.noteWrite(dataList);
        }
    }
}

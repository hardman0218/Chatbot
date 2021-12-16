using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Chatbot
{
    public partial class Form1 : Form
    {
        //シリアライズ化するための変数
        XMLEdit xmlEdit=new XMLEdit();
        List<Datas> dataList;
        Datas datas = new Datas();
        DateTime dateTime;
        //喋っている最中かの判定
        bool talk;
        DataEdit dataEdit = new DataEdit();
     
        private Csharp _chan;
        public Form1()
        {
            InitializeComponent();
        }
        //ログを表示する関数
        private void PutLog(string str)
        {
            textBox2.AppendText(str + "\r\n");
        }
        //ログに表示される女の子の名前
        private string Prompt(){
            string p = dataList[0].girlName;
            return p + ">";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            //シリアライズ処理
            datas = new Datas();
            dataList = new List<Datas>();
            dataList.Add(datas);
            dataList = xmlEdit.noteRead(dataList);
            dataList[0].Im = "私";
            xmlEdit.noteWrite(dataList);
            //女の子の一人称を代入
            _chan = new Csharp(dataList[0].girlName);
            TextWrite(dataList[0].girlName, dataList[0].Intimacy, dataList[0].level);
            //パネルを見えなくさせる
            panel1.Visible = false;
            panel2.Visible = false;
            //指定した時間になると女の子が話す内容が変わる
            DateTime time = DateTime.Now;
            DateTime morningTime = new DateTime(time.Year, time.Month, time.Day, 7, 00, 00);
            DateTime afternoonTime = new DateTime(time.Year, time.Month, time.Day, 15, 00, 00);
            DateTime eveningTime = new DateTime(time.Year, time.Month, time.Day, 18, 00, 00);
            DateTime nightTime = new DateTime(time.Year, time.Month, time.Day, 23, 59, 00);
            talk = true;
            textBox4.Text = string.Format("現在の時刻は{0:HH:mm}ですね", time);
            if (time>=morningTime&&time<=afternoonTime){
                this.BackgroundImage = Properties.Resources.afternoonRoom;
                textBox4.Text += "おはようございます！今日はお仕事ですか";
            }
            else if (time >= afternoonTime && time <= eveningTime){
                this.BackgroundImage = Properties.Resources.eveningRoom;
                textBox4.Text += "もう半日が過ぎましたね、仕事帰りですか？";
            }else if (time >= eveningTime && time <= nightTime)
            {
                this.BackgroundImage = Properties.Resources.nightRoom;
                textBox4.Text += "もう夜ですね！お風呂は入りましたかー？" +
                dataList[0].Im+"はもう入りました！ゆっくりして下さいね";
            }
            else
            {
                this.BackgroundImage = Properties.Resources.deepNightRoom;
                textBox4.Text += "もう深夜です！眠くなってきました～";
            }
        }
        //話すボタンを押したときの処理
        private void button1_Click(object sender, EventArgs e)
        {
            //ボタンを押したとき何も入力してなかったら疑問形で言われる
            string value = textBox1.Text;
            if(string.IsNullOrEmpty(value)){
                talk = true;
                textBox4.Text = "なに？";
            }
            else{
                //ユーザが入力してた場合にその言葉をの意味を判断する
                talk = false;
                talk = true;
                string response = _chan.Dialogue(value);
                dataList=xmlEdit.noteRead(dataList);
                dataList[0].girlName = dataList[0].girlName;
                dataList[0].mainName = dataList[0].mainName;
                intimacy.Text = "親密:" + dataList[0].Intimacy;
                level.Text = "レベル:" + dataList[0].level;
                //親密度がレベルの指数関数より上の場合レベルアップ
                if (dataList[0].Intimacy >= dataList[0].level * dataList[0].level)
                {
                    level.Text = "レベル:" + ++dataList[0].level;
                }
                //シリアライズ化、ユーザ情報を書き込む
                xmlEdit.noteWrite(dataList);
                textBox4.Text = response;
                PutLog(dataList[0].mainName + ">" + value);
                PutLog(Prompt() + response);
                textBox1.Clear();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
        }
        float count;

        private void timer2_Tick(object sender, EventArgs e)
        {
            //現在の日時を代入
            this.dateTime = DateTime.Now;
            label2.Text = this.dateTime.ToString();
            //女の子が話している時に好感度によって内容が変わる
            if (talk)
            {
                if (dataList[0].Intimacy >= 100)
                {
                    this.pictureBox1.Image = Properties.Resources.loveTalk;
                }
                else if (dataList[0].Intimacy >= 30)
                {
                    this.pictureBox1.Image = Properties.Resources.CatTalk;
                }
                else if (dataList[0].Intimacy > 0)
                {
                    this.pictureBox1.Image = Properties.Resources.preview2;
                }
                else if (dataList[0].Intimacy >= -10)
                {
                    this.pictureBox1.Image = Properties.Resources.SadTalk;
                }else if (dataList[0].Intimacy <= -30)
                {
                    this.pictureBox1.Image = Properties.Resources.AngarTalk;
                }
                this.textBox4.Visible = true;
                this.pictureBox2.Visible = true;
                count++;
                //6秒経つと、喋るのをやめる
                if(count>=600){
                    count = 0;
                    talk = false;
                }
            }
            else
            {
                if (dataList[0].Intimacy >= 100)
                {
                    this.pictureBox1.Image = Properties.Resources.love;
                }
                else if (dataList[0].Intimacy >= 30)
                {
                    this.pictureBox1.Image = Properties.Resources.CatNormal;

                }
                else if (dataList[0].Intimacy > 0)
                {
                    this.pictureBox1.Image = Properties.Resources.preview;
                }else if (dataList[0].Intimacy >= -15){
                    this.pictureBox1.Image = Properties.Resources.SadNormal;
                }else if(dataList[0].Intimacy <= -40){
                    this.pictureBox1.Image = Properties.Resources.AngarNormal;
                }
                this.textBox4.Visible = false;
                this.pictureBox2.Visible = false;
            }
        }
    
        private void addButton_Click(object sender, EventArgs e){

        }
       //名前の変更メニュー
        private void 名前の変更ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel2.Visible = false;
            this.panel1.Location = new System.Drawing.Point(101, 87);
            this.textBox5.Text =dataList[0].mainName;
            this.textBox6.Text = dataList[0].girlName;

            panel1.Visible = true;
        }
        //パネルの表示
        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }
        //セーブボタン
        private void button4_Click(object sender, EventArgs e)
        {
            dataList[0].mainName = textBox5.Text;
            dataList[0].girlName = textBox6.Text;
            dataList[0].level = dataList[0].level;
            dataList[0].Intimacy = dataList[0].Intimacy;
            dataList =xmlEdit.noteWrite(dataList);
            girlName.Text = dataList[0].girlName;
            panel1.Visible = false;
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
        }
        private void button1_Enter(object sender, EventArgs e)
        {
            MessageBox.Show("a");
        }
        //初めからにするメニュー
        private void 初めからToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult　result = MessageBox.Show("初めから遊びますか？",
                                              "ファイルを初期化",
                                              MessageBoxButtons.YesNoCancel,
                                              MessageBoxIcon.Exclamation,
                                              MessageBoxDefaultButton.Button1);


            if (result == DialogResult.Yes)
            {
                dataList=dataEdit.DefaultData(dataList);
                TextWrite(dataList[0].girlName, dataList[0].Intimacy, dataList[0].level);
            }

        }
        //現在の女の子の情報を代入
        public void TextWrite(string girlName,int intimacy,int level){
            this.girlName.Text = girlName;
            this.intimacy.Text = "親密:"+intimacy;
            this.level.Text = "レベル:"+level;
        }
        //レベルを代入
        public void levelWrite(int level)
        {
            this.level.Text = "レベル:"+level;
        }
        //女の子の名前を代入
        public void girlWrite(string girlName)
        {
            this.girlName.Text = girlName;
        }
        //好感度を代入
        public void intimacyWrite(int intimacy)
        {
            this.intimacy.Text = "親密:"+intimacy;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }
        //セーブボタン
        private void button5_Click(object sender, EventArgs e)
        {
            dataList=xmlEdit.noteRead(dataList);
            dataList[0].level = Convert.ToInt32(textBox7.Text);
            dataList[0].Intimacy = Convert.ToInt32(textBox8.Text);
            TextWrite(dataList[0].girlName, dataList[0].Intimacy, dataList[0].level);
            xmlEdit.noteWrite(dataList);
            panel2.Visible = false;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        //デバッグモードメニュー
        private void チートモードToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel2.Location = new System.Drawing.Point(101, 87);
            panel1.Visible = false;
            panel2.Visible = true;
            this.textBox7.Text = Convert.ToString(dataList[0].level);
     　     this.textBox8.Text= Convert.ToString(dataList[0].Intimacy);
        }
    }

    public class Datas{
        
            public string mainName;
            public string girlName;
            public int level;
            public int Intimacy;
            public string Im;
            public string endWord;
        
    }
    //シリアライズ化するための情報を持ったクラス
    public class DataEdit{
        XMLEdit xmlEdit = new XMLEdit();
        Datas datas = new Datas();
        
        public List<Datas> DefaultData(List<Datas> dataList)
        {
            datas = new Datas();
            dataList = new List<Datas>();
            dataList.Add(datas);
            dataList[0].mainName = "あなた";
            dataList[0].girlName = "妄想ちゃん";
            dataList[0].Intimacy = 1;
            dataList[0].level = 1;
            dataList[0].Im = "私";
            dataList[0].endWord = "です";
            xmlEdit.noteWrite(dataList);
            return dataList;
        }
    }
}

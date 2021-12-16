using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Chatbot
{
    internal class RepeatResponder:Responder
    {
        Intimacy intimacyLogic = new Intimacy();
        string lineFeed;
        XMLEdit xmlWrite = new XMLEdit();
        Datas datas;
        List<Datas> dataList;

        public RepeatResponder(string name):base(name){
       
        }

        public override string Response(string input)
        {
            datas = new Datas();
            dataList = new List<Datas>();
            dataList.Add(datas);
            dataList=xmlWrite.noteRead(dataList);
            String respond = null;

            respond = matchRepeat(input);

            if(respond==null)
                respond = minusBigWord(input);
            if (respond==null){
                respond=minusSmallWord(input);
            }

            if (respond == null)
            {
                respond = plusSmallWord(input);
            }

            if (respond == null)
            {
                respond = plusBigWord(input);
            }

            if(respond==null){
                respond = randomWord();
            }

            return respond;
        }

        public string matchRepeat(string input)
        {

            int count=0;
            string[] gooChopar = { "グー", "チョキ", "パー" };
            string[] words = { "今日.*気分?","じゃんけん","おはよう","こんにちは","こんばんは","暇",
            "何時|時間|時刻","今.*[何|なに].*やってる","何日","何年|西暦","^こん$"};
            Random rnd = new Random();
            foreach (string name in words)
            {
                if (Regex.IsMatch(input, name))
                {
                    switch(count){
                        case 0:
                            intimacyLogic.intimacyCalc(1);
                            return "今日は気分いいですよ！";
                        break;
                        case 1:
                            intimacyLogic.intimacyCalc(1);
                            return gooChopar[rnd.Next(0,gooChopar.Length)];
                        break;
                        case 2:
                            intimacyLogic.intimacyCalc(1);
                            return "おはようございます！";
                            break;
                        case 3:
                            intimacyLogic.intimacyCalc(1);
                            return "こんにちは～♪";
                            break;
                            case 4:
                            intimacyLogic.intimacyCalc(1);
                            return "こんばんは～♪";
                            break;
                        case 5:
                            intimacyLogic.intimacyCalc(1);
                            return "暇ですね～";
                            break;
                        case 6:
                            intimacyLogic.intimacyCalc(1);
                            DateTime time = DateTime.Now;
                            return String.Format("現在の時刻は{0:HH時mm分}ですね", time);
                            break;
                        case 7:
                            intimacyLogic.intimacyCalc(1);
                            return "今はゲームをやっています！";
                            break;
                        case 8:
                            time = DateTime.Now;
                            return String.Format("現在は{0:MM月dd日}ですね", time);
                            break;
                        case 9:
                            time = DateTime.Now;
                            return String.Format("現在は{0:yyyy年}ですね", time);
                            break;
                        case 10:
                            return "こんです～♪";
                            break;
                    }
                }
                count++;
            }
            return null;
        }

        public string minusBigWord(string input)
        {
            string[] word = File.ReadAllLines(
            @"..\..\minusword.txt",
            System.Text.Encoding.UTF8
            );
            string[] sadWord = { "なんでそんな事いうんですか", "酷いです！", "絶交です", "口も効きたくないです" };
            Random rnd = new Random();
            //            string pattern = "(.*バカ.*)";
            // string[] names = { ".*バカ.*", };
            foreach (string name in word)
            {
                lineFeed= name.Replace("\n", "");
                if (name != "")
                {
                    if (Regex.IsMatch(input,lineFeed))
                    {
                        intimacyLogic.intimacyCalc(-10);
                        return sadWord[rnd.Next(0, sadWord.Length)];
                    }
                }
            }

            return null;
        }

        public string minusSmallWord(string input)
        {
            string[] word = File.ReadAllLines(
            @"..\..\minusSmallword.txt",
            System.Text.Encoding.UTF8
            );
            string[] sadWord = { "じゃありません！", "ではないです！", "って・・・ええっ！？", "って言わないで下さい！" };
            Random rnd = new Random();
            foreach (string name in word)
            {
                lineFeed = name.Replace("\n", "");
                if (name != "")
                {
                    if (Regex.IsMatch(input, lineFeed))
                    {
                        intimacyLogic.intimacyCalc(-1);
                        return name+sadWord[rnd.Next(0, sadWord.Length)];
                    }
                }
            }
            return null;
        }

        public string plusSmallWord(string input)
        {
            string[] word = File.ReadAllLines(
            @"..\..\plusSmallWord.txt",
            System.Text.Encoding.UTF8
            );
            string goodWord = "さんの方が";
            Random rnd = new Random();
            foreach (string name in word)
            {
                lineFeed = name.Replace("\n", "");
                if (name != "")
                {
                    if (Regex.IsMatch(input, lineFeed))
                    {
                        intimacyLogic.intimacyCalc(2);
                        return dataList[0].mainName+ goodWord+name+"です";
                    }
                }
            }
            return null;
        }

        public string plusBigWord(string input)
        {
            string[] word = File.ReadAllLines(
            @"..\..\plusBigWord.txt",
            System.Text.Encoding.UTF8
            );
            string[] goodWord = {"ありがとうございます！", "嬉しいです！" };
            Random rnd = new Random();
            foreach (string name in word)
            {
                lineFeed = name.Replace("\n", "");
                if (name != "")
                {
                    if (Regex.IsMatch(input, lineFeed))
                    {
                        intimacyLogic.intimacyCalc(5);
                        return goodWord[rnd.Next(0, goodWord.Length)];
                    }
                }
            }
            return null;
        }

        public string randomWord()
        {
            string[] word = File.ReadAllLines(
            @"..\..\randomDialogue.txt",
            System.Text.Encoding.UTF8
            );
            Random rnd = new Random();
            intimacyLogic.intimacyCalc(1);

            return word[rnd.Next(0, word.Length)];

        }
    }
}
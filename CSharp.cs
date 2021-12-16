using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot
{
    //ユーザが入力するコメント管理クラス
    class Csharp
    {
        private string name;
        private RandomResponder res_random;
        private RepeatResponder res_repeat;
        private Responder responder;

        public string Name{
            get{ return name; }
            set{ name = value; }
        }

        public Csharp(string name){
            this.name = name;
            res_random = new RandomResponder("Random");
            res_repeat = new RepeatResponder("Repeat");
        }

        public string Dialogue(string input){
            Random rnd = new Random();
            int num = rnd.Next(0, 10);
            this.responder = res_repeat;
            return this.responder.Response(input);
        }

        public string GetName(){
            return responder.Name; 
        }
    }
}

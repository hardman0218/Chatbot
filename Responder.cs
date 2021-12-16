namespace Chatbot
{
    //女の子が返信する人の編集
    internal class Responder
    {
        private string name;

        public string Name{
            get{ return name; }
            set{ name = value; }
        }

        public Responder(string name){
            this.name = name;
        }

        public virtual string Response(string input){
            return "";
        }
    }
}
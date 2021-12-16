using System;
using System.Text.RegularExpressions;

namespace Chatbot
{
    internal class RandomResponder : Responder
    {
        private string[] responses ={
            "いい天気だね",
            "ぶっちゃけ、そういうことね",
            "10円ひろった",
            "じゃあこれ知ってる？",
            "それいいじゃない",
            "それかわいい♪"
        };


        public RandomResponder(string name) : base(name)
        {
            
        }

        public override string Response(string input)
        {



            //            return responses[rnd.Next(0, responses.Length)];
            //   return minusBigWord(input);
            return "a";
        }

       
    }
}
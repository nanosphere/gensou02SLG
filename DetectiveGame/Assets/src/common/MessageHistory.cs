
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace common
{
    public class MessageHistory
    {
        List<string> messages = new List<string>();
        int maxline = 0;
        bool fcount = false;
        int count = 0;
        bool reverse = false;

        public MessageHistory(int maxLine,bool fcount,bool reverse)
        {
            this.fcount = fcount;
            maxline = maxLine;
            this.reverse = reverse;
        }
        
        public void addMessage(string str)
        {
            if (fcount)
            {
                count++;
                str = "" + (count) + "." + str;
            }
            if (messages.Count >= maxline)
            {
                for (int i = 0; i < messages.Count-1; i++)
                {
                    messages[i] = messages[i + 1];
                }
                messages[messages.Count - 1] = str;
            }
            else
            {
                messages.Add(str);
            }
        }

        public string getMessage()
        {
            string s = "";
            for (int i=0;i<messages.Count;i++)
            {
                if (reverse)
                {
                    s = messages[i] + "\n" + s;
                }
                else
                {
                    s += messages[i]+ "\n";
                }
            }
            return s;
        }

    }
}
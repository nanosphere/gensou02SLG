
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
        int count = 1;

        public MessageHistory(int maxLine)
        {
            maxline = maxLine;
        }
        
        public void addMessage(string str)
        {
            count++;
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

        public string getMessage(bool reverse, bool fcount)
        {
            string s = "";
            for (int i=0;i<messages.Count;i++)
            {
                string line = messages[i];
                if (fcount)
                {
                    line = "" + (count- messages.Count+i) + "." + line;
                }
                if (reverse)
                {
                    s = line + "\n" + s;
                }
                else
                {
                    s += line + "\n";
                }
            }
            return s;
        }
        public void clear()
        {
            count = 0;
            messages.Clear();
        }
    }
}
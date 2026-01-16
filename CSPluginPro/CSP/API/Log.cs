using Akequ.Items;
using Mirror;
using System.IO;
using UnityEngine;

namespace PluginExample
{
    public class Log : Akequ.Base.Room
    {
        private static Log _instance;
        public static Log instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Log();
                    return _instance;
                }
                return _instance;
            }
        }

        public void Print(object obj)
        {
            if (netEvent.isClient)
            {
                SendToServer("Print_Client", $"{obj}");
            }
            else
            {

                Debug.Log($"[Server]{obj}");
            }
        }

        public void Print_Client(string text, NetworkConnectionToClient conn)
        {
            Debug.Log($"[Client][Conn:{conn}]{text}");
        }

        public void ToFile(string fileName,string tag, string text)
        {
            if(netEvent.isClient)
            {
                this.Print("Can't write in client");
            }
            else
            {
                CustomLogger.LogInFile($"./log/{fileName}.txt", tag, text);
            }
        }

    }
}
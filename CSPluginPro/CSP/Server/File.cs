using System;
using System.IO;
using UnityEngine;

namespace PluginExample
{

    public class File : Akequ.Base.Room
    {
        static void Read(string filePath, Action<string> action)
        {
            using(StreamReader reader = new StreamReader(filePath))
            {

                action(reader.ReadToEnd());
            }
        }

        

        static void Write(string filePath,string v, Action action)
        {
            using(StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(v);
                action();
            }
        }

        static void UpData(string filePath, long v, Action<bool> action)
        {
            Read(filePath, (e) =>
            {
                long number;
                if (long.TryParse(e, out number))
                {
                    Write(filePath, (number + v).ToString(), () => { action(true); });
                }
                else
                {
                    action(false);
                }
            });
        }
    }
}

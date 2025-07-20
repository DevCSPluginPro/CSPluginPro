using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace PluginExample
{

    public class RoomMgr : Akequ.Base.Room
    {
       public static Rooms FindByName(string name)
       {
            return GameObjectMgr.Find<Rooms>(GameObjectMgr.GetAllRoom(), p => p.roomName == name);
            
       }

        public static void SpawnNewRoom()
        {

        }
    }
}

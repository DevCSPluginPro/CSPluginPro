using Akequ.Base;
using UnityEngine;

namespace PluginExample
{
    public class GameObjectMgr : Akequ.Base.Room
    {
        //单例
        private static GameObjectMgr _instance;
        public static GameObjectMgr instance { get {
                if (_instance == null) {
                    _instance = new GameObjectMgr();
                    return _instance;
                }
                return _instance;
            }
        }

        public static Player[] GetAllPlayer()
        {
            return GameObject.FindObjectsOfType<Player>();
        }

        public static Rooms[] GetAllRoom()
        {
            return GameObject.FindObjectsOfType<Rooms>();
        }

        public static Door[] GetAllDoor()
        {
            return GameObject.FindObjectsOfType<Door>();
        }


    }
}
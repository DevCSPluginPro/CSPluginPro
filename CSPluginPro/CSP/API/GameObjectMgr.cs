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

        public Player[] GetAllPlayer()
        {
            return GameObject.FindObjectsOfType<Player>();
        }

        public Rooms[] GetAllRoom()
        {
            return GameObject.FindObjectsOfType<Rooms>();
        }


    }
}
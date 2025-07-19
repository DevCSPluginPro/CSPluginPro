using Akequ.Base;
using UnityEngine;

namespace PluginExample
{
    public class GameObjectMgr : Akequ.Base.Room
    {
        static GameObjectMgr instance = new GameObjectMgr();

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
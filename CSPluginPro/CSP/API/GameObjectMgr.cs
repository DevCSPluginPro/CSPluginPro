using Akequ.Base;
using System;
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

        /// <summary>
        /// 在数组中查找满足条件的元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="array">要搜索的数组</param>
        /// <param name="match">判断条件委托，返回true表示找到匹配元素</param>
        /// <returns>找到的元素，未找到返回default(T)</returns>
        public static T Find<T>(T[] array, Predicate<T> match)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (match == null) throw new ArgumentNullException(nameof(match));

            foreach (T item in array)
            {
                if (match(item))
                {
                    return item;
                }
            }
            return default(T);
        }
    }
}
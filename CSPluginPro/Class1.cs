using Akequ.Plugins;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PluginExample
{ 
    public class Info : Akequ.Plugins.PluginInfo
    {
        public override string Name => "";

        public override string Id => "com.akequ.test";

        public override string Version => "0.0.1";

        public override ushort BundleVersion => (ushort)Random.Range(1, 255);

        public Info() { }
    }

    class AdminPanePlugin
    {
        public Type Type { get; set; }
        public string Name { get; set; }
    }

    class RoomPlugin
    {
        public Type Type { get; set; }
        public bool IsNetworked { get; set; }
    }

    class PluginLoader : PluginInitializator
    {
        // 需要生成的角色类
        private static Type[] units = new Type[] 
        { 
        
        
        
        
        };

        // 需要生成的房间或者网络附加类
        private static RoomPlugin[] rooms = new RoomPlugin[] 
        { 
        
        
        
        };

        //需要生成的管理员菜单类
        private static AdminPanePlugin[] adminPanes = new AdminPanePlugin[] 
        { 
        
        
        
        
        
        };

        //需要生成的物品类
        private static Type[] items = new Type[] 
        { 
        
        
        
        
        };

        public override void InitClient()
        {
            LoadItem(items);
            LoadUnit(units);
            LoadRoom(rooms);
            LoadAdminPane(adminPanes);

        }

        public override void InitServer()
        {
            LoadItem(items);
            LoadUnit(units);
            LoadRoom(rooms);
            LoadAdminPane(adminPanes);

            HookManager.Add("onMapGenerationComplete", (obj) =>
            {
                foreach (var item in rooms)
                {
                    if (item.IsNetworked) PluginAPI.RegisterRoomEvent(item.Type);
                }
            });

        }

        static void LoadUnit(Type[] types)
        {
            foreach (var item in types)
            {
                PluginAPI.RegisterPlayerClass(item, false);
            }
        }

        static void LoadItem(Type[] types)
        {
            foreach (var item in types)
            {

                PluginAPI.RegisterItem(item, false);
            }
        }

        static void LoadRoom(RoomPlugin[] types)
        {
            foreach (var item in types)
            {
                PluginAPI.RegisterRoomEvent(item.Type);
            }
        }

        static void LoadAdminPane(AdminPanePlugin[] types)
        {
            foreach (var item in types)
            {

                PluginAPI.RegisterAdminPanel(item.Type, item.Name);
            }
        }
    }
}

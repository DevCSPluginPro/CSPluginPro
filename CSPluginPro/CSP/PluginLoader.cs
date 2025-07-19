using Akequ.Plugins;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PluginExample
{
    class PluginLoader : PluginInitializator
    {
        private static Type[] units = new Type[] { };
        private static Type[] rooms = new Type[] { };
        private static Type[] others = new Type[] { };
        private static Type[] items = new Type[] { };

        public override void InitClient()
        {
            
            
        }

        public override void InitServer()
        {
            PluginAPI.RegisterRoomEvent(typeof(PluginLoader));

            HookManager.Add("onMapGenerationComplete", (obj) =>
            {
                PluginLoader map = new PluginLoader();
                PluginAPI.SpawnNetworkedEvent(map);
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

        static void LoadRoom(Type[] types)
        {
            foreach (var item in types)
            {
                PluginAPI.RegisterRoomEvent(item);
            }
        }

        static void LoadOAdminPane(Type[] types)
        {
            foreach (var item in types)
            {

                PluginAPI.RegisterAdminPanel(item, "permission");
            }
        }
    }
}

using Akequ.AdminPanel;
using Akequ.Base;
using Mirror;
using UnityEngine;

namespace PluginExample
{
    public class PlayerMgr : Akequ.Base.Room
    {
        static PlayerMgr instance = new PlayerMgr();

        public bool IsPluginClass(Player player)
        {
            if (player.playerClass.GetType() == typeof(PlayerClassProxy))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Hit(Player player, short damage, string reason = "", Player hiter = null)
        {
            if (netEvent.isServer)
            {
                DamageHandler damageHandler = new DamageHandler();
                damageHandler.damage = damage;
                damageHandler.deathReason = reason;
                if (hiter != null)
                {
                    damageHandler.killer = hiter;
                    damageHandler.killID = hiter.accountUID.ToString();
                }


                player.ChangeHealth(damageHandler);
            }
        }

        public void GiveItem<T>(Player player, string name = null, T item = null) where T : Item
        {
            if (item != null)
            {
                player.AddToInventory(item);
                return;
            }

            if (name != null)
            {
                player.GiveItem(name);
                return;
            }

        }

        public void GiveAmmo(Player player)
        {

        }

        public string GetTeam(Player player)
        {
            if (player.playerClass.GetType() == typeof(PlayerClassProxy))
            {
                PlayerClassProxy proxy = player.playerClass as PlayerClassProxy;
                return proxy.GetTeamID();
            }
            else
            {
                return player.playerClass.GetTeamID();
            }

        }

        public string GetName(Player player)
        {
            if (player.playerClass.GetType() == typeof(PlayerClassProxy))
            {
                PlayerClassProxy proxy = player.playerClass as PlayerClassProxy;
                return proxy.GetName();
            }
            else
            {
                return player.playerClass.GetName();
            }
        }
    }
}
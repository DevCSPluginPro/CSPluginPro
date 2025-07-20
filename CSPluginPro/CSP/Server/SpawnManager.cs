using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace 好好插件人物集
{

    /**
     * Doc 教程文档 
     * 
     * SpawnManager a = new SpawnManager()
     * a.Add(new UnitInfo("","",""))
     * a.SpawnAtSupport("","ClassD")
     * a.SpawnSupport(200, 0, new string[]{"指挥官","士官","士官","列兵","列兵","列兵", "列兵"})
     * 
     * 
     * 
     */

    public class SpawnManager : Akequ.Base.Room
    {
        GameObject node = new GameObject();
        private List<UnitInfo> units = new List<UnitInfo>();

        static SpawnManager instance = new SpawnManager();
        public override void Init()
        {
            if (netEvent.isServer)
            {
                this.SpawnAtHook("onRoundStart");
                //添加后续逻辑
            }
            else
            {
                //添加后续逻辑
            }
        }

        /// <summary>
        /// 生成一次团队支援
        /// </summary>
        /// <param name="time"> 生成的时间</param>
        /// <param name="joinWay"> 生成的方式 0 为 直升机 1 为 汽车</param>
        /// <param name="units">从左到右，生成的数量越少 例子：指挥官，士官，列兵</param>
        /// <param name="music">生成时播放的音乐链接(可选)</param>
        public  void SpawnSupport(int time, int joinWay, string[] units, string music = null)
        {
            Invoke(() =>
            {
                if (joinWay == 0)
                {
                    HookManager.Run("onSupportRequest", new object[] { "MTF" });
                }
                else
                {
                    HookManager.Run("onSupportRequest", new object[] { "CI" });
                }

                SendToEveryone("SpawnSupport_Client", new object[] { music });
                Invoke(() =>
                {
                    List<Player> pls = new List<Player>();
                    foreach (var item in GameObject.FindObjectsOfType<Player>())
                    {
                        if (item.playerClass.GetType() != typeof(PlayerClassProxy))
                        {
                            if (item.playerClass.GetTeamID() == "Spectator")
                            {
                                pls.Add(item);
                            }
                        }
                    }
                    SpawnStepwise(pls, units.ToList());
                }, 3);
            }, time);
        }

        public void SpawnSupport_Client(object[] objects)
        {
            AudioSource source;
            string url = (string)objects[0];
            source = this.netEvent.gameObject.AddComponent<AudioSource>();
            source.volume = 2f;
            ScriptHelper.DownloadClip(url, AudioType.MPEG, (e) =>
            {
                source.clip = e;
                Invoke(() =>
                {
                    source.Play();
                }, 3);
            });
        }

        /// <summary>
        /// 梯度角色生成算法
        /// </summary>
        /// <param name="pls"> 生成的玩家列表</param>
        /// <param name="units">生成的</param>
        public void SpawnStepwise(List<Player> pls, List<string> units)
        {

            if (pls.Count < units.Count)
            {
                for (int i = 0; i < pls.Count; i++)
                {
                    pls[i].SetClass(units[i]);
                }

                return;
            }

            bool run = true;
            while (run)
            {
                foreach (var str in units)
                {

                    if (pls.Count == 0)
                    {
                        run = false;
                        break;
                    }

                    Player pl = pls[UnityEngine.Random.Range(0, pls.Count)];
                    pl.SetClass(str);
                    pls.Remove(pl);
                }
            }

            return;

        }

        /// <summary>
        /// 在支援中生成
        /// </summary>
        /// <param name="name">需要生成的角色类名</param>
        /// <param name="team">在什么团队支援下生成</param>
        public void SpawnAtSupport(string name, string team)
        {
            HookManager.Add(node, "onSupportRequest", (e) =>
            {
                Invoke(() =>
                {
                    List<Player> pls = new List<Player>();
                    foreach (var item in GameObject.FindObjectsOfType<Player>())
                    {
                        if (item.playerClass.GetType() != typeof(PlayerClassProxy))
                        {
                            if (item.playerClass.GetTeamID() == "Spectator")
                            {
                                pls.Add(item);
                            }
                        }
                    }
                    if ((string)e[0] == team) pls[UnityEngine.Random.Range(0, pls.Count)].SetClass(name);
                }, 4);
            });
        }

        /// <summary>
        /// 从玩家当中随机抽取一名角色生成
        /// </summary>
        /// <param name="pls">玩家列表</param>
        /// <param name="name">需要生成的类名</param>
        /// <returns></returns>
        public List<Player> RangeSpawn(List<Player> pls, string name)
        {
            if (netEvent.isClient)
            {
                Debug.Log("[SpawnManager]请不要在客户端产生角色");
                return null;
            }

            int number = UnityEngine.Random.Range(0, pls.Count);
            pls[number].SetClass(name);
            pls.RemoveAt(number);
            return pls;
        }

        /// <summary>
        /// 添加一个需要在某个钩子上生成的角色
        /// </summary>
        /// <param name="name">钩子名称</param>
        public void SpawnAtHook(string name)
        {
            HookManager.Add(node, name, (ew) =>
            {
                List<Player> ClassDs = new List<Player>();
                List<Player> SCPs = new List<Player>();
                List<Player> MTFs = new List<Player>();
                foreach (var pl in GameObject.FindObjectsOfType<Player>())
                {
                    //排除插件角色
                    if (pl.playerClass.GetType() != typeof(PlayerClassProxy))
                    {
                        if (pl.playerClass.GetTeamID() == "SCP") ClassDs.Add(pl);
                        if (pl.playerClass.GetTeamID() == "MTF") ClassDs.Add(pl);
                        if (pl.playerClass.GetTeamID() == "ClassD") ClassDs.Add(pl);
                    }
                }

                List<UnitInfo> startUnits = new List<UnitInfo>();
                units.ForEach(unit =>
                {
                    if (unit.Event == name)
                    {
                        startUnits.Add(unit);
                    }
                });



                startUnits.ForEach(e =>
                {
                    if (e.Team == "SCP")
                    {
                        SCPs = this.RangeSpawn(SCPs, e.Name);
                    }

                    if (e.Team == "ClassD")
                    {
                        ClassDs = this.RangeSpawn(SCPs, e.Name);
                    }

                    if (e.Team == "MTF")
                    {
                        MTFs = this.RangeSpawn(SCPs, e.Name);
                    }
                });

            });
        }


        /// <summary>
        /// 添加一个需要生成的角色
        /// </summary>
        /// <param name="unit"> 角色信息 </param>
        public void Add(UnitInfo unit)
        {
            this.units.Add(unit);
        }
    }

    public class UnitInfo
    {
        // 角色的类名
        public string Name { get; set; }
        // 角色的团队类型
        public string Team { get; set; }
        // 角色生成的钩子
        public string Event { get; set; }

        public UnitInfo(string name, string Team, string e)
        {
            this.Name = name;
            this.Team = Team;
            this.Event = e;
        }
    }

}

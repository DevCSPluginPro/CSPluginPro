using UnityEngine;

namespace PluginExample
{

    public class DoorMgr : Akequ.Base.Room
    {
        public override void Init()
        {
            if (netEvent.isClient)
            {
                SendToServer("GetFromClient", "Test");
            }
        }

        public static void OpenAllDoor()
        {
            foreach (var item in GameObjectMgr.GetAllDoor())
            {
                if (!item.opened) item.ChangeState();
            }
        }

        public static void BreakAllDoor()
        {
            foreach (var item in GameObjectMgr.GetAllDoor())
            {
                item.BreakDoor(item.gameObject.transform.position);
            }
        }

        public static Vector3 GetPos(Door door)
        {
            return door.transform.position;
        }

    }
}

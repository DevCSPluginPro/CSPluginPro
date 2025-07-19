using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace PluginExample
{
    public class UIMgr : Akequ.Base.Room
    {
        public static GameObject CreatText(string text2, Vector3 pos, Vector3 size)
        {
            GameObject gameObject = UIManager.SpawnText();
            gameObject.transform.parent = GameObject.Find("PlayerCanvas").transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            RectTransform component6 = gameObject.GetComponent<RectTransform>();
            component6.anchoredPosition = pos;
            component6.sizeDelta = size;
            Text text = gameObject.GetComponentInChildren<Text>();
            text.color = Color.white;
            text.text = text2;
            text.fontSize = 12;
            text.raycastTarget = false;
            return gameObject;
        }

        public static GameObject CreatImage(Vector3 pos, Vector3 size)
        {
            GameObject gameObject = UIManager.SpawnImage();
            gameObject.transform.parent = GameObject.Find("PlayerCanvas").transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            RectTransform component6 = gameObject.GetComponent<RectTransform>();
            component6.anchoredPosition = pos;
            component6.sizeDelta = size;
            return gameObject;
        }

        public static void DrawImage(bool isLocal = false, string name = "")
        {
            Sprite sprite = ResourcesManager.GetSprite(name);
            if (isLocal)
            {
                GameObject gm = CreatImage(new Vector3(0,0),new Vector3(50,50));
                gm.GetComponent<Image>().sprite = sprite;
            }
        }


        static public void ChangeText(GameObject gameObject, string text)
        {
            Text text1 = null;
            if (gameObject.TryGetComponent<Text>(out text1))
            {
                text1.text = text;
            }
        }

        public static T CreatComponent<T>(GameObject gameObject) where T : Component
        {
          
            if (!GetComponent<T>(gameObject))
            {
                return gameObject.AddComponent<T>();
            }
            else
            {
                return GetComponent<T>(gameObject);
            }
        }

        public static T GetComponent<T>(GameObject gameObject) where T : Component
        {
            T component = null;
            if (gameObject.TryGetComponent<T>(out component))
            {
                return component;
            }
            else
            {
                return null;
            }

        }

        public static T GetComponentInChildren<T>(GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponentInChildren<T>())
            {
                return gameObject.GetComponentInChildren<T>();
            }
            else
            {
                return null;
            }

        }

static Transform GetPlayerCanvasParent()
        {
            if (netEvent.isClient)
            {
                Transform transform = GameObject.Find("PlayerCanvas").transform;
                return transform;
            }
            return null;
        }
    }
}
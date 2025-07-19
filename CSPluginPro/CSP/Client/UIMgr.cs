using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace PluginExample
{
    public class UIMgr : Akequ.Base.Room
    {
        public GameObject CreatText(string text2, Vector3 pos, Vector3 size)
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

        public GameObject CreatImage(Vector3 pos, Vector3 size)
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

        public void DrawImage(bool isLocal = false, string name = "")
        {
            ResourcesManager.GetSprite(name);
        }


        static public void ChangeText(GameObject gameObject, string text)
        {
            Text text1 = null;
            if (gameObject.TryGetComponent<Text>(out text1))
            {
                text1.text = text;
            }
        }

        public T CreatComponent<T>(GameObject gameObject) where T : Component
        {
          
            if (!this.GetComponent<T>(gameObject))
            {
                return gameObject.AddComponent<T>();
            }
            else
            {
                return this.GetComponent<T>(gameObject);
            }
        }

        public T GetComponent<T>(GameObject gameObject) where T : Component
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

        public T GetComponentInChildren<T>(GameObject gameObject) where T : Component
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
            return GameObject.Find("PlayerCanvas").transform;
        }
    }
}
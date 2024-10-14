using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RickAndMemory.Utility
{
    public static class SpriteLoader
    {
        private static Dictionary<string, Sprite> spriteMap = new Dictionary<string, Sprite>();

        public static IEnumerator LoadSprite(string url, Action<Sprite> callback) 
        {
            UnityWebRequest lRequest = UnityWebRequestTexture.GetTexture(url);
            yield return lRequest.SendWebRequest();

            if (lRequest.result == UnityWebRequest.Result.ConnectionError
                || lRequest.result == UnityWebRequest.Result.ProtocolError
                || lRequest.result == UnityWebRequest.Result.DataProcessingError)
                Debug.Log("Error: " + lRequest.error);
            else
            {
                Texture2D lTexture = new Texture2D(2, 2);
                lTexture.LoadImage(lRequest.downloadHandler.data);
                Sprite sprite = Sprite.Create(lTexture, new Rect(0.0f, 0.0f, lTexture.width, lTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                AddToMap(url, sprite);
                callback?.Invoke(sprite);
            }
        }

        private static void AddToMap(string url, Sprite sprite) 
        {
            if (spriteMap.ContainsKey(url) is false)
            {
                spriteMap.Add(url, sprite);
                return;
            }

            spriteMap[url] = sprite;
        }
    }
}

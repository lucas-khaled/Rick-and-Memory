using RickAndMemory.Data;
using RickAndMemory.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace RickAndMemory
{
    public class RickAndMortyProvider : ICardInfoProvider
    {
        private const string GET_ALL_CHARACTERS_URL = "https://rickandmortyapi.com/api/character";
        public async Task<CardInfo[]> GetCards(int amount)
        {
            AllCharactersData allCharacters = await GetAllCharactersInfo();
            List<int> charactersIndexList = GenerateCharactersIndexList(allCharacters.info.count, amount);

            AllCharactersData multipleCharacters = await GetMultipleCharacters(charactersIndexList);
            return GetCardsInfo(multipleCharacters);
        }

        private async Task<AllCharactersData> GetMultipleCharacters(List<int> charactersIndexList)
        {
            string url = GenerateURL(charactersIndexList);

            using (var http = UnityWebRequest.Get(url))
            {
                http.SendWebRequest();

                while (!http.isDone)
                {
                    await Task.Delay(100);
                }

                if (http.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Not able to connect to {url}\n {http.error}");
                    return null;
                }

                string json = http.downloadHandler.text;

                Debug.Log($"<color=green> Recieved from {url}:  {json}</color>");
                json = "{\"results\":" + json + "}";
                AllCharactersData charactersData = JsonUtility.FromJson<AllCharactersData>(json);

                return charactersData;
            }
        }

        private string GenerateURL(List<int> charactersIndexList)
        {
            string url = GET_ALL_CHARACTERS_URL + "/";

            for(int i = 0; i < charactersIndexList.Count - 1; i++) 
            {
                url += charactersIndexList[i] + ",";
            }

            url += charactersIndexList[charactersIndexList.Count - 1];

            return url;
        }

        private List<int> GenerateCharactersIndexList(int count, int amount)
        {
            List<int> indexes = new List<int>();
            for(int i = 0; i<amount; i++) 
            {
                indexes.Add(GenerateNewIndex(indexes, count));
            }

            return indexes;
        }

        private int GenerateNewIndex(List<int> list, int count) 
        {
            int index = UnityEngine.Random.Range(0, count);
            return list.Contains(index) ? GenerateNewIndex(list, count) : index;
        }

        private async Task<AllCharactersData> GetAllCharactersInfo() 
        {
            using (var http = UnityWebRequest.Get(GET_ALL_CHARACTERS_URL))
            {
                http.SendWebRequest();

                while (!http.isDone)
                {
                    await Task.Delay(100);
                }

                if (http.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Not able to connect to {GET_ALL_CHARACTERS_URL}\n {http.error}");
                    return null;
                }

                string json = http.downloadHandler.text;

                Debug.Log($"<color=green> Recieved from {GET_ALL_CHARACTERS_URL}:  {json}</color>");
                AllCharactersData charactersData = JsonUtility.FromJson<AllCharactersData>(json);

                return charactersData;
            }
        }

        private CardInfo[] GetCardsInfo(AllCharactersData charactersData) 
        {
            CardInfo[] infos = new CardInfo[charactersData.results.Length];
            for (int i = 0; i<infos.Length; i++) 
            {
                infos[i] = ConvertIntoCardInfo(charactersData.results[i]);
            }

            return infos;
        }

        private CardInfo ConvertIntoCardInfo(CharacterData data) 
        {
            return new CardInfo()
            {
                id = data.id,
                name = data.name,
                cardURL = data.image
            };
        }

        

        [Serializable]
        private class AllCharactersData 
        {
            public AllCharactersInfo info;
            public CharacterData[] results;
        }

        [Serializable]
        private class AllCharactersInfo 
        {
            public int count;
        }

        [Serializable]
        private class CharacterData 
        {
            public int id;
            public string name;
            public string image;
        }
    }
}

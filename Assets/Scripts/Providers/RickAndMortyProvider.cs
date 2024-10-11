using RickAndMemory.Data;
using RickAndMemory.Providers;
using System;
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
            using (var http = UnityWebRequest.Get(GET_ALL_CHARACTERS_URL)) 
            {
                http.SendWebRequest();

                while (!http.isDone) 
                {
                    await Task.Delay(100);
                }

                if(http.result != UnityWebRequest.Result.Success) 
                {
                    Debug.LogError($"Not able to connect to {GET_ALL_CHARACTERS_URL}\n {http.error}");
                    return null;
                }

                string json = http.downloadHandler.text;

                Debug.Log($"<color=green> Recieved from {GET_ALL_CHARACTERS_URL}:  {json}</color>");

                return GetCardsInfo(json);
            }
        }

        private CardInfo[] GetCardsInfo(string json) 
        {
            AllCharactersData charactersData = JsonUtility.FromJson<AllCharactersData>(json);
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
            public CharacterData[] results;
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

using RickAndMemory.Data;
using RickAndMemory.UI;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace RickAndMemory.Core
{
    public static class SavingManager
    {
        private const string SAVE_KEY = "Save";
        private static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static void Save(SaveInfo saveInfo) 
        {
            string saveJSON = JsonConvert.SerializeObject(saveInfo, settings);
            PlayerPrefs.SetString(SAVE_KEY, saveJSON);
        }

        public static SaveInfo Load() 
        {
            if (!PlayerPrefs.HasKey(SAVE_KEY)) 
                return null;

            string saveJSON = PlayerPrefs.GetString(SAVE_KEY);
            SaveInfo saveInfo = JsonConvert.DeserializeObject<SaveInfo>(saveJSON, settings);
            return saveInfo;
        }

        public static void ClearSave() 
        {
            PlayerPrefs.DeleteKey(SAVE_KEY);
        }
    }

    [Serializable]
    public class SaveInfo 
    {
        public string mode;
        public Layout layout;
        public List<CardInfo> cardsInfo;
        public ModeInfo modeInfo;
    }

    [Serializable]
    public class ModeInfo 
    {
        public int score;
        public int errors;
        public int streak;
    }
}

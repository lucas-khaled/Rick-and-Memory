using RickAndMemory.Data;
using RickAndMemory.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory.Core
{
    public static class SavingManager
    {
        private const string SAVE_KEY = "Save";
        public static void Save(SaveInfo saveInfo) 
        {
            string saveJSON = JsonUtility.ToJson(saveInfo);
            PlayerPrefs.SetString(SAVE_KEY, saveJSON);
        }

        public static SaveInfo Load() 
        {
            if (!PlayerPrefs.HasKey(SAVE_KEY)) 
                return null;

            string saveJSON = PlayerPrefs.GetString(SAVE_KEY);
            SaveInfo saveInfo = JsonUtility.FromJson<SaveInfo>(saveJSON);
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
        public int score;
        public int errors;
    }
}

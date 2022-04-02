using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        const string LAST_SCENE_BUILD_INDEX = "lastSceneBuildIndex";
        
        public IEnumerator LoadLastScene(string filename)
        {
            Dictionary<string, object> dictionary = LoadFile(filename);
            int sceneIndex = 0;

            if (dictionary.ContainsKey(LAST_SCENE_BUILD_INDEX)) {
                sceneIndex = (int)dictionary[LAST_SCENE_BUILD_INDEX];

                if (sceneIndex != SceneManager.GetActiveScene().buildIndex) {

                    yield return SceneManager.LoadSceneAsync(sceneIndex);
                }

                RestoreAllStates(dictionary);
            }
            else {
                //print("No build index saved!");

            }
        }

  
        public void Save(string filename) 
        { 
            print("Saving to " + filename + ".");

            Dictionary<string, object> state = LoadFile(filename);
            CaptureAllStates(state);


            SaveFile(filename, state);
        }

        private void SaveFile(string filename, object state)
        {
            string path = GetPathFromSaveFile(filename);
            using (FileStream fileStream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(fileStream, state);
            }
        }

        
        public void Load(string filename)
        {           
            print("Loading from " + filename + ".");

            RestoreAllStates(LoadFile(filename));
        }

        private Dictionary<string, object> LoadFile(string filename)
        {
            string path = GetPathFromSaveFile(filename);

            if (!File.Exists(path)) return new Dictionary<string, object>(); // if there was no file
            
            // if there is a file, load normally
            using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
            {
                if (fileStream.Length == 0) return new Dictionary<string, object>(); // if file exists but is empty

                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(fileStream);
            }
        }

        private void RestoreAllStates(Dictionary<string, object> loadingState)
        {
            Dictionary<string, object> dictionary = loadingState;
            SaveableEntity[] entityList = FindObjectsOfType<SaveableEntity>();

            foreach(SaveableEntity entity in entityList) {
                string uniqueId = entity.GetUniqueIdentifier();

                if (dictionary.ContainsKey(uniqueId)) {
                    //print("About to load: " + uniqueId);
                    entity.RestoreState(dictionary[uniqueId]);
                }
            }
        }


        private void CaptureAllStates(Dictionary<string, object> dictionary)
        {
            SaveableEntity[] entityList = FindObjectsOfType<SaveableEntity>();

            foreach (SaveableEntity entity in entityList)
            {
                dictionary[entity.GetUniqueIdentifier()] = entity.CaptureState();
            }

            dictionary[LAST_SCENE_BUILD_INDEX] = SceneManager.GetActiveScene().buildIndex;
        }

        private string GetPathFromSaveFile(string filename)
        {

            //return Path.Combine(Application.persistentDataPath, filename + ".sav");
            return Path.Combine("C:/Unity Projects/Combat RPG Project", filename + ".sav");
        }

        public void DeleteSaveFile(string filename)
        {
            string path = GetPathFromSaveFile(filename);

            if (!File.Exists(path)) return; // if there was no file

            File.Delete(path); // if there is a file, kill it
        }
    }
}

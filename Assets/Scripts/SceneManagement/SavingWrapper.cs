using System.Collections;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        SavingSystem savingSystem;

        //private void Awake()
        //{
        //    StartCoroutine(LoadLastScene());
        //}

        private IEnumerator Start()
        {
            savingSystem = GetComponent<SavingSystem>();
            ScreenFader fader = GameObject.FindObjectOfType<ScreenFader>();

            fader.BlackOut();
            yield return savingSystem.LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(1.0f);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.Delete)) {
                savingSystem.DeleteSaveFile(defaultSaveFile);
                Debug.Log("Deleted " + defaultSaveFile + ".sav!");
            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
            }
        }

        public void Save() 
        {
            savingSystem.Save(defaultSaveFile);
        }

        public void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }
    }
}
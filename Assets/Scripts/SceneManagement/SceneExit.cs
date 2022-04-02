using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class SceneExit : MonoBehaviour
    {
        [SerializeField] int sceneIndex = -1;
        [SerializeField] int entranceIndex;
        [SerializeField] float fadeOutTime = 1.0f;
        [SerializeField] float fadeInTime = 1.0f;
        [SerializeField] float fadeWaitTime = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Terrain") return; // delete this

            //print("***" + other.tag + "***");
            if (other.tag == "Player")
            {
                StartCoroutine(LoadSceneAsync());
                
            }
            
        }

        IEnumerator LoadSceneAsync()
        {      
            DontDestroyOnLoad(gameObject);
            ScreenFader screenFader = GameObject.FindObjectOfType<ScreenFader>();
            SavingWrapper saverLoader = GameObject.FindObjectOfType<SavingWrapper>();

            saverLoader.Save();

            yield return screenFader.FadeOut(fadeOutTime);

            if (sceneIndex != SceneManager.GetActiveScene().buildIndex) {
                yield return SceneManager.LoadSceneAsync(sceneIndex);
                print("Scene Loaded");
            }
            else {
                print("Scene load unnecessary.");
            }

            saverLoader.Load();

            GameObject player = GameObject.FindWithTag("Player");
            GameObject entrances = GameObject.Find("EntranceList");
            Transform spawnPoint;

            if (entranceIndex < entrances.transform.childCount)
            {
                spawnPoint = entrances.transform.GetChild(entranceIndex);
            }
            else
            {
                spawnPoint = new GameObject().transform; // this might not work
                print("Entrance index out of bounds");
            }

            UpdatePlayer(player, spawnPoint.position, spawnPoint.rotation);

            saverLoader.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return screenFader.FadeIn(fadeInTime);
            Destroy(gameObject);
        }

        void UpdatePlayer(GameObject player, Vector3 pos, Quaternion rot)
        {
            if (player) {
                //print("Player exists!");
                NavMeshAgent navMeshAgent = player.GetComponent<NavMeshAgent>();
                if (navMeshAgent) {
                    navMeshAgent.enabled = false;
                    player.transform.SetPositionAndRotation(pos, rot);
                    navMeshAgent.enabled = true;
                    //print(pos);
                }
            }
            else {
                print("No player");
            }
        }
    }
}

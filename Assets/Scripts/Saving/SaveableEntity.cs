using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        static Dictionary<string, SaveableEntity> s_idList = new Dictionary<string, SaveableEntity>();
        [SerializeField] string uniqueIdentifier = "";// System.Guid.NewGuid().ToString();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            ISaveable[] stateComponents = GetComponents<ISaveable>();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            foreach(ISaveable stateComponent in stateComponents) {
                //dictionary[stateComponent.GetType().Name] = stateComponent.CaptureState();
                dictionary[stateComponent.GetType().ToString()] = stateComponent.CaptureState();
            }

            return dictionary;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> dictionary = (Dictionary<string, object>)state;

            foreach (ISaveable saveable in GetComponents<ISaveable>()) {
                string typeString = saveable.GetType().ToString();

                if (dictionary.ContainsKey(typeString)) {
                    saveable.RestoreState(dictionary[typeString]);
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            s_idList[property.stringValue] = this;

            //if (!Application.IsPlaying(gameObject) )
            //    print("Editing");
        }

        private bool IsUnique(string id)
        {
            // if the list has this ID and it doesn't point to this object, then it's not unique!
            //if (s_idList.ContainsKey(id) && s_idList[id] != this)
            //    return false;
            //else
            //    return true;

            //return !(s_idList.ContainsKey(id) && s_idList[id] != this);

            if (!s_idList.ContainsKey(id)) return true;

            if (s_idList[id] == this) return true;

            if (s_idList[id] == null) { // if it's been deleted, remove entry from dictionary and declare the id unique
                s_idList.Remove(id);
                return true;
            }

            if (s_idList[id].GetUniqueIdentifier() != id) { // this shouldn't happen
                s_idList.Remove(id);
                return true;
            }

            return false;
        }
#endif
    }
}
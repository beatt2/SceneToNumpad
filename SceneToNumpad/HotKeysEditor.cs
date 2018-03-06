using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Plugins.SceneToNumpad
{
    [InitializeOnLoad]
    public class HotKeysEditor : MonoBehaviour
    {
        #region Entrys




        private static bool _groupEnabled;
        private static bool _specialKeyUsed;
        private static int _contextCalc;
        private static int _indexOne;
        private static int _indexThree;
        private static string[] _sceneName = new string[9];
        private static int [] _buildIndex = new int [9];


        public static string[] Entrys = new[]
        {
            "NumPadOne",
            "NumPadTwo",
            "NumPadThree",
            "NumPadFour",
            "NumPadFive",
            "NumPadSix",
            "NumPadSeven",
            "NumpadEight",
            "NumpadNine",
        };

        #endregion

        private static bool[] used = new bool[9];
        private static int _lastScene;
        private static int _modifierCount;

        static HotKeysEditor()
        {
            NumbersChanged();

            System.Reflection.FieldInfo info = typeof(EditorApplication).GetField("globalEventHandler",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);


            EditorApplication.CallbackFunction value = (EditorApplication.CallbackFunction) info.GetValue(null);
            value += EditorGlobalKeyPress;
            info.SetValue(null, value);

        }


        public static void NumbersChanged()
        {
            if (PlayerPrefs.HasKey("group" + HotKeySaver.UNIQUEID))
            {
                _groupEnabled = Convert.ToBoolean(PlayerPrefs.GetInt("group" + HotKeySaver.UNIQUEID));
            }
            else
            {
                _groupEnabled = true;
                PlayerPrefs.SetInt("group" + HotKeySaver.UNIQUEID, Convert.ToInt32(_groupEnabled));
            }

            _specialKeyUsed = Convert.ToBoolean(PlayerPrefs.GetInt("specialKeyUsed" + HotKeySaver.UNIQUEID));
            _contextCalc = PlayerPrefs.GetInt("contextCalc" + HotKeySaver.UNIQUEID);
            _indexOne = PlayerPrefs.GetInt("indexOne" + HotKeySaver.UNIQUEID);

            for (int i = 0; i < Entrys.Length; i++)
            {
                _sceneName[i] = PlayerPrefs.GetString(Entrys[i] +HotKeySaver.UNIQUEID);
                _buildIndex[i] = PlayerPrefs.GetInt(Entrys[i]+ "build" + HotKeySaver.UNIQUEID);
            }
        }


        static void EditorGlobalKeyPress()
        {
            if (EditorGUIUtility.editingTextField || Event.current.type == EventType.keyDown) return;
            int sceneToLoad = -1;
            switch (Event.current.keyCode)
            {
                case KeyCode.Keypad1:
                    sceneToLoad = 0;
                    break;
                case KeyCode.Keypad2:
                    sceneToLoad = 1;
                    break;
                case KeyCode.Keypad3:
                    sceneToLoad = 2;
                    break;
                case KeyCode.Keypad4:
                    sceneToLoad = 3;
                    break;
                case KeyCode.Keypad5:
                    sceneToLoad = 4;
                    break;
                case KeyCode.Keypad6:
                    sceneToLoad = 5;
                    break;
                case KeyCode.Keypad7:
                    sceneToLoad = 6;
                    break;
                case KeyCode.Keypad8:
                    sceneToLoad = 7;
                    break;
                case KeyCode.Keypad9:
                    sceneToLoad = 8;
                    break;
            }
            if (sceneToLoad == -1) return;
            if (_sceneName[sceneToLoad] == "null")
            {
                Debug.Log("Scene not found in build index");
                return;
            }
            if (!CheckValue) return;
            if (_groupEnabled)
            {
                if (EditorBuildSettings.scenes[sceneToLoad] != null && _lastScene != sceneToLoad)
                {
                    Debug.Log("LOADING");
                    EditorSceneManager.OpenScene(EditorBuildSettings.scenes[sceneToLoad].path,
                        OpenSceneMode.Single);
                    Debug.Log(String.Format("Scene loaded {0}", _sceneName[sceneToLoad]));
                    _lastScene = sceneToLoad;
                }
                else if (_lastScene != sceneToLoad)
                {
                    Debug.Log("Scene not found in build index");
                }
            }
            else
            {
                if (_buildIndex[sceneToLoad] != -1  && _lastScene != sceneToLoad)
                {
                    int value = _buildIndex[sceneToLoad];
                    Debug.Log(value);
                    Debug.Log("LOADING SCENE");
                    EditorSceneManager.OpenScene(EditorBuildSettings.scenes[value].path, OpenSceneMode.Single);
                    Debug.Log(String.Format("Scene loaded {0}", _sceneName[sceneToLoad]));
                    _lastScene = sceneToLoad;
                }
                else if (_lastScene != sceneToLoad)
                {
                    Debug.Log("Scene not found in build index");
                }
            }
            for (int i = 0; i < used.Length; i++)
            {
                used[i] = i == sceneToLoad;
            }
        }

        //TODO RFI
        private static bool CheckValue
        {
            get
            {
                if (!_specialKeyUsed)
                {
                    return true;
                }
                if (_contextCalc < 1)
                {
                    if (_indexOne == 1)
                    {
                        if (Event.current.alt)
                        {
                            return true;
                        }
                    }
                    else if (Event.current.control)
                    {
                        return true;
                    }
                }
                if (_contextCalc < 1) return false;
                if (Event.current.control)
                {
                    if (Event.current.alt)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}

using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Plugins.SceneToNumpad
{

    public class HotKeySaver
    {
        #region Entrys
        public static string[] Entrys =
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

        public static readonly  string UNIQUEID = "SHOARMAVLEEEEEEEEEEEEEEES";
        private Object[] _localSceneObject;
        public Object[]  SceneObjects
        {
            get
            { 
                _localSceneObject = new Object[9];


                for (int i = 0; i < 9; i++)
                {

                        int buildIndex = PlayerPrefs.GetInt(Entrys[i]+ "build" + UNIQUEID);
                        if(buildIndex != -1)
                        _localSceneObject[i] =
                            AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[buildIndex].path);



                }

                return _localSceneObject;
            }
            set
            {
                _localSceneObject = value;

                for (int i = 0; i < 9; i++)
                {
                    if (_localSceneObject[i] != null)
                    {
                        for (int j = 0; j < EditorBuildSettings.scenes.Length; j++)
                        {

                            string tempString = EditorBuildSettings.scenes[j].path.Substring(
                                EditorBuildSettings.scenes[j].path.LastIndexOf("/") + 1,
                                EditorBuildSettings.scenes[j].path.IndexOf(".unity") - 1 -
                                EditorBuildSettings.scenes[j].path.LastIndexOf("/")

                            );
                            if (_localSceneObject[i].name == tempString)
                            {
                                int buildIndex = j;

                                PlayerPrefs.SetInt(Entrys[i]+ "build" + UNIQUEID, buildIndex);

                                PlayerPrefs.SetString(Entrys[i] + UNIQUEID, tempString);
                            }
                        }

                    }
                    else
                    {
                        //pro tip never ever ever call them the same name
                        PlayerPrefs.SetInt(Entrys[i] + "build" + UNIQUEID, -1);
                        PlayerPrefs.SetString(Entrys[i] + UNIQUEID, "null");
                    }

                }
                HotKeysEditor.NumbersChanged();

            }
        }
    }
}

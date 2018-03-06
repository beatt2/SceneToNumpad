using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugins.SceneToNumpad
{
    public class HotKeyWindow : EditorWindow
{
    private bool _groupEnabled;
    private bool _useSpecialKey;

    private HotKeySaver _hks = new HotKeySaver();
    private PopUpInfo _pui = new PopUpInfo();

    private Object[] _myObject = new Object[9];
    private Object[] _localMemory = new Object[9];
    private Object[] _autoMemory = new Object[9];
    private bool[] _sync = new bool[9];
    private bool myBool;



    private const string Clear = "Clear";
    private int[] _contentIndex = new int[3];
    private int _contextCalc;
    private string _currentIndexNumberString;



    private void Awake()
    {

        _contextCalc = 0;
        _contentIndex = new int[3];

        _groupEnabled = PlayerPrefs.HasKey("group" + HotKeySaver.UNIQUEID) && Convert.ToBoolean(PlayerPrefs.GetInt("group" + HotKeySaver.UNIQUEID));
        Debug.Log(_groupEnabled);
        LoadSpecialKey();
        LoadScenes();
        titleContent = new GUIContent("ScenePad");


    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("group"+ HotKeySaver.UNIQUEID,Convert.ToInt32(_groupEnabled));
        SaveSpecialKey();
        _hks.SceneObjects = _myObject;
    }

    private void LoadScenes()
    {
        for (int i = 0; i < EditorBuildSettings.scenes.Length && i < 9; i++)
        {
            _autoMemory[i] = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[i].path);

        }


        if (!_groupEnabled)
        {
            _myObject = _hks.SceneObjects;
        }
        else
        {


            _myObject = _autoMemory;

        }
        _localMemory = _myObject;
    }

    private void LoadSpecialKey()
    {
        _useSpecialKey = PlayerPrefs.HasKey("specialKeyUsed" + HotKeySaver.UNIQUEID) && Convert.ToBoolean(PlayerPrefs.GetInt("specialKeyUsed" + HotKeySaver.UNIQUEID));
        if (_useSpecialKey)
        {
            _contextCalc = PlayerPrefs.GetInt("contextCalc" + HotKeySaver.UNIQUEID);
            _contentIndex[0] = PlayerPrefs.GetInt("indexOne" + HotKeySaver.UNIQUEID);

        }
    }

    private void SaveSpecialKey()
    {

        if (_useSpecialKey)
        {
            PlayerPrefs.SetInt("indexOne" + HotKeySaver.UNIQUEID,_contentIndex[0]);
            Debug.Log(_contentIndex[0]);
            Debug.Log(_contentIndex[1]);

            if (_contextCalc > 0)
            {
                PlayerPrefs.SetInt("indexTwo" + HotKeySaver.UNIQUEID, _contentIndex[1]);
            }
            PlayerPrefs.SetInt("contextCalc" + HotKeySaver.UNIQUEID, _contextCalc);
        }
        PlayerPrefs.SetInt("specialKeyUsed" + HotKeySaver.UNIQUEID,Convert.ToInt32(_useSpecialKey));
        //TODO if hotkeywindow is active it doenst save new binds

    }


    [MenuItem("HotKey/Show Window")]
    public static void ShowWindow()
    {
       GetWindow(typeof(HotKeyWindow)).Show();




    }

    void OnGUI()
    {
        GUI.Label(new Rect(0,40,100,40),GUI.tooltip );
        GUILayout.Label("SceneHotkey", EditorStyles.boldLabel);


        //_modeIndex = EditorGUILayout.Popup("Mode", _modeIndex,_modeStrings, GUILayout.MaxWidth(250));
        _groupEnabled = EditorGUILayout.ToggleLeft("Auto", _groupEnabled, EditorStyles.boldLabel);
        GUILayout.Space(5);
        EditorGUI.BeginDisabledGroup(_groupEnabled);



        for (int i = 0; i < _myObject.Length ; i++)
        {
            _currentIndexNumberString = string.Format("Numpad {0}", i + 1);


            if (_groupEnabled)
            {
                _myObject[i] = _autoMemory[i];
                _myObject[i] = EditorGUILayout.ObjectField(_currentIndexNumberString, _autoMemory[i], typeof(SceneAsset), true, options: GUILayout.MinWidth(300));

            }
            else
            {
                _myObject[i] = EditorGUILayout.ObjectField(_currentIndexNumberString, _myObject[i], typeof(SceneAsset), true, options: GUILayout.MinWidth(300));
            }
//            else if (_sync[i])
//            {
//                _myObject[i] = _localMemory[i];
//                _sync[i] = false;
//            }
//            else
//            {
//                _localMemory[i] = _myObject[i];
//            }
        }
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(10);
        if (!_groupEnabled)
        {
            if (GUILayout.Button(Clear, GUILayout.Width(50)))
            {

                _myObject = new Object[9];

            }
        }

        GUILayout.Space(10);
        _useSpecialKey = EditorGUILayout.BeginToggleGroup("Use special key", _useSpecialKey);
        if (_useSpecialKey)
        {
            GUILayout.BeginHorizontal();
            _contentIndex[0] = EditorGUILayout.Popup(_contentIndex[0], _pui.ArrayOne, GUILayout.MaxWidth(100));
            if (_contextCalc  > 0)
            {
                _contentIndex[1] =
                    EditorGUILayout.Popup(_contentIndex[1], _pui.ArrayGetTwo(_contentIndex[0]), GUILayout.MaxWidth(100));


            }
            if (_contextCalc < 1)
            {
                if (GUILayout.Button("+", GUILayout.Width(25), GUILayout.MaxHeight(15)))
                {
                    _contextCalc += 1;
                }

            }
            if (_contextCalc > 0)
            {
                if (GUILayout.Button("-", GUILayout.Width(25), GUILayout.MaxHeight(15)))
                {
                    _contextCalc -= 1;
                }
            }

            GUILayout.EndHorizontal();
        }
    }

}

}


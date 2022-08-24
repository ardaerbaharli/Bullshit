#if  UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    [SerializeField] private string directoryName = "Screenshots";
    [SerializeField] private string filePrefix = "screenshot_";
    [SerializeField] private string fileType = ".png";
    [SerializeField] private KeyCode screenshotKey = KeyCode.F10;
    
    private static int _index;
    private static string _path;

    private static string _filePrefix;
    private static string _fileType;

    public static Screenshot Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _index = 0;
        _path = Path.Combine(Application.dataPath, directoryName);
        _filePrefix = filePrefix;
        _fileType = fileType;
        Directory.CreateDirectory(_path);
    }

    private void Update()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            TakeScreenshot();
        }
    }

    public static void TakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot(_path + "/" + _filePrefix + _index + _fileType);
        
        AssetDatabase.Refresh();
        _index++;
    }
}
#endif

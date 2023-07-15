using UnityEngine;
using System.IO;

public class LogToFile : MonoBehaviour
{
    private static LogToFile instance;
    private StreamWriter logWriter;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        string logFileName = "log.txt";
        string logFilePath = Path.Combine(Application.persistentDataPath, logFileName);

        logWriter = File.CreateText(logFilePath);

        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (logWriter != null)
        {
            string log = string.Format("[{0}] {1}: {2}", System.DateTime.Now, type, logString);
            logWriter.WriteLine(log);
            logWriter.WriteLine(stackTrace);
            logWriter.WriteLine("----------------------------------------");
        }
    }

    void OnDestroy()
    {
        if (logWriter != null)
        {
            logWriter.Close();
            logWriter = null;
        }
    }
}

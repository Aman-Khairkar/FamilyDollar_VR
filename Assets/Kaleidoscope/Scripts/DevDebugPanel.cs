using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DevDebugPanel : Singleton<DevDebugPanel>
{
    public Canvas debugAndInstructions;
    public TextMeshProUGUI debugPanel;

    private string output;
    private string stack;
    [SerializeField] private int maxLines = 15;
    // Start is called before the first frame update
    void Start()
    {
        debugPanel.text = "";
    }

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
    }
    
    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    private void Update()
    {
       
    }
    
    public void Log(string logs, string stackTrace, LogType type)
    {

        output = logs + "\n";
        stack = stackTrace;

        if (debugPanel.textInfo.lineCount > maxLines)
            debugPanel.text = output;
        else
            debugPanel.text += output;
    }
}

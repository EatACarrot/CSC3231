using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;
using System.Text;

public class GM : MonoBehaviour
{
    string _stats;
    ProfilerRecorder systemMemoryRecorder;

    int _frameCounter = 0;
    float _timeCounter = 0.0f;
    float _lastFramerate = 0.0f;

    [SerializeField]
    float refreshTime = 0.5f;

    // Start is called before the first frame update
    void OnEnable()
    {
        systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
    }

    void OnDisable()
    {
        systemMemoryRecorder.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        //Getting FPS and total memory being used
        var sb = new StringBuilder(500);
        if(_timeCounter < refreshTime)
        {
            _timeCounter += Time.deltaTime;
            _frameCounter++;
        }
        else
        {
            _lastFramerate = (float)_frameCounter / _timeCounter;
            _frameCounter = 0;
            _timeCounter = 0.0f;
        }
        sb.AppendLine($"System Memory: {systemMemoryRecorder.LastValue / (1024 * 1024)} MB");
        sb.AppendLine($"Fps : {_lastFramerate}");
        _stats = sb.ToString();
    }

    void OnGUI()
    {
        //displaying them on screen
        GUI.TextArea(new Rect(10, 30, 250, 70), _stats);
    }
}

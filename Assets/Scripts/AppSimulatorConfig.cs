using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using File = UnityEngine.Windows.File;

#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
#else
using System.Net.Http;
#endif

[System.Serializable]
public class SystemParams
{
    public string ServerIP = "0.0.0.0";
    public string DebugLogPort = "9999";
    public string ObjectTrackingPort = "12000";
	public string EyeTrackingPort = "13000";
    public float ConfidenceThreshold = 0.0f;
    public float MaxObjectRadius = 0.4f;
    public int GeometryCapacity = 60;

    // public int FilterWindowSize = 120;
    // public int FilterWindowMinCount = 5;
    // public float FilterMinDist = 0.2f;
}

[System.Serializable]
public class UIParams
{
	// How long till we delete unconfirmed objects. 0.0f for never delete.
    public float ConfirmTimeout = 0.0f; // Seconds.
    // How long to focus on object label before select.
	public float FocusConfirmTime = 1.0f; // Seconds.
    public float PositionUpdateRate = 60.0f; // Seconds.
    public string DeterminePoseMethod = "average"; // "average" or "median".
    public int MedianAverageWindow = 5;
}

public enum MultitaskingType
{
    ManualInSitu, Manual, InSitu, Automatic, Invalid
}

[System.Serializable]
public class ExperimentParams : ISerializationCallbackReceiver
{
    public bool SaveLogs = false;
    public MultitaskingType Multitasking;

	// INTERNAL IMPLEMENTATION. DO NOT USE.
	// Serializable types to convert to above member variables.
    public string _multitasking = "InSitu";
    
    public HashSet<string> AppObjects1 = new HashSet<string>();
    public List<string> _appObjects1 = new List<string>();

    public HashSet<string> AppObjects2 = new HashSet<string>();
    public List<string> _appObjects2 = new List<string>();

    public HashSet<string> AppObjects3 = new HashSet<string>();
    public List<string> _appObjects3 = new List<string>();

    public void OnBeforeSerialize() {
        _multitasking = Multitasking.ToString();

        _appObjects1 = new List<string>(AppObjects1);
        _appObjects2 = new List<string>(AppObjects2);
        _appObjects3 = new List<string>(AppObjects3);
    }

    public void OnAfterDeserialize() {
        Multitasking = (MultitaskingType)System.Enum.Parse(
            typeof(MultitaskingType), _multitasking);

        AppObjects1 = new HashSet<string>(_appObjects1);
        AppObjects2 = new HashSet<string>(_appObjects2);
        AppObjects3 = new HashSet<string>(_appObjects3);
    }
}

[System.Serializable]
public class AppSimulatorConfig
{
	public SystemParams System = new SystemParams();
	public UIParams UI = new UIParams();
	public ExperimentParams Experiment = new ExperimentParams();
}

// public class AppSimulatorConfig {
//     public ConfigParams Params = new ConfigParams();
//     public SystemParams System
//     {
//         get { return Params.System; }
//         private set {}
//     }
//     public UIParams UI
//     {
//         get { return Params.UI; }
//         private set {}
//     }
//     public ExperimentParams Experiment
//     {
//         get { return Params.Experiment; }
//         private set {}
//     }
// }
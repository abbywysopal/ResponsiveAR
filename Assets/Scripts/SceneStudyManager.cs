using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine.Events;
using Microsoft.MixedReality.Toolkit.Extensions;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft;


public static class Const
{
    public const int FRAME_RATE = 60;
    public const float TICK_RATE = 6f;
}

[System.Serializable]
public class RecordStudy
{
    public StudyObject obj;

}

//Gaze and Head pos frame
[System.Serializable]
public struct StudyFrame
{
    public long frameNum;
    public long timestamp;

    public Vector3 hPos;
    public Vector3 hDir;
    public Quaternion hRot;
    public Vector3 hAngl;

    public Vector3 gazeOrigin;
    public Vector3 gazeDirection;

    public Vector3 rightHandRay;
    public Vector3 leftHandRay;

    public List<ExperimentEventData> experimentEvents;

    public ResponsiveData responsiveData;
}


public struct HandRay
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public bool hitObject;
    public string hitObjectName;
}

//Session recording which is written to json
[System.Serializable]
public class SessionRecording
{
    public long numFrames;
    public string task;
    public long start_time;
    public long end_time;
    public long total_time;
    public long sessionNumber;
    public float tickRate = Const.TICK_RATE;
    public List<StudyFrame> frames;
    public bool isReponsive;

    public SessionRecording(float t = Const.TICK_RATE)
    {
        tickRate = t;
        frames = new List<StudyFrame>();
    }
}

//Json entry for a single user
[System.Serializable]
public class StudyObject
{
    public float tickRate = Const.TICK_RATE;
    public string userID = "abc";

    public List<SessionRecording> sessionRecordings;
}

public class SceneStudyManager : MonoBehaviour
{
    //objects to write to json
    public StudyObject obj;

    float logTimer = 0f;
    float studyTimer = 0f;
    public StudyFrame currentFrame;
    public SessionRecording currentSession;

    [SerializeField] private RecordStudy _RecordStudy = new RecordStudy();

    public long startTime;
    public string filename;

    public int sessionNumber = 0;
    public bool newSession = false;
    private bool recording = false;


    #region Public methods

    public void startRecording()
    {
        Debug.Log("StartRecording");
        recording = true;
    }

    public void stopRecording()
    {
        recording = false;
        SaveStudy();
        LogStudy();
    }

    public string getUserID()
    {
        return obj.userID;
    }

    public void SaveIntoJson()
    {
        filename = "/RecordStudy_Session_" + sessionNumber + "_" + startTime + ".json";
        if (sessionNumber == 0)
        {
            string data = JsonUtility.ToJson(_RecordStudy);
            System.IO.File.WriteAllText(Application.persistentDataPath + filename, data);
            Debug.Log("filename:" + Application.persistentDataPath + filename);
        }
        else
        {
            string data = JsonUtility.ToJson(_RecordStudy.obj.sessionRecordings[0]);
            System.IO.File.WriteAllText(Application.persistentDataPath + filename, data);
        }
    }

    //Record data (called every 10 ticks)
    public void SaveStudy()
    {
        currentFrame.frameNum++;
        currentFrame.timestamp = System.DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime;

        currentFrame.hPos = Camera.main.transform.position;
        currentFrame.hRot = Camera.main.transform.rotation;
        currentFrame.hDir = Camera.main.transform.forward;
        currentFrame.hAngl = Camera.main.transform.rotation.eulerAngles;

        currentFrame.gazeOrigin = CoreServices.InputSystem.EyeGazeProvider.GazeOrigin;
        currentFrame.gazeDirection = CoreServices.InputSystem.EyeGazeProvider.GazeDirection;

        currentFrame.rightHandRay = getRightHandRay();
        currentFrame.leftHandRay = getLeftHandRay();

        currentFrame.experimentEvents = UserStudyTask.GetExperimentEventData();
        currentFrame.responsiveData = ResponsiveDesign.GetResponsiveData();

        obj.sessionRecordings[0].frames.Add(currentFrame);
    }

    //Write other saved data too (every second)
    public void LogStudy()
    {
        obj.sessionRecordings[0].numFrames = obj.sessionRecordings[0].frames.Count;
        obj.sessionRecordings[0].sessionNumber = sessionNumber;
        _RecordStudy.obj = obj;
        SaveIntoJson();

        if (newSession)
        {
            sessionNumber++;
            obj.sessionRecordings.Clear();
            currentSession = new SessionRecording();
            obj.sessionRecordings.Add(currentSession);
            newSession = false;
        }

    }

    public void startNewSession(long st, long et, string desc, bool b, long t)
    {
        obj.sessionRecordings[0].start_time = st;
        obj.sessionRecordings[0].end_time = et;
        obj.sessionRecordings[0].task = desc;
        obj.sessionRecordings[0].isReponsive = b;
        obj.sessionRecordings[0].total_time = t;

        newSession = true;
        LogStudy();
    }

    public Vector3 getRightHandRay()
    {
        Vector3 rightEndPoint;
        if (PointerUtils.TryGetHandRayEndPoint(Handedness.Right, out rightEndPoint))
        {
            return rightEndPoint;
        }

        return new Vector3(0, 0, 0);
    }

    public Vector3 getLeftHandRay()
    {
        Vector3 leftEndPoint;
        if (PointerUtils.TryGetHandRayEndPoint(Handedness.Left, out leftEndPoint))
        {
            return leftEndPoint;
        }

        return new Vector3(0, 0, 0);
    }

    #endregion

    #region Unity methods

    GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("SceneStudy Start");
        parent = gameObject;
        Application.targetFrameRate = Const.FRAME_RATE;
        startTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        obj = new StudyObject();

        obj.tickRate = Const.TICK_RATE;
        obj.userID = startTime.ToString();
        obj.sessionRecordings = new List<SessionRecording>();
        currentSession = new SessionRecording();
        obj.sessionRecordings.Add(currentSession);
        currentFrame = new StudyFrame();
        currentFrame.frameNum = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (recording)
        {
            long timestamp = System.DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime;
            //Log the data every second
            logTimer += Time.deltaTime;

            //Record data every 10 ticks
            studyTimer += 1f;
            if (studyTimer == Const.TICK_RATE)
            {
                studyTimer = 0f;
                SaveStudy();
            }

            //Log data every second
            if (logTimer > 1f)
            {
                logTimer = 0f;
                LogStudy();
            }
        }
    }

    #endregion
}
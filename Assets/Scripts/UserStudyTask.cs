using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Microsoft.MixedReality.Toolkit.Audio;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserStudyTask : MonoBehaviour
{

    [SerializeField]
    List<GameObject> Dialogues;

    [SerializeField]
    List<GameObject> Reminders;

    [SerializeField]
    List<GameObject> Task_Objs;

    [SerializeField]
    GameObject Final_popup;

    [SerializeField]
    GameObject Start_popup;

    [SerializeField] GameObject keyboard;

    List<Task> tasks;

    private long task_start_time;
    private long task_end_time;
    private static bool task_complete;
    private int current_task;
    private int total_tasks;
    public long UserID;

    private bool responsive = true;

    SceneStudyManager record;
    bool isRecording = false;

    private static List<ExperimentEventData> currentEvents;

    private string correct_answer;

    public bool complete(string answer)
    {

        Debug.Log("current_task: " + current_task.ToString());
        Debug.Log(answer.Length + ", " + correct_answer.Length);
        Debug.Log("answer entered: " + answer);
        Debug.Log("correct_answer: " + correct_answer);
        bool task_complete = false;

        if(current_task == 6)
        {
            correct_answer = Task_Objs[current_task].GetComponent<Weather>().getMainTemp();
        }

        
        if(current_task == 7)
        {
            correct_answer = Task_Objs[current_task].GetComponent<Weather>().getLocation();
        }

        
        if(current_task == 8)
        {
            correct_answer = Task_Objs[current_task].GetComponent<Weather>().getMinTemp();
        }



        if(answer.CompareTo(correct_answer) == 0)
        {
            task_complete = true;
        }

        if (task_complete)
        {
            Debug.Log("SUCCESS");
            tasks[current_task].setTaskComplete(true);
            if (isRecording)
            {
               log_data_end(answer);
               tasks[current_task].recordTask(record);
            }
            current_task += 1;
            keyboard.SetActive(false);
            NextTask();
            
            return true;
        }

        Debug.Log("INCORRECT");

        return false;
    }

    public bool touchpad_enter(string entered_num)
    {
        if (isRecording)
        {
            log_data_entered(entered_num);
        }
      
        return complete(entered_num);
    }

    public void setResponsive(bool b)
    {
        Debug.Log("set Responsive in U.S.");
        responsive = b;
        foreach(Task t in tasks)
        {
            t.setResponsive(b);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentEvents = new List<ExperimentEventData>();
        record = gameObject.GetComponent<SceneStudyManager>();
        UserID = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        current_task = 0;
        keyboard.SetActive(false);
        //keyboard.transform.position = keyboard.transform.position + new Vector3( 0.0f, -1.8f, 0.38f);
        NumberDisplay.setTask(this);
        Final_popup.SetActive(false);
        //Start_popup.SetActive(true);
        tasks = new List<Task>();
        setUpTask_Objs();
        WelcomeMessage();
    }

    public void WelcomeMessage()
    {
        Start_popup.SetActive(true);
		TextToSpeech tts = Start_popup.transform.GetComponent<TextToSpeech>();
        Transform tran = Start_popup.transform.Find("DescriptionText");
        TextMeshPro tmp = tran.GetComponent<TextMeshPro>();
		tts.StartSpeaking(tmp.text);
    }

    public void StartStudyResponsive()
    {
        setResponsive(true);
        Start_popup.SetActive(false);
        record.startRecording();
        isRecording = true;
        NextTask();
    }

    public void StartStudyNonResponsive()
    {
        setResponsive(false);
        Start_popup.SetActive(false);
        record.startRecording();
        isRecording = true;
        NextTask();
    }



    void NextTask()
    {
        if(current_task < total_tasks)
        {
            Debug.Log("NextTask " + current_task);
            setUpTask();
        }
        else
        {
            Debug.Log("Finished Experiement");
            Final_popup.SetActive(true);
            record.stopRecording();
            isRecording = false;
        }
    }


    double dist(Transform t1, Transform t2)
    {
        return (t1.position - t2.position).magnitude;
    }

    void GetResults()
    {
        Transform tran = Final_popup.transform.Find("DescriptionText");
        TextMeshPro tmp = tran.GetComponent<TextMeshPro>();
        tmp.text = "";
        int count = 0;
        foreach(Task t in tasks)
        {
            long time = t.total_time();
            tmp.text += "Task " + count.ToString() + " Time: ";
            tmp.text += time.ToString() + "\n";
            count += 1;
        }

    }

    void setUpTask_Objs()
    {
        Start_popup.SetActive(false);
        Final_popup.SetActive(false);
        for(int i = 0; i < Dialogues.Count; i++)
        {
            Task task = new Task(i, Dialogues[i], Reminders[i], Task_Objs[i]);
            //Task task = new Task(i, Dialogues[i], Reminders[i], Task_Objs[i], keyboard);

            tasks.Add(task);
            Dialogues[i].SetActive(false);
            Reminders[i].SetActive(false);
            Task_Objs[i].SetActive(false);
        }

        total_tasks = tasks.Count;
    }

    public void setCorrentAnswer(string a)
    {
        Debug.Log("set answer to " + a);
        correct_answer = a.ToLower();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToggleTask(int t)
    {
        if (tasks[t].isStarted)
        {
            ToggleEndTask(t);
        }
        else
        {
            ToggleStartTask(t);
        }
    }

    void ToggleStartTask(int t)
    {
        if (tasks[current_task].isStarted)
        {
            ToggleEndTask(current_task);
        }
        current_task = t;
        setUpTask();
    }

    void ToggleEndTask(int t)
    {
        keyboard.SetActive(false);
        Dialogues[t].SetActive(false);
        tasks[t].EndTask();
    }

    /*
     * to turn off responsive design  tasks[current_task].setResponsive(false);
     */

    public void setUpTask()
    {
        Debug.Log("setUpTask " + current_task);
        tasks[current_task].SetUp();
        //moved from .65 to .55
        tasks[current_task].setTPosition(0.0f, 0.03f, 0.6f, Camera.main.transform.position);
        keyboard.transform.position = Camera.main.transform.position + new Vector3( 0.053f, -0.45f, 0.5f);
        if(current_task == 0)
        {
            tasks[current_task].setTScale(.25f);
            //tasks[current_task].moveDescY(-0.1f);
            correct_answer = "2nd Title";
        }
        if (current_task == 1)
        {
            tasks[current_task].setTScale(.25f);
            //tasks[current_task].moveDescY(-0.1f);
            correct_answer = "5th Title";
        }
        if (current_task == 2)
        {
            tasks[current_task].setTScale(.35f);
            tasks[current_task].moveDescY(-0.05f);
            correct_answer = "4th Author";
        }
        if (current_task == 3)
        {
            tasks[current_task].setTScale(.55f);
            tasks[current_task].moveDescY(-0.09f);
            correct_answer = "1st Conf";
        }
        if(current_task == 4)
        {
            tasks[current_task].setTScale(.55f);
            //tasks[current_task].moveDescY(-0.1f);
            correct_answer = "9499274580";
        }
        if (current_task == 5)
        {
            tasks[current_task].setTScale(.9f);
            //tasks[current_task].moveDescY(-0.2f);
            correct_answer = "8058272338";
        }
        if (current_task == 6)
        {
            tasks[current_task].setTPosition(0.0f, 0.08f, 0.6f, Camera.main.transform.position);
            correct_answer = Task_Objs[current_task].GetComponent<Weather>().getMainTemp();
            tasks[current_task].setTScale(.3f);
            tasks[current_task].moveDescY(0.12f);
        }
        if (current_task == 7)
        {
            tasks[current_task].setTPosition(0.0f, 0.08f, 0.6f, Camera.main.transform.position);
            tasks[current_task].setTScale(.43f);
            tasks[current_task].moveDescY(0.12f);
            correct_answer = Task_Objs[current_task].GetComponent<Weather>().getLocation();
        }
        if (current_task == 8)
        {
            tasks[current_task].setTPosition(0.0f, 0.08f, 0.6f, Camera.main.transform.position);
            correct_answer = Task_Objs[current_task].GetComponent<Weather>().getMinTemp();
            tasks[current_task].setTScale(.77f);
            tasks[current_task].moveDescY(0.12f);
        }

        if (isRecording)
        {
           log_data("instruction");

        }

    }

    private void log_data(string type)
    {
        ExperimentEventData currentEventData = new ExperimentEventData();
        currentEventData.unixTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        currentEventData.systemTime = System.DateTime.Now.ToString("HH-mm-ss-ff");
        currentEventData.isResponsive = responsive;
        currentEventData.eventName = type;
        currentEventData.task_number = current_task;

        currentEventData.task_type = tasks[current_task].getObjectName();
        currentEventData.object_position = tasks[current_task].getObjectPosition();
        currentEventData.object_scale = tasks[current_task].getObjectScale();

        
        currentEventData.correct_answer = correct_answer;
        currentEventData.guess = "";
        currentEvents.Add(currentEventData);
    }


    public void StartTask()
    {
        task_complete = false;
        tasks[current_task].StartTask();
        if (isRecording)
        {
            log_data("start");
        }
    }

    public void EnterAnswer(TextMeshProUGUI text)
    {
        string input = text.text.Substring(0, text.text.Length - 1).ToLower();
        char[] charsToTrim = {' '};
        input = input.Trim(charsToTrim);
        input = input.ToLower();

        while(input.Contains("  "))
        {
            int index = input.IndexOf("  ");
            input =  input.Substring(0, index) +  input.Substring(index+1);
        }
        Debug.Log("eneterd: " + input);
        
        if (isRecording)
        {
           log_data_entered(input);
        }
        complete(input);
    }

    /*
    public void closeKeyboard(GameObject keyboard)
    {
        if (task_complete)
        {
            keyboard.SetActive(false);
        }
    }
    */

    public void EnterAnswer(string text)
    {
        Debug.Log("pressed text: " + text);
        if (isRecording)
        {
           log_data_entered(text);
        }
        complete(text);
    }

    private void log_data_end(string guess)
    {
        ExperimentEventData currentEventData = new ExperimentEventData();
        currentEventData.unixTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        currentEventData.systemTime = System.DateTime.Now.ToString("HH-mm-ss-ff");
        currentEventData.isResponsive = responsive;
        currentEventData.eventName = "complete";
        currentEventData.task_number = current_task;

        currentEventData.task_type = tasks[current_task].getObjectName();
        currentEventData.object_position = tasks[current_task].getObjectPosition();
        currentEventData.object_scale = tasks[current_task].getObjectScale();

        currentEventData.correct_answer = correct_answer;
        currentEventData.guess = guess;
        currentEvents.Add(currentEventData);
    }

    private void log_data_entered(string guess)
    {
        ExperimentEventData currentEventData = new ExperimentEventData();
        currentEventData.unixTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        currentEventData.systemTime = System.DateTime.Now.ToString("HH-mm-ss-ff");
        currentEventData.isResponsive = responsive;
        currentEventData.eventName = "answer";
        currentEventData.task_number = current_task;

        currentEventData.task_type = tasks[current_task].getObjectName();
        currentEventData.object_position = tasks[current_task].getObjectPosition();
        currentEventData.object_scale = tasks[current_task].getObjectScale();

        currentEventData.correct_answer = correct_answer;
        currentEventData.guess = guess;
        currentEvents.Add(currentEventData);

    }

    public static List<ExperimentEventData> GetExperimentEventData()
    {
        List<ExperimentEventData> eventDataCopy = currentEvents;
        currentEvents = new List<ExperimentEventData>();
        return eventDataCopy;
    }

}

[System.Serializable]
public class ExperimentEventData
{
    public long unixTime;
    public string systemTime;
    public bool isResponsive;
    public string eventName;

    public int task_number;
    public string task_type;
    public Vector3 object_scale;
    public Vector3 object_position;

    public string correct_answer;
    public string guess;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Microsoft.MixedReality.Toolkit.Audio;
//using UnityEngine.CoreModule;

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

    [SerializeField] GameObject Display;


    List<Task> tasks;

    private long task_start_time;
    private long task_end_time;
    private static bool task_complete;
    private int current_task;
    private int total_tasks;
    public long UserID;

    SceneStudyManager record;

    private string correct_answer;

    public bool complete(string answer)
    {
        Debug.Log("current_task: " + current_task.ToString());
        Debug.Log(answer.Length + ", " + correct_answer.Length);
        Debug.Log("answer entered: " + answer);
        Debug.Log("correct_answer: " + correct_answer);
        bool task_complete = false;

        if(answer.CompareTo(correct_answer) == 0)
        {
            task_complete = true;
        }

        if (task_complete)
        {
            Debug.Log("SUCCESS");
            tasks[current_task].setTaskComplete(true);
            tasks[current_task].recordTask(record);
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
        return complete(entered_num);
    }

    // Start is called before the first frame update
    void Start()
    {
        record = gameObject.GetComponent<SceneStudyManager>();
        UserID = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        current_task = 0;
        keyboard.SetActive(false);
        //keyboard.transform.position = keyboard.transform.position + new Vector3( 0.0f, -1.6f, 0.36f);
        NumberDisplay.setTask(this);
        Weather.setTask(this);
        Display.SetActive(true);
        Final_popup.SetActive(false);
        //Start_popup.SetActive(true);
        tasks = new List<Task>();
        setUpTask_Objs();
        //WelcomeMessage();

    }

    public void WelcomeMessage()
    {
        Start_popup.SetActive(true);
		TextToSpeech tts = Start_popup.transform.GetComponent<TextToSpeech>();
        Transform tran = Start_popup.transform.Find("DescriptionText");
        TextMeshPro tmp = tran.GetComponent<TextMeshPro>();
		tts.StartSpeaking(tmp.text);
    }

    public void StartStudy()
    {
        Start_popup.SetActive(false);
        record.startRecording();
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
            GetResults();
            Final_popup.SetActive(true);
            record.stopRecording();
        }
    }

    void UpdateDisplay()
    {
        Transform tran = Display.transform.Find("DescriptionText");
        TextMeshPro tmp = tran.GetComponent<TextMeshPro>();
        tmp.text = "";
        tmp.text = "localPosition: " + Task_Objs[current_task].transform.localPosition.ToString() + "\n";
        tmp.text += "localScale: " + Task_Objs[current_task].transform.localScale.ToString() + "\n";

        var headPosition = Camera.main.transform.position;
        tmp.text += "headPosition: " + headPosition.ToString() + "\n";
        double distance = Math.Abs(dist(Task_Objs[current_task].transform, Camera.main.transform));
        tmp.text += "distance: " + distance.ToString() + "\n";
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
        Debug.Log("total_tasks: " + total_tasks);

        foreach (Task t in tasks)
        {
            t.Debug_Log();
        }
    }

    public void setCorrentAnswer(string a)
    {
        correct_answer = a.ToLower();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
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
        tasks[current_task].setRPosition(0.0f, -0.1f, 1f);
        tasks[current_task].setTPosition(0.0f, 0.03f, 0.70f, Camera.main.transform.position);
        keyboard.transform.position = Camera.main.transform.position + new Vector3( 0.053f, -0.2f, 0.5f);
        if(current_task == 0)
        {
            //tasks[current_task].setTPosition(0.0f, 0.0f, 3.0f);
            tasks[current_task].setTScale(.3f);
            tasks[current_task].moveDescY(-0.1f);
            correct_answer = "2nd Title";
        }
        if (current_task == 1)
        {
            //tasks[current_task].setTPosition(0.0f, 0.0f, 3.0f);
            tasks[current_task].setTScale(.3f);
            tasks[current_task].moveDescY(-0.1f);
            correct_answer = "5th Title";
        }
        if (current_task == 2)
        {
            //tasks[current_task].setTPosition(0.0f, 0.0f, 2.0f);
            tasks[current_task].setTScale(.4f);
            tasks[current_task].moveDescY(-0.1f);
            correct_answer = "4th Author";
        }
        if (current_task == 3)
        {
            //tasks[current_task].setTPosition(0.0f, 0.0f, 1.5f);
            tasks[current_task].setTScale(.5f);
            tasks[current_task].moveDescY(-0.2f);
            correct_answer = "1st Conf";
        }
        if(current_task == 4)
        {
            //tasks[current_task].setTPosition(0.0f, 0.0f, 2.5f);
            //tasks[current_task].setResponsive(true);
            tasks[current_task].setTScale(.65f);
            tasks[current_task].moveDescY(-0.1f);
            correct_answer = "9499274580";
        }
        if (current_task == 5)
        {
            //tasks[current_task].setTPosition(0.0f, 0.0f, 1.6f);
            tasks[current_task].setTScale(.9f);
            tasks[current_task].moveDescY(-0.2f);
            correct_answer = "8058272338";
        }
        if (current_task == 6)
        {
            //tasks[current_task].setTPosition(0.0f, 0.0f, 4.7f);
            //tasks[current_task].setNeedKeyboard(true);
            tasks[current_task].setTScale(.3f);
        }
        if (current_task == 7)
        {
            //tasks[current_task].setTPosition(0.0f, 0.0f, 3.2f);
            //tasks[current_task].setNeedKeyboard(true);
            tasks[current_task].setTScale(.4f);
        }
        if (current_task == 8)
        {
            //tasks[current_task].setTPosition(0.0f, 0.0f, 2.0f);
            //tasks[current_task].setNeedKeyboard(true);
            tasks[current_task].setTScale(.5f);
        }



    }

    public void StartTask()
    {
        task_complete = false;
        tasks[current_task].StartTask();
    }

    public void EnterAnswer(TextMeshProUGUI text)
    {
        string input = text.text.Substring(0, text.text.Length - 1).ToLower();
        input = input.Trim();
        complete(input);
    }

    public void closeKeyboard(GameObject keyboard)
    {
        if (task_complete)
        {
            keyboard.SetActive(false);
        }
    }

    public void EnterAnswer(string text)
    {
        Debug.Log("pressed text: " + text);
        complete(text);
    }


}

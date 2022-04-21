using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    [SerializeField] GameObject keyboard;


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
        Final_popup.SetActive(false);
        tasks = new List<Task>();
        setUpTask_Objs();
        StartStudy();

    }

    public void StartStudy()
    {
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
        for(int i = 0; i < Dialogues.Count; i++)
        {
            Task task = new Task(i, Dialogues[i], Reminders[i], Task_Objs[i], keyboard);

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
        
    }

    public void setUpTask()
    {
        Debug.Log("setUpTask " + current_task);
        tasks[current_task].SetUp();
        tasks[current_task].setRPosition(0.0f, -0.3f, 2.64f);
        if(current_task == 0)
        {
            tasks[current_task].setTPosition(0.0f, 0.0f, 2.5f);
            tasks[current_task].setResponsive(true);
            correct_answer = "9499174580";
        }
        if (current_task == 1)
        {
            tasks[current_task].setTPosition(0.0f, 0.0f, 1.6f);
            correct_answer = "8058272338";
        }
        if (current_task == 2)
        {
            tasks[current_task].setTPosition(0.0f, 0.0f, 2.5f);
            tasks[current_task].setResponsive(false);
            correct_answer = "9499174580";
        }
        if (current_task == 3)
        {
            tasks[current_task].setTPosition(0.0f, 0.0f, 4.7f);
             tasks[current_task].setNeedKeyboard(true);
        }
        if (current_task == 4)
        {
            tasks[current_task].setTPosition(0.0f, 0.0f, 3.5f);
            tasks[current_task].setNeedKeyboard(true);
        }
        if (current_task == 5)
        {
            tasks[current_task].setTPosition(0.0f, 0.0f, 2.0f);
            tasks[current_task].setNeedKeyboard(true);
        }


    }

    public void StartTask()
    {
        tasks[current_task].StartTask();
    }

    public void EnterAnswer(TextMeshProUGUI text)
    {
        string input = text.text.Substring(0, text.text.Length - 1).ToLower();
        complete(input);
    }

}

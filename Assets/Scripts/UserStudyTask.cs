using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    List<Task> tasks;

    private long task_start_time;
    private long task_end_time;
    private static bool task_complete;
    private int current_task;
    private int total_tasks;
    public long UserID;

    SceneStudyManager record;

    private string touch_pad_task_num;

    public bool complete(string num)
    {
        Debug.Log("current_task: " + current_task.ToString());
        Debug.Log("number entered: " + num);
        if ((current_task == 0 || current_task == 1) && num == touch_pad_task_num)
        {
            tasks[current_task].setTaskComplete(true);
            tasks[current_task].recordTask(record);
            current_task += 1;
            NextTask();
            return true;
        }

        if ((current_task == 2 || current_task == 3) && num == touch_pad_task_num)
        {
            tasks[current_task].setTaskComplete(true);
            tasks[current_task].recordTask(record);
            current_task += 1;
            NextTask();
            return true;
        }

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
        NumberDisplay.setTask(this);
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
            Task task = new Task(i, Dialogues[i], Reminders[i], Task_Objs[i]);

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setUpTask()
    {
        Debug.Log("setUpTask " + current_task);
        if(current_task == 0)
        {
            tasks[current_task].setTPosition(0.0f, 0.0f, 1.8f);
            tasks[current_task].setRPosition(0.0f, -0.7f, 0.0f);
            tasks[current_task].setResponsive(true);
            tasks[current_task].SetUp();
            touch_pad_task_num = "9499174580";
        }
        if (current_task == 1)
        {
            tasks[current_task].SetUp();
            tasks[current_task].setTPosition(0.0f, 0.0f, 1.4f);
            tasks[current_task].setRPosition(0.0f, -0.7f, 0.0f);
            touch_pad_task_num = "8058272338";
        }
        if (current_task == 2)
        {
            tasks[current_task].setTPosition(0.0f, 0.0f, 1.8f);
            tasks[current_task].setRPosition(0.0f, -0.7f, 0.0f);
            tasks[current_task].SetUp();
            tasks[current_task].setResponsive(false);
            touch_pad_task_num = "9499174580";
        }
        if (current_task == 3)
        {
            tasks[current_task].SetUp();
            tasks[current_task].setTPosition(0.0f, 0.0f, 1.4f);
            tasks[current_task].setRPosition(0.0f, -0.7f, 0.0f);
            tasks[current_task].setResponsive(false);
            touch_pad_task_num = "8058272338";
        }
    }

    public void StartTask()
    {
        tasks[current_task].StartTask();
    }

}

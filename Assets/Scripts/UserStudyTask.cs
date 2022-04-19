using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStudyTask : MonoBehaviour
{
/*    [SerializeField]
    GameObject Dialogue;*/

    [SerializeField]
    GameObject Dialogue;

    [SerializeField]
    GameObject Task;

    public long task_start_time;
    bool task_complete;
    int task_num;

    public void StartTask1()
    {
        Dialogue.SetActive(false);
        task_complete = false;
        task_start_time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        Task.SetActive(true);
    }

    public void EndTask1()
    {
        if (task_complete)
        {

        }
        Dialogue.SetActive(false);
        task_start_time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        Task.SetActive(true);
        task_num = 1;
    }

    public bool touchpad_enter(string entered_num)
    {
        if (task_num == 1 && entered_num == touch_pad_task_num)
        {
            task_complete = true;
            return true;
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Dialogue.SetActive(true);
        Task1.SetActive(false);
        task_complete = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

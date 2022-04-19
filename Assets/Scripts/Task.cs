﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Task
{
	int task_num;
	GameObject Dialogue;
	GameObject Task_Object;
	GameObject Task_Reminder;
	bool task_complete;
	long start_time;
	long end_time;
	bool isReponsive;

	public Task(int t, GameObject d, GameObject r, GameObject g)
	{
		task_num = t;
		Dialogue = d;
		Task_Reminder = r;
		Task_Object = g;
	}

	public void SetUp()
    {
		//task object at distance
		//dialogue set to true
		Dialogue.SetActive(true);
		Task_Object.SetActive(false);
		Task_Reminder.SetActive(false);

	}

	public void StartTask()
    {
		Debug.Log("StartTask " + task_num);
		Dialogue.SetActive(false);
		task_complete = false;
		start_time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
		Task_Object.SetActive(true);
		Task_Reminder.SetActive(true);
	}

	public void EndTask()
    {
		Debug.Log("EndTask");

/*		Dialogue.SetActive(true);*/
		end_time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
		Task_Object.SetActive(false);
		Task_Reminder.SetActive(false);
		//save_data
	}

	public void setTaskComplete(bool v)
    {
		task_complete = v;
        if (v)
        {
			EndTask();
        }
    }

	public void Debug_Log()
    {
		Debug.Log(Dialogue.name);
		Debug.Log(Task_Object.name);
		Debug.Log(Task_Reminder.name);
	}

	public void recordTask(SceneStudyManager record)
    {
		Transform t = Dialogue.transform.Find("DescriptionText");
		TextMeshPro tmp = t.GetComponent<TextMeshPro>();
		record.startNewSession(start_time, end_time, tmp.text, isReponsive, end_time - start_time);
	}

	public void setTPosition(float d1, float d2, float d3)
    {

		Task_Object.transform.position = new Vector3(d1, d2, d3);
	}

	public void setRPosition(float d1, float d2, float d3)
	{
		Task_Reminder.transform.position = new Vector3(Task_Reminder.transform.position.x, d2, Task_Reminder.transform.position.z);
	}

	public void setResponsive(bool b)
    {
		isReponsive = b;
		ResponsiveDesign rd = Task_Object.GetComponent<ResponsiveDesign>();
        if (!b)
        {
			rd.enabled = false;
        }
	}

	public long total_time()
    {
		return end_time - start_time;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Audio;

public class Task
{
	int task_num;
	GameObject Dialogue;
	GameObject Task_Object;
	GameObject Task_Reminder;
	//GameObject keyboard;
	bool task_complete;
	long start_time;
	long end_time;
	bool isReponsive;
	public bool isStarted;
	string description;
	//bool needKeyboard;

	public Task(int t, GameObject d, GameObject r, GameObject g)
	{
		task_num = t;
		Dialogue = d;
		Task_Reminder = r;
		Task_Object = g;
		//keyboard = k;

		
		Transform tran = Dialogue.transform.Find("DescriptionText");
		TextMeshPro tmp = tran.GetComponent<TextMeshPro>();
		description = tmp.text;
		//needKeyboard = false;
		isReponsive = true;
		task_complete = false;
	}

	public void SetUp()
    {
		//task object at distance
		//dialogue set to true
		Dialogue.SetActive(true);
		Task_Object.SetActive(true);
		Task_Reminder.SetActive(false);
		TextToSpeech tts = Dialogue.transform.GetComponent<TextToSpeech>();
		tts.StartSpeaking(description);
		isStarted = true;
	}

	public void StartTask()
    {
		Debug.Log("StartTask " + task_num);
		Dialogue.SetActive(false);
		task_complete = false;
		start_time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

		Task_Reminder.SetActive(true);

		/*
		if(needKeyboard)
        {
			keyboard.SetActive(true);
        }
		*/
	}

	public void EndTask()
    {
		Debug.Log("EndTask");
		/*
		if(needKeyboard)
        {
			keyboard.SetActive(false);
        }
		*/

		end_time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
		Task_Object.SetActive(false);
		Task_Reminder.SetActive(false);
		//save_data
		isStarted = false;
	}

	public void setTaskComplete(bool v)
    {
		task_complete = v;
        if (v)
        {
			EndTask();
        }
    }
	/*
	public void setNeedKeyboard(bool b)
    {
		needKeyboard = b;
    }
	*/
	public void Debug_Log()
    {
		Debug.Log(Dialogue.name);
		Debug.Log(Task_Object.name);
		Debug.Log(Task_Reminder.name);
	}

	public void recordTask(SceneStudyManager record)
    {
		record.startNewSession(start_time, end_time, description, isReponsive, end_time - start_time);
	}

	public void setTPosition(float d1, float d2, float d3)
    {
		Task_Object.transform.position = new Vector3(d1, d2, d3);
	}

	public void setTPosition(float d1, float d2, float d3, Vector3 cam_pos)
    {
		Task_Object.transform.position = cam_pos + new Vector3(d1, d2, d3);
	}

	public void setTScale(float s1)
    {
		Task_Object.transform.localScale = new Vector3(s1, s1, s1);
	}

	public void setRPosition(float d1, float d2, float d3)
	{
		Task_Reminder.transform.position = new Vector3(Task_Reminder.transform.position.x, d2, d3);
	}

	public void moveDescY(float f1)
	{
		Task_Reminder.transform.position = Task_Reminder.transform.position + new Vector3(0.0f, f1, 0.0f);
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
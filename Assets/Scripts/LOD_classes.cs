using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Microsoft.MixedReality.Toolkit.UI;

public class LOD_TMP
{
	int LOD;
	double ratio;
	bool set;
	List<TextMeshPro> text = new List<TextMeshPro>();

	public LOD_TMP(){}

	public LOD_TMP(int l, double r, List<TextMeshPro> t, bool s)
	{
		LOD = l;
		ratio = r;
		text = t;
		set = !s;
		setLOD(s);
		set = s;
	}

	public void setLOD(int l){LOD = l;}
	public int getLOD(){return LOD;}
	public void setRatio(double r){ratio = r;}
	public double getRatio(){return ratio;}
	public void setSet(bool s){set = s;}
	public bool getSet(){return set;}
	public void setText(List<TextMeshPro> t){text = t;}
	public List<TextMeshPro> getText(){return text;}

	public void setLOD(bool v)
    {
		if (set == v)
		{
			return;
		}
		foreach (TextMeshPro t in text)
        {
			t.gameObject.SetActive(v);
        }
		set = v;
    }

	public void decreaseTransparency()
	{
		if (text[0].color[3] > 0)
		{
			foreach (TextMeshPro obj in text)
			{
				Color32 color = obj.color;
				byte a = (byte)(color[3] - 1);
				if (color[3] - 1 <= 0)
				{
					a = 0;
					if (set == false)
					{
						obj.gameObject.SetActive(false);
					}
				}
				obj.color = new Color32(color[0], color[1], color[2], a);
			}
		}
	}

	public void increaseTransparency()
	{
		if (text[0].color[3] < 255)
		{
			foreach (TextMeshPro obj in text)
			{
				Color32 color = obj.color;
				byte a = (byte)(color[3] + 1);
				if (color[3] + 1 >= 255)
				{
					a = 255;
				}
				obj.color = new Color32(color[0], color[1], color[2], a);
			}
		}
	}

	public void setTransparency( byte a)
	{
		foreach (TextMeshPro obj in text)
		{
			Color32 color = obj.color;
			obj.color = new Color32(color[0], color[1], color[2], a);
		}

	}

	public double getTextSize()
	{
		TextMeshPro t = text[0];
		Transform pt = t.transform.parent;
		double scale = t.fontSize * t.transform.localScale.x;
		while (pt != null)
		{
			scale *= pt.transform.localScale.x;
			pt = pt.parent;
		}

		return scale;
	}

	public string getName(){
		TextMeshPro t = text[0];
		return t.transform.gameObject.name;
	}

}

public class LOD_Obj
{
	int LOD;
	double ratio;
	bool set;
	List<GameObject> objects = new List<GameObject>();

	public LOD_Obj(){}

	public LOD_Obj(int l, double r, List<GameObject> g, bool s)
	{
		LOD = l;
		ratio = r;
		objects = g;
		set = !s;
		setLOD(s);
		set = s;
	}

	public void setLOD(int l) { LOD = l; }
	public int getLOD() { return LOD; }
	public void setRatio(double r) { ratio = r; }
	public double getRatio() { return ratio; }
	public void setSet(bool s) { set = s; }
	public bool getSet() { return set; }
	public void setObjects(List<GameObject> g) { objects = g; }
	public List<GameObject> getObjects() { return objects; }

	public double getSize()
	{
		GameObject g = objects[0];
		Transform t = g.transform;
		//return t.localScale.x * pt.localScale.x * t.localScale.y * pt.localScale.y * t.localScale.z * pt.localScale.z;
		return t.localScale.x * t.localScale.y * t.localScale.z;
	}

	public string getName(){
		GameObject g = objects[0];
		return g.name;
	}

	public double getLocalSize()
	{
		GameObject g = objects[0];
		Transform t = g.transform;
		Transform pt = t.transform.parent;
		double volume = t.transform.localScale.x * t.transform.localScale.y * t.transform.localScale.z;
		double scale = volume;
		while (pt != null)
		{
			volume = pt.transform.localScale.x * pt.transform.localScale.y * pt.transform.localScale.z;
			RectTransform rectTransform = pt.transform.GetComponent<RectTransform>();
			if(rectTransform != null)
            {
				double area = rectTransform.rect.width * rectTransform.rect.height;
				if(area > 0)
                {

					volume *= area;

                }
            }
			scale *= volume;
			pt = pt.parent;
		}

		return scale;
	}

	public void setLOD(bool v)
	{
		if (set == v)
		{
			return;
		}
		foreach (GameObject g in objects)
		{
			if(g != null)
            {
				g.SetActive(v);
            }
		}
		set = v;
	}

	void disableObjects()
	{
		foreach (GameObject g in objects)
		{
			g.SetActive(false);
		}
	}

	void enableObjects()
	{
		foreach (GameObject g in objects)
		{
			g.SetActive(true);
		}
	}
}


public class LOD_Interact
{
	int LOD;
	double ratio;
	bool set;
	List<Interactable> interactables = new List<Interactable>();

	public LOD_Interact(){}

	public LOD_Interact(int l, double r, List<Interactable> g, bool s)
	{
		LOD = l;
		ratio = r;
		interactables = g;
		set = !s;
		setLOD(s);
		set = s;
	}

	public void setLOD(int l) { LOD = l; }
	public int getLOD() { return LOD; }
	public void setRatio(double r) { ratio = r; }
	public double getRatio() { return ratio; }
	public void setSet(bool s) { set = s; }
	public bool getSet() { return set; }
	public void setInteractables(List<Interactable> g) { interactables = g; }
	public List<Interactable> getInteractables() { return interactables; }

	public void setLOD(bool v)
	{
        if (set == v)
        {
			return;
        }
		foreach (Interactable g in interactables)
		{
			//g.IsInteractive = v;
			g.IsEnabled = v;

		}
		set = v;
	}

	public void setUpInteraction()
	{

		foreach (Interactable g in interactables)
		{
			g.transform.gameObject.SetActive(true);
			//g.IsInteractive = false;
			g.IsEnabled = false;
/*			Debug.Log("g: " + g + ", g.t.gO: " + g.transform.gameObject + ", IsInteractive = false");*/
		}

		set = false;
	}

	public string getName(){
		Interactable g = interactables[0];
		return g.transform.gameObject.name;
	}

	public double getLocalSize()
	{
		GameObject g = interactables[0].gameObject;
		Transform t = g.transform;
		Transform pt = t.transform.parent;
		double volume = t.transform.localScale.x * t.transform.localScale.y * t.transform.localScale.z;
		double scale = volume;
		while (pt != null)
		{
			volume = pt.transform.localScale.x * pt.transform.localScale.y * pt.transform.localScale.z;
			RectTransform rectTransform = pt.transform.GetComponent<RectTransform>();
			if(rectTransform != null)
            {
				double area = rectTransform.rect.width * rectTransform.rect.height;
				if(area > 0)
                {

					volume *= area;

                }
            }
			scale *= volume;
			pt = pt.parent;
		}

		return scale;
	}
}


public class LOD_TMP_GUI
{
	int LOD;
	double ratio;
	bool set;
	List<TextMeshProUGUI> text = new List<TextMeshProUGUI>();

	public LOD_TMP_GUI() { }

	public LOD_TMP_GUI(int l, double r, List<TextMeshProUGUI> t, bool s)
	{
		LOD = l;
		ratio = r;
		text = t;
		set = !s;
		setLOD(s);
		set = s;
	}

	public void setLOD(int l) { LOD = l; }
	public int getLOD() { return LOD; }
	public void setRatio(double r) { ratio = r; }
	public double getRatio() { return ratio; }
	public void setSet(bool s) { set = s; }
	public bool getSet() { return set; }
	public void setText(List<TextMeshProUGUI> t) { text = t; }
	public List<TextMeshProUGUI> getText() { return text; }

	public void setLOD(bool v)
	{
		if (set == v)
		{
			return;
		}
		foreach (TextMeshProUGUI t in text)
		{
			t.gameObject.SetActive(v);
		}
		set = v;
	}

	public void decreaseTransparency()
	{
		if (text[0].color[3] > 0)
		{
			foreach (TextMeshProUGUI obj in text)
			{
				Color32 color = obj.color;
				byte a = (byte)(color[3] - 1);
				if (color[3] - 1 <= 0)
				{
					a = 0;
					if (set == false)
					{
						obj.gameObject.SetActive(false);
					}
				}
				obj.color = new Color32(color[0], color[1], color[2], a);
			}
		}
	}

	public void increaseTransparency()
	{
		if (text[0].color[3] < 255)
		{
			foreach (TextMeshProUGUI obj in text)
			{
				Color32 color = obj.color;
				byte a = (byte)(color[3] + 1);
				if (color[3] + 1 >= 255)
				{
					a = 255;
				}
				obj.color = new Color32(color[0], color[1], color[2], a);
			}
		}
	}

	public void setTransparency(byte a)
	{
		foreach (TextMeshProUGUI obj in text)
		{
			Color32 color = obj.color;
			obj.color = new Color32(color[0], color[1], color[2], a);
		}

	}

	public double getTextSize()
	{
		TextMeshProUGUI t = text[0];
		Transform pt = t.transform.parent;
		double scale = t.fontSize * t.transform.localScale.x;
		while (pt != null)
		{
			scale *= pt.transform.localScale.x;
			pt = pt.parent;
		}

		return scale;
	}

	public string getName(){
		TextMeshProUGUI t = text[0];
		return t.transform.gameObject.name;
	}

}


public class LOD_Select
{
	int LOD;
	double ratio;
	bool set;
	List<Selectable> selectables = new List<Selectable>();

	public LOD_Select(){}

	public LOD_Select(int l, double r, List<Selectable> g, bool s)
	{
		LOD = l;
		ratio = r;
		selectables = g;
		set = !s;
		setLOD(s);
		set = s;
	}

	public void setLOD(int l) { LOD = l; }
	public int getLOD() { return LOD; }
	public void setRatio(double r) { ratio = r; }
	public double getRatio() { return ratio; }
	public void setSet(bool s) { set = s; }
	public bool getSet() { return set; }
	public void setSelectables(List<Selectable> g) { selectables = g; }
	public List<Selectable> getSelectables() { return selectables; }

	public void setLOD(bool v)
	{
        if (set == v)
        {
			return;
        }
		foreach (Selectable g in selectables)
		{
			g.interactable = v;
			//g.IsEnabled = v;

		}
		set = v;
	}

	public void setUpSelection()
	{

		foreach (Selectable g in selectables)
		{
			g.transform.gameObject.SetActive(true);
			g.interactable = false;
			//g.IsEnabled = false;

		}

		set = false;
	}

	public string getName(){
		Selectable g = selectables[0];
		return g.transform.gameObject.name;
	}

	public double getLocalSize()
	{
		GameObject g = selectables[0].gameObject;
		Transform t = g.transform;
		Transform pt = t.transform.parent;
		double volume = t.transform.localScale.x * t.transform.localScale.y * t.transform.localScale.z;
		double scale = volume;
		while (pt != null)
		{

			volume = pt.transform.localScale.x * pt.transform.localScale.y * pt.transform.localScale.z;
			RectTransform rectTransform = pt.transform.GetComponent<RectTransform>();
			if(rectTransform != null)
            {
				double area = rectTransform.rect.width * rectTransform.rect.height;
				if(area > 0)
                {

					volume *= area;

                }
            }
			scale *= volume;
			pt = pt.parent;
		}

		return scale;
	}
}

/*
 * Welcome to the Improving Usability of AR Applications User Study. 
 * 
 * 
 */
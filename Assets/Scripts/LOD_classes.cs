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

	public double getLocalSize()
	{
		GameObject g = objects[0];
		Transform t = g.transform;
		Transform pt = t.transform.parent;
		double volume = t.transform.localScale.x * t.transform.localScale.y * t.transform.localScale.z;
		Debug.Log(t + ": " + t.transform.localScale);
		double scale = volume;
		while (pt != null)
		{
			Debug.Log(pt + ": " + pt.transform.localScale);
			volume = pt.transform.localScale.x * pt.transform.localScale.y * pt.transform.localScale.z;
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
			g.SetActive(v);
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
		Debug.Log("set active and interactive " + v);
		foreach (Interactable g in interactables)
		{
			g.IsInteractive = v;
		}
		set = v;
	}

	public void setUpInteraction()
	{
		Debug.Log("set active and disable interactive ");
		foreach (Interactable g in interactables)
		{
			g.transform.gameObject.SetActive(true);
			g.IsInteractive = false;
		}

		set = false;
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
		Debug.Log("t: " + t + ", font: " + scale);
		while (pt != null)
		{
			scale *= pt.transform.localScale.x;
			Debug.Log("pt: " + pt + ", scale: " + pt.transform.localScale.x + ", font: " + scale);
			pt = pt.parent;
		}

		return scale;
	}

}
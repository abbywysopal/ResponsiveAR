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

	public LOD_TMP()
	{
		int LOD;
		double ratio;
		List<TextMeshPro> text = new List<TextMeshPro>();
	}

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
		foreach(TextMeshPro t in text)
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

	public double getTextSize(float p_scale)
	{
		TextMeshPro t = text[0];
		return t.fontSize * t.transform.localScale.x * p_scale;
	}

}

public class LOD_Obj
{
	int LOD;
	double ratio;
	bool set;
	List<GameObject> objects = new List<GameObject>();

	public LOD_Obj()
	{
		int LOD;
		double ratio;
		List<TextMeshPro> text = new List<TextMeshPro>();
	}

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

	public double getSize(Transform pt)
	{
		GameObject g = objects[0];
		Transform t = g.transform;
		return t.localScale.x * pt.localScale.x * t.localScale.y * pt.localScale.y * t.localScale.z * pt.localScale.z;
	}

	public void setLOD(bool v)
	{
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

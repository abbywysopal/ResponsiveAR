using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utils
{
    private static System.Random rand = new System.Random();
    public static double SampleGaussian(double mean, double stddev)
    {
        // The method requires sampling from a uniform random of (0,1]  
        // but Random.NextDouble() returns a sample of [0,1).
        double x1 = 1 - rand.NextDouble();
        double x2 = 1 - rand.NextDouble();

        double y1 = System.Math.Sqrt(-2.0 * System.Math.Log(x1)) * System.Math.Cos(2.0 * System.Math.PI * x2);
        return y1 * stddev + mean;
    }

	private static System.DateTime epochStart = 
        new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);	
	public static long UnixTimestamp()
    {
		double elapsed = (System.DateTime.UtcNow - epochStart).TotalSeconds;
		return System.Convert.ToInt64(elapsed);
	}
	public static long UnixTimestampMilliseconds()
    {
		double elapsed = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		return System.Convert.ToInt64(elapsed);
	}

    public static bool GetKeyDownNumeric(out int value)
    {
        KeyCode[] keyCodes = {
            KeyCode.Alpha0,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9
        };

        for (int i = 0; i < keyCodes.Length; ++i)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                value = i;
                return true;
            }
        }
        value = -1;
        return false;
    }

    public static Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 2.0f), 3.0f);
    }

    public static Vector3 RandomMenuPosition(Vector3 objectPosition)
    {
        float delta = 0.25f;
        List<Vector3> deltas = new List<Vector3>() {
            new Vector3(delta, 0.0f, 0.0f),
            new Vector3(delta, delta, 0.0f),
            new Vector3(0.0f, delta, 0.0f),
            new Vector3(-delta, delta, 0.0f),
            new Vector3(-delta, 0.0f, 0.0f)
        };
        var menuPositions = deltas.Select(x => objectPosition + x).ToList();
        int rand = Random.Range(0, 5);
        return menuPositions[rand]; 
    }

	public static string GetFullName (GameObject go) {
		string name = go.name;
		while (go.transform.parent != null) {

			go = go.transform.parent.gameObject;
			name = go.name + "/" + name;
		}
		return name;
	}

    public static string WrapText(string text, int maxLineLength)
    {
        string[] words = text.Split(' ');
        string retval = "";
        int lineLength = 0;

        foreach(string word in words)
        {
            if (word.Length > maxLineLength)
            {
                throw new System.ArgumentException(
                    "Text contains words larger than maxLineLength");
            }
            
            lineLength += word.Length;
            if (lineLength <= maxLineLength)
            {
                retval += word + " ";
                lineLength += 1;
            }
            else
            {
                retval += "\n" + word + " ";
                lineLength = word.Length + 1;
            }
        }
        return retval;
    }

    public static string TruncateEllipsis(this string value, int maxChars)
    {
        return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
    }

    public static void SetLayerInChildren(this GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach(Transform child in obj.transform)
        {
            SetLayerInChildren(child.gameObject, layer);
        }
    }

    public static void SetLayerInChildren(this GameObject obj, string layerName)
    {
        SetLayerInChildren(obj, LayerMask.NameToLayer(layerName));
    }
}
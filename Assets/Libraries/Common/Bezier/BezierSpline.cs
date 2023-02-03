using UnityEngine;
using System;

public class BezierSpline : MonoBehaviour {

	[SerializeField]
	private Vector3[] points;

	[SerializeField]
	private BezierControlPointMode[] modes;

	[SerializeField]
	private bool loop;

	public bool Loop {
		get {
			return loop;
		}
		set {
			loop = value;
			if (value == true) {
				modes[modes.Length - 1] = modes[0];
				SetControlPoint(0, points[0]);
			}
		}
	}


    Vector3[] curvePoints = new Vector3[4];
    public Vector3[] GetCurve(int index)
    {
        int startIndex = index * 3;

        curvePoints[0] = points[startIndex];
        curvePoints[1] = points[startIndex + 1];
        curvePoints[2] = points[startIndex + 2];
        curvePoints[3] = points[startIndex + 3];

        return curvePoints;
    }

	public int ControlPointCount {
		get {
			return points.Length;
		}
	}

	public Vector3 GetControlPoint (int index) {
		return points[index];
	}

	public void SetControlPoint (int index, Vector3 point) {
		if (index % 3 == 0) {
			Vector3 delta = point - points[index];
			if (loop) {
				if (index == 0) {
					points[1] += delta;
					points[points.Length - 2] += delta;
					points[points.Length - 1] = point;
				}
				else if (index == points.Length - 1) {
					points[0] = point;
					points[1] += delta;
					points[index - 1] += delta;
				}
				else {
					points[index - 1] += delta;
					points[index + 1] += delta;
				}
			}
			else {
				if (index > 0) {
					points[index - 1] += delta;
				}
				if (index + 1 < points.Length) {
					points[index + 1] += delta;
				}
			}
		}
		points[index] = point;
		EnforceMode(index);
	}

	public BezierControlPointMode GetControlPointMode (int index) {
		return modes[(index + 1) / 3];
	}

	public void SetControlPointMode (int index, BezierControlPointMode mode) {
		int modeIndex = (index + 1) / 3;
		modes[modeIndex] = mode;
		if (loop) {
			if (modeIndex == 0) {
				modes[modes.Length - 1] = mode;
			}
			else if (modeIndex == modes.Length - 1) {
				modes[0] = mode;
			}
		}
		EnforceMode(index);
	}

	private void EnforceMode (int index) {
		int modeIndex = (index + 1) / 3;
		BezierControlPointMode mode = modes[modeIndex];
		if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1)) {
			return;
		}

		int middleIndex = modeIndex * 3;
		int fixedIndex, enforcedIndex;
		if (index <= middleIndex) {
			fixedIndex = middleIndex - 1;
			if (fixedIndex < 0) {
				fixedIndex = points.Length - 2;
			}
			enforcedIndex = middleIndex + 1;
			if (enforcedIndex >= points.Length) {
				enforcedIndex = 1;
			}
		}
		else {
			fixedIndex = middleIndex + 1;
			if (fixedIndex >= points.Length) {
				fixedIndex = 1;
			}
			enforcedIndex = middleIndex - 1;
			if (enforcedIndex < 0) {
				enforcedIndex = points.Length - 2;
			}
		}

		Vector3 middle = points[middleIndex];
		Vector3 enforcedTangent = middle - points[fixedIndex];
		if (mode == BezierControlPointMode.Aligned) {
			enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
		}
		points[enforcedIndex] = middle + enforcedTangent;
	}

	public int CurveCount {
		get {
			return (points.Length - 1) / 3;
		}
	}

	public Vector3 GetPoint (float t) 
    {
		int i;
		if (t >= 1f) 
        {
			t = 1f;
			i = points.Length - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
	}

    public Vector3 GetPointForCurveAtIndex(float t, int curveIndex)
    {
        curveIndex = Mathf.Clamp(curveIndex, 0, CurveCount);

        int i = curveIndex * 3;

        return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
    }
	
	public Vector3 GetVelocity (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Length - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
	}

    public Vector3 GetVelocityForCurveAtIndex(float t, int curveIndex)
    {
        curveIndex = Mathf.Clamp(curveIndex, 0, CurveCount);

        int i = curveIndex * 3;

        return transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
    }
	
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}

    public Vector3 GetDirectionForCurveAtIndex(float t, int curveIndex)
    {
        return GetVelocityForCurveAtIndex(t, curveIndex).normalized;
    }

    public void AddCurve (float offset, Vector3 axis, int curveIndex = -1) 
    {
		Vector3 point = points[points.Length - 1];
		Array.Resize(ref points, points.Length + 3);
	    
        point = points[points.Length - 4] + axis * 0.3f * offset;
		points[points.Length - 3] = point;
        point = points[points.Length - 4] + axis * 0.7f * offset;
		points[points.Length - 2] = point;
        point = points[points.Length - 4] + axis * offset;
		points[points.Length - 1] = point;

		Array.Resize(ref modes, modes.Length + 1);
		modes[modes.Length - 1] = modes[modes.Length - 2];

        if (curveIndex >= 0)
        {
            int pointIndex = curveIndex * 3 + 1;

            for (int i = points.Length - 4; i >= pointIndex; i--)
            {
                points[i + 3] = points[i];
            }

            EnforceMode(pointIndex);
        }

		EnforceMode(points.Length - 4);

		if (loop) {
			points[points.Length - 1] = points[0];
			modes[modes.Length - 1] = modes[0];
			EnforceMode(0);
		}
	}

    public void AddCurve(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        Array.Resize(ref points, points.Length + 3);
        points[points.Length - 3] = p0;
        points[points.Length - 2] = p1;
        points[points.Length - 1] = p2;

        Array.Resize(ref modes, modes.Length + 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];

        if (loop) {
            points[points.Length - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode(0);
        }
    }

    public void RemoveCurve(int curveIndex)
    {
        if (CurveCount <= 1)
        {
            return;
        }

        int pointIndex = curveIndex * 3 + 1;

        for (int i = pointIndex; i < points.Length - 3; i++)
        {
            points[i] = points[i + 3];
        }

        Array.Resize(ref points, points.Length - 3);
        Array.Resize(ref modes, modes.Length - 1);

        EnforceMode(points.Length - 4);

        if (loop) {
            points[points.Length - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode(0);
        }
    }
	
	public void Reset () {
		points = new Vector3[] {
			new Vector3(0f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
		modes = new BezierControlPointMode[] {
			BezierControlPointMode.Free,
			BezierControlPointMode.Free
		};
	}
}
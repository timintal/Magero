using UnityEngine;
using System.Collections.Generic;

public static class Bezier {

	public static Vector3 GetPoint(Vector3[] points, float t)
	{
		if (points.Length < 2 || points.Length > 4)
		{
			Debug.LogError($"Bezier: Invalid number of points: {points.Length}");
		}
		
		switch (points.Length)
		{
			case 2:
				return Vector3.Lerp(points[0], points[1], t);
			case 3:
				return GetPoint(points[0], points[1], points[2], t);
			case 4:
				return GetPoint(points[0], points[1], points[2], points[3], t);
			default:
				return Vector3.zero;
		}
			
	}
	
	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			oneMinusT * oneMinusT * p0 +
			2f * oneMinusT * t * p1 +
			t * t * p2;
	}

	public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		return
			2f * (1f - t) * (p1 - p0) +
			2f * t * (p2 - p1);
	}

	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float OneMinusT = 1f - t;
		return
			OneMinusT * OneMinusT * OneMinusT * p0 +
			3f * OneMinusT * OneMinusT * t * p1 +
			3f * OneMinusT * t * t * p2 +
			t * t * t * p3;
	}

	public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			3f * oneMinusT * oneMinusT * (p1 - p0) +
			6f * oneMinusT * t * (p2 - p1) +
			3f * t * t * (p3 - p2);
	}

    const float LENGTH_CALCULATION_EPSILON = 0.1f;
    const float MAX_ITERATIONS = 8;
    public static float GetBezierLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float precision = LENGTH_CALCULATION_EPSILON)
    {
        List<Vector3> curvePoints = new List<Vector3>();

        curvePoints.Add(p0);
        curvePoints.Add(p1);

        List<Vector3> tempPoints = new List<Vector3>();

        float currentStep = 1;

        float currentLength = (p3 - p0).magnitude;

        float previousLength = 0;

        int iterations = 0;

        while (Mathf.Abs(currentLength - previousLength) > precision)
        {
            currentStep /= 2;
            tempPoints.Clear();
            previousLength = currentLength;
            iterations++;

            float currentFactor = 0;

            for (int i = 0; i < curvePoints.Count - 1; i++)
            {
                var p = curvePoints[i];

                tempPoints.Add(p);
                tempPoints.Add(Bezier.GetPoint(p0, p1, p2, p3, currentFactor + currentStep));

                currentFactor += currentStep * 2;
            }

            tempPoints.Add(p3);

            curvePoints.Clear();
            curvePoints.AddRange(tempPoints);

            currentLength = GetApproximatedLength(tempPoints);

            if (iterations > MAX_ITERATIONS)
            {
                break;
            }
        }

        return currentLength;
    }

    static float GetApproximatedLength(List<Vector3> points)
    {
        float length = 0;

        for (int i = 0; i < points.Count - 1; i++)
        {
            length += (points[i + 1] - points[i]).magnitude;
        }

        return length;
    }


    public static float GetBezierLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int subDivisions)
    {

        float length = 0;

        float step = 1f / (float)subDivisions;

        float current_t = 0;

        Vector3 previousPoint = p0;

        while (current_t < 1)
        {
            current_t += step;

            Vector3 p = Bezier.GetPoint(p0, p1, p2, p3, current_t);
            length += (p - previousPoint).magnitude;

            previousPoint = p;
        }

        return length;
    }
}
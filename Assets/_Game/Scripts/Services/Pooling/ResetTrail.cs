using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTrail : MonoBehaviour, IPoolReset
{
    [SerializeField] private TrailRenderer[] trails;

    public void ResetForReuse()
    {
        StartCoroutine(ResetTrailRoutine(trails));
    }

    
    static IEnumerator ResetTrailRoutine(TrailRenderer[] trails)
    {
        List<float> trailTimes = new();
        foreach (var trail in trails)
        {
            trailTimes.Add(trail.time);
            trail.time = 0;
            trail.enabled = false;
        }

        yield return null;

        for (var i = 0; i < trails.Length; i++)
        {
            var trail = trails[i];
            trail.time = trailTimes[i];
            trail.enabled = true;
        }
    }
}
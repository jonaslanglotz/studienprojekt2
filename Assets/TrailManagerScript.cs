using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrailManagerScript : MonoBehaviour
{
    private Dictionary<GameObject, LineRenderer> TrackedObjectsToRenderers = new Dictionary<GameObject, LineRenderer>();
    private Dictionary<LineRenderer, float> RenderersToLastUpdates = new Dictionary<LineRenderer, float>();
    public GameObject trailPrefab;

    // Update is called once per frame
    private void Update()
    {
        var currentCamera = Camera.main;

        var newRenderersToLastUpdates = new Dictionary<LineRenderer, float>();

        foreach (var rendererToLastUpdate in RenderersToLastUpdates)
        {
            if (Time.time - rendererToLastUpdate.Value > 3)
            {
                StartCoroutine(RemoveRenderer(rendererToLastUpdate.Key));
            }
            else
            {
                newRenderersToLastUpdates.Add(rendererToLastUpdate.Key, rendererToLastUpdate.Value);
            }
        }

        RenderersToLastUpdates = newRenderersToLastUpdates;

        foreach (var lineRenderer in TrackedObjectsToRenderers.Values)
        {
            if (currentCamera == null || lineRenderer == null || lineRenderer.positionCount == 0)
            {
                continue;
            }

            var distance = (currentCamera.transform.position - lineRenderer.GetPosition(0)).magnitude;

            lineRenderer.widthMultiplier = Mathf.Max(distance/ 100, 1);

        }
    }

    public void ReportPosition(GameObject caller, Vector3 position)
    {
        if (!TrackedObjectsToRenderers.TryGetValue(caller, out var lineRenderer))
        {
            var parent = Instantiate(trailPrefab, transform);
            lineRenderer = parent.GetComponent<LineRenderer>();
            TrackedObjectsToRenderers[caller] = lineRenderer;

            var color = caller.TryGetComponent<UnguidedMissile>(out _) ? Color.red : Color.blue;
            lineRenderer.startColor = color;
            color.a = 0;
            lineRenderer.endColor = color;
        }

        if (lineRenderer == null)
        {
            return;
        }
        
        AddPointToRenderer(lineRenderer, position);
        RenderersToLastUpdates[lineRenderer] = Time.time;
    }

    private static void AddPointToRenderer(LineRenderer lineRenderer, Vector3 point)
    {

        lineRenderer.positionCount = Math.Min(lineRenderer.positionCount + 1, 2000);
        
        var positions = new Vector3[lineRenderer.positionCount + 1];
        lineRenderer.GetPositions(positions);

        positions = positions.Prepend(point).ToArray();
        
        lineRenderer.SetPositions(positions);
    }

    private IEnumerator RemoveRenderer(LineRenderer lineRenderer)
    {
        for (var i = lineRenderer.positionCount; i >= 0 ; i--)
        {
            lineRenderer.positionCount = i;
            yield return new WaitForEndOfFrame();
        }

        var pair = TrackedObjectsToRenderers.FirstOrDefault(pair => pair.Value == lineRenderer);
        TrackedObjectsToRenderers.Remove(pair.Key);
        
        Destroy(lineRenderer);

        yield return null;
    }
}

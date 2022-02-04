using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DetailsScript : MonoBehaviour
{

    public TextMeshProUGUI startTimeLabel;
    public TextMeshProUGUI startBaseLabel;
    public TextMeshProUGUI targetLabel;
    public TextMeshProUGUI velocityLabel;
    public TextMeshProUGUI heightLabel;
    
    public TextMeshProUGUI launchAngleLabel;
    public TextMeshProUGUI fireTimeLabel;
    public TextMeshProUGUI etaLabel;

    public GameObject launchAnglePanel;
    public GameObject fireTimePanel;
    public GameObject etaPanel;

    public GameObject selectedRocket;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var raycastResults = GetEventSystemRaycastResults();
            
            Debug.Log(raycastResults.Count);

            if (raycastResults.Count == 0)
            {
                selectedRocket = null;
            }
            
            foreach (var raycastResult in raycastResults.Where(raycastResult => raycastResult.gameObject.CompareTag("RocketIcon")))
            {
                selectedRocket = RocketIconScript.IconsToRocketObject[raycastResult.gameObject];
                Debug.Log(raycastResult.gameObject);
                break;
            }
        }
        
        //Debug.Log(selectedRocket);
        
        var rocketComponent = selectedRocket != null ? selectedRocket.GetComponent<MissileScript>(): null;

        startTimeLabel.text = rocketComponent != null ? rocketComponent.initializationTime.ToString() : "/";
        startBaseLabel.text = rocketComponent != null ? rocketComponent.launcher.name : "/";
        targetLabel.text = rocketComponent != null ? rocketComponent.target.name : "/";
        velocityLabel.text = rocketComponent != null ? rocketComponent.rb.velocity.magnitude.ToString() : "/";
        heightLabel.text = rocketComponent != null ? rocketComponent.transform.position.y.ToString() : "/";

        var unguidedMissileComponent = selectedRocket != null ? selectedRocket.GetComponent<UnguidedMissile>(): null;

        var hideUnguidedSection = unguidedMissileComponent == null;
        launchAnglePanel.SetActive(!hideUnguidedSection);
        fireTimePanel.SetActive(!hideUnguidedSection);
        etaPanel.SetActive(!hideUnguidedSection);

        if (hideUnguidedSection)
        {
            return; 
        }

        launchAngleLabel.text = unguidedMissileComponent.launchAngle.ToString();
        fireTimeLabel.text = unguidedMissileComponent.fireTime.ToString();
        etaLabel.text = unguidedMissileComponent.eta.ToString();

    }
    
    private static List<RaycastResult> GetEventSystemRaycastResults()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }
}

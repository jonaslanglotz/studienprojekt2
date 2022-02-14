using System.Linq;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class DetailsScript : MonoBehaviour
{

    public TextMeshProUGUI startTimeLabel;
    public TextMeshProUGUI startBaseLabel;
    public TextMeshProUGUI targetLabel;
    public TextMeshProUGUI velocityLabel;
    public TextMeshProUGUI heightLabel;
    
    public TextMeshProUGUI launchAngleLabel;
    public TextMeshProUGUI fireTimeLabel;

    public GameObject launchAnglePanel;
    public GameObject fireTimePanel;

    public Button cameraButton;

    public GameObject selectedRocket;

    private void Start()
    {
        cameraButton.onClick.AddListener(SelectRocketCamera);
    }

    private void SelectRocketCamera()
    {
        if (selectedRocket == null)
        {
            return;
        }

        var rocketCamera = selectedRocket.GetComponentInChildren<Camera>();
        
        CameraManager.SetCamera(rocketCamera);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var raycastResults = Util.GetEventSystemRaycastResults();
            
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

        startTimeLabel.text = rocketComponent != null ? rocketComponent.initializationTime.ToString("n1") + " s" : "/";
        startBaseLabel.text = rocketComponent != null ? rocketComponent.launcher.name : "/";
        targetLabel.text = rocketComponent != null ? rocketComponent.target.name : "/";
        velocityLabel.text = rocketComponent != null ? rocketComponent.rb.velocity.magnitude.ToString("n1") + " m/s" : "/";
        heightLabel.text = rocketComponent != null ? rocketComponent.transform.position.y.ToString("n1") + " m" : "/";

        var unguidedMissileComponent = selectedRocket != null ? selectedRocket.GetComponent<UnguidedMissile>(): null;

        var hideUnguidedSection = unguidedMissileComponent == null;
        launchAnglePanel.SetActive(!hideUnguidedSection);
        fireTimePanel.SetActive(!hideUnguidedSection);

        if (hideUnguidedSection)
        {
            return; 
        }

        launchAngleLabel.text = unguidedMissileComponent.launchAngle.ToString("n1") + "°";
        fireTimeLabel.text = unguidedMissileComponent.fireTime.ToString("n1") + " s";

    }
    
}

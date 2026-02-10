using UnityEngine;

public class HitCheckAction : MonoBehaviour
{
    [SerializeField] private string targetTag = "HeatArea";
    [SerializeField] private TemperatureGauge gauge;

    private bool isHit = false;

    void Start() 
    {

        if (gauge == null)
        {
            gauge = GetComponent<TemperatureGauge>();
        }
        // ActionFunc ‚Éu“–‚½‚è”»’è‚ğ•Ô‚·ŠÖ”v‚ğ“o˜^
        gauge.ActionFunc = IsHitting;
    }

    // TemperatureGauge ‚©‚çŒÄ‚Î‚ê‚éŠÖ”
    private bool IsHitting()
    {
        return isHit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("“–‚½‚Á‚Ä‚¢‚é");
            isHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            isHit = false;
        }
    }
}

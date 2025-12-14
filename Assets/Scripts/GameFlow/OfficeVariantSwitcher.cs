using UnityEngine;

public class OfficeVariantSwitcher : MonoBehaviour
{
    public GameObject officeNormal;
    public GameObject officeAnomaly;

    public int pressCount = 0;
    public int switchAtCount = 1;

    private void Start()
    {
        SetNormal();
    }
    
    public void OnElevatorButtonPressed()
    {
        Debug.Log("[OnElevatorButtonPressed] ELEVATOR BUTTON PRESSED");
        pressCount++;

        if (pressCount >= switchAtCount)
            SetAnomaly();
        else
            SetNormal();
    }

    public void SetNormal()
    {
        if (officeNormal != null) officeNormal.SetActive(true);
        if (officeAnomaly != null) officeAnomaly.SetActive(false);
    }

    public void SetAnomaly()
    {
        if (officeNormal != null) officeNormal.SetActive(false);
        if (officeAnomaly != null) officeAnomaly.SetActive(true);
    }
}
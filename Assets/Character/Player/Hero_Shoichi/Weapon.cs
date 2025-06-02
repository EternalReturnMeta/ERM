using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject briefCase;

    private void Awake()
    {
        briefCase.transform.SetParent(leftHand.transform);
        briefCase.transform.localPosition = new Vector3(0f, 0f, -0.06f);
        briefCase.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        
        knife.transform.SetParent(rightHand.transform);
        knife.transform.localPosition = new Vector3(0f, 0f, -0.06f);
    }
}

using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject briefCase;

    private void Awake()
    {
        // briefCase.transform.SetParent(leftHand.transform);
        // briefCase.transform.localPosition = Vector3.zero;
        // briefCase.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        //
        // knife.transform.SetParent(rightHand.transform);
        // knife.transform.localPosition = Vector3.zero;
    }
}

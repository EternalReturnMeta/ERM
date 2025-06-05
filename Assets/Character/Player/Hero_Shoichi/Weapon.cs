using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject briefCase;

    public void OnDeath()
    {
        knife.SetActive(false);
        briefCase.SetActive(false);
    }
}

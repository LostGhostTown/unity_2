using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWaiter : MonoBehaviour
{
    public static SwitchWaiter Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public Transform groups;

    void Start()
    {
        groups.GetChild(0).gameObject.SetActive(false);
        groups.GetChild(1).gameObject.SetActive(false);
        int i= Random.Range(0,2);
        groups.GetChild(i).gameObject.SetActive(true);

    }

    public void SwitchWaiterDisplay()
    {
        GameObject waiter1= groups.GetChild(0).gameObject;
        GameObject waiter2= groups.GetChild(1).gameObject;

        if(waiter1.activeSelf)
        {
            waiter1.SetActive(false);
            waiter2.SetActive(true);
        }
        else
        {
            waiter1.SetActive(true);
            waiter2.SetActive(false);
        }
    }
}

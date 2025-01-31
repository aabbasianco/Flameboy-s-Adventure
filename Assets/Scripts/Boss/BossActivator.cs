using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject bossToActiivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            bossToActiivate.SetActive(true);

            gameObject.SetActive(false);
        }
    }
}

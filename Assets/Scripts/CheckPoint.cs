using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject effect;
    public Transform effectPoint;
    public Animator anim;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && LevelManager.instance.respawnPoint != transform.position)
        {
            LevelManager.instance.respawnPoint = transform.position;

            if (effect != null)
            {
                Instantiate(effect, effectPoint.position, Quaternion.identity);
            }

            CheckPoint[] allCP = FindObjectsOfType<CheckPoint>();
            foreach (CheckPoint cp in allCP)
            {
                cp.anim.SetBool("active", false);
            }

            anim.SetBool("active", true);

            AudioManager.instance.PlaySFX(3);
        }
    }
}

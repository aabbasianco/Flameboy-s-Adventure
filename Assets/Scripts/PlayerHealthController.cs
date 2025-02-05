using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    private void Awake()
    {
        instance = this;
    }

    private int currentHealth;
    public int maxHealth;
    public float invincibilityLength = 1f;
    private float invincCounter;
    public GameObject[] modelDisplay;
    private float flashCounter;
    public float flashTime = .1f;

    // Start is called before the first frame update
    void Start()
    {
        FillHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;

            if (flashCounter <= 0)
            {
                flashCounter = flashTime;
                foreach (GameObject piece in modelDisplay)
                {
                    piece.SetActive(!piece.activeSelf);
                }
            }
            flashCounter -= Time.deltaTime;

            if (invincCounter <= 0)
            {
                foreach (GameObject piece in modelDisplay)
                {
                    piece.SetActive(true);
                }
            }
        }
    }

    public void DamagePlayer()
    {
        if (invincCounter <= 0)
        {
            invincCounter = invincibilityLength;

            currentHealth--;
            if (currentHealth <= 0)
            {
                LevelManager.instance.Respawn();
            }
            else
            {
                AudioManager.instance.PlaySFX(12);
            }
            UIController.Instance.UpdateHealthDisplay(currentHealth);
        }
    }

    public void FillHealth()
    {
        currentHealth = maxHealth;

        UIController.Instance.UpdateHealthDisplay(currentHealth);
    }
}

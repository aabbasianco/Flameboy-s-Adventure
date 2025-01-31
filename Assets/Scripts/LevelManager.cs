using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private void Awake()
    {
        instance = this;

        currentCoins = PlayerPrefs.GetInt("Coins");
        currentCrystals = PlayerPrefs.GetInt("Crystals");
    }

    [HideInInspector]
    public Vector3 respawnPoint;

    private CameraController cam;

    [HideInInspector]
    public float levelTimer;

    public float waitBeforRespawning;

    [HideInInspector]
    public bool respawning;

    private PlayerController player;

    public int currentCoins, coinThreshold = 100, currentCrystals;

    public float waitToEndLevel = 1;

    [HideInInspector]
    public bool levelCompelete;

    public bool reloadOnRespawn;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        respawnPoint = player.transform.position;

        cam = GameObject.FindObjectOfType<CameraController>();

        UIController.Instance.CoinText.text = currentCoins.ToString();
        UIController.Instance.CrystalText.text = currentCrystals.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelCompelete)
        {
            levelTimer += Time.deltaTime;
            UIController.Instance.timeText.text = levelTimer.ToString("0");
        }
        else
        {
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.Euler(0f, 180f, 0f), 10f * Time.deltaTime);
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
        }
#endif
    }


    public void Respawn()
    {
        if (!respawning)
        {
            respawning = true;

            StartCoroutine(RespawnCo());
        }
    }

    public IEnumerator RespawnCo()
    {
        player.gameObject.SetActive(false);

        UIController.Instance.FadeToBlack();

        AudioManager.instance.PlaySFX(10);

        yield return new WaitForSeconds(waitBeforRespawning);

        if (reloadOnRespawn)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            player.transform.position = respawnPoint;

            cam.SnapToTarget();

            player.gameObject.SetActive(true);

            respawning = false;

            UIController.Instance.FadeFromBlack();

            PlayerHealthController.instance.FillHealth();
        }
    }

    public void GetCoin()
    {
        currentCoins++;

        if (currentCoins >= coinThreshold)
        {
            GetCrystal();
            currentCoins -= coinThreshold;
        }

        UIController.Instance.CoinText.text = currentCoins.ToString();

        PlayerPrefs.SetInt("Coins", currentCoins);
    }
    public void GetCrystal()
    {
        currentCrystals++;
        UIController.Instance.CrystalText.text = currentCrystals.ToString();

        PlayerPrefs.SetInt("Crystals", currentCrystals);
    }

    public void EndLevel(string nextLevel)
    {
        StartCoroutine(EndLevelCo(nextLevel));
    }

    IEnumerator EndLevelCo(string nextLevel)
    {
        levelCompelete = true;

        player.anim.SetTrigger("endLevel");

        yield return new WaitForSeconds(waitToEndLevel - 1f);

        UIController.Instance.FadeToBlack();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(nextLevel);
    }
}

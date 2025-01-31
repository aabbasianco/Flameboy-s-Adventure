using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelSelectController : MonoBehaviour
{
    public static LevelSelectController instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject levelInfoBox;
    public TMP_Text levelText, actionText;

    // Start is called before the first frame update
    void Start()
    {        
        AudioManager.instance.PlayMusic(1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] SaveLevel saveLevel;
    [SerializeField] int buttonLevel;
    [SerializeField] Sprite unlockSprite;
    [SerializeField] Sprite lockedSprite;

    private int currentLevel;
    private Button button;
    private Image img;
    private Animator anim;

    private
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = saveLevel.LoadGame();
        button = GetComponent<Button>();
        img = GetComponent<Image>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentLevel >= buttonLevel)
        {
            button.interactable = true;
            img.sprite = unlockSprite;
            anim.enabled = true;
        }
        else
        {
            button.interactable = false;
            img.sprite = lockedSprite;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Sprite onHiLight;
    [SerializeField] Sprite offHiLight;
    [SerializeField] string spriteName;

    private Image img;
    private Button button;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        img = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();
        anim = gameObject.GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.interactable = true;
        img.sprite = onHiLight;
        anim.enabled = true;
        anim.Play(spriteName, -1, 0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.interactable = false;
        img.sprite = offHiLight;
        anim.enabled = false;
    }
}

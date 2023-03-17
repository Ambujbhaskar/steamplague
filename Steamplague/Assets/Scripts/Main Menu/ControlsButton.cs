using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsButton : MonoBehaviour
{

    [SerializeField] private Sprite normalControlsButton;
    [SerializeField] private Sprite glowControlsButton;
    [SerializeField] private SpriteRenderer controlsButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        controlsButton.sprite = glowControlsButton;
    }

    void OnMouseExit()
    {
        controlsButton.sprite = normalControlsButton;
    }

    void OnMouseDown()
    {
        print("Controls Button Clicked");
        SceneManager.LoadScene("ControlScene");
    }
}

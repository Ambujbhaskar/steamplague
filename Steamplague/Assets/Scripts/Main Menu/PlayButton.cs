using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{

    [SerializeField] private Sprite normalPlayButton;
    [SerializeField] private Sprite glowPlayButton;
    [SerializeField] private SpriteRenderer playButton;

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
        playButton.sprite = glowPlayButton;
    }

    void OnMouseExit()
    {
        playButton.sprite = normalPlayButton;
    }

    void OnMouseDown()
    {
        print("Play Button Clicked");
        SceneManager.LoadScene("backupScene");
    }
}

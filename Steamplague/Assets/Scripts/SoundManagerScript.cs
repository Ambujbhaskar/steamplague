using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    [SerializeField] private AudioClip player_attack, pickup_item, player_dying, player_hurt, player_jump, player_hit, player_swing, zombie_attack, zombie_death, zombie_hurt;
    static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSound(string sfx)
    {
        switch (sfx)
        {
            case "player_attack":
                audioSrc.PlayOneShot(player_attack);
                break;
            case "pickup_item":
                audioSrc.PlayOneShot(pickup_item);
                break;
            case "player_dying":
                audioSrc.PlayOneShot(player_dying);
                break;
            case "player_hurt":
                audioSrc.PlayOneShot(player_hurt);
                break;
            case "player_jump":
                audioSrc.PlayOneShot(player_jump);
                break;
            case "player_hit":
                audioSrc.PlayOneShot(player_hit);
                break;
            case "player_swing":
                audioSrc.PlayOneShot(player_swing);
                break;
            case "zombie_attack":
                audioSrc.PlayOneShot(zombie_attack);
                break;
            case "zombie_death":
                audioSrc.PlayOneShot(zombie_death);
                break;
            case "zombie_hurt":
                audioSrc.PlayOneShot(zombie_hurt);
                break;
        }
    }
}
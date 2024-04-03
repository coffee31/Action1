using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDam : MonoBehaviour
{
    Player player;
    MainUI mainUI;

    private void Start()
    {
        mainUI = FindObjectOfType<MainUI>();
        player = FindObjectOfType<Player>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            player.CurHP -= 20;
            mainUI.HPslider();
        }
    }
}

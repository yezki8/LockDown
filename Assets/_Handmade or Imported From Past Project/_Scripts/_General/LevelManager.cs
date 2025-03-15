using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Players and environment")]
    public GameObject Player;
    public GameObject LastCheckpoint;
    public GameObject DeathPanel;
    public GameObject FinishPanel;

    [Header("Statistic")]
    public int deathCount;
    public int killCount;

    public enum GameState
    {
        playing,
        notPlaying,
        pause,
        gameOver
    }
    public GameState gameState;
    // Start is called before the first frame update
    void Start()
    {
        DeathPanel.SetActive(false);
        gameState = GameState.playing;
        Player = GameObject.FindGameObjectWithTag("Player");
        LastCheckpoint = new GameObject("First spawn location");
        LastCheckpoint.transform.position = Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {        
        CheckState();
    }

    void CheckState()
    {
        if (gameState == GameState.gameOver)
        {
            DeathPanel.SetActive(true);
        }
        else
        {
            //Check Player
            if (Player.GetComponent<PlayerManager>().aliveState == PlayerManager.AliveState.dead)
            {
                gameState = GameState.gameOver;
            }
            DeathPanel.SetActive(false);
        }
        if (gameState == GameState.notPlaying)
        {
            FinishPanel.SetActive(true);
        }
    }

    //For Buttons
    public void Respawn()
    {
        Player.SetActive(true);
        Player.GetComponent<PlayerManager>().aliveState = PlayerManager.AliveState.alive;
        Player.GetComponent<PlayerManager>().PlayerRespawn(LastCheckpoint.transform);
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        deathCount += 1;
        gameState = GameState.playing;
    }
}

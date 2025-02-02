﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public ParticleSystem enemyExplosion;
    public ParticleSystem playerExplosion;

    public int lives = 3;
    public int score = 0;

    public float respawnTime = 3.0f;

    public void PlayerDestroyed()
    {
        playerExplosion.transform.position = player.transform.position;
        playerExplosion.Play();

        lives--;

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), respawnTime);
        }
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        Score.scoreValue += 50;
        explosion.transform.position = asteroid.transform.position;
        explosion.Play();
    }

    public void EnemyDestroyed(Enemy enemy)
    {
        Score.scoreValue += 200;
        enemyExplosion.transform.position = enemy.transform.position;
        enemyExplosion.Play();
    }

    private void Respawn()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        lives = 3;

        SceneManager.LoadScene("Game Over");
    }
}

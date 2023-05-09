using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Apple : MonoBehaviour
{
    public BoxCollider2D grid;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    int currentScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        highscoreText.text = currentHighScore.ToString();

        InvokeRepeating("RandomFoodPlacement", 0, 10);
        scoreText.text = score.ToString();
    }

    public void RandomFoodPlacement()
    {
        Bounds bounds = this.grid.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            RandomFoodPlacement();
            CancelInvoke();
            InvokeRepeating("RandomFoodPlacement", 0, 10);
                        
            int pointsToAdd = 1 + (currentScore / 5);
            score += pointsToAdd;
            currentScore++;
            
            // Update high score if necessary
            int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
            if (score > currentHighScore)
            {
                PlayerPrefs.SetInt("HighScore", score);
                PlayerPrefs.Save();
                highscoreText.text = score.ToString();
            }
            
            scoreText.text = score.ToString();
        }
    }

}

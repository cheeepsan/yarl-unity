using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
public class SceneLoader : MonoBehaviour
{

    bool isMainMenu = true;
    public Text highScoreText;
    // Start is called before the first frame update
    void Start()
    {
        ShowHighScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameScene()
    {
        SceneManager.LoadScene("GameStartScene", LoadSceneMode.Single);
    }


    public void ShowHighScore()
    {
        string score = "Highscore: \n";
        string scoreJson = "";
        List<HighScore> hList = new List<HighScore>();
        
        if (System.IO.File.Exists("score.json"))
        {
            scoreJson = System.IO.File.ReadAllText("score.json");
            hList = JsonUtility.FromJson<HighScoreWrapper>(scoreJson).l;
            hList = hList.OrderByDescending(x => x.score).Take(10).ToList();
        }

        for(int i = 0; i < hList.Count(); i++)
        {
            score += (i + 1) + ". " + hList[i].name + " : " + hList[i].score + "\n";
        }

        this.highScoreText.text = score;
    }

    public void Quit()
    {
        Application.Quit();
    }
}

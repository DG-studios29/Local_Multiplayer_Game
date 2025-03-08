using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           
            

                pausePanel.SetActive(true);
           

        }
    }



    void Awake ()
    {

        pausePanel.SetActive (false);

    }



    public void GamePaused ()
    {

        pausePanel.SetActive(true);

    }


    public void BackToGame()
    {

        pausePanel.SetActive(false);

    }


}

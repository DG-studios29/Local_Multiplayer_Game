using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{

    public void PlayScene()
    {

        SceneManager.LoadScene("GamePlay");

    }

    public void ControlsScene()
    {

        //SceneManager.LoadScene("Controls");

    }


    public void ExitGame()
    {

        Application.Quit();

    }


    public void BackToMenu()
    {

        SceneManager.LoadScene("MainMenu");

    }





}

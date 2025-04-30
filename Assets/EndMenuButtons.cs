using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Igra bi sada trebalo da se zatvori");
    }
}

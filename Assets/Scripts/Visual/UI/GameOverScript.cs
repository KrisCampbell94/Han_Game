using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public static string SceneToLoad { get; set; }
    public void TryAgain()
    {
        SceneManager.LoadScene(SceneToLoad);
    }
}

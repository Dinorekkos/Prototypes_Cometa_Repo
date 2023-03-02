using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RythmResetScene : MonoBehaviour
{
    public void ResetScene()
    {
        StartCoroutine("WaitToReset");
    }

    IEnumerator WaitToReset()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

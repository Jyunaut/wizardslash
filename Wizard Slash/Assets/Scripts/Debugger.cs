using UnityEngine;
using UnityEngine.SceneManagement;

public class Debugger : MonoBehaviour
{   
    [Range(0, 1)] public float timeSlow;
    
    void Update()
    {
        // ay debuggin'
        if (Input.GetKeyDown("["))
        {
            Time.timeScale = timeSlow;
        }
        else if (Input.GetKeyDown("]"))
        {
            Time.timeScale = 1f;
        }
        else if (Input.GetKey("p"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void openScene(Component sender, object data)
    {
        Debug.Log("prada");
        if (data is string) {
            SceneManager.LoadScene((string)data);
        }
    }
}

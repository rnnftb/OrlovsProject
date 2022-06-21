using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayGame : MonoBehaviour
{
    public Button PlayBtn;
    // Start is called before the first frame update
    void Start()
    {
        PlayBtn.onClick.AddListener (TaskOnClick);
    }

    // Update is called once per frame
    void TaskOnClick()
    {
        SceneManager.LoadScene(1);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene(1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour
{
    public PortalScript ps;
    public GameObject mainMenu, optionsMenu;
    public GameObject playButton, volumeSlider, optionsButton;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("ControllerSelect") == -1)
        {
            Debug.Log("Circle");
        }
    }

    public void PlayGame()
    {
        ps.active = true;
        mainMenu.SetActive(false);
        ps.SendMessage("EnterPortal");
    }

    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(volumeSlider);
    }

    public void CloseOptions()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(optionsButton);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}

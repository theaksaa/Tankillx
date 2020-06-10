using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Slider bg_volume_slider;
    public Slider sfx_volume_slider;

    public void SingleplayerGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MultiplayerGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Load()
    {
        if (File.Exists(Path.Combine(Application.dataPath, "options.txt")))
        {
            string[] options = File.ReadAllText(Path.Combine(Application.dataPath, "options.txt")).Split('|');
            float bg_volume = 0f, sfx_volume = 0f;
            try
            {
                if (float.TryParse(options[0], out bg_volume) && float.TryParse(options[1], out sfx_volume))
                {
                    bg_volume_slider.value = bg_volume;
                    sfx_volume_slider.value = sfx_volume;
                }
                else
                {
                    Save(0.5f, 0.5f);
                    Load();
                    return;
                }
            }
            catch(Exception e)
            {
                Save(0.5f, 0.5f);
                Load();
                return;
            }
        }
        else Save(0.5f, 0.5f);

        Debug.Log("Succesfully loaded!");
    }

    public void Save()
    {
        Save(bg_volume_slider.value, sfx_volume_slider.value);
    }

    public void Save(float bg_volume, float sfx_volume)
    {
        File.WriteAllText(Path.Combine(Application.dataPath, "options.txt"), bg_volume.ToString() + '|' + sfx_volume.ToString());
        Debug.Log("Options saved!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/* This class is responsible for the backend of the application (not game related) */
public class app_manager : MonoBehaviour
{
    public db_manager db_man;

    public AudioSource main_menu_bg_sound_src;

    public AudioClip main_menu_bg_sound;

    public Text mini_log_txt;

    float mini_log_timer = 3.0f;

    #region Auxilaries
    public void mini_log_screen_info(string msg, float duration)
    {
        if (mini_log_txt)
        {
            mini_log_txt.text = msg;
            mini_log_timer = duration;
        }
    }
    void play_sound(AudioSource audio_src, AudioClip clip)
    {
        audio_src.clip = clip;
        audio_src.Play();
    }
    #endregion

    void Start()
    {
        // play background music initially on the main menu level
        if (main_menu_bg_sound && main_menu_bg_sound_src)
        {
            play_sound(main_menu_bg_sound_src, main_menu_bg_sound);
            mini_log_screen_info("Now Playing: 지민, 유나 (JIMIN, YuNa) (AOA) - 니가 나라면 (If You Were Me) (Feat. 유회승 of N.Flying) MV", 15.0f);
        }
    }
    void Update()
    {
        // mini log info time handling 
        if (mini_log_txt)
        {
            if (mini_log_timer <= 0.0f)
            {
                mini_log_txt.text = "";
            }
            else
            {
                mini_log_timer -= Time.deltaTime;
            }
        }
    }
    bool check_password_strength(string password)
    {
        string[] chars = {"!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "_", "+", "=", "~", "/", "\\", ">", "<", ":", ";"};
        if (password.Length >= 10)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                if (password.Contains(chars[i]))
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }
    public string check_login_credentials(string username, string password)
    {
        if (username == "" || password == "")
        {
            return "Empty Field";
        }
        else if (db_man.GetPassword(username) == password)
        {
            return "Success";
        }
        else
        {
            return "Failure";
        }
    }
    public string create_user(string username, string email, string gendre, string password, string confirmed_password)
    {
        if (db_man)
        {
            if (username == "" || email == "" || gendre == "" || password == "" || confirmed_password == "")
            {
                return "Empty Field";
            }
            else if (password != confirmed_password)
            {
                return "Password Mismatch";
            }
            else if (!(check_password_strength(password)))
            {
                return "Weak Password";
            }
            else if (!(email.Contains("@")))
            {
                return "Invalid Email";
            }
            else if (db_man.GetUsername(username) != null)
            {
                return "Username Taken";
            }
            else
            {
                db_man.CreateUser(username, gendre, email, password);
                return "Success";
            }
        }
        else
        {
            Debug.Log("DBMan is not assigned. Null exception handler");
            return "";
        }
    }
}

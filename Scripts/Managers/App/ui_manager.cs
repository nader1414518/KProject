using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleFileBrowser;
//using TestGoogleSearch;
//#if UNITY_EDITOR_WIN
//using UnityEditor;
//#endif
using System.IO;
//using System.Windows.Forms;
// any 

/* This class is a container for Global variables */
public class Globals
{
    public static bool is_user_logged_in = false;
    public static string username = "";
    public static int failed_login_attempts = 0;
    public static bool is_login_locked = false;
    public static string document_location = "";
}

/* This Class is responsible for managing the User Interface functionality */
public class ui_manager : MonoBehaviour
{
    public GameObject main_menu_panel;
    public GameObject pause_menu_panel;
    public GameObject login_panel;
    public GameObject signup_panel;
    public GameObject chat_panel;
    public GameObject slide_show_panel;
    public GameObject notifications_panel;
    public GameObject navigation_panel;
    public GameObject dashboard_panel;
    public GameObject music_panel;
    public GameObject pdf_processing_panel;
    public GameObject show_chat_btn;
    public GameObject show_music_player_btn;
    public GameObject process_pdf_btn;
    public GameObject message_box_panel;
    public GameObject google_search_panel_primary;
    public GameObject google_search_panel_secondary;

    public app_manager app_man;
    public HomeController home_controller;

    public InputField login_username_if;
    public InputField login_password_if;
    public InputField signup_username_if;
    public InputField signup_email_if;
    public InputField signup_password_if;
    public InputField signup_password_confirm_if;
    public InputField google_search_if_primary;
    public InputField google_search_if_secondary;

    public Scrollbar google_results_scrollbar;

    public Dropdown signup_gendre_dd;

    public Text log_txt;
    public Text pdf_output_text;

    public float log_timer = 3.0f;

    AsyncOperation asyncLoadLevel;

    #region TestVars
    public RawImage test_image;
    #endregion


    #region Auxilaries
    IEnumerator load_dashboard_level()
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync("Dashboard", LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            Debug.Log("Loading the Scene");
            yield return null;
        }
    }
    IEnumerator show_load_document_dialog_coroutine()
    {
        /* FileBrowser Initializations */
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Documents", ".pdf"));
        FileBrowser.SetDefaultFilter(".pdf");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load document", "Load");
        // Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
        if (FileBrowser.Success)
        {
            Debug.Log("Document lives at location: " + FileBrowser.Result);
            Globals.document_location = FileBrowser.Result;
            if (Globals.document_location != "")
            {
                if (pdf_output_text)
                {
                    pdf_output_text.text = document_scan_manager.scan_document(Globals.document_location);
                }
            }
        }
    }
    public void log_screen_info(string msg, float duration)
    {
        if (log_txt)
        {
            log_txt.text = msg;
            log_timer = duration;
        }
    }
    void display_message_box_simple(string message)
    {
        if (message_box_panel)
        {
            message_box_panel.GetComponentInChildren<Text>().text = message;
        }
    }
    void reset_scrollbar(Scrollbar scrollbar)
    {
        if (scrollbar)
        {
            scrollbar.value = 1;
        }
    }
    void hide_panels()
    {
        hide_main_menu();
        hide_pause_menu();
        hide_login_panel();
        hide_signup_panel();
        hide_chat_panel();
        hide_slide_show();
        hide_notifications_panel();
        hide_navigation_panel();
        hide_dashboard_panel();
        hide_music_panel();
        hide_pdf_processing_panel();
        hide_message_box_panel();
        hide_primary_google_search_panel();
        hide_secondary_google_search_panel();
    }
    void show_main_menu()
    {
        if (main_menu_panel)
        {
            main_menu_panel.SetActive(true);
        }
    }
    void hide_main_menu()
    {
        if (main_menu_panel)
        {
            main_menu_panel.SetActive(false);
        }
    }
    void show_pause_menu()
    {
        if (pause_menu_panel)
        {
            pause_menu_panel.SetActive(true);
        }
    }
    void hide_pause_menu()
    {
        if (pause_menu_panel)
        {
            pause_menu_panel.SetActive(false);
        }
    }
    void show_login_panel()
    {
        if (login_panel)
        {
            login_panel.SetActive(true);
        }
    }
    void hide_login_panel()
    {
        if (login_panel)
        {
            login_panel.SetActive(false);
        }
    }
    void show_signup_panel()
    {
        if (signup_panel)
        {
            signup_panel.SetActive(true);
        }
    }
    void hide_signup_panel()
    {
        if (signup_panel)
        {
            signup_panel.SetActive(false);
        }
    }
    void show_chat_panel()
    {
        if (chat_panel)
        {
            chat_panel.SetActive(true);
        }
    }
    void hide_chat_panel()
    {
        if (chat_panel)
        {
            chat_panel.SetActive(false);
        }
    }
    void show_slide_show()
    {
        if (slide_show_panel)
        {
            slide_show_panel.SetActive(true);
        }
    }
    void hide_slide_show()
    {
        if (slide_show_panel)
        {
            slide_show_panel.SetActive(false);
        }
    }
    void show_notifications_panel()
    {
        if (notifications_panel)
        {
            notifications_panel.SetActive(true);
        }
    }
    void hide_notifications_panel()
    {
        if (notifications_panel)
        {
            notifications_panel.SetActive(false);
        }
    }
    void show_navigation_panel()
    {
        if (navigation_panel)
        {
            navigation_panel.SetActive(true);
        }
    }
    void hide_navigation_panel()
    {
        if (navigation_panel)
        {
            navigation_panel.SetActive(false);
        }
    }
    void show_dashboard_panel()
    {
        if (dashboard_panel)
        {
            dashboard_panel.SetActive(true);
        }
    }
    void hide_dashboard_panel()
    {
        if (dashboard_panel)
        {
            dashboard_panel.SetActive(false);
        }
    }
    void show_show_chat_btn()
    {
        if (show_chat_btn)
        {
            show_chat_btn.SetActive(true);
        }
    }
    void hide_show_chat_btn()
    {
        if (show_chat_btn)
        {
            show_chat_btn.SetActive(false);
        }
    }
    void show_music_panel()
    {
        if (music_panel)
        {
            music_panel.SetActive(true);
        }
    }
    void hide_music_panel()
    {
        if (music_panel)
        {
            music_panel.SetActive(false);
        }
    }
    void show_pdf_processing_panel()
    {
        if (pdf_processing_panel)
        {
            pdf_processing_panel.SetActive(true);
        }
    }
    void hide_pdf_processing_panel()
    {
        if (pdf_processing_panel)
        {
            pdf_processing_panel.SetActive(false);
        }
    }
    void show_show_music_player_btn()
    {
        if (show_music_player_btn)
        {
            show_music_player_btn.SetActive(true);
        }
    }
    void hide_show_music_player_btn()
    {
        if (show_music_player_btn)
        {
            show_music_player_btn.SetActive(false);
        }
    }
    void show_process_pdf_btn()
    {
        if (process_pdf_btn)
        {
            process_pdf_btn.SetActive(true);

        }
    }
    void hide_process_pdf_btn()
    {
        if (process_pdf_btn)
        {
            process_pdf_btn.SetActive(false);
        }
    }
    void show_message_box_panel()
    {
        if (message_box_panel)
        {
            message_box_panel.SetActive(true);
        }
    }
    void hide_message_box_panel()
    {
        if (message_box_panel)
        {
            message_box_panel.SetActive(false);
        }
    }
    void show_primary_google_search_panel()
    {
        if (google_search_panel_primary)
        {
            google_search_panel_primary.SetActive(true);
        }
    }
    void hide_primary_google_search_panel()
    {
        if (google_search_panel_primary)
        {
            google_search_panel_primary.SetActive(false);
        }
    }
    void show_secondary_google_search_panel()
    {
        if (google_search_panel_secondary)
        {
            google_search_panel_secondary.SetActive(true);
        }
    }
    void hide_secondary_google_search_panel()
    {
        if (google_search_panel_secondary)
        {
            google_search_panel_secondary.SetActive(false);
        }
    }
    void search_google(string query)
    {
        if (home_controller)
        {
            home_controller.StoreResults(query);
        }
    }
    #endregion

    #region Callbacks
    public void login_btn_callback()
    {
        check_login_credentials();
    }
    public void signup_btn_callback()
    {
        check_signup_credentials();
    }
    public void goto_signup_btn_callback()
    {
        hide_login_panel();
        show_signup_panel();
    }
    public void gobackto_login_btn_callback()
    {
        hide_signup_panel();
        show_login_panel();
    }
    public void logout_btn_callback()
    {
        Globals.username = "";
        Globals.is_user_logged_in = false;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    public void show_chat_btn_callback()
    {
        hide_show_chat_btn();
        show_chat_panel();
    }
    public void close_chat_btn_callback()
    {
        hide_chat_panel();
        show_show_chat_btn();
    }
    public void show_music_player_btn_callback()
    {
        hide_show_music_player_btn();
        show_music_panel();
    }
    public void hide_music_player_btn_callback()
    {
        show_show_music_player_btn();
        hide_music_panel();
    }
    public void process_pdf_btn_callback()
    {
        hide_process_pdf_btn();
        show_pdf_processing_panel();
    }
    public void close_process_pdf_panel_btn_callback()
    {
        hide_pdf_processing_panel();
        show_process_pdf_btn();
    }
    public void choose_image_btn_callback()
    {
        //Texture2D texture = new Texture2D(1024, 1024, TextureFormat.DXT1, false); ;
        //string path = EditorUtility.OpenFilePanel("Choose a photo from your device", "", "png");
        //if (path.Length != 0)
        //{
        //    Debug.Log(path);
        //    var file_content = File.ReadAllBytes(path);
        //    texture.LoadImage(file_content);
        //    if (test_image)
        //    {
        //        test_image.texture = texture;
        //    }
        //    JSONInformation.image = texture;
        //}
    }
    public void display_pdf_extracted_text()
    {
        //System.Windows.Forms.OpenFileDialog open_file_dialogue = new System.Windows.Forms.OpenFileDialog();
        //System.Windows.Forms.DialogResult result = open_file_dialogue.ShowDialog();
        //if (result == System.Windows.Forms.DialogResult.OK)
        //{
        //    string path = open_file_dialogue.FileName;
        //    if (pdf_output_text)
        //    {
        //        pdf_output_text.text = document_scan_manager.scan_document(path);
        //    }
        //}
        StartCoroutine(show_load_document_dialog_coroutine());
    }
    public void close_message_box_simple()
    {
        hide_message_box_panel();
    }
    public void show_search_google_btn_callback()
    {
        show_primary_google_search_panel();
    }
    public void primary_submit_google_search_btn_callback()
    {
        if (google_search_if_primary)
        {
            if (google_search_if_primary.text != "")
            {
                hide_primary_google_search_panel();
                show_secondary_google_search_panel();
                reset_scrollbar(google_results_scrollbar);
                if (google_search_if_secondary)
                {
                    google_search_if_secondary.text = google_search_if_primary.text;
                }
                log_screen_info("Searching google ... ", 5.0f);
                //List<Result> results = search_google((google_search_if_secondary.text));
                search_google(google_search_if_secondary.text);
                Debug.Log("Searched For: " + google_search_if_secondary.text);
            }
        }
    }
    public void secondary_submit_google_search_btn_callback()
    {
        if (google_search_if_secondary)
        {
            if (google_search_if_secondary.text != "")
            {
                log_screen_info("Searching google ... ", 5.0f);
                reset_scrollbar(google_results_scrollbar);
                search_google(google_search_if_secondary.text);
                //List<Result> results = search_google((google_search_if_secondary.text));
                //search_google(google_search_if_secondary.text);
                Debug.Log("Searched For: " + google_search_if_secondary.text);
            }
        }
    }
    public void close_search_google_panel_btn_callback()
    {
        hide_primary_google_search_panel();
        hide_secondary_google_search_panel();
    }
    public void exit_application_btn_callback()
    {
        // may be save some stuff before exiting here 
        Application.Quit();
    }
    #endregion

    void Start()
    {
        // replace password chars with * 
        if (login_password_if && signup_password_if && signup_password_confirm_if)
        {
            login_password_if.contentType = InputField.ContentType.Password;
            signup_password_if.contentType = InputField.ContentType.Password;
            signup_password_confirm_if.contentType = InputField.ContentType.Password;
        }
        // hide all panels
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            hide_panels();
            if (Globals.is_user_logged_in)
            {
                // show the maps of the game or something else
                show_main_menu();
            }
            else
            {
                // show the login menu panel
                show_login_panel();
            }
        }
        // Important initializations
        /* Reset the scrollbar pos for google search results */
        reset_scrollbar(google_results_scrollbar);
    }

    void Update()
    {
        // log info timer handling 
        if (log_txt)
        {
            if (log_timer > 0.0f)
            {
                log_timer -= Time.deltaTime;
            }
            else
            {
                log_txt.text = "";
            }
        }
        // wait for 30 seconds to unlock login 
        if (Globals.is_login_locked && Globals.failed_login_attempts > 0 && (int)Time.deltaTime%60 == 0)
        {
            Globals.failed_login_attempts--;
        }
        if (Globals.failed_login_attempts <= 0)
        {
            Globals.is_login_locked = false;
        }
    }

    // This is called on the reloading of a new level
    void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "Dashboard")
        {
            Globals.is_user_logged_in = true;
            hide_panels();
            show_dashboard_panel();
        }
        else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (Globals.is_user_logged_in == false)
            {
                hide_panels();
                show_main_menu();
            }
            else
            {
                hide_panels();
                show_login_panel();
            }
        }
    }

    void check_login_credentials()
    {
        if (app_man)
        {
            if (login_username_if && login_password_if && !(Globals.is_login_locked))
            {
                login_password_if.contentType = InputField.ContentType.Standard;
                string login_status = app_man.check_login_credentials(login_username_if.text, login_password_if.text);
                login_password_if.contentType = InputField.ContentType.Password;
                if (login_status == "Success")
                {
                    // login succeeded 
                    Globals.username = login_username_if.text;
                    log_screen_info("Welcome Back!! " + Globals.username, 3.0f);
                    display_message_box_simple("Welcome Back!!" + Globals.username);
                    // reset number of failed attempts
                    Globals.failed_login_attempts = 0;
                    // clear input fields 
                    login_username_if.text = "";
                    login_password_if.text = "";
                    // may be go to another level here 
                    SceneManager.LoadScene("Dashboard", LoadSceneMode.Single);
                    Globals.is_user_logged_in = true;
                    // LoadLevel("Dashboard");
                    // StartCoroutine(load_dashboard_level());
                }
                else if (login_status == "Empty Field")
                {
                    // some fields are empty 
                    log_screen_info("Some Fields are empty ... ", 3.0f);
                }
                else if (login_status == "Failure")
                {
                    // Login Failed 
                    log_screen_info("Login Failed. Check Credentials and try again ..", 3.0f);
                    // count failed attempts
                    Globals.failed_login_attempts++;
                }
                else
                {
                    // Unexpected error 
                    log_screen_info("Unexpected Error occured. Check Runtime ..", 3.0f);
                }
                if (Globals.failed_login_attempts >= 5)
                {
                    Globals.is_login_locked = true;
                }
            }
            else if (Globals.is_login_locked)
            {
                // wait for 5 seconds to try again 
                log_screen_info("Too many failed attempts. Wait for some time and try again ...", 3.0f);
            }
        }
    }
    void check_signup_credentials()
    {
        if (app_man)
        {
            if (signup_username_if && signup_password_if && signup_password_confirm_if && signup_email_if && signup_gendre_dd)
            {
                signup_password_if.contentType = InputField.ContentType.Standard;
                signup_password_confirm_if.contentType = InputField.ContentType.Standard;
                string signup_status = app_man.create_user(signup_username_if.text, signup_email_if.text, signup_gendre_dd.itemText.text, signup_password_if.text, signup_password_confirm_if.text);
                signup_password_if.contentType = InputField.ContentType.Password;
                signup_password_confirm_if.contentType = InputField.ContentType.Password;
                if (signup_status == "Empty Field")
                {
                    log_screen_info("Some fields are empty ... ", 3.0f);
                }
                else if (signup_status == "Password Mismatch")
                {
                    log_screen_info("Passwords don't match ... ", 3.0f);
                }
                else if (signup_status == "Weak Password")
                {
                    log_screen_info("Choose stronger password ... ", 3.0f);
                }
                else if (signup_status == "Invalid Email")
                {
                    log_screen_info("Enter a valid Email Address ...", 3.0f);
                }
                else if (signup_status == "Success")
                {
                    // Signup Succeeded
                    log_screen_info("Signup Succeeded. \nEnter your credentials and login to your account ...", 3.0f);
                    display_message_box_simple("Signup Succeeded. \nEnter your credentials and login to your account");
                    // clear input fields 
                    signup_username_if.text = "";
                    signup_email_if.text = "";
                    signup_password_confirm_if.text = "";
                    signup_password_if.text = "";
                    signup_gendre_dd.value = 0;
                    // Go to login panel
                    hide_signup_panel();
                    show_login_panel();
                }
            }
        }
    }
}

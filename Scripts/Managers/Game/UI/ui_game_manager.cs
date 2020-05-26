using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* This Class holds The game global variables */
public class game_globals
{
    public static string playerUsername;
    public static string playerCharacterName = "";
    public static int playerLevel;
    public static string playerCharacterCategory;
    public static string playerMainWeapon;
    public static string playerSecondaryWeapon;
    public static int playerCharacterXP;
    public static float playerCharacterHealth;
    public static int playerCharacterMoney;
    public static List<Mission> playerMissions;
    public static List<Item> playerItems;
    public static List<Item> itemTypes;
    public static string gameStatus;
    public static bool isPlayerLoggedIn;
    public static bool neverShowLocalStorageWarning;
    public static bool playerHasACharacter = false;

    public static int GetPlayerCharacterLevel()
    {
        playerLevel = game_db_manager.RetrieveCharacterLevel(playerUsername);
        return playerLevel;
    }

    public static void LevelUpPlayer()
    {
        game_db_manager.LevelUpPlayerCharacter(playerUsername, playerCharacterName);
    }

    public static List<Mission> GetMissionsForPlayer()
    {
        playerMissions = game_db_manager.GetMissions(playerUsername);
        return playerMissions;
    }

    public static void CreateDefaultCharacter()
    {
        if (playerUsername != null && playerUsername != "")
        {
            playerCharacterName = "Ganful";
            playerCharacterCategory = "Warrior";
            // Check if the player has a character
            playerHasACharacter = game_db_manager.CheckPlayerCharacter(playerUsername);
            if (playerHasACharacter != true)
            {
                game_db_manager.CreatePlayerCharacter(playerUsername, playerCharacterName, playerCharacterCategory, playerMainWeapon, playerSecondaryWeapon);
                game_db_manager.SignThePlayerCharacter(playerUsername);
            }
            else
            {
                Debug.Log("Player already has a character ... ");
            }
        }
    }
}

/* This class is responsible of Game User Interface Management */
public class ui_game_manager : MonoBehaviour
{
    [Tooltip("The game main panel that contains hud panel and buttons and so on .. ")]
    public GameObject gameMainPanel;
    [Tooltip("The panel that contains the controls for fight and heal stuff")]
    public GameObject hudPanel;
    [Tooltip("The panel that holds the graphics settings and resolution and sound settings and so on .. ")]
    public GameObject optionsPanel;
    [Tooltip("The panel that contains the chat messages between game players")]
    public GameObject chatPanel;
    [Tooltip("The panel that contains the inventory pack")]
    public GameObject inventoryPanel;
    [Tooltip("The panel that contains the game notifications for the player")]
    public GameObject notificationsPanel;
    [Tooltip("The panel that contains the missions of the current player")]
    public GameObject missionsPanel;
    [Tooltip("The panel that contains the attack and defense controls")]
    public GameObject attackPanel;
    [Tooltip("The panel that contains the pause menu controls (volume, resolution, etc)")]
    public GameObject pauseMenuPanel;
    [Tooltip("The panel that contains the warning messages for the player")]
    public GameObject warningMessagesPanel;
    [Tooltip("The panel that contains the main UI for the Login menu level")]
    public GameObject mainLoginMenuPanel;
    [Tooltip("The panel that contains the login UI for the Login menu level")]
    public GameObject loginLoginMenuPanel;
    [Tooltip("The panel that contains the signup UI for the Login menu level")]
    public GameObject signupLoginMenuPanel;

    [Tooltip("The string types of the items (names of the items)")]
    public List<Item> itemTypes;
    public List<Item> testItems;
    //[Tooltip("The list that holds all the objects that can be collected")]
    //public List<GameObject> itemsCollectableList;

    [Tooltip("The list that holds all the missions that is offered for the player")]
    public List<Mission> missionsList;

    [Tooltip("The inputfield where the user enters his login username")]
    public InputField loginUsernameIF;
    [Tooltip("The inputfield where the user enters his login password")]
    public InputField loginPasswordIF;
    [Tooltip("The inputfield where the user enters his signup username")]
    public InputField signupUsernameIF;
    [Tooltip("The inputfield where the user enters his signup email (can be used to recover password)")]
    public InputField signupEmailIF;
    [Tooltip("The inoutfield where the user enters his password (Alphanumeric more than 8)")]
    public InputField signupPasswordIF;
    [Tooltip("The inputfield where the user confirms his password")]
    public InputField signupPasswordConfirmIF;

    [Tooltip("The text that outputs the logs to the user (error msgs and confirmation msgs and so on)")]
    public Text logTxt;
    [Tooltip("The text that contains the warnings for the user")]
    public Text warningTxt;

    //public Image testImg;

    private float logTxtCounter = 3.0f;

    #region Auxilaries
    bool CheckPasswordStrength(string password)
    {
        string[] chars = { "!", "@", "$", "%", "^", "*", "(", ")", "_", "-", "+", "=", "/", "\\", "~", "`", "#", "<", ">", "." };
        bool hasChars = false;
        bool isLong = false;
        for (int i = 0; i < chars.Length; i++)
        {
            if (password.Contains(chars[i]))
            {
                hasChars = true;
                break;
            }
        }
        if (password.Length >= 8)
        {
            isLong = true;
        }
        if (isLong && hasChars)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    string CheckPlayerCredentials(string username, string password)
    {
        if (username == "" || password == "")
        {
            // Some fields are Empty
            LogScreenDetails("Some Fields are empty ... ", 3.0f);
            return "Empty Field";
        }
        else if (game_db_manager.GetPassword(username) == password)
        {
            // Login Successful 
            LogScreenDetails("Welcome " + username, 3.0f);
            return "Login Successful";
        }
        else
        {
            // Invalid Credentials
            LogScreenDetails("Invalid Credentials ... ", 3.0f);
            return "Invalid Credentials";
        }
    }
    string CreateAccount(string username, string email, string password, string passwordConfirm)
    {
        if (username == "" || email == "" || password == "" || passwordConfirm == "")
        {
            // Some fields are empty 
            LogScreenDetails("Some Fields are empty ... ", 3.0f);
            return "Empty Field";
        }
        else if (password != passwordConfirm)
        {
            // Passwords don't match 
            LogScreenDetails("Passwords don't match ... ", 3.0f);
            return "Password Mismatch";
        }
        else if (!(CheckPasswordStrength(password)))
        {
            // Password is weak 
            LogScreenDetails("Password is weak. Insert some chars and make it longer than 8 ... ", 3.0f);
            return "Weak Password";
        }
        else if (game_db_manager.GetUsername(username) != null)
        {
            // Username is taken 
            LogScreenDetails("Username not availabe ... \nChoose Another one ", 4.0f);
            return "Username Taken";
        }
        else if (!(email.Contains("@")))
        {
            // Invalid Email
            LogScreenDetails("Please enter a valid email address ... ", 4.0f);
            return "Invalid Email";
        }
        else
        {
            // tests passed create account then 
            game_db_manager.CreateUser(username, email, password);
            LogScreenDetails("Account created successfully ... ", 5.0f);
            return "Signup Successful";
        }
    }
    void LogScreenDetails(string msg, float duration)
    {
        if (logTxt)
        {
            logTxt.text = msg;
            logTxtCounter = duration;
        }
    }
    // Gets the missions from the database and stores them in the Globals' missionsList
    void CheckForMissions(string username)
    {
        if (username != null && username != "")
        {
            game_globals.playerMissions = game_db_manager.GetMissions(username);
        }
        else
        {
            Debug.Log("Username is empty ... ");
        }
    }
    // Gets the items from the databases and stores them in the Globals' itemList 
    void CheckForItems(string username)
    {
        if (username != null && username != "")
        {
            game_globals.playerItems = game_db_manager.GetItemsForPlayer(username);
            testItems = game_globals.playerItems;
        }
        else
        {
            Debug.Log("Username is empty ... ");
        }
    }
    void ShowGameMainPanel()
    {
        if (gameMainPanel)
        {
            gameMainPanel.SetActive(true);
        }
    }
    void HideGameMainPanel()
    {
        if (gameMainPanel)
        {
            gameMainPanel.SetActive(false);
        }
    }
    void ShowHudPanel()
    {
        if (hudPanel)
        {
            hudPanel.SetActive(true);
        }
    }
    void HideHudPanel()
    {
        if (hudPanel)
        {
            hudPanel.SetActive(false);
        }
    }
    void ShowOptionsPanel()
    {
        if (optionsPanel)
        {
            optionsPanel.SetActive(true);
        }
    }
    void HideOptionsPanel()
    {
        if (optionsPanel)
        {
            optionsPanel.SetActive(false);
        }
    }
    void ShowChatPanel()
    {
        if (chatPanel)
        {
            chatPanel.SetActive(true);
        }
    }
    void HideChatPanel()
    {
        if (chatPanel)
        {
            chatPanel.SetActive(false);
        }
    }
    void ShowInventoryPanel()
    {
        if (inventoryPanel)
        {
            inventoryPanel.SetActive(true);
        }
    }
    void HideInventoryPanel()
    {
        if (inventoryPanel)
        {
            inventoryPanel.SetActive(false);

        }
    }
    void ShowNotificationsPanel()
    {
        if (notificationsPanel)
        {
            notificationsPanel.SetActive(true);
        }
    }
    void HideNotificationsPanel()
    {
        if (notificationsPanel)
        {
            notificationsPanel.SetActive(false);
        }
    }
    void ShowMissionsPanel()
    {
        if (missionsPanel)
        {
            missionsPanel.SetActive(true);
        }
    }
    void HideMissionsPanel()
    {
        if (missionsPanel)
        {
            missionsPanel.SetActive(false);
        }
    }
    void ShowAttackPanel()
    {
        if (attackPanel)
        {
            attackPanel.SetActive(true);
        }
    }
    void HideAttackPanel()
    {
        if (attackPanel)
        {
            attackPanel.SetActive(false);
        }
    }
    void ShowPauseMenuPanel()
    {
        if (pauseMenuPanel)
        {
            pauseMenuPanel.SetActive(true);
        }
    }
    void HidePauseMenuPanel()
    {
        if (pauseMenuPanel)
        {
            pauseMenuPanel.SetActive(false);
        }
    }
    void ShowWarningMessagesPanel(string warning)
    {
        if (warningMessagesPanel)
        {
            warningMessagesPanel.SetActive(true);
        }
        if (warningTxt)
        {
            warningTxt.text = warning;
        }
    }
    void HideWarningMessagesPanel()
    {
        if (warningMessagesPanel)
        {
            warningMessagesPanel.SetActive(false);
        }
    }
    void ShowMainLoginMenuPanel()
    {
        if (mainLoginMenuPanel)
        {
            mainLoginMenuPanel.SetActive(true);
        }
    }
    void HideMainLoginMenuPanel()
    {
        if (mainLoginMenuPanel)
        {
            mainLoginMenuPanel.SetActive(false);
        }
    }
    void ShowLoginLoginMenuPanel()
    {
        if (loginLoginMenuPanel)
        {
            loginLoginMenuPanel.SetActive(true);
        }
    }
    void HideLoginLoginMenuPanel()
    {
        if (loginLoginMenuPanel)
        {
            loginLoginMenuPanel.SetActive(false);
        }
    }
    void ShowSignUpLoginMenuPanel()
    {
        if (signupLoginMenuPanel)
        {
            signupLoginMenuPanel.SetActive(true);
        }
    }
    void HideSignUpLoginMenuPanel()
    {
        if (signupLoginMenuPanel)
        {
            signupLoginMenuPanel.SetActive(false);
        }
    }
    void ShowAllPanels()
    {
        ShowAttackPanel();
        ShowChatPanel();
        ShowHudPanel();
        ShowInventoryPanel();
        ShowNotificationsPanel();
        ShowPauseMenuPanel();
        ShowWarningMessagesPanel("");
        ShowMainLoginMenuPanel();
        ShowLoginLoginMenuPanel();
        ShowSignUpLoginMenuPanel();
        ShowGameMainPanel();
        ShowOptionsPanel();
        ShowMissionsPanel();
    }
    void HideAllPanels()
    {
        HideAttackPanel();
        HideChatPanel();
        HideHudPanel();
        HideInventoryPanel();
        HideNotificationsPanel();
        HidePauseMenuPanel();
        HideWarningMessagesPanel();
        HideMainLoginMenuPanel();
        HideLoginLoginMenuPanel();
        HideSignUpLoginMenuPanel();
        HideGameMainPanel();
        HideOptionsPanel();
        HideMissionsPanel();
    }
    #endregion

    #region Callbacks
    public void LoginAsAGuestBtnCallback()
    {
        // Show local storage warning & go to credentials entry 
        if (!(game_globals.neverShowLocalStorageWarning))
        {
            // show local storage warning 
            ShowWarningMessagesPanel("Guest account's data is saved locally so if you login from another device you won't be able to login to the same account ... ");
        }
        else
        {
            // Proceed to login immediately
            CloseLocalStorageWarningBtnCallback();
        }
    }
    public void CloseLocalStorageWarningBtnCallback()
    {
        // Hide local storage warning
        HideWarningMessagesPanel();
        // Show Login Panel
        ShowLoginLoginMenuPanel();
    }
    public void CloseWarningsPanelBtnCallback()
    {
        // Hide Warnings Panel
        HideWarningMessagesPanel();
    }
    public void LoginBtnCallback()
    {
        if (loginUsernameIF && loginPasswordIF)
        {
            string username = loginUsernameIF.text;
            loginPasswordIF.contentType = InputField.ContentType.Standard;
            string password = loginPasswordIF.text;
            loginPasswordIF.contentType = InputField.ContentType.Password;
            string loginResult = CheckPlayerCredentials(username, password);
            if (loginResult == "Empty Field")
            {
                // Some fields are empty 
                Debug.Log("Some fields are empty ... ");
                return;
            }
            else if (loginResult == "Invalid Credentials")
            {
                // Invalid Credentials
                Debug.Log("Invalid Credentials ... ");
                return;
            }
            else if (loginResult == "Login Successful")
            {
                // Login Successful
                Debug.Log("Login Successful ");
                // Load Main City Level
                game_globals.playerUsername = username;
                game_globals.isPlayerLoggedIn = true;
                CheckForItems(game_globals.playerUsername);
                CheckForMissions(game_globals.playerUsername);
                SceneManager.LoadScene("MainCity", LoadSceneMode.Single);
            }
            else
            {
                // Unintended Error 
                Debug.Log("Unintended Error Occurred ... ");
                LogScreenDetails("Unintended Error Occurred Please let me know at nader19113@gmail.com", 10.0f);
            }
        }
    }
    public void CreateAccountBtnCallback()
    {
        // Open Signup Panel 
        HideAllPanels();
        ShowSignUpLoginMenuPanel();
    }
    public void SignUpBtnCallback()
    {
        // Proceed with creating user 
        if (signupUsernameIF && signupEmailIF && signupPasswordIF && signupPasswordConfirmIF)
        {
            string username = signupUsernameIF.text;
            string email = signupEmailIF.text;
            signupPasswordIF.contentType = InputField.ContentType.Standard;
            signupPasswordConfirmIF.contentType = InputField.ContentType.Standard;
            string password = signupPasswordIF.text;
            string passwordConfirm = signupPasswordConfirmIF.text;
            signupPasswordIF.contentType = InputField.ContentType.Password;
            signupPasswordConfirmIF.contentType = InputField.ContentType.Password;
            string signUpResult = CreateAccount(username, email, password, passwordConfirm);
            if (signUpResult == "Signup Successful")
            {
                // Signup Successful 
                Debug.Log("Account created successfully ... ");
                // go back to login panel and clear all the fields
                signupUsernameIF.text = "";
                signupEmailIF.text = "";
                signupPasswordIF.text = "";
                signupPasswordConfirmIF.text = "";
                HideAllPanels();
                ShowLoginLoginMenuPanel();
            }
            else if (signUpResult == "Empty Field")
            {
                // Empty Fields 
                Debug.Log("Some fields are empty ... ");
                return;
            }
            else if (signUpResult == "Password Mismatch")
            {
                // Passwords mismatch 
                Debug.Log("Passwords Mismatch ... ");
                return;
            }
            else if (signUpResult == "Weak Password")
            {
                // Weak password
                Debug.Log("Choose a strong password ... ");
                return;
            }
            else if (signUpResult == "Username Taken")
            {
                // Username not availabe
                Debug.Log("Username not available. Choose another one ");
                return;
            }
            else if (signUpResult == "Invalid Email")
            {
                // Invalid Email 
                Debug.Log("Invalid email form ... ");
                return;
            }
            else
            {
                // Unintended Error occurred 
                LogScreenDetails("Unintended Error occurred.\nPlease inform me here: nader19113@gmail.com", 5.0f);
                Debug.Log("Unintended Error occurred.\nPlease inform me here: nader19113@gmail.com");
                return;
            }
        }
    }
    public void BackToMainMenuPanel()
    {
        // Hide all panels 
        HideAllPanels();
        // SHow the main menu panel
        ShowMainLoginMenuPanel();
    }
    public void LogOutBtnCallback()
    {
        // Go back to main login level (may be you want to save some stuff before log out)
        SceneManager.LoadScene("GameLoginMenu", LoadSceneMode.Single);
    }
    public void PauseGameBtnCallback()
    {
        // Hide game main panel
        HideGameMainPanel();
        // Show Pause Menu 
        ShowPauseMenuPanel();
        // Pause the game time 
        Time.timeScale = 0;
    }
    public void ResumeGameBtnCallback()
    {
        // Hide Pause Menu
        HidePauseMenuPanel();
        // Show main game panel
        ShowGameMainPanel();
        // Resume the game time 
        Time.timeScale = 1;
    }
    public void OptionsBtnCallback()
    {
        // Show options panel
        ShowOptionsPanel();
    }
    public void CloseOptionsPanelBtnCallback()
    {
        // Hide options panel
        HideOptionsPanel();
    }
    public void GoBackToGameLoginMenu()
    {
        // Load Game Main Menu (Login Screen)
        SceneManager.LoadScene("GameLoginMenu", LoadSceneMode.Single);
    }
    public void ExitGame()
    {
        // May be you want to save some data before that 
        Application.Quit();
    }
    public void ShowAvatar()
    {
        if (game_globals.playerCharacterName != "")
        {
            // Show the avatar (may be on a new level)
            Debug.Log("Not implemented yet ... ");
        }
        else
        {
            // Charater name is not assigned 
            Debug.Log("Character name is not assigned ... ");
        }
    }
    public void InventoryBtnCallback()
    {
        // Show inventory UI
        ShowInventoryPanel();
    }
    public void CloseInventoryPanelBtnCallback()
    {
        // Hide the inventory UI
        HideInventoryPanel();
    }
    public void ShowPlayerMissionsBtnCallback()
    {
        // Display missions list 
        ShowMissionsPanel();
    }
    public void CloseMissionsPanelBtnCallback()
    {
        HideMissionsPanel();
    }
    #endregion

    #region MainFunctions
    void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "GameLoginMenu")
        {
            HideAllPanels();
            game_globals.isPlayerLoggedIn = false;
            ShowMainLoginMenuPanel();
        }
        else if (SceneManager.GetActiveScene().name == "MainCity")
        {
            // Load Hud and attack panel based on the data retrieved from the database
            game_globals.isPlayerLoggedIn = true;
            Debug.Log("Username: " + game_globals.playerUsername);
            //testItems = game_db_manager.GetItemsForPlayer(game_globals.playerUsername);
            // Create player character (default: Ganful)
            game_globals.CreateDefaultCharacter();
            Debug.Log("Player: " + game_globals.playerUsername + " has a character? " + game_db_manager.CheckPlayerCharacter(game_globals.playerUsername).ToString());
            testItems = game_globals.playerItems;
            HideAllPanels();
            // Show main panel
            ShowGameMainPanel();
            // Set the game time to run
            Time.timeScale = 1;
            // Load inventory items into the UI
            //if (inventoryPanel)
            //{
            //    inventoryPanel.GetComponent<inventory_controller>().RetrieveInventoryFromDB(game_globals.playerUsername);
            //}
            //if (game_globals.playerItems != null)
            //{
            //    for (int i = 0; i < game_globals.playerItems.Count; i++)
            //    {
            //        if (game_globals.playerItems[i] != null)
            //        {
            //            if (itemTypes != null)
            //            {
            //                for (int j = 0; j < itemTypes.Count; j++)
            //                {
            //                    if (game_globals.playerItems[i].name == itemTypes[j].name)
            //                    {
            //                        if (itemTypes[j].icon != null)
            //                        {
            //                            game_globals.playerItems[i].icon = itemTypes[j].icon;
            //                            if (inventoryPanel.GetComponent<inventory_controller>())
            //                            {
            //                                // Assign inventory slot
            //                                inventoryPanel.GetComponent<inventory_controller>().RetrieveInventoryFromDB(game_globals.playerItems[i]);
            //                            }
            //                        }
            //                        else
            //                        {
            //                            Debug.Log("Assign sprites to the item types list ... ");
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            // Assign items types of the ui man to the globals' item types
            game_globals.itemTypes = itemTypes;
            //if (testImg)
            //{
            //    testImg.sprite = game_globals.itemTypes[0].icon;
            //}
            // May be load missions here into the missions list 
        }
    }
    void Update()
    {
        if (logTxtCounter <= 0.0f)
        {
            if (logTxt)
            {
                logTxt.text = "";
            }
        }
        else
        {
            logTxtCounter -= Time.deltaTime;
        }
    }
    #endregion
}

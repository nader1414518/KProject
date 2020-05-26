using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;

/* This Class is responsible for Database functionality */
public class game_db_manager : MonoBehaviour
{
    bool never_show_local_storage_warning = false;

    void Start()
    {
        
        // Create Database initially 
        if (!(File.Exists(Application.persistentDataPath + "/player_accounts.s3db")))
        {
            CreateDatabase();
            InitializeWeaponsSchema();
            InitializePlayerStatsSchema();
            InitializePlayerCharacterSchema();
            InitializeMissionsSchema();
            InitializeItemsSchema();
        }
        else
        {
            Debug.Log("Database already created ... ");
            Debug.Log("DB in: " + Application.persistentDataPath + "/player_accounts.s3db");
        }
    }

    public static string GetUsername(string username)
    {
        string un;

        // Connect to accounts database 
        if (File.Exists(Application.persistentDataPath + "/player_accounts.s3db"))
        {
            // connect to database
            string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

            IDbConnection dbconn;
            dbconn = (IDbConnection)new SqliteConnection(conn);

            if (dbconn != null)
            {
                // get all strings that match username from the database
                dbconn.Open();
                // Create DB Command 
                IDbCommand dbcmd = dbconn.CreateCommand();
                string sqlQuery = "select username from [user] where username = '" + username + "';";
                dbcmd.CommandText = sqlQuery;
                IDataReader reader = dbcmd.ExecuteReader();

                try
                {
                    if (reader.Read())
                    {
                        un = reader.GetString(0);
                        // Disposing Variables
                        reader.Close();
                        reader = null;
                        dbcmd.Dispose();
                        dbcmd = null;
                        dbconn.Close();
                        dbconn = null;

                        // return un
                        return un;
                    }
                    else
                    {
                        // Disposing Variables
                        reader.Close();
                        reader = null;
                        dbcmd.Dispose();
                        dbcmd = null;
                        dbconn.Close();
                        dbconn = null;

                        return null;
                    }
                }
                catch
                {
                    // Disposing Variables
                    reader.Close();
                    reader = null;
                    dbcmd.Dispose();
                    dbcmd = null;
                    dbconn.Close();
                    dbconn = null;

                    return null;
                }
            }
            return null;
        }
        return null;
    }

    public static void CreateDatabase()
    {
        // Create accounts database 
        SqliteConnection.CreateFile(Application.persistentDataPath + "/player_accounts.s3db");

        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        if (dbconn != null)
        {
            dbconn.Open();

            IDbCommand dbcmd = dbconn.CreateCommand();
            // create user table 
            string sqlQuery = "create table [user] (username text, email text, password text, has_character boolean);";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;

            Debug.Log("Database Created ... ");
        }
    }
    public static void InitializePlayerCharacterSchema()
    {
        // Create Database Initially 
        if (!(File.Exists(Application.persistentDataPath + "/player_accounts.s3db")))
        {
            CreateDatabase();
        }

        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        if (dbconn != null)
        {
            dbconn.Open();

            IDbCommand dbcmd = dbconn.CreateCommand();
            // create user table 
            string sqlQuery = "create table [character] (username text, character_name text, character_level integer, character_category text, main_weapon text, secondary_weapon text, character_xp integer, character_money integer, character_health float);";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;

            Debug.Log("Created Characters Schema ... ");
        }
    }
    public static void InitializePlayerStatsSchema()
    {
        // Create Database initially
        if (!(File.Exists(Application.persistentDataPath + "/player_accounts.s3db")))
        {
            CreateDatabase();
        }

        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        if (dbconn != null)
        {
            dbconn.Open();

            IDbCommand dbcmd = dbconn.CreateCommand();
            // create user table 
            string sqlQuery = "create table [stats] (username text, never_show_storage_warning Text);";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;

            Debug.Log("Created Stats Schema ... ");
        }
    }
    public static void InitializeWeaponsSchema()
    {
        // Create Database initially 
        if (!(File.Exists(Application.persistentDataPath + "/player_accounts.s3db")))
        {
            CreateDatabase();
        }
  
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        if (dbconn != null)
        {
            dbconn.Open();

            IDbCommand dbcmd = dbconn.CreateCommand();
            // create user table 
            string sqlQuery = "create table [weapon] (username text, weapon_name text, attack_value integer, defense_value integer, weapon_level integer, weapon_type text);";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;

            Debug.Log("Created Weapons Schema ... ");
        }
    }
    public static void InitializeMissionsSchema()
    {
        // Create Database initially 
        if (!(File.Exists(Application.persistentDataPath + "/player_accounts.s3db")))
        {
            CreateDatabase();
        }

        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        if (dbconn != null)
        {
            dbconn.Open();

            IDbCommand dbcmd = dbconn.CreateCommand();
            // create user table 
            string sqlQuery = "create table [mission] (username text, mission_name text, xp_reward integer, money_reward integer, mission_description text, status text, mission_level integer);";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;

            Debug.Log("Created Missions Schema ... ");
        }
    }
    public static void InitializeItemsSchema()
    {
        // Create Database initially 
        if (!(File.Exists(Application.persistentDataPath + "/player_accounts.s3db")))
        {
            CreateDatabase();
        }

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            // create item table
            string sqlQuery = "create table [item] (username text, item_id integer primary key autoincrement, item_name text, category text, description text);";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();


            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;

            Debug.Log("Created Items Schema ... ");
        }
    }
    // Inserts a player character initially 
    public static void CreatePlayerCharacter(string username, string character_name, string character_category, string main_weapon, string secondary_weapon)
    {
        // path to the database 
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            // Inserts a character with initial level (level 1)
            string sqlQuery = "insert into [character] (username, character_name, character_category, main_weapon, secondary_weapon, character_level, character_xp, character_money, character_health) values('"
                + username + "', '" + character_name + "', '" + character_category + "', '" + main_weapon + "', '" + secondary_weapon + "', " + 1.ToString() + ", " + 0.ToString() + ", " + 0.ToString() + ", "
                + 1.ToString() + ");";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing Variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }
    }
    // Sign the player character 
    public static void SignThePlayerCharacter(string username)
    {
        // path to the database 
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "Update [user] set has_character=" + 1.ToString() + " where username='" + username + "';";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables 
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }
    }
    // Increase character xp 
    public static void IncreaseCharacterXP(string username, string character_name, int character_xp_raise)
    {
        // Get the current xp of the player character
        int characterCurrentXP = RetrieveCharacterXP(username);

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            // Assigns character_level++ 
            //string sqlQuery = "update [character] set character_level=" + level++.ToString() + " where username='" + username + "' and character_name='" + character_name + "';";
            string sqlQuery = "update [character] set character_xp=" + (characterCurrentXP + character_xp_raise).ToString() + " where username='" + username + "' and character_name='"
                + character_name + "';";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }
    }
    // Increase character money 
    public static void IncreaseCharacterMoney(string username, string character_name, int character_money_raise)
    {
        // Get the current money of the player character
        int characterCurrentMoney = RetrieveCharacterXP(username);

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            // Assigns character_level++ 
            //string sqlQuery = "update [character] set character_level=" + level++.ToString() + " where username='" + username + "' and character_name='" + character_name + "';";
            string sqlQuery = "update [character] set character_money=" + (characterCurrentMoney + character_money_raise).ToString() + " where username='" + username + "' and character_name='"
                + character_name + "';";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }
    }
    // Increase character health 
    public static void IncreaseCharacterHealth(string username, string character_name, float character_health_raise)
    {
        float currentHealth = RetrieveCharacterHealth(username);

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery;
            if ((currentHealth + character_health_raise) > 1.0f)
            {
                // Health value would go above 100% then the value of the health is set to the value of unity 
                sqlQuery = "update [character] set character_health=" + 1.ToString() + " where username='" + username + "' and character_name='" + character_name + "';";
            }
            else if ((currentHealth + character_health_raise) < 0.0f)
            {
                // Health value would go below 0% then the value of the health is set to the value of zero 
                sqlQuery = "update [character] set character_health=" + 0.ToString() + " where username='" + username + "' and character_name='" + character_name + "';";
            }
            else
            {
                // Set the health to the sum 
                sqlQuery = "update [character] set character_health=" + (currentHealth + character_health_raise).ToString() + " where username='" + username + "' and character_name='" + character_name + "';";
            }
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }
    }
    // Level up the player character (To be implemented tomorrow)
    public static void LevelUpPlayerCharacter(string username, string character_name)
    {
        // Get the current character level from the database
        int level = RetrieveCharacterLevel(username);

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            // Assigns character_level++ 
            string sqlQuery = "update [character] set character_level=" + level++.ToString() + " where username='" + username + "' and character_name='" + character_name + "';";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }
    }
    // Never Show Local Storage warning 
    public static void InsertLocalStorageWarningStatusForPlayer(string username, string never_show_local_storage_warning)
    {
        // path to database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();  // Open connection to the database
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "insert into [stats] (username, never_show_storage_warning) values ('" + username + "', '" + never_show_local_storage_warning + "');";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        // Disposing Variables
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    // Adds a weapon to a user account 
    public static void InsertWeapon(string username, string weapon_name, int attack_value, int defense_value, int weapon_level, string weapon_type)
    {
        // path to database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); // open connection to the database
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "insert into [weapon] (username, weapon_name, attack_value, defense_value, weapon_level, weapon_type) values ('" 
            + username + "', '" + weapon_name + "', " + attack_value.ToString() + ", " + defense_value.ToString()
            + ", " + weapon_level.ToString() + ", '" + weapon_type + "');";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        // Disposing Variables
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    // Add an item to a certain player
    public static void InsertItem(string username, string item_name, string category)
    {
        // path to database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); // open connection to the database
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "insert into [item] (username, item_name, category) values('" + username + "', '" + item_name + "', '" + category + "');";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        // Disposing Variables
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    // Add an item to a certain player (with a description of the item)
    public static void InsertItem(string username, string item_name, string category, string description)
    {
        // path to database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); // open connection to the database
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "insert into [item] (username, item_name, category, description) values('" + username + "', '" + item_name + "', '" + category + "', '" + description + "');";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        // Disposing Variables
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    // Inserts a mission for the player 
    public static void InsertMission(string username, string mission_name, int xp_reward, int money_reward, string mission_description, string status)
    {
        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open(); // Open a connection to the database
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "insert into [mission] (username, mission_name, xp_reward, money_reward, mission_description, status) values('" +
                username + "', '" + mission_name + "', " + xp_reward + ", " + money_reward + ", '" + mission_description + "', '" + status + "');";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }
    } 
    // Deletes an item from the database t that player (with an id)
    public static void DeleteItem(string username, string item_name, int item_id)
    {
        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open(); // Open a connection to the database
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "delete from [item] where username='" + username + "' and mission_name='" + item_name + "' and item_id=" + item_id + ";";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }
    }
    // Deletes a mission from the database for that player
    public static void DeleteMission(string username, string mission_name)
    {
        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open(); // Open a connection to the database
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "delete from [mission] where username='" + username + "' and mission_name='" + mission_name + "';";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }
    }
    // Inserts a character into the users database
    public static void InsertCharacter(string username, string character_name, int character_level, string character_category, string main_weapon, string secondary_weapon)
    {
        // Path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        if (dbconn != null)
        {
            dbconn.Open();

            IDbCommand dbcmd = dbconn.CreateCommand();
            // create user table 
            string sqlQuery = "create table [character] (username, character_name, character_level, character_category, main_weapon, secondary_weapon) values('" + username +
                "', '" + character_name + "', " + character_level + ", '" + character_category + "', '" + main_weapon + "', '" + secondary_weapon + "');";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;

            Debug.Log("Add Character to database ... ");
        }
    }
    // Inserts a user into the database
    public static void CreateUser(string username, string email, string password)
    {
        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); // open connection to the database
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "insert into [user] (username, email, password, has_character) values ('" + username + "', '" + email + "', '" + password + "', " + 0.ToString() + ");";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        // Disposing Variables
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    // Retrieves the mission stored in the database (All the missions not level-dependent)
    public static List<Mission> GetMissions(string username)
    {
        List<Mission> missions = new List<Mission>();

        // path to database 
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "select mission_name, xp_reward, money_reward, mission_description, status, mission_level from [mission] where username='" + username + "';";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    string mission_name = reader.GetString(0);
                    int xp_reward = reader.GetInt32(1);
                    int money_reward = reader.GetInt32(2);
                    string mission_description = reader.GetString(3);
                    string status = reader.GetString(4);
                    int mission_level = reader.GetInt32(5);
                    missions.Add(new Mission(mission_name, mission_description, xp_reward, money_reward, status, mission_level));
                }
                Debug.Log("Retrieved " + missions.Count + " missions for Player: " + game_globals.playerUsername);
            }
            catch
            {
                // Disposing Variables
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
                dbconn = null;

                missions = null;
            }
        }

        return missions;
    }
    // Retrieves the items stored for a certain player (To be called at the begining of each game session)
    public static List<Item> GetItemsForPlayer(string username)
    {
        List<Item> items = new List<Item>();

        // path to database 
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "select item_id, item_name, category, description from [item] where username='" + username + "';";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                int item_id = reader.GetInt32(0);
                string item_name = reader.GetString(1);
                string item_category = reader.GetString(2);
                //string item_description = reader.GetString(3);
                items.Add(new Item(item_id, item_name, item_category, ""));
            }
            Debug.Log("Retrieved " + items.Count + " items for Player: " + game_globals.playerUsername);
            // Disposing Variables
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }

        return items;
    }
    // Retrieve if the player has a character or not 
    public static bool CheckPlayerCharacter(string username)
    {
        bool has_character = false;

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "select has_character from [user] where username='" + username + "';";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            try 
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        has_character = reader.GetBoolean(0);
                        // Disposing Variables
                        reader.Close();
                        reader = null;
                        dbcmd.Dispose();
                        dbcmd = null;
                        dbconn.Close();
                        dbconn = null;
                        return has_character;
                    }
                    else
                    {
                        Debug.Log("Unable to read from database ... returned bad boolean");
                        return has_character;
                    }
                }
                else
                {
                    Debug.Log("Unable to read from database (reader obj is null ... returned bad boolean");
                    return has_character;
                }
            }
            catch
            {
                Debug.Log("Unable to read from database (exception occured) ... returned bad boolean");
                return has_character;
            }
        }
        else
        {
            Debug.Log("Unable to read from database (connection string is not valid) ... returned bad boolean");
            return has_character;
        }
    }
    // Retrieve Player level from database
    public static int RetrieveCharacterLevel(string username)
    { 
        int characterLevel;

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "select character_level from [character] where username='" + username + "';";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            try
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        characterLevel = reader.GetInt32(0);
                        // Disposing Variables
                        reader.Close();
                        reader = null;
                        dbcmd.Dispose();
                        dbcmd = null;
                        dbconn.Close();
                        dbconn = null;
                        // return level 
                        return characterLevel;
                    }
                    else
                    {
                        Debug.Log("Error in reading rows from database ... ");
                        return 0;
                    }
                }
                else
                {
                    Debug.Log("Reader object is null ... ");
                    return 0;
                }
            }
            catch
            {
                // Disposing Variables
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
                dbconn = null;

                return 0;
            }
        }
        else
        {
            Debug.Log("Null database connection ... ");
            return 0;
        }
    }
    // Retrieve Character current xp
    public static int RetrieveCharacterXP(string username)
    {
        int characterXP;

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "select character_xp from [character] where username='" + username + "';";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            try
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        characterXP = reader.GetInt32(0);
                        // Disposing Variables
                        reader.Close();
                        reader = null;
                        dbcmd.Dispose();
                        dbcmd = null;
                        dbconn.Close();
                        dbconn = null;
                        // return level 
                        return characterXP;
                    }
                    else
                    {
                        Debug.Log("Error in reading rows from database ... ");
                        return 0;
                    }
                }
                else
                {
                    Debug.Log("Reader object is null ... ");
                    return 0;
                }
            }
            catch
            {
                // Disposing Variables
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
                dbconn = null;

                return 0;
            }
        }
        else
        {
            Debug.Log("Null database connection ... ");
            return 0;
        }
    }
    // Retrieve Character current xp (specific character if the player has more than one character)
    public static int RetrieveCharacterXP(string username, string character_name)
    {
        int characterXP;

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "select character_xp from [character] where username='" + username + "' and character_name='" + character_name + "');";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            try
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        characterXP = reader.GetInt32(0);
                        // Disposing Variables
                        reader.Close();
                        reader = null;
                        dbcmd.Dispose();
                        dbcmd = null;
                        dbconn.Close();
                        dbconn = null;
                        // return level 
                        return characterXP;
                    }
                    else
                    {
                        Debug.Log("Error in reading rows from database ... ");
                        return 0;
                    }
                }
                else
                {
                    Debug.Log("Reader object is null ... ");
                    return 0;
                }
            }
            catch
            {
                // Disposing Variables
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
                dbconn = null;

                return 0;
            }
        }
        else
        {
            Debug.Log("Null database connection ... ");
            return 0;
        }
    }
    // Retrieve Character current money 
    public static int RetrieveCharacterMoney(string username)
    {
        int characterMoney;

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "select character_money from [character] where username='" + username + "';";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            try
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        characterMoney = reader.GetInt32(0);
                        // Disposing Variables
                        reader.Close();
                        reader = null;
                        dbcmd.Dispose();
                        dbcmd = null;
                        dbconn.Close();
                        dbconn = null;
                        // return level 
                        return characterMoney;
                    }
                    else
                    {
                        Debug.Log("Error in reading rows from database ... ");
                        return 0;
                    }
                }
                else
                {
                    Debug.Log("Reader object is null ... ");
                    return 0;
                }
            }
            catch
            {
                Debug.Log("Returned Bad Money for the player ... ");
                // Disposing Variables
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
                dbconn = null;

                return 0;
            }
        }
        else
        {
            Debug.Log("Null database connection ... ");
            return 0;
        }
    }
    // Retrieve Character current money (specific character if the player has more than one character)
    public static int RetrieveCharacterMoney(string username, string character_name)
    {
        int characterMoney;

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "select character_money from [character] where username='" + username + "' and character_name='" + character_name + "');";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            try
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        characterMoney = reader.GetInt32(0);
                        // Disposing Variables
                        reader.Close();
                        reader = null;
                        dbcmd.Dispose();
                        dbcmd = null;
                        dbconn.Close();
                        dbconn = null;
                        // return level 
                        return characterMoney;
                    }
                    else
                    {
                        Debug.Log("Error in reading rows from database ... ");
                        return 0;
                    }
                }
                else
                {
                    Debug.Log("Reader object is null ... ");
                    return 0;
                }
            }
            catch
            {
                Debug.Log("Returned Bad XP for the player ... ");
                // Disposing Variables
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
                dbconn = null;

                return 0;
            }
        }
        else
        {
            Debug.Log("Null database connection ... ");
            return 0;
        }
    }
    // Retrieve Character current health 
    public static float RetrieveCharacterHealth(string username)
    {
        float characterHealth;

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "select character_health from [character] where username='" + username + "');";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            try
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        characterHealth = reader.GetFloat(0);
                        // Disposing Variables
                        reader.Close();
                        reader = null;
                        dbcmd.Dispose();
                        dbcmd = null;
                        dbconn.Close();
                        dbconn = null;
                        // return level 
                        return characterHealth;
                    }
                    else
                    {
                        Debug.Log("Error in reading rows from database ... ");
                        return 0;
                    }
                }
                else
                {
                    Debug.Log("Reader object is null ... ");
                    return 0;
                }
            }
            catch
            {
                Debug.Log("Returned Bad Health for the player ... ");
                // Disposing Variables
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
                dbconn = null;

                return 0;
            }
        }
        else
        {
            Debug.Log("Null database connection ... ");
            return 0;
        }
    }
    // Retrieve Character current health (specific character if the player has more than one character)
    public static float RetrieveCharacterHealth(string username, string character_name)
    {
        float characterHealth;

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        if (dbconn != null)
        {
            dbconn.Open();
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "select character_health from [character] where username='" + username + "' and character_name='" + character_name + "');";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            try
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        characterHealth = reader.GetFloat(0);
                        // Disposing Variables
                        reader.Close();
                        reader = null;
                        dbcmd.Dispose();
                        dbcmd = null;
                        dbconn.Close();
                        dbconn = null;
                        // return level 
                        return characterHealth;
                    }
                    else
                    {
                        Debug.Log("Error in reading rows from database ... ");
                        return 0;
                    }
                }
                else
                {
                    Debug.Log("Reader object is null ... ");
                    return 0;
                }
            }
            catch
            {
                Debug.Log("Returned Bad Health for the player ... ");
                // Disposing Variables
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
                dbconn = null;

                return 0;
            }
        }
        else
        {
            Debug.Log("Null database connection ... ");
            return 0;
        }
    }
    // check the password for a specific user 
    public static string GetPassword(string username)
    {
        //// Create Database if it doesn't exist
        //CreateDatabase();

        string password;

        string conn = "URI=file://" + Application.persistentDataPath + "/player_accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "select password from [user] where username='" + username + "';";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        try
        {
            if (reader.Read())
            {
                password = reader.GetString(0);
                // Disposing Variables
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
                dbconn = null;
                // Show pass on console just to make sure 
                // Debug.Log("Password: " + password);
                //log_text.text = "Password Acquired ... ";
                //log_text_counter = 3.0f;
                // return password
                return password;
            }
            else
            {
                // Disposing Variables
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
                dbconn = null;

                return null;
            }
        }
        catch
        {
            // Disposing Variables
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;

            return null;
        }
    }
}


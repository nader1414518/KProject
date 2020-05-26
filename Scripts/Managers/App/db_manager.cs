using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.UI;
using UnityEditor;

/* This Class is responsible for Database functionality */
public class db_manager : MonoBehaviour
{
    public Text log_text;
    public float log_text_counter = 3.0f;

    void Start()
    {
        // Create Database initially 
        if (!(File.Exists(Application.persistentDataPath + "/accounts.s3db")))
        {
            CreateDatabase();
        }
        else
        {
            Debug.Log("Database already created ... ");
        }
    }

    public string GetUsername(string username)
    {
        string un;

        // Connect to accounts database 
        if (File.Exists(Application.persistentDataPath + "/accounts.s3db"))
        {
            // connect to database
            string conn = "URI=file://" + Application.persistentDataPath + "/accounts.s3db";

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

    public void CreateDatabase()
    {
        // Create accounts database 
        SqliteConnection.CreateFile(Application.persistentDataPath + "/accounts.s3db");

        string conn = "URI=file://" + Application.persistentDataPath + "/accounts.s3db";

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        if (dbconn != null)
        {
            log_text.text = "database file created ... ";
            log_text_counter = 3.0f;
            dbconn.Open();

            IDbCommand dbcmd = dbconn.CreateCommand();
            // create user table 
            string sqlQuery = "create table [user] (username text, gendre text, email text, password text);";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            // Disposing variables
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;

            //Debug.Log("Database exists at " + Application.persistentDataPath + "/accounts.s3db");
            //log_text.text = "DB exists at " + Application.persistentDataPath + "/accounts.s3db";
            //log_text_counter = 3.0f;
        }
    }

    public void CreateUser(string username, string gendre, string email, string password)
    {
        //// Create Database if it doesn't exist
        //CreateDatabase();

        // path to the database
        string conn = "URI=file://" + Application.persistentDataPath + "/accounts.s3db";

        IDbConnection dbconn;
        dbconn = new SqliteConnection(conn);
        dbconn.Open(); // open connection to the database
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "insert into [user] (username, gendre, email, password) values ('" + username + "', '" +
                          gendre + "', '" + email + "', '" + password + "');";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        // Disposing Variables
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    // check the password for a specific user 
    public string GetPassword(string username)
    {
        //// Create Database if it doesn't exist
        //CreateDatabase();

        string password;

        string conn = "URI=file://" + Application.persistentDataPath + "/accounts.s3db";

        IDbConnection dbconn;
        dbconn = new SqliteConnection(conn);
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
                Debug.Log("Password: " + password);
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

    void Update()
    {
        if (log_text)
        {
            if (log_text_counter <= 0.0f)
            {
                log_text.text = "";
            }
            else
            {
                log_text_counter -= Time.deltaTime;
            }
        }
    }
}

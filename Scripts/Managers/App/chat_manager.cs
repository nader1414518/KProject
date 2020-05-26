using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;
using UnityEngine.UI;

/* This class holds the info of the sent messages */
public class JSONInformation
{
    public string username = Globals.username;
    public string text;
    public static Texture2D image;
}

/* This class holds the functionality of the messaging service */
public class chat_manager : MonoBehaviour
{
    public static PubNub pubnub;
    public Font custom_font;
    public Button submit_button;
    public Canvas canvas_object;
    public InputField text_input;
    public int counter = 0;
    public int index_counter = 0;
    public Text delete_text;
    public Text move_text_upwards;
    private Text text;

    Queue<GameObject> chat_message_queue = new Queue<GameObject>();

    void Start()
    {
        // Use this for initialization
        PNConfiguration pn_configuration = new PNConfiguration();
        pn_configuration.PublishKey = "pub-c-c9fedf8b-6c91-46cf-b6d9-14b5ec353a21";
        pn_configuration.SubscribeKey = "sub-c-5b8e3a72-8796-11ea-b883-d2d532c9a1bf";
        pn_configuration.LogVerbosity = PNLogVerbosity.BODY;
        pn_configuration.UUID = System.Guid.NewGuid().ToString();
        pubnub = new PubNub(pn_configuration);

        // Add listener to submit button to send messages
        Button btn = submit_button.GetComponent<Button>();
        btn.onClick.AddListener(task_on_click);

        // Fetch the last 13 messages sent on the given PubNub channel
        pubnub.FetchMessages().Channels(new List<string> { "chatchannel13" }).Count(10).Async((result, status) =>
        {
            if (status.Error)
            {
                Debug.Log(string.Format("Fetch Messages Error: {0} {1} {2}", status.StatusCode, status.ErrorData, status.Category));
            }
            else
            {
                foreach (KeyValuePair<string, List<PNMessageResult>> kvp in result.Channels)
                {
                    foreach (PNMessageResult pn_message_result in kvp.Value)
                    {
                        // Format Data into readable format 
                        JSONInformation chatmessage = JsonUtility.FromJson<JSONInformation>(pn_message_result.Payload.ToString());
                        // Call the function to display the message in plaintext
                        create_chat(chatmessage);
                        // Counter used for positioning the text UI
                        if (counter != 200)
                        {
                            counter += 20;
                        }
                    }
                }
            }
        });

        // Subscribe to a PubNub Channel to receive messages when they are sent on that channel 
        pubnub.Subscribe().Channels(new List<string>()
        {
            "chatchannel13"
        }).WithPresence().Execute();

        // This is the subscribe callback function where data is received that is received on the channel
        pubnub.SubscribeCallback += (sender, e) =>
        {
            SubscribeEventEventArgs message = e as SubscribeEventEventArgs;
            if (message.MessageResult != null)
            {
                // Format data into readable format 
                JSONInformation chatmessage = JsonUtility.FromJson<JSONInformation>(message.MessageResult.Payload.ToString());
                // Call the function to display the message in plaintext 
                create_chat(chatmessage);
                // When a new Chat is created, remove the first chat and transform all the messages on the page up
                sync_chat();
                // Counter used for positioning the text UI
                if (counter != 200)
                {
                    counter += 20;
                }
            }
        };
    }
    // Function used to create new chat objects based on the data received from PubNub
    void create_chat(JSONInformation payload)
    {
        // Create a string with the username and text
        string current_object = string.Concat(payload.username, ": ", payload.text);
        // Create a new GameObject that will display the text of the data sent via PubNub
        GameObject chat_message = new GameObject(current_object);
        chat_message.transform.SetParent(canvas_object.GetComponent<Canvas>().transform);
        chat_message.AddComponent<Text>().text = current_object;
        // Assign text to the gameobject and add visual properties to text
        var chat_text = chat_message.GetComponent<Text>();
        chat_text.font = custom_font;
        chat_text.color = UnityEngine.Color.black;
        chat_text.fontSize = 15;
        // Assign a RectTransform to gameobject to manipulate positioning chat 
        RectTransform rect_transform;
        rect_transform = chat_text.GetComponent<RectTransform>();
        rect_transform.localPosition = new Vector2(-50, 60 - counter);
        rect_transform.sizeDelta = new Vector2(200, 20);
        rect_transform.localScale = new Vector3(1F, 1F, 1F);
        // Assign the gameobject to the queue of chatmessages
        chat_message_queue.Enqueue(chat_message);
        // Keep track of how many objects displayed on the screen
        index_counter++;
    }

    void sync_chat()
    {
        // If more than 13 objects are on the screen we need to start removing them 
        if (index_counter >= 11)
        {
            // Delete the first gameobject in the queue
            GameObject deleted_chat = chat_message_queue.Dequeue();
            Destroy(deleted_chat);
            // Move all existing text gameobjects up the Y axis 50 pixels
            foreach (GameObject move_chat in chat_message_queue)
            {
                RectTransform move_text = move_chat.GetComponent<RectTransform>();
                move_text.offsetMax = new Vector2(move_text.offsetMax.x, move_text.offsetMax.y + 20);
            }
        }
    }

    void task_on_click()
    {
        // when the user clicks the submit button 
        // Create a JSON object from input field input 
        JSONInformation publish_message = new JSONInformation();
        publish_message.username = Globals.username;
        publish_message.text = text_input.text;
        string publish_message_to_json = JsonUtility.ToJson(publish_message);
        // Publish the JSON object to the assigned PubNub channel
        pubnub.Publish().Channel("chatchannel13").Message(publish_message_to_json).Async((result, status) =>
        {
            if (status.Error)
            {
                Debug.Log(status.Error);
                Debug.Log(status.ErrorData.Info);
            }
            else
            {
                Debug.Log(string.Format("Publish Timetoken:  {0}", result.Timetoken));
            }
        });
        text_input.text = "";
    }
}

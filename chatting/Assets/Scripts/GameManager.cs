using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string username;
    public int maxMessages = 25;

    public GameObject chatPanel, textObject;
    public InputField chatBox;

    public Color playerMessage, info;

    [SerializeField]
    List<Message> messageList = new();

    void Start()
    {
        
    }

    void Update()
    {
        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat($"{DateTime.UtcNow} - {username}: {chatBox.text}", Message.MessageType.Player);
                chatBox.text = "";
            }
        }
        else
        {
            if (!chatBox.isFocused)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    chatBox.ActivateInputField();
                }
            }
        }
        
        if (!chatBox.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SendMessageToChat($"{DateTime.UtcNow} You pressed the space!", Message.MessageType.Info);
        }
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);

        }

        Message newMessage = new()
        {
            text = text,
            messageType = messageType
        };

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        switch (messageType)
        {
            case Message.MessageType.Player:
                return playerMessage;
            case Message.MessageType.Info:
                return info;
            default:
                return Color.black;
        }
    }
}

[Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        Player,
        Info
    }
}

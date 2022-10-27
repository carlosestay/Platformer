using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown comportDropdown, baudRateDropdown;

    [SerializeField]
    string[] comportNames;

    [SerializeField]
    Button connect, startGame;

    [SerializeField]
    GameObject portPanel;
    [SerializeField]
    TextMeshProUGUI portPanelText;
    Color textColor;

    void Start()
    {
        portPanelText = portPanel.GetComponentInChildren<TextMeshProUGUI>();
        textColor = portPanelText.color;
        portPanel.SetActive(false);
        comportNames = SerialPort.GetPortNames();
        comportDropdown = GetComponentInChildren<TMP_Dropdown>();
        foreach (string comportName in comportNames)
        {
            comportDropdown.options.Add(new TMP_Dropdown.OptionData(comportName));
        }
        connect.gameObject.SetActive(false);
        startGame.gameObject.SetActive(false);      
        baudRateDropdown.gameObject.SetActive(false);
        EventManager.Instance.onPortOpenResult.AddListener(ConnectionResult);
        comportDropdown.onValueChanged.AddListener(PortSelected);
        baudRateDropdown.onValueChanged.AddListener(BrSelected);
        connect.onClick.AddListener(ConnectPort);
        startGame.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    private void ConnectionResult(GameObject sender, CustomEventArgs args)
    {
        if(args.Successful)
        {
            portPanelText.color = textColor;
            print("Connection sucess!");
            portPanelText.text = "Port Successfully Connected!";
            portPanel.SetActive(true);
            startGame.gameObject.SetActive(true);
        }
        else
        {
            portPanelText.color = Color.red;
            print("Port Already Open, Try a different one");
            portPanelText.text = "It looks like the port is already Open, Try a different one";
            portPanel.SetActive(true);
        }
    }

    private void BrSelected(int arg)
    {
        portPanel.SetActive(false);
        if (arg != 0)
        {
            connect.gameObject.SetActive(true);
        }
        else
        {
            connect.gameObject.SetActive(false);
        }
    }

    private void ConnectPort()
    {
        string portName = comportDropdown.options[comportDropdown.value].text;
        int baudRate = int.Parse(baudRateDropdown.options[baudRateDropdown.value].text);
        print(portName);
        print(baudRate);
        EventManager.Instance.onPortTryOpen.Invoke(gameObject, new CustomEventArgs(portName, baudRate));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PortSelected(int arg)
    {
        portPanel.SetActive(false);
        if (arg != 0)
        {
            baudRateDropdown.gameObject.SetActive(true);
        }
        else
        {
            baudRateDropdown.gameObject.SetActive(false);
            connect.gameObject.SetActive(false);
        }
    }
}

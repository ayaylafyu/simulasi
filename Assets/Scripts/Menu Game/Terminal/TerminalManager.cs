using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerminalManager : MonoBehaviour
{
    public GameObject directoryLine;
    public GameObject responseLine;

    public TMP_InputField terminalInput;
    public GameObject userInputLine;
    public ScrollRect sr;
    public GameObject msgList;

    Interpreter interpreter;
    public string userInput { get; private set; }

    public static TerminalManager Instance;

    private int maxHistorySize = 10;
    private List<string> userInputHistory = new List<string>();
    private int historyIndex = -1;

    public static bool meterpreter = false;
    private bool meterpreterOut = false;
    public static bool netcat = false;
    private bool netcatOut = false;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        interpreter = GetComponent<Interpreter>();
        netcat = PlayerPrefs.GetInt("terminalnetcat", 0) == 1;
        meterpreter = PlayerPrefs.GetInt("terminalmeterpreter", 0) == 1;
        TMP_Text textTerminal = userInputLine.GetComponentInChildren<TMP_Text>();
        if (meterpreter)
        {
            textTerminal.text = "meterpreter>";
            meterpreterOut = true;
        }
        else if (netcat)
        {
            textTerminal.text = "reverse>";
            netcatOut = true;
        }
        else
        {
            textTerminal.text = "terminal>";
            meterpreterOut = false;
            netcatOut = false;
        }
        //set the width of the first text component based on the number of characters
        int numCharacters = textTerminal.text.Length;
        float widthPerCharacter = 9.5f; // Adjust this value as needed
        float textWidth = numCharacters * widthPerCharacter;
        RectTransform rectTransform = textTerminal.rectTransform;
        rectTransform.sizeDelta = new Vector2(textWidth, rectTransform.sizeDelta.y);
        // Menambahkan listener ke event ketika obyek aktif
        //Interpreter.OnLoadingCompleted += HandleLoadingComplete;
    }

    private void OnDisable()
    {
        // Menghapus listener dari event ketika obyek nonaktif atau dihancurkan
        //Interpreter.OnLoadingCompleted -= HandleLoadingComplete;
    }

    private void Update()
    {
        if (terminalInput.isFocused && Input.GetKeyDown(KeyCode.UpArrow))
        {
            ShowPreviousHistory();
        }
        else if (terminalInput.isFocused && Input.GetKeyDown(KeyCode.DownArrow))
        {
            ShowNextHistory();
        }
    }


    private void OnGUI()
    {
        if (terminalInput.isFocused && terminalInput.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            //Store whatever the user typed
            string userInput = terminalInput.text;


            //Clear the input field
            ClearInputField();

            AddToHistory(userInput);

            //instantiate a game object with a directory prefix
            AddDirectoryLine(userInput);

            //add the interpretation lines
            int lines = AddInterpreterLines(interpreter.Interpret(userInput));

            //scroll to the bottom of the scrollrect
            ScrollToBottom(lines);

            //Refocus the input field
            terminalInput.ActivateInputField();
            terminalInput.Select();

            //Move the user input to the end
            userInputLine.transform.SetAsLastSibling();

            TMP_Text textTerminal = userInputLine.GetComponentInChildren<TMP_Text>();

            //Instantiate the directory line
            //GameObject msg = Instantiate(userInputLine, msgList.transform);
            //TMP_Text[] textTerminal = msg.GetComponentsInChildren<TMP_Text>();

            //Set the text of this new game object
            if (meterpreter)
            {
                textTerminal.text = "meterpreter>";
                meterpreterOut = true;
            }
            else if (netcat)
            {
                textTerminal.text = "reverse>";
                netcatOut = true;
            }
            else
            {
                textTerminal.text = "terminal>";
                meterpreterOut = false;
                netcatOut = false;
            }
            

            //set the width of the first text component based on the number of characters
            int numCharacters = textTerminal.text.Length;
            float widthPerCharacter = 9.5f; // Adjust this value as needed
            float textWidth = numCharacters * widthPerCharacter;
            RectTransform rectTransform = textTerminal.rectTransform;
            rectTransform.sizeDelta = new Vector2(textWidth, rectTransform.sizeDelta.y);

            //CheckInput();
            Debug.Log("user input : " + userInput);

            UpdateUserInput(userInput);
        }
    }

    // Method untuk menambahkan input ke riwayat
    private void AddToHistory(string userInput)
    {
        userInputHistory.Insert(0, userInput);

        // Memastikan riwayat tidak melebihi batas maksimum
        while (userInputHistory.Count > maxHistorySize)
        {
            userInputHistory.RemoveAt(userInputHistory.Count - 1);
        }

        // Set ulang indeks riwayat
        historyIndex = -1;
    }

    // Method untuk menampilkan riwayat input sebelumnya
    private void ShowPreviousHistory()
    {
        if (historyIndex < userInputHistory.Count - 1)
        {
            historyIndex++;
            Debug.Log("HistoryIndex: " + historyIndex);
            terminalInput.text = userInputHistory[historyIndex];
            terminalInput.MoveTextEnd(false);
        }
        
    }

    // Method untuk menampilkan riwayat input berikutnya
    private void ShowNextHistory()
    {
        /*if (historyIndex >= 0)
        {
            historyIndex--;
            if (historyIndex >= 0)
            {
                terminalInput.text = userInputHistory[historyIndex];
            }
            else
            {
                terminalInput.text = "";
            }
        }*/

        if (historyIndex > 0)
        {
            historyIndex--;
            Debug.Log("HistoryIndex: " + historyIndex);
            terminalInput.text = userInputHistory[historyIndex];
            terminalInput.MoveTextEnd(false); // Agar kursor berada di akhir teks
        }
        else if (historyIndex == 0)
        {
            historyIndex--;
            Debug.Log("HistoryIndex: " + historyIndex);
            terminalInput.text = "";
        }
    }

    public void UpdateUserInput(string input)
    {
        userInput = input;

        // Tambahkan input ke dalam histori
        /*inputHistory.Insert(0, userInput);

        // Batasi histori hingga sejumlah tertentu
        if (inputHistory.Count > maxInputHistory)
        {
            inputHistory.RemoveAt(inputHistory.Count - 1);
        }

        Debug.Log("history : " + inputHistory[historyIndex]);

        // Reset indeks histori
        historyIndex = -1;*/
        
    }

    void ClearInputField()
    {
        terminalInput.text = "";
    }

    public void AddDirectoryLine(string userInput)
    {
        //Resizing the command line container, so the scrollRect doesn't throw a fit
        Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;
        msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 25f);

        //Instantiate the directory line
        GameObject msg = Instantiate(directoryLine, msgList.transform);

        //Set its child index
        msg.transform.SetSiblingIndex(msgList.transform.childCount - 1);

        //Set the text of this new game object
        /*if (meterpreter)
        {
            msg.GetComponentsInChildren<TMP_Text>()[0].text = "meterpreter>";
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(msg.GetComponentsInChildren<TMP_Text>()[0].rectTransform);
        Debug.Log(msg.GetComponentsInChildren<TMP_Text>()[0].text);
        msg.GetComponentsInChildren<TMP_Text>()[1].text = userInput;*/

        // Set the text of this new game object
        TMP_Text[] textComponents = msg.GetComponentsInChildren<TMP_Text>();

        if (meterpreterOut)
        {
            textComponents[0].text = "meterpreter>";
        }
        else if (netcatOut)
        {
            textComponents[0].text = "reverse>";
        }
        else
        {
            textComponents[0].text = "terminal>";
        }

        // Set text for user input
        textComponents[1].text = userInput;

        // Force rebuild layout to adjust the width of the first text component
        //LayoutRebuilder.ForceRebuildLayoutImmediate(textComponents[0].rectTransform);

        //set the width of the first text component based on the number of characters
        int numCharacters = textComponents[0].text.Length;
        float widthPerCharacter = 9.5f; // Adjust this value as needed
        float textWidth = numCharacters * widthPerCharacter;
        RectTransform rectTransform = textComponents[0].rectTransform;
        rectTransform.sizeDelta = new Vector2(textWidth, rectTransform.sizeDelta.y);
    }

    public int AddInterpreterLines(List<string> interpretation)
    {
        // Debug log untuk melihat berapa banyak baris hasil interpretasi
        Debug.Log("Number of interpretation lines: " + interpretation.Count);
        for (int i = 0; i < interpretation.Count; i++)
        {
            //instantiate the response line
            GameObject res = Instantiate(responseLine, msgList.transform);

            //set it to the end of all the messages
            res.transform.SetAsLastSibling();

            //get the size of the message list, and resize
            Vector2 listSize = msgList.GetComponent<RectTransform>().sizeDelta;
            msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(listSize.x, listSize.y + 25f);

            //set the text of this response line to be whatever the interpreter string is
            res.GetComponentInChildren<TMP_Text>().text = interpretation[i];

            Debug.Log("interpreterline");
        }

        return interpretation.Count;
    }

    void ScrollToBottom(int lines)
    {
        /*if (lines > 4)
        {
            //sr.verticalScrollbar.gameObject.SetActive(false);
            sr.velocity = new Vector2(0, 450);
        }
        else
        {
            //sr.verticalScrollbar.gameObject.SetActive(true);
            sr.verticalNormalizedPosition = 0;
        }*/
        sr.verticalNormalizedPosition = 0;
    }

    public void CleanUpClonedData()
    {
        Transform msgListTransform = msgList.transform;

        // Loop melalui semua child pada msgList
        foreach (Transform child in msgListTransform)
        {
            // Cek apakah child memiliki tag "ClonedObject"
            if (child.CompareTag("ClonedObject"))
            {
                Debug.Log("Destroying cloned object: " + child.name);
                Destroy(child.gameObject);
            }
        }

        float originalMsgListHeight = 120f;
        // Kembalikan tinggi msgList ke nilai semula
        RectTransform msgListRectTransform = msgList.GetComponent<RectTransform>();
        msgListRectTransform.sizeDelta = new Vector2(msgListRectTransform.sizeDelta.x, originalMsgListHeight);
    }
}

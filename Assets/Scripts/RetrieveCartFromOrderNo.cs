using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;

public class RetrieveCartFromOrderNo : MonoBehaviour
{
    #region private members 	
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    #endregion

    #region public members 	
    public string serverAddress = "172.21.0.90";
    public string orderNo = "3158";
    public string mainRequestDescription = "SQLDataStudents.php?Command=cart&ONo=";
    public bool useWebRequest = true; // Set to false if you want to use TCP socket instead
    public string fieldNameToExtract = "cart"; // Set this to the field name you want to extract from the JSON
    #endregion

    void Start()
    {
        if (!useWebRequest)
        {
            ConnectToTcpServer();
        }
    }

    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }

    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient(serverAddress, 2000);
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incomingData = new byte[length];
                        Array.Copy(bytes, 0, incomingData, 0, length);
                        string serverMessage = Encoding.ASCII.GetString(incomingData);
                        Debug.Log("Server message received as: " + serverMessage);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    private void SendMessageToServer(string message)
    {
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(message);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                Debug.Log("Message has been sent by client - should be received by the server");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void SendRequestToFactory()
    {
        if (useWebRequest)
        {
            StartCoroutine(GetRequest("http://" + serverAddress + "/" + mainRequestDescription + orderNo));
        }
        else
        {
            string requestMessage = "/" + mainRequestDescription + orderNo;
            SendMessageToServer(requestMessage);
        }
        Debug.Log("Request sent");
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string jsonResponse = webRequest.downloadHandler.text;
                    Debug.Log("Received: " + jsonResponse);

                    string extractedValue = ExtractFieldFromJson(jsonResponse, fieldNameToExtract);
                    if (extractedValue != null)
                    {
                        Debug.Log($"Extracted {fieldNameToExtract} value: " + extractedValue);
                    }
                    break;
            }
        }
    }

    public string ExtractFieldFromJson(string jsonString, string fieldName)
    {
        try
        {
            JToken jsonToken = JToken.Parse(jsonString);

            if (jsonToken is JObject jsonObject)
            {
                JToken fieldValue = jsonObject[fieldName];
                if (fieldValue != null)
                {
                    return fieldValue.ToString();
                }
            }
            else if (jsonToken is JArray jsonArray)
            {
                // If it's an array, we'll look for the field in the first object of the array
                if (jsonArray.Count > 0 && jsonArray[0] is JObject firstObject)
                {
                    JToken fieldValue = firstObject[fieldName];
                    if (fieldValue != null)
                    {
                        return fieldValue.ToString();
                    }
                }
            }

            Debug.LogWarning($"Field '{fieldName}' not found in JSON response.");
            return null;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error parsing JSON: {e.Message}");
            return null;
        }
    }

    void OnApplicationQuit()
    {
        if (socketConnection != null)
        {
            socketConnection.Close();
        }
        if (clientReceiveThread != null)
        {
            clientReceiveThread.Abort();
        }
    }
}

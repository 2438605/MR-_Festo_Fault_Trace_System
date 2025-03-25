using Newtonsoft.Json;

using UnityEngine;

using UnityEngine.Networking;

using System.Collections;

using System.Collections.Generic;

using TMPro;

using System.Threading;

using System;

public class PCBFullCheck : MonoBehaviour
{

    public List<string> PCBFullCheckData = new List<string>();

    public PCBFullJSON[] pcbFullCheckObjectArray;

    public string listInfo;

    public TMP_Text pcbFullCheckInfo;

    // Method to receive data from the server and update UI

    public void ReceieveData(string PCBFullCheckStringPHPMany)

    {

        // Ensure JSON is formatted correctly

        string newPCBFullCheckStringPHPMany = fixJson(PCBFullCheckStringPHPMany);

        Debug.LogWarning(newPCBFullCheckStringPHPMany);

        ExtractFillLevel(newPCBFullCheckStringPHPMany);

    }

    // Ensure JSON format compatibility

    string fixJson(string value)

    {

        value = "{\"Items\":" + value + "}";

        return value;

    }

    // Coroutine to make a GET request to the server

    public void GetPCBFullCheck()

    {

        StartCoroutine(GetRequest("http://172.21.0.90/SQLDataStudents.php?Command=Boxes&Fill"));     //calls coroutine and sets string

    }

    // Coroutine to handle the GET request and update UI based on response

    IEnumerator GetRequest(string url)

    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))

        {

            // Request and wait for the desired page.

            yield return webRequest.SendWebRequest();

            string[] pages = url.Split('/');

            int page = pages.Length - 1;

            // Handle different results of the web request

            switch (webRequest.result)

            {

                case UnityWebRequest.Result.ConnectionError:

                case UnityWebRequest.Result.DataProcessingError:

                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);

                    break;

                case UnityWebRequest.Result.ProtocolError:

                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);

                    break;

                case UnityWebRequest.Result.Success:

                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                    // Process received data

                    ReceieveData(webRequest.downloadHandler.text);

                    Debug.LogError("PCB Check Success");

                    break;

            }

        }

    }

    public void ExtractFillLevel(string boxInfo)
    {
        var jsonObject = JsonConvert.DeserializeObject<RootObject>(boxInfo);
        string textFillLevel = "";

        if (jsonObject.Items.Count >= 2)
        {
            string fillLevel = jsonObject.Items[1].FillLevel;
            Debug.Log("Fill Level of second item: " + fillLevel);

            // Parse the percentage value (new addition)
            if (float.TryParse(fillLevel.TrimEnd('%'), out float fillPercentage))
            {
                // Keep original display text
                textFillLevel = "Fill Level of second item: " + fillLevel;

                // Add warning message (new addition)
                if (fillPercentage <= 20)
                {
                    textFillLevel += "\n<color=red>WARNING: High failure risk!</color>";
                }
                else if (fillPercentage <= 50)
                {
                    textFillLevel += "\n<color=yellow>WARNING: Possible failure risk</color>";
                }
                else
                {
                    textFillLevel += "\n<color=green>No failure risk</color>";
                }
            }
            else
            {
                // Original behavior if parsing fails
                textFillLevel = "Fill Level of second item: " + fillLevel;
            }
        }
        else
        {
            Debug.LogWarning("There are less than two items in the JSON data.");
            textFillLevel = "Insufficient data";
        }

        pcbFullCheckInfo.text = textFillLevel;
    }


}

[Serializable]

public class PCBFullJSON

{

    [JsonProperty("BoxPNo-ID")]

    public string BoxPNoID;

    [JsonProperty("Box Description")]

    public string BoxDescription;

    [JsonProperty("Parts inside")]

    public string PartsInside;

    [JsonProperty("Capacity")]

    public string Capacity;

    [JsonProperty("Fill Level")]

    public string FillLevel;

}

[Serializable]

public class RootObject

{

    public List<PCBFullJSON> Items;

}


//using realvirtual;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class UIUpdateManagerBoxInside: MonoBehaviour
//{
//    [Header("Project Scripts")]
//    public OPCUA_Interface[] interfacess;
//    public NodeReader[] RFIDInNodeReaders;
//    public NodeReader[] BoxInsideNodeReaders;
//    public SendOrder sendOrder;

//    [Header("Project UI Elements")]
//    public Image[] connectionImages;
//    public Image[] BoxInside;
//    public TMP_Text[] RFIDInTMP;



//    public TMP_Dropdown orderQuantityDropdown;
//    public TMP_Dropdown partNumberDropdown;

//    // Method to update connection status images based on OPC UA interface connection
//    public void UpdateConnectionImages(int interfaceToRead)
//    {
//        if (interfacess[interfaceToRead].IsConnected)
//        {
//            connectionImages[interfaceToRead].color = Color.green;
//        }
//        else
//        {
//            connectionImages[interfaceToRead].color = Color.red;
//        }
//    }

//    // Method to update text information from OPC UA node to TMP_Text component
//    public void UpdateDataFromNodeTMP(int interfaceToRead, string node)
//    {

//        if (node == "RFIDIn")
//        {
//            Debug.LogWarning(interfaceToRead + 1 + " is reading: " + RFIDInNodeReaders[interfaceToRead].dataFromOPCUANode);
//        }

//        if (node == "BoxInside")
//        {
//        }

//    }

//    // Method to send an order to the machine using values from UI dropdowns
//    public void SendOrderToMachine()
//    {
//        sendOrder.partNumber = partNumberDropdown.options[partNumberDropdown.value].text;
//        sendOrder.qty = orderQuantityDropdown.options[orderQuantityDropdown.value].text;
//        sendOrder.SendOrderToFactory();
//    }

//    private void Update()
//    {
//        for (int i = 0; i < RFIDInNodeReaders.Length; i++)
//        {
//            RFIDInTMP[i].text = RFIDInNodeReaders[i].dataFromOPCUANode;
//        }

//        for (int i = 0; i < BoxInsideNodeReaders.Length; i++)
//        {
//            if (BoxInsideNodeReaders[i].dataFromOPCUANode == "True")
//            {
//                BoxInside[i].color = Color.green;
//            }
//            if (BoxInsideNodeReaders[i].dataFromOPCUANode == "False")
//            {
//                BoxInside[i].color = Color.red;
//            }
//        }
//    }
//}
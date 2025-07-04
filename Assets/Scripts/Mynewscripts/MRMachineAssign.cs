using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MRMachineAssign : MonoBehaviour
{
    [Header("Nodes Being Read & Holders")]
    public GameObject rfidReadersHolder;
    public GameObject emgStopReadersHolder;
    public MRNodeReader PCBboxStatusHolder;
    public MRNodeReader PCBboxNumberHolder;
    public MRNodeReader PCBboxIDHolder;
    public MRNodeReader[] rfidReaders;
    public MRNodeReader[] emgStopReaders;
    
    
    [Header("UI & Output Display")]
    //public TMP_Dropdown machineNumberDropdown;
    public TMP_Text machineNumberDisplay;
    public TMP_Text rFIDDisplay;
    public TMP_Text emgStopDisplay;
    public TMP_Text PCBboxStatusDisplay;
    public TMP_Text PCBboxNumberDisplay;
    public TMP_Text PCBboxIDDisplay;
    private bool machineAssigned = false;
    public int machineNumber;

    public bool isMachine2;

    private void Start()
    {
        rfidReadersHolder = GameObject.Find("RFIDIn");
        emgStopReadersHolder = GameObject.Find("EMGStop");
        //PCBboxStatusHolder = GameObject.Find("PCBBoxStatus");
        rfidReaders = rfidReadersHolder.GetComponentsInChildren<MRNodeReader>();
        emgStopReaders = emgStopReadersHolder.GetComponentsInChildren<MRNodeReader>();
        //PCBboxStatusDisplay = PCBboxStatusHolder.GetComponentInChildren<TMP_Text>();

        PCBboxStatusHolder = GameObject.Find("PCB BOX Reader").GetComponent<MRNodeReader>();
        PCBboxNumberHolder = GameObject.Find("PCB BOX ID Reader").GetComponent<MRNodeReader>();
        PCBboxIDHolder = GameObject.Find("PCB BOX Num Reader").GetComponent<MRNodeReader>();

        AssignMachineNumber();
    }
    private void Update()
    {
        if (machineAssigned)
        {
            machineNumberDisplay.text = "Machine Number " + (machineNumber).ToString();

            rFIDDisplay.text = "The current cart in this machine is " + rfidReaders[machineNumber - 1].dataFromOPCUANode;
            //Debug.Log("RFID is" + rfidReaders[machineNumber - 1].dataFromOPCUANode);

            if(emgStopReaders[machineNumber - 1].dataFromOPCUANode == "False")
            {
                emgStopDisplay.text = "EMERGENCY!!!!!";
                emgStopDisplay.color = Color.red;
            }
            else
            {
                emgStopDisplay.text = "No current emergency";
                emgStopDisplay.color = Color.green;
            }
            //Debug.Log("EMG Stop is" + emgStopReaders[machineNumber - 1].dataFromOPCUANode);

            if (isMachine2)
            {
                PCBboxStatusDisplay.text ="PCB Box Presence is " + PCBboxStatusHolder.dataFromOPCUANode ;
              
                PCBboxNumberDisplay.text = "PCB Num is " + PCBboxNumberHolder.dataFromOPCUANode;
                PCBboxIDDisplay.text = "PCB ID is " + PCBboxIDHolder.dataFromOPCUANode;
            }
        }

           
    }
    public void AssignMachineNumber()
    {
         machineAssigned = true;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScrollUI : MonoBehaviour
{
    public GameObject       medicinesButtonPanel;
    public GameObject       medicinesButtonPrefab;
    public List<GameObject> medicinesButtonsList;

    public PlayerInventory  _playerInventory;

    private void Start ()
    {
        medicinesButtonsList = new List<GameObject>();
    }
    public void CreateMedicinesButton (MedicinesScriptableObject medicines)
    {
        GameObject but = Instantiate(medicinesButtonPrefab, medicinesButtonPanel.transform);
        but.GetComponent<Button>().onClick.AddListener(delegate { _playerInventory.RemoveItem(medicines); });
        but.GetComponent<Image>().sprite = medicines.icon;
        but.GetComponentInChildren<Text>().text = "1";
        but.name = medicines.name;
        medicinesButtonsList.Add(but);
    }

    public void ModifiedIndexOnButton (int modifiedIndex)
    {
        int numberItems = int.Parse(medicinesButtonsList[_playerInventory.lastModifiedIndex].GetComponentInChildren<Text>().text) + modifiedIndex;
        medicinesButtonsList[_playerInventory.lastModifiedIndex].GetComponentInChildren<Text>().text = numberItems.ToString();
    }

    public void RemoveMedicinesButton (int buttonIndex)
    {
        Destroy(medicinesButtonsList[buttonIndex]);
        medicinesButtonsList.RemoveAt(buttonIndex);
    }
}

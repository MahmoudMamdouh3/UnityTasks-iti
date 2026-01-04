using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryPanel; // The panel to open/close
    public Transform contentContainer; // The "Content" object inside Scroll View
    
    [Header("Settings")]
    public GameObject itemPrefab;      // The blue prefab we made
    public int numberOfItems = 20;     // Requirement: Set count in Editor

    void Start()
    {
        GenerateItems();
        inventoryPanel.SetActive(false); // Hide it at start
    }

    // Requirement: Panel opens up when button is clicked
    public void ToggleInventory()
    {
        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive);
    }

    // Requirement: Items created in Runtime
    void GenerateItems()
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            Instantiate(itemPrefab, contentContainer);
        }
    }
}
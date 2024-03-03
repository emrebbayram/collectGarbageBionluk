using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float colletDistance;
    [SerializeField]
    private LayerMask lootMask;


    [SerializeField]
    private List<item> items = new List<item>();
    [SerializeField]
    private int inventoryLimit;
    [SerializeField]
    private GameObject itemUIObj;
    [SerializeField]
    private Transform itemUIParent;
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, colletDistance, lootMask))
        {
            if (hit.collider.gameObject.tag == "trash")
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (items.Count < inventoryLimit) {
                        items.Add(hit.collider.gameObject.GetComponent<itemObj>().item);
                        Destroy(hit.collider.gameObject);
                        showInventory();
                    }else
                    {
                        //Envanter dolu
                    }
                }
            }else if (hit.collider.gameObject.tag == "bin"){
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    bool found = false;
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i].GarbageType == hit.collider.gameObject.GetComponent<itemObj>().item.GarbageType)
                        {
                            found = true;
                            items.RemoveAt(i);
                            break;
                        }
                    }
                    showInventory();
                    if (!found)
                    {
                        Debug.Log("Boþ");
                    }
                }
            }
        }
    }
    public void showInventory()
    {
        for (int i = 0; i < itemUIParent.childCount; i++)
        {
            Destroy(itemUIParent.GetChild(0).gameObject);
        }
        List<item> itemsNew = new List<item>();
        List<int> itemsCount = new List<int>();
        for (int i = 0; i < items.Count; i++)
        {
            if (!itemsNew.Contains(items[i]))
            {
                itemsNew.Add(items[i]);
                itemsCount.Add(1);
            }else
            {
                itemsCount[itemsNew.IndexOf(items[i])] += 1;
            }
        }
        for (int i = 0; i < itemsNew.Count; i++)
        {
            GameObject itemObj = Instantiate(itemUIObj);
            itemObj.transform.parent = itemUIParent;
            itemObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(100,-70 + (i * -180));
            itemObj.GetComponentInChildren<TextMeshProUGUI>().text = itemsCount[i].ToString() + "x";
            itemObj.transform.GetChild(0).GetComponent<Image>().sprite = itemsNew[i].Image;

        }
    }
}

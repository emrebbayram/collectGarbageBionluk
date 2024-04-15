using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField]
    private Transform itemPreviewParent;
    [SerializeField]
    private GameObject viewCameraObj;
    [SerializeField]
    private RenderTexture[] renderTextures;
    [SerializeField]
    private Animator warningAnim;
    private float warningEndTime;
    [SerializeField]
    private RectTransform inputShowObj;

    private Canvas mainCanvas;
    private void Start()
    {
        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, colletDistance, lootMask))
        {
            if (hit.collider.gameObject.tag == "trash")
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (items.Count < inventoryLimit)
                    {
                        items.Add(hit.collider.gameObject.GetComponent<itemObj>().item);
                        Destroy(hit.collider.gameObject);
                        showInventory();
                    }
                    else
                    {
                        //Envanter dolu
                    }
                }
                showIndicator(hit);
            }
            else if (hit.collider.gameObject.tag == "bin")
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    bool found = false;
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i].GarbageType == hit.collider.gameObject.GetComponent<itemObj>().item.GarbageType)
                        {
                            found = true;
                            items.RemoveAt(i);
                            gameManager.Instance.point += gameManager.Instance.earnPoint;
                            gameManager.Instance._gameEndTime += gameManager.Instance.timeEarnAmount;
                            showInventory();
                            break;
                        }
                    }
                    if (!found)
                    {
                        StartCoroutine(warningSend(3));
                        gameManager.Instance.point -= gameManager.Instance.finePoint;
                    }
                    else
                    {
                        print(items.Count);
                        showInventory();
                    }
                }
                showIndicator(hit);
            }
            
        }
        else
        {
            inputShowObj.anchoredPosition = new Vector2(99999, 99999);
        }
    }
    public void showIndicator(RaycastHit hit)
    {
        Vector2 positionOnScreen = mainCamera.WorldToScreenPoint(hit.collider.gameObject.transform.position);
        float scaleFactor = mainCanvas.scaleFactor;
        Vector2 finalPosition = new Vector2(positionOnScreen.x / scaleFactor, positionOnScreen.y / scaleFactor) - (new Vector2(Screen.width, Screen.height) / 2);

        inputShowObj.anchoredPosition = finalPosition;
    }
    public void showInventory()
    {
        foreach (Transform item in itemPreviewParent.transform)
        {
            DestroyImmediate(item.gameObject);
        }
        foreach (Transform item in itemUIParent.transform)
        {
            DestroyImmediate(item.gameObject);
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
        foreach (Transform item in itemPreviewParent.transform)
        {
            DestroyImmediate(item.gameObject);
        }
        for (int i = 0; i < itemsNew.Count; i++)
        {
            GameObject viewObj = Instantiate(itemsNew[i].Object);
            Vector3 newPos = new Vector3(1000 + i * 1000, 1000 + i * 1000, 1000 + i * 1000);
            viewObj.transform.position = newPos;
            GameObject viewCamera = Instantiate(viewCameraObj);
            viewCamera.transform.position = newPos + new Vector3(itemsNew[i].PreviewDistance, 1, 0);
            //viewCamera.transform.LookAt(viewObj.transform.position);
            viewCamera.GetComponent<Camera>().targetTexture = renderTextures[i];
            viewObj.AddComponent<rotateObj>();
            viewObj.transform.parent = itemPreviewParent;
            viewCamera.transform.parent = itemPreviewParent;
        }
        for (int i = 0; i < itemsNew.Count; i++)
        {
            GameObject itemObj = Instantiate(itemUIObj);
            itemObj.transform.SetParent(itemUIParent);
            itemObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(60,-60 + (i * -90));
            itemObj.GetComponentInChildren<TextMeshProUGUI>().text = itemsCount[i].ToString() + "x";
            itemObj.transform.GetChild(0).GetComponent<RawImage>().texture = renderTextures[i];
        }
    }
    public IEnumerator warningSend(float duration)
    {
        if (warningEndTime <= Time.time)
        {
            warningAnim.SetTrigger("enter");
            
        }else
        {
            yield break;
        }
        warningEndTime = Mathf.Clamp(Time.time+warningEndTime + duration, 0, Time.time + warningEndTime + 5);
        while (Time.time <= warningEndTime)
        {
            yield return new WaitForEndOfFrame();
        }
        warningAnim.SetTrigger("exit");
    }
}

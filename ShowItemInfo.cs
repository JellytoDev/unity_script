using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{         //Tooltip
                               //The tooltip script
    public GameObject tooltipGameObject;                        //the tooltip as a GameObject
    public RectTransform canvasRectTransform;                    //the panel(Inventory Background) RectTransform
    public RectTransform tooltipRectTransform;                  //the tooltip RectTransform
    private Item item;


    void Start()
    {
        
        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
        {
            tooltipGameObject = GameObject.FindGameObjectWithTag("Tooltip");
            tooltipRectTransform = tooltipGameObject.GetComponent<RectTransform>() as RectTransform;
        }
        canvasRectTransform = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>() as RectTransform;
    }

    


    public void OnPointerEnter(PointerEventData data)                               //if you hit a item in the slot
    {                             //set all informations of the item in the tooltip

        //tooltipGameObject.GetComponent<CanvasGroup>().alpha = 1;
        if (canvasRectTransform == null)
                return;


        Vector3[] slotCorners = new Vector3[4];//get the corners of the slot
        GetComponent<RectTransform>().GetWorldCorners(slotCorners); //get the corners of the slot                

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, slotCorners[3]*0.9f, data.pressEventCamera, out localPointerPosition))   // and set the localposition of the tooltip...
        {
            Debug.Log("tooltip open");

            tooltipGameObject.transform.Find("ItemName").GetComponent<Text>().text =
                this.transform.Find("name").GetComponent<Text>().text;

                
            tooltipGameObject.transform.Find("ItemImage").GetComponent<RawImage>().texture =
                this.transform.Find("image").GetComponent<RawImage>().texture;
                
            tooltipGameObject.transform.Find("ItemName").GetComponent<Text>().text =
                this.transform.Find("name").GetComponent<Text>().text;

            tooltipGameObject.transform.Find("ItemDesc").GetComponent<Text>().text =
                this.transform.Find("price").GetComponent<Text>().text +"원" +
                "\n\n"+this.transform.Find("description").GetComponent<Text>().text;

            /*
            if (transform.parent.parent.parent.GetComponent<Hotbar>() == null)
                tooltipRectTransform.localPosition = localPointerPosition;          //at the right bottom side of the slot
            else
                tooltipRectTransform.localPosition = new Vector3(localPointerPosition.x, localPointerPosition.y+200);
                */
        }

        

    }

    public void OnPointerExit(PointerEventData data)                //if we go out of the slot with the item
    {
        //tooltipGameObject.GetComponent<CanvasGroup>().alpha = 0;      //the tooltip getting deactivated
    }

}

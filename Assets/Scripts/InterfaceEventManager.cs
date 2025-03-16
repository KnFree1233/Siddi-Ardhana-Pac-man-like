using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InterfaceEventManager : MonoBehaviour
{
    [SerializeField] Image selectionImage;

    public void OnMouseEnter(float posY)
    {
        selectionImage.rectTransform.localPosition = new Vector3(-250, posY, 0);
    } 
}

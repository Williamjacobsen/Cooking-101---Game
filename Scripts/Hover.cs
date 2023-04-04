using UnityEngine;
using UnityEngine.EventSystems;

public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Object;
    private Transform ObjectTransform;
    private void Start() 
    {
        ObjectTransform = Object.GetComponent<Transform>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ObjectTransform.localScale = new Vector3((float)1.1, (float)1.1, (float)1.1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ObjectTransform.localScale = new Vector3(1, 1, 1);
    }
}

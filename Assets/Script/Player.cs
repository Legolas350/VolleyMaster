using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;
using System.Linq;

public class Player : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    private RectTransform rectTransform;
    private Image image;
    //public Vector3 targetPosition; // The target position for this object
    private Vector3 targetPosition;
    public float acceptableDistance; // The distance within which we'll consider the object to be at the target
    private Vector3 mOffset= new Vector3(30,1606,0);
    private Dictionary<string, List<Vector3>> targetPositions = new Dictionary<string, List<Vector3>>();

    public Image filterImage;
    private Vector3 initialPosition;
    private bool firstTouch = true;

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.color = Color.gray;
        if (firstTouch)
        {
            initialPosition = transform.position;
            firstTouch = false;
        }
        //Debug.Log("OnBeginDrag");
        //Debug.Log(targetPositions[gameObject.name][0]);
        /*Debug.Log("GameObject name: " + gameObject.name);
        foreach (KeyValuePair<string, List<Vector3>> entry in targetPositions)
        {
            Debug.Log("Key: " + entry.Key + ", Value: " + string.Join(", ", entry.Value.Select(v => v.ToString())));
        }*/
    }
    public void OnDrag(PointerEventData eventData)
    {
        //rectTransform.anchoredPosition += eventData.delta;
        transform.position = Input.mousePosition;
        /*Debug.Log("OnDrag");
        if (Vector3.Distance(transform.position, targetPosition+mOffset) <= acceptableDistance)
        {
            Debug.Log("Object is at the target!");
        }*/
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.color = Color.white;
        //Debug.Log("OnEndDrag");
        if (Vector3.Distance(transform.position, targetPosition+mOffset) <= acceptableDistance)
        {
            Debug.Log("Object is at the target!");
        }
        else
        {
            Debug.Log("Object is not at the target.");
        }
    }
    public bool IsAtTarget(int index)
    {
        return Vector3.Distance(transform.position, targetPositions[gameObject.name][index]+mOffset) <= acceptableDistance;
    }

    public void SetPlayerColor(Color color)
    {
        filterImage.color = color;
    }

    public void ResetPosition()
    {
        transform.position = initialPosition; // or any other default position
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Awake called");
        rectTransform = GetComponent<RectTransform>();
        image=GetComponent<Image>();
        
        List<Vector3> Passeur = new List<Vector3>() { new Vector3(923f, -1169f, 0),new Vector3(656,-502,0),new Vector3(529,-500,0),new Vector3(107,-500,0) ,new Vector3(160,-813,0),new Vector3(675,-588,0)};
        List<Vector3> Central = new List<Vector3>(){new Vector3(211f,-731,0),new Vector3(120,-718,0), new Vector3(907,-708,0),new Vector3(175,-712,0) ,new Vector3(114,-728,0),new Vector3(910,-718,0)};
        List<Vector3> R4 = new List<Vector3>() { new Vector3(800f, -1090f, 0),new Vector3(212,-1058,0) ,new Vector3(215,-1097,0),new Vector3(530,-1094,0),new Vector3(530,-1094,0),new Vector3(835,-1100,0)};
        List<Vector3> Pointu = new List<Vector3>() { new Vector3(91f, -724f, 0),new Vector3(385,-1221,0),new Vector3(682,-1228,0),new Vector3(904,-1237,0) ,new Vector3(917,-672,0),new Vector3(832,-489,0)};
        List<Vector3> R42 = new List<Vector3>() { new Vector3(221f, -1080f, 0),new Vector3(525,-1061,0) ,new Vector3(842,-1090,0),new Vector3(251,-1094,0),new Vector3(251,-1094,0),new Vector3(531,-1093,0)};
        List<Vector3> Libero = new List<Vector3>() { new Vector3(532f, -1094f, 0),new Vector3(826,-1045,0),new Vector3(531,-1097,0),new Vector3(806,-1097,0) ,new Vector3(806,-1097,0),new Vector3(221,-1103,0)};

        targetPositions.Add("Central", Central);
        targetPositions.Add("R4", R4);
        targetPositions.Add("Passeur", Passeur);
        targetPositions.Add("Pointu", Pointu);
        targetPositions.Add("R42", R42);
        targetPositions.Add("Libero", Libero);

        targetPosition= targetPositions[gameObject.name][0];
        //Debug.Log(targetPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

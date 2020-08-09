using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject canvasParent;
    private Canvas canvasGameObject;
    private GameObject textParent;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        canvasParent = new GameObject();
        canvasParent.name = "GameEndCanvas";
        canvasParent.AddComponent<Canvas>();

        canvasGameObject = canvasParent.GetComponent<Canvas>();
        canvasGameObject.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasParent.AddComponent<CanvasScaler>();
        canvasParent.AddComponent<GraphicRaycaster>();
        //canvasParent.transform.position = new Vector3(0, 0, 0);

        RectTransform rectTransform;
        rectTransform = canvasParent.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.sizeDelta = new Vector2(400, 200);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Text
    public Text AddTextToCanvas(string textString)
    {
        textParent = new GameObject();
        textParent.transform.parent = canvasParent.transform;
        textParent.name = "text";

        text = textParent.AddComponent<Text>();
        text.font = (Font)Resources.Load("Arial");
        text.text = textString;
        text.fontSize = 25;

        // Text position
        //RectTransform rectTransform;
        //rectTransform = text.GetComponent<RectTransform>();
        //rectTransform.localPosition = new Vector3(0, 0, 0);
        //rectTransform.sizeDelta = new Vector2(400, 200);

        return text;
    }
}

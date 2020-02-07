using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScrollerSize : MonoBehaviour
{
    private int lineCount = 0;

    private void Awake()
    {
        this.GetComponent<RectTransform>().offsetMin = new Vector2(this.GetComponent<RectTransform>().offsetMin.x, 20);
    }

    void Update()
    {
        if (lineCount != this.transform.childCount)
        {
            lineCount = this.transform.childCount;
            if (this.lineCount * 100 - 10 > this.GetComponentInParent<RectTransform>().rect.height-40)
            {
                this.GetComponent<RectTransform>().offsetMin = new Vector2(this.GetComponent<RectTransform>().offsetMin.x,
                   20 -((this.lineCount * 100 - 10) - (this.GetComponentInParent<RectTransform>().rect.height - 40)));
                //this.GetComponent<RectTransform>().sizeDelta = new Vector2(100, lineCount * 100 - 10);
                //this.GetComponent<RectTransform>().localPosition = new Vector2(this.GetComponent<RectTransform>().localPosition.x, -100 * (lineCount - 5));
            }
            else this.GetComponent<RectTransform>().offsetMin = new Vector2(this.GetComponent<RectTransform>().offsetMin.x, 20);
        }
    }

    public void DeleteChildren()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}

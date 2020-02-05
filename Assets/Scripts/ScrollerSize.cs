using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScrollerSize : MonoBehaviour
{
    private int lineCount = 0;

    private void Awake()
    {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(100,5*100 - 10);
    }

    void Update()
    {
        if (lineCount != this.transform.childCount)
        {
            lineCount = this.transform.childCount;
            if (this.lineCount > 5)
            {
                this.GetComponent<RectTransform>().sizeDelta = new Vector2(100, lineCount * 100 - 10);
                this.GetComponent<RectTransform>().localPosition = new Vector2(this.GetComponent<RectTransform>().localPosition.x, -100 * (lineCount - 5));
            }
            else this.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 5 * 100 - 10);
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

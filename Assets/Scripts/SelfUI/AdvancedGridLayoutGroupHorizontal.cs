using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedGridLayoutGroupHorizontal : GridLayoutGroup
{
    [SerializeField] protected int cellsPerLine = 1;
    [SerializeField] protected float aspectRatio = 1;

    public int cellNum = 0;
    public int lastNum = 0;

    public override void SetLayoutHorizontal()
    {
        if (cellNum != lastNum) 
        {
            float height = (this.GetComponent<RectTransform>()).sizeDelta.y;
            float width = (this.GetComponent<RectTransform>()).sizeDelta.x;
            float cellWidth = this.cellSize.x;
            int colCount;

            if (cellNum % cellsPerLine != 0) colCount = cellNum / cellsPerLine + 1;
            else colCount = cellNum / cellsPerLine;
            float newWidth = this.padding.horizontal + colCount * cellWidth + (colCount - 1) * this.spacing.x;

            if (newWidth != width) this.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, height);
            lastNum = cellNum;
        }

        base.SetLayoutHorizontal();
    }
}

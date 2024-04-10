using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedGridLayoutGroupVertical : GridLayoutGroup
{
    [SerializeField] protected int cellsPerLine = 1;
    [SerializeField] protected float aspectRatio = 1;

    public int cellNum = 0;
    public int lastNum = 0;

    public override void SetLayoutVertical()
    {
        //根據背包物件數量自動調整大小
        if (cellNum != lastNum)
        {
            float height = (this.GetComponent<RectTransform>()).sizeDelta.y;
            float width = (this.GetComponent<RectTransform>()).sizeDelta.x;
            float cellHeight = this.cellSize.y;
            int rowCount;

            if (cellNum % cellsPerLine != 0) rowCount = cellNum / cellsPerLine + 1;
            else rowCount = cellNum / cellsPerLine;

            float newHeight = this.padding.vertical + rowCount * cellHeight + (rowCount - 1) * this.spacing.y;
            //Debug.Log(newHeight);

            if (newHeight != height) this.GetComponent<RectTransform>().sizeDelta = new Vector2(width, newHeight);
            lastNum = cellNum;
        }

        base.SetLayoutVertical();
    }
}

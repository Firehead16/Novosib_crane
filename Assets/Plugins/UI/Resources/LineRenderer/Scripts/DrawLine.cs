/*
Unity UI Extensions Line Renderer demo for drawing lines

Script has several operation modes:

1: Drag and Draw
In this mode the Drag events are used to drag a line between two points, when the next drag occurs the line continues

2: Click and Draw
In this mode, each click starts a new line with the new line followng the cursor until the next click. Pressing Escape or Right-Clicking stops drawing.
The next click will continue drawing the line (to create separate lines, you will need to update to using segments rather than just points

3: Follow
In this mode, the selected object will follow the drawn line

*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Core.Ui.Extensions;

public class DrawLine : MonoBehaviour, IDragHandler, IDropHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum DemoMode { DragDraw, ClickDraw, Follow };

    public DemoMode SceneDemoMode = DemoMode.DragDraw;

    private UILineRenderer lineRenderer;
    private RectTransform rectTransform;
    private Vector2 rectPos;
    private List<Vector2> points = new List<Vector2>();
    private int currentLine = 0;

    void Start()
    {
        lineRenderer = GetComponent<UILineRenderer>();
        rectTransform = GetComponent<RectTransform>();
        rectPos = rectTransform.position;
    }

    #region Drag and Draw mode

    private Vector2 dragStartPos = Vector2.zero;

    /// <summary>
    /// EventData/MousePosition updated every frame.  Grab the first drag start point as the beginning as the first point
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (SceneDemoMode == DemoMode.DragDraw)
        {
            if (dragStartPos == Vector2.zero) // New click/drag
            {
                dragStartPos = eventData.position;
                if (points.Count < 1)
                {
                    //New line, add origin
                    points.Add(new Vector2(dragStartPos.x - rectPos.x, dragStartPos.y - rectPos.y));
                    currentLine += 1;
                }
                points.Add(new Vector2(dragStartPos.x - rectPos.x, dragStartPos.y - rectPos.y));
            }
            else
            {
                DrawLineToPoint(eventData.position);
            }
        }
    }

    /// <summary>
    /// When the user has finished clicking, add the end point and draw the line
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        if (SceneDemoMode == DemoMode.DragDraw)
        {
            points[currentLine] = new Vector2(eventData.position.x - rectPos.x, eventData.position.y - rectPos.y);

            //Use the gathered points and update the line renderer
            RefreshLine();

            dragStartPos = Vector2.zero;
            currentLine += 1;
        }
    }

    #endregion Drag and Draw mode

    #region Click and Draw mode

    private bool drawing = false;
    private bool mouseDown = false;

    void Update()
    {
        // If in Click Draw mode, update will continue to move the last line to the current mouse position until Esc or Right-Click is pressed
        if (SceneDemoMode == DemoMode.ClickDraw)
        {
            if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Escape))
            {
                drawing = false;
            }
            if (drawing)
            {
                DrawLineToPoint(Input.mousePosition);
            }
        }
    }

    /// <summary>
    /// For continuous lines, finish the last and create a new line for each click.
    /// 
    /// </summary>
    /// <remarks>
    /// I have used the Pointer Up and Pointer Down handlers here, as the generic Pointer Handler blocks the OnDragEnd handler.
    /// If you only intend to use click, you can use the IPointerClickHandler instead and drop the mouseDown properties.
    /// </remarks>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!drawing)
        {
            drawing = true;
        }

        if (!mouseDown && SceneDemoMode == DemoMode.ClickDraw && drawing)
        {
            if (points.Count < 1)
            {
                points.Add(new Vector2(eventData.position.x - rectPos.x, eventData.position.y - rectPos.y));
            }
            points.Add(new Vector2(eventData.position.x - rectPos.x, eventData.position.y - rectPos.y));
            RefreshLine();
            currentLine += 1;
        }
        mouseDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mouseDown = false;
    }

    #endregion Click and Draw mode

    #region Common Functions

    private void RefreshLine()
    {
        lineRenderer.Points = points.ToArray();
        lineRenderer.SetAllDirty();
    }

    private void DrawLineToPoint(Vector3 position)
    {
        if (points.Count > currentLine)
        {
            points[currentLine] = new Vector2(position.x - rectPos.x, position.y - rectPos.y);
            RefreshLine();
        }
    }

    #endregion  Common Functions
}
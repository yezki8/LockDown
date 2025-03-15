using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipUIHandler : MonoBehaviour
{
    //TODO: Expand these parameters to other scripts, if you want to use DI
    [Header("UI Parameter")]
    [SerializeField] private CanvasGroup _tooltipCanvasGroup;
    [SerializeField] private TextMeshProUGUI _headerField;
    [SerializeField] private TextMeshProUGUI _contentField;

    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private Transform _canvasCenter;

    [Header("Movement Parameters")]
    public bool IsAllowedToMove = true;       //be allowed to move following the cursor
    [SerializeField] private Transform _tooltipTransform;
    [SerializeField] private Transform _monitorCenter;
    private Vector2 _centerPosition;
    [SerializeField] private Camera _targetCamera;

    [Header("Bool for calculation")]
    public bool DynamicX = true;
    public bool DynamicY = true;
    public bool HasSpawned = false;

    private void Start()
    {
        _centerPosition = new Vector2(0, 0);
    }

    public void Update()
    {
        MoveTooltip();
        AdjustPanelSize();
    }

    #region Panel and Content
    public void AdjustPanelSize()
    {
        int headerLenght = _headerField.text.Length;
        int contentLenght = _contentField.text.Length;

        ///<SUMMARY>d
        ///conversion from the float unit of preferred width to the float of Text Mesh Pro Character lenght:
        ///The default measurements need 450 for Layout element's prefferedWidth and 50 for character lenght, for the panel to work
        ///Thus the calculation divide the former with 8, so the measurements can have middle grounds if there are changes to be made in the prefab's LayoutElement.preferredWidth
        ///</SUMMARY>
        float characterWrapLimit = _layoutElement.preferredWidth / 8;

        _layoutElement.enabled =
            (headerLenght > characterWrapLimit ||
            contentLenght > characterWrapLimit)
            ? true : false;
    }

    public void SpawnTooltip()
    {
        //TODO: Animate tooltip here
        _tooltipCanvasGroup.alpha = 1;
        _tooltipCanvasGroup.interactable = true;
        _tooltipCanvasGroup.blocksRaycasts = true;
        HasSpawned = true;
    }

    public void DespawnTooltip()
    {
        //TODO: Animate tooltip here
        if (HasSpawned == true)
        {
            _tooltipCanvasGroup.alpha = 0;
            _tooltipCanvasGroup.interactable = false;
            _tooltipCanvasGroup.blocksRaycasts = false;
            HasSpawned = false;
        }
    }

    public void ShowContent(string header, string content)
    {
        //Set header
        if (header != string.Empty)
        {
            _headerField.gameObject.SetActive(true);
            _headerField.SetText(header);
        }
        else
        {
            _headerField.gameObject.SetActive(false);
        }

        //Set content paragraph
        if (content != string.Empty)
        {
            _contentField.gameObject.SetActive(true);
            _contentField.SetText(content);
        }
        else
        {
            _contentField.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Positioning
    void MoveTooltip()
    {
        IsAllowedToMove = HasSpawned;
        if (HasSpawned)
        {
            DeterminePivotPosition();
        }

        if (IsAllowedToMove)
        {
            if (_tooltipTransform != null)
            {
                MoveToTarget();
            }
            else
            {
                Debug.LogError("No Tooltip Transform has been assigned");
            }
        }
    }

    void DeterminePivotPosition()
    {
        _tooltipTransform.GetComponent<RectTransform>().pivot = GetEdgeLocation();
    }

    Vector2 GetEdgeLocation()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 centerPosition = _monitorCenter.position;
        float distanceX = mousePosition.x - centerPosition.x;
        float distanceY = mousePosition.y - centerPosition.y;

        if (distanceX <= 0 && distanceY <= 0)       //bottom left
        {
            return new Vector2(0, 0);
        }
        else if (distanceX > 0 && distanceY <= 0)   //top left
        {
            return new Vector2(1, 0);
        }
        else if (distanceX > 0 && distanceY > 0)    //top right
        {
            return new Vector2(1, 1);
        }
        else                                        //bottom right
        {
            return new Vector2(0, 1);
        }
    }

    public void MoveToTarget()
    {
        //Position Llock status check
        Vector3 posToFollow = Input.mousePosition;

        Vector3 targetPos = posToFollow +      //meaning the true target position = target object location +            
                _tooltipTransform.right  +               //x value of offset in right side of the target +
                _tooltipTransform.up;                   //y value of offset in front of the target

        //Lerping the follow speed so the movement won't be stiff
        _tooltipTransform.position = Vector3.Lerp(_tooltipTransform.position,
            targetPos,
            10 * Time.deltaTime);
    }

    Vector3 StaticCheck(Vector3 position)
    {
        Transform tooltipTransform = _tooltipTransform;
        float newX = tooltipTransform.position.x;
        float newY = tooltipTransform.position.y;

        if (DynamicX)
            newX = position.x;
        if (DynamicY)
            newY = position.y;

        Vector3 newPos = new Vector3(newX, newY, 0);

        return newPos;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    //content
    [SerializeField] private TooltipUIHandler _tooltipUIHandler;
    private string _header;
    private string _contentParagraph;

    public static TooltipController Instance;

    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            HideTooltip();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #region Data for UI
    public void SetParagraph(string header, string content)
    {
        _header = header;
        _contentParagraph = content;
    }

    public void ResetContent()
    {
        _header = string.Empty;
        _contentParagraph = string.Empty;
    }
    #endregion

    #region Spawn and Despawn
    public void ShowTooltip()
    {
        _tooltipUIHandler.ShowContent(
            _header,
            _contentParagraph);
        _tooltipUIHandler.SpawnTooltip();
    }

    public void HideTooltip()
    {
        _tooltipUIHandler.DespawnTooltip();
    }
    #endregion
}


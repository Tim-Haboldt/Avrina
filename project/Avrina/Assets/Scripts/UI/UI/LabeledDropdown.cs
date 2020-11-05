using UnityEngine;
using UnityEngine.UI;

public class LabeledDropdown : MonoBehaviour
{
    /// <summary>
    ///  Dropdown element
    /// </summary>
    [SerializeField] public Dropdown dropdown;
    /// <summary>
    ///  Background image of the labeled dropdown
    /// </summary>
    [SerializeField] private Image background;
    /// <summary>
    ///  Label of the dropdown
    /// </summary>
    [SerializeField] private Text label;


    /// <summary>
    ///  Are the elements active or not
    /// </summary>
    public void SetActive(bool active)
    {
        this.dropdown.gameObject.SetActive(active);
        this.background.gameObject.SetActive(active);
        this.label.gameObject.SetActive(active);
    }
}

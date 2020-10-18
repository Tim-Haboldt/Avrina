using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    /// <summary>
    ///  Tip of the life bar. This element can't be scaled
    /// </summary>
    [SerializeField] private Image lifeBarTip = null;
    /// <summary>
    ///  Body of the life bar. This element can be scaled but not moved
    /// </summary>
    [SerializeField] private Image lifeBarBody = null;
    /// <summary>
    ///  Marks the point in the ui were the player has no life left
    /// </summary>
    [SerializeField] private float noLifePosX = 0f;
    /// <summary>
    ///  Marks the point in the ui were the player has full life left
    /// </summary>
    [SerializeField] private float fullLifePosX = 0f;


    /// <summary>
    ///  Will be called at the start of the game once.
    /// </summary>
    public void Start()
    {
        this.UpdateLifeBar(1f);
        this.gameObject.SetActive(false);
    }

    /// <summary>
    ///  Will be called every time the player life is updated
    /// </summary>
    /// <param name="lifePoints">Remaining life points of the player. Goes from 0f to 1f (1f means full life)</param>
    public void UpdateLifeBar(float lifePoints)
    {
        // Place tip
        var lifeBarSize = this.fullLifePosX - this.noLifePosX;
        var tipPos = this.lifeBarTip.rectTransform.localPosition;
        tipPos.x = this.noLifePosX + lifeBarSize * lifePoints;
        this.lifeBarTip.rectTransform.localPosition = tipPos;
        // Scale Body
        var bodyPos = this.lifeBarBody.rectTransform.localPosition;
        var bodySize = this.lifeBarBody.rectTransform.sizeDelta;
        var halfTipSize = this.lifeBarTip.rectTransform.sizeDelta.x * 0.5f;
        if (this.fullLifePosX > this.noLifePosX)
        {
            bodySize.x = tipPos.x - halfTipSize - this.noLifePosX;
            bodyPos.x = this.noLifePosX + bodySize.x * 0.5f;
        }
        else
        {
            bodySize.x = (tipPos.x + halfTipSize - this.noLifePosX) * -1;
            bodyPos.x = this.noLifePosX - bodySize.x * 0.5f;
        }
        this.lifeBarBody.rectTransform.sizeDelta = bodySize;
        this.lifeBarBody.rectTransform.localPosition = bodyPos;
    }
}

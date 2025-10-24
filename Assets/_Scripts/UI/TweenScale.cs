using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenScale : MonoBehaviour
{
    [SerializeField] bool scaleUpEnable = true;
    public float scaleUpDelay = 0f;
    [SerializeField] float scaleUpTime = 3f;
    [SerializeField] Ease scaleUpEase = Ease.OutCubic;
    [SerializeField] Vector3 scaleUpVector = Vector3.one;

    [Space]
    [SerializeField] bool scaleX = true;
    [SerializeField] bool scaleY = true;
    [SerializeField] bool scaleZ = true;

    [Space]
    [SerializeField] bool originTo = false;
    [SerializeField] bool pingPong = false;

    [Space]
    [SerializeField] bool useScaleDown = true;
    [SerializeField] float scaleDownDelay = 3f;
    [SerializeField] float scaleDownTime = 0.5f;
    [SerializeField] Ease scaleDownEase = Ease.OutCubic;
    [SerializeField] bool disableOnScaleDownComplete = false;

    private Vector3 originScale;

    private void Awake()
    {
        originScale = this.transform.localScale;
    }

    private void OnEnable()
    {
        if (scaleUpEnable)
        {
            ScaleUp();
        }
  
    }

    public void ScaleUp()
    {
        if (originTo)
            this.transform.localScale = originScale;
        else
            this.transform.localScale = new Vector3(scaleX ? 0.001f : 1f, scaleY ? 0.001f : 1f, scaleZ ? 0.001f : 1f);

        if (pingPong)
            this.transform.DOScale(scaleUpVector, scaleUpTime).SetDelay(scaleUpDelay).SetEase(scaleUpEase).OnComplete(HoldTween).SetLoops(-1, LoopType.Yoyo);
        else
            this.transform.DOScale(scaleUpVector, scaleUpTime).SetDelay(scaleUpDelay).SetEase(scaleUpEase).OnComplete(HoldTween);
    }

    void HoldTween()
    {
        if (useScaleDown)
        {
            ScaleDown();
        }
    }

    public void ScaleDown()
    {
        this.transform.DOScale(new Vector3(0.001f, 0.001f, 0.001f), scaleDownTime).SetDelay(scaleDownDelay).SetEase(scaleDownEase).OnComplete(DisableObject);
    }

    void DisableObject()
    {
        if (disableOnScaleDownComplete)
            this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        this.transform.DOKill();
    }
}

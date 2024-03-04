using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] private float timer = 0f;
    [SerializeField] private Animator _animator;
    private AnimatorClipInfo[] currentClipInfo;

    private void Start()
    {
        currentClipInfo = _animator.GetCurrentAnimatorClipInfo(0);
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= currentClipInfo[0].clip.length - 0.16f)
        {
            Destroy(this.gameObject);
        }
    }
}

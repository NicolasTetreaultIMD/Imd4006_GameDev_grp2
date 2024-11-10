using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CarController;

public class AnimationController : MonoBehaviour
{

    string currentAnimation = "";
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckAnimationFinished()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangeAnimation(string animation, int layer, float crossfade = 0.2f)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            animator.CrossFadeInFixedTime(animation, crossfade);
        }
    }

    public void ChangeWeight(int weight, int layer)
    {
        animator.SetLayerWeight(layer, weight);
    }
}

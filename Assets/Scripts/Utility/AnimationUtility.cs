using System.Collections;
using UnityEngine;

public class AnimationUtility
{
    public static IEnumerator WaitForAnimationCompletion(Transform transform)
    {
        GameObjectUtility.SetInteractability(transform, false);

        var animator = transform.GetComponent<Animator>();

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);

        GameObjectUtility.SetInteractability(transform);
    }

    public static IEnumerator ExecuteTriggerThenWait(Transform transform, string triggerName)
    {
        var animator = transform.GetComponent<Animator>();

        GameObjectUtility.SetInteractability(transform, false);

        animator.SetTrigger(triggerName);

        yield return new WaitForSeconds(0.1f);

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);

        GameObjectUtility.SetInteractability(transform);
    }
}
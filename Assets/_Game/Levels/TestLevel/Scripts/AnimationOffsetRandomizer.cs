using UnityEngine;

public class AnimationOffsetRandomizer : MonoBehaviour
{
	private void Awake()
	{
		Animator animator = GetComponent<Animator>();
		animator.Play("Walking", 0, Random.Range(0f, 1f));
	}
}

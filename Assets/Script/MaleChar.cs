using UnityEngine;

public class MaleChar : ARInteractableObject
{
    private Animator _animator;

	private void OnEnable()
	{
		_animator = GetComponent<Animator>();
	}

	protected override void SetState(State state)
	{
		base.SetState(state);
		switch(state)
		{
			case State.Idle:
				_animator.SetTrigger("GoToIdle");
				break;
			case State.Active:
				_animator.SetTrigger("StartInteraction");
				break;
		}
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

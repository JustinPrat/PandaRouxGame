using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBrain : MonoBehaviour, IHealth
{
    private BaseCharacterState currentState;
    private List<BaseCharacterState> characterStates;

    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    public bool IsUsed = true;

    public float Health { get { return health; } private set { health = value; } }
    public float MaxHealth { get { return maxHealth; } private set { maxHealth = value; } }

    public UnityEvent OnTakeDamage;
    public UnityEvent OnHeal;
    public UnityEvent OnDeath;

    public enum State
    {
        Walking,
        Rolling,
        Attacking,
        Falling,
        Jumping,
        Death
    }

    public void TakeDamage(float damage)
    {
        OnTakeDamage?.Invoke();
        Health = Mathf.Clamp(Health - damage, 0f, MaxHealth);

        if (Health <= 0f)
        {
            OnDeath.Invoke();
        }
    }

    public void Heal(float healing)
    {
        OnHeal?.Invoke();
        Health = Mathf.Clamp(Health + healing, 0, MaxHealth);
    }

    private void Update()
    {
        if (IsUsed)
        {
            currentState.UpdateState();
        }
    }

    public void ChangeState (State targetState)
    {
        BaseCharacterState nextState = FindStateRef(targetState);

        if (nextState != null && nextState.isActiveAndEnabled)
        {
            currentState.ExitState();
            currentState = nextState;
            currentState.EnterState();
        }
        else
        {
            Debug.Log("Cannot go to state : " + targetState.ToString());
        }
    }

    public void AddState (BaseCharacterState state, bool isActive = true)
    {
        characterStates.Add(state);
        state.enabled = isActive;
    }

    public void DisableState (State state)
    {
        BaseCharacterState characterStateToDisable = FindStateRef(state);
        characterStateToDisable.enabled = false;
        if (currentState.State == state)
        {
            currentState = FindNextState();
        }
    }

    private BaseCharacterState FindNextState ()
    {
        BaseCharacterState charState = currentState;

        int enumCount = Enum.GetNames(typeof(State)).Length;
        int[] enumValues = (int[])Enum.GetValues(typeof(State));

        for (int i = 0; i < enumCount; i++)
        {
            charState = FindStateRef((State)enumValues[i]);
            if (charState != null && charState.isActiveAndEnabled)
            {
                return charState;
            }
        }

        return charState;
    }

    private BaseCharacterState FindStateRef(State state)
    {
        foreach (BaseCharacterState charState in characterStates)
        {
            if (charState.State == state)
            {
                return charState;
            }
        }

        return null;
    }
}

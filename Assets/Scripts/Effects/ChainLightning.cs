using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    [SerializeField] private LightningLine _lightningLineTemplate = null;
    public int MaxTargets = 3;
    public int Damage = 10;
    public float MaxBounceRange = 5f;

    private List<Transform> _targets = new List<Transform>();
    private List<LightningLine> _lines = new List<LightningLine>();
    private Vector3 _currentPosition;
    private int _current = 0;
    private bool _finished = false;

    public void Activate(Transform firstTarget)
    {
        _targets.Add(firstTarget);
        //DealDamage(firstTarget);
        _currentPosition = firstTarget.position;
        StartCoroutine(ChainToTargets());
    }

    private void OnDestroy ()
    {
        StopAllCoroutines();
    }

    private IEnumerator ChainToTargets ()
    {
        while (_current < MaxTargets)
        {
            Transform nextTarget = UnitManager.GetClosestEnemy(_currentPosition, _targets.ToArray());
            if (nextTarget == null) { break; }
            if ((nextTarget.position - _currentPosition).magnitude > MaxBounceRange) { break; }

            _targets.Add(nextTarget);
            _current++;
            
            yield return AddToChain();
        }
        while (!_finished)
        {
            yield return new WaitForSeconds(1f);
            _finished = !AnyLinesActive();
        }
        Destroy(gameObject);
    }

    private IEnumerator AddToChain ()
    {
        LightningLine newLine = Instantiate(_lightningLineTemplate, transform);
        _lines.Add(newLine);
        newLine.Origin = _targets[_current - 1];
        newLine.LastOriginPosition = _currentPosition;
        newLine.Target = _targets[_current];
        newLine.LastTargetPosition = _targets[_current].position;

        while(!newLine.Connected)
        {
            yield return null;
        }

        DealDamage(_targets[_current]);

        _currentPosition = newLine.LastTargetPosition;
    }

    private void DealDamage (Transform target)
    {
        if (target == null) { return; }
        Damagable damagable = target.GetComponentInChildren<Damagable>();
        if (damagable != null)
        {
            damagable.Hit(Damage);
        }
    }

    private bool AnyLinesActive ()
    {
        foreach (var line in _lines)
        {
            if (line != null) { return true; }
        }
        return false;
    }
}

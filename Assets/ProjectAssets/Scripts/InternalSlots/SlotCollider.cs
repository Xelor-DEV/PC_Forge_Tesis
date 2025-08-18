using UnityEngine;
using System.Collections.Generic;

public class SlotCollider : MonoBehaviour
{
    public enum AlignmentDirection
    {
        Up,
        Down,
        Forward,
        Back,
        Right,
        Left
    }

    [Header("Base Slot Configuration")]
    [SerializeField] protected string requiredTag;
    [SerializeField] protected InternalSlot assemblySystem;
    [SerializeField] protected AlignmentDirection alignmentDirection = AlignmentDirection.Forward;
    [SerializeField] protected float alignmentThreshold = 0.9f;

    protected BoxCollider boxCollider;
    protected HashSet<Collider> validCollisions = new HashSet<Collider>();
    [SerializeField] protected int slotIndex;

    public int SlotIndex
    {
        set
        {
            slotIndex = value;
        }
    }

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredTag) && ValidateAlignment(other))
        {
            validCollisions.Add(other);
            assemblySystem.ReportCollision(slotIndex, other.transform.parent.gameObject);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (validCollisions.Contains(other))
        {
            validCollisions.Remove(other);
            assemblySystem.ReportCollisionEnd(slotIndex, other.transform.parent.gameObject);
        }
    }

    protected virtual bool ValidateAlignment(Collider other)
    {
        Vector3 slotDirection = GetAlignmentVector();
        Vector3 toComponent = (other.transform.position - transform.position).normalized;
        return Vector3.Dot(slotDirection, toComponent) >= alignmentThreshold;
    }

    private Vector3 GetAlignmentVector()
    {
        switch (alignmentDirection)
        {
            case AlignmentDirection.Up: return transform.up;
            case AlignmentDirection.Down: return -transform.up;
            case AlignmentDirection.Forward: return transform.forward;
            case AlignmentDirection.Back: return -transform.forward;
            case AlignmentDirection.Right: return transform.right;
            case AlignmentDirection.Left: return -transform.right;
            default: return transform.forward;
        }
    }
}
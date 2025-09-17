using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine.Events;

// Tests para las funciones críticas del simulador de ensamblaje
public class Testingxd
{
    // Test 1: Validar desactivación de componentes en LatchController
    [Test]
    public void LatchController_DeactivateComponents_DisablesAllComponents()
    {
        // Arrange
        GameObject latchObject = new GameObject();
        LatchController latchController = latchObject.AddComponent<LatchController>();
        Grabbable grabbable = latchObject.AddComponent<Grabbable>();
        GrabInteractable grabInteractable = latchObject.AddComponent<GrabInteractable>();
        HandGrabInteractable handGrabInteractable = latchObject.AddComponent<HandGrabInteractable>();
        OneGrabRotateTransformer oneGrabRotateTransformer = latchObject.AddComponent<OneGrabRotateTransformer>();

        latchController._grabbable = grabbable;
        latchController._grabInteractable = grabInteractable;
        latchController._handGrabInteractable = handGrabInteractable;
        latchController._oneGrabRotateTransformer = oneGrabRotateTransformer;

        // Act
        latchController.DeactivateComponents();

        // Assert
        Assert.IsFalse(grabbable.enabled, "Grabbable should be disabled");
        Assert.IsFalse(grabInteractable.enabled, "GrabInteractable should be disabled");
        Assert.IsFalse(handGrabInteractable.enabled, "HandGrabInteractable should be disabled");
        Assert.IsFalse(oneGrabRotateTransformer.enabled, "OneGrabRotateTransformer should be disabled");
    }

    // Test 2: Validar desactivación de componentes en InternalHardware
    [Test]
    public void InternalHardware_DeactivateComponents_DisablesAllComponentsAndFreezesRigidbody()
    {
        // Arrange
        GameObject hardwareObject = new GameObject();
        InternalHardware hardware = hardwareObject.AddComponent<InternalHardware>();
        Rigidbody rigidbody = hardwareObject.AddComponent<Rigidbody>();
        BoxCollider boxCollider = hardwareObject.AddComponent<BoxCollider>();
        Grabbable grabbable = hardwareObject.AddComponent<Grabbable>();
        GrabInteractable grabInteractable = hardwareObject.AddComponent<GrabInteractable>();
        HandGrabInteractable handGrabInteractable = hardwareObject.AddComponent<HandGrabInteractable>();

        hardware._compRigidbody = rigidbody;
        hardware._compBoxCollider = boxCollider;
        hardware._compGrabbable = grabbable;
        hardware._grabInteractable = grabInteractable;
        hardware._handGrabInteractable = handGrabInteractable;

        // Act
        hardware.DeactivateComponents();

        // Assert
        Assert.IsFalse(grabbable.enabled, "Grabbable should be disabled");
        Assert.IsFalse(grabInteractable.enabled, "GrabInteractable should be disabled");
        Assert.IsFalse(handGrabInteractable.enabled, "HandGrabInteractable should be disabled");
        Assert.IsFalse(boxCollider.enabled, "BoxCollider should be disabled");
        Assert.IsTrue(rigidbody.isKinematic, "Rigidbody should be kinematic");
        Assert.AreEqual(RigidbodyConstraints.FreezeAll, rigidbody.constraints, "Rigidbody should be frozen");
    }

    // Test 3: Validate position and rotation snapping in InternalHardware
    [Test]
    public void InternalHardware_SnapToCorrectPosition_SetsParentLocalPositionAndLocalRotation()
    {
        // Arrange
        GameObject hardwareObject = new GameObject();
        InternalHardware hardware = hardwareObject.AddComponent<InternalHardware>();
        GameObject motherboardObject = new GameObject();
        Transform motherboardTransform = motherboardObject.transform;

        Vector3 expectedPosition = new Vector3(1, 2, 3);
        Vector3 expectedRotation = new Vector3(90, 0, 0);
        hardware.correctPosition = expectedPosition;
        hardware.correctRotation = expectedRotation;

        // Act
        hardware.SnapToCorrectPosition(motherboardTransform);

        // Assert
        Assert.AreEqual(motherboardTransform, hardware.transform.parent, "Parent should be set to motherboard");
        Assert.AreEqual(expectedPosition, hardware.transform.localPosition, "Local position should match correctPosition");

        // Compare rotation using angle tolerance to avoid floating-point issues
        Quaternion expectedQuaternion = Quaternion.Euler(expectedRotation);
        float angle = Quaternion.Angle(expectedQuaternion, hardware.transform.localRotation);
        Assert.IsTrue(angle < 0.1f, $"Local rotation should match correctRotation. Angle difference: {angle}");
    }

    // Test 4: Validar cierre de latch y desactivación de componentes (corrutina)
    [UnityTest]
    public IEnumerator LatchController_CloseLatch_LocksLatchAndDeactivatesComponents()
    {
        // Arrange
        GameObject latchObject = new GameObject();
        LatchController latchController = latchObject.AddComponent<LatchController>();
        Grabbable grabbable = latchObject.AddComponent<Grabbable>();
        GrabInteractable grabInteractable = latchObject.AddComponent<GrabInteractable>();
        HandGrabInteractable handGrabInteractable = latchObject.AddComponent<HandGrabInteractable>();
        OneGrabRotateTransformer oneGrabRotateTransformer = latchObject.AddComponent<OneGrabRotateTransformer>();

        latchController._grabbable = grabbable;
        latchController._grabInteractable = grabInteractable;
        latchController._handGrabInteractable = handGrabInteractable;
        latchController._oneGrabRotateTransformer = oneGrabRotateTransformer;

        // Act
        latchController.CloseLatch();
        yield return new WaitForSeconds(latchController.closeDuration + 0.1f); // Esperar a que la corrutina termine

        // Assert
        Assert.IsTrue(latchController.IsLocked, "Latch should be locked");
        Assert.IsFalse(grabbable.enabled, "Grabbable should be disabled");
        Assert.IsFalse(grabInteractable.enabled, "GrabInteractable should be disabled");
        Assert.IsFalse(handGrabInteractable.enabled, "HandGrabInteractable should be disabled");
        Assert.IsFalse(oneGrabRotateTransformer.enabled, "OneGrabRotateTransformer should be disabled");
    }

    // Test 5: Validar alineación correcta en SlotCollider para múltiples direcciones
    [Test]
    public void SlotCollider_ValidateAlignment_ReturnsTrueForCorrectDirections()
    {
        // Arrange
        GameObject colliderObject = new GameObject();
        TestSlotCollider slotCollider = colliderObject.AddComponent<TestSlotCollider>();
        slotCollider.alignmentThreshold = 0.9f;

        GameObject otherObject = new GameObject();
        BoxCollider otherCollider = otherObject.AddComponent<BoxCollider>();

        // Prueba para dirección Forward
        slotCollider.alignmentDirection = SlotCollider.AlignmentDirection.Forward;
        otherObject.transform.position = colliderObject.transform.position + colliderObject.transform.forward * 1.0f;
        bool forwardAligned = slotCollider.PublicValidateAlignment(otherCollider);
        Assert.IsTrue(forwardAligned, "Should be aligned for Forward direction");

        // Prueba para dirección Up
        slotCollider.alignmentDirection = SlotCollider.AlignmentDirection.Up;
        otherObject.transform.position = colliderObject.transform.position + colliderObject.transform.up * 1.0f;
        bool upAligned = slotCollider.PublicValidateAlignment(otherCollider);
        Assert.IsTrue(upAligned, "Should be aligned for Up direction");

        // Prueba para dirección Right
        slotCollider.alignmentDirection = SlotCollider.AlignmentDirection.Right;
        otherObject.transform.position = colliderObject.transform.position + colliderObject.transform.right * 1.0f;
        bool rightAligned = slotCollider.PublicValidateAlignment(otherCollider);
        Assert.IsTrue(rightAligned, "Should be aligned for Right direction");
    }
}

// Clase helper para exponer el método protegido ValidateAlignment de SlotCollider
public class TestSlotCollider : SlotCollider
{
    public bool PublicValidateAlignment(Collider other)
    {
        return ValidateAlignment(other);
    }
}
using LayeredBlendMasks.Runtime;
using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script showing the basic usage of Layered Blend Mask, of course you can do more than this.
/// </summary>
public class SampleWeaponController : MonoBehaviour
{

    public enum WeaponState
    {
        None,
        Draw,
        Idle,
        Aim
    }

    private WeaponState m_WeaponState;
    private WeaponState m_LastWeaponState;
    public WeaponState State
    {
        get => m_WeaponState;
        private set
        {
            if (m_WeaponState != value)
            {
                WeaponStateChange(m_WeaponState, value);
            }
            m_WeaponState = value;
        }
    }

    private StarterAssetsInputs inputs;

    [SerializeField] private LayeredBlendMasksComponent LayeredBlendMask;
    [SerializeField] private GameObject Weapon;

    [System.Serializable]
    public class BlendMaskClips
    {
        public AnimationClip Clip;
        public BlendProfile Profile;
    }

    [SerializeField] private BlendMaskClips Draw, Idle, Aim;
    private BlendMaskLayer m_DrawLayer, m_IdleLayer, m_AimLayer;

    void Start()
    {
        inputs = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        if (inputs == null)
        {
            return;
        }

        if (m_LastWeaponState != m_WeaponState)
        {
            m_LastWeaponState = m_WeaponState;
            SyncWeights();
        }

        if (inputs.weapon)
        {
            inputs.weapon = false;
            if (State == WeaponState.None)
            {
                State = WeaponState.Draw;
                Weapon.SetActive(true);
            }
            else
            {
                State = WeaponState.None;
                Weapon.SetActive(false);
            }
        }

        if (inputs.aim == true && State == WeaponState.Idle)
        {
            State = WeaponState.Aim;
        }
        else if (inputs.aim == false && State == WeaponState.Aim)
        {
            State = WeaponState.Idle;
        }

        if (m_DrawLayer != null && m_DrawLayer.NormalizeTime == 1)
        {
            State = WeaponState.Idle;
        }
    }

    private void RemoveAllLayers()
    {
        if (LayeredBlendMask.HasLayer("Draw"))
        {
            LayeredBlendMask.RemoveLayer("Draw");
            m_DrawLayer = null;
        }
        if (LayeredBlendMask.HasLayer("Idle"))
        {
            LayeredBlendMask.RemoveLayer("Idle");
            m_IdleLayer = null;
        }
        if (LayeredBlendMask.HasLayer("Aim"))
        {
            LayeredBlendMask.RemoveLayer("Aim");
            m_AimLayer = null;
        }
    }

    private void WeaponStateChange(WeaponState from, WeaponState to)
    {
        if (to == WeaponState.None)
        {
            RemoveAllLayers();
        }
        else if (to == WeaponState.Draw)
        {
            if (LayeredBlendMask.HasLayer("Draw") == false)
            {
                LayeredBlendMask.AddLayer("Draw", Draw.Clip, Draw.Profile);
                LayeredBlendMask.GetLayer("Draw", out m_DrawLayer);
            }
        }
        else if (from == WeaponState.Draw && to == WeaponState.Idle)
        {
            LayeredBlendMask.RemoveLayer("Draw");
            LayeredBlendMask.AddLayer("Idle", Idle.Clip, Idle.Profile);
            LayeredBlendMask.GetLayer("Idle", out m_IdleLayer);
        }
        else if (from == WeaponState.Idle && to == WeaponState.Aim)
        {
            LayeredBlendMask.RemoveLayer("Idle");
            LayeredBlendMask.AddLayer("Aim", Aim.Clip, Aim.Profile);
            LayeredBlendMask.GetLayer("Aim", out m_AimLayer);
        }
        else if (from == WeaponState.Aim && to == WeaponState.Idle)
        {
            LayeredBlendMask.RemoveLayer("Aim");
            LayeredBlendMask.AddLayer("Idle", Idle.Clip, Idle.Profile);
            LayeredBlendMask.GetLayer("Idle", out m_IdleLayer);
        }
    }

    #region Runtime API
    private float m_HipWeight;
    public float HipWeight
    {
        get => m_HipWeight;
        set
        {
            m_HipWeight = value;
            UpdateRuntimeWeights();
        }
    }
    public UnityEvent<float> OnHipWeightChanged;

    private float m_SpineWeight;
    public float SpineWeight
    {
        get => m_SpineWeight;
        set
        {
            m_SpineWeight = value;
            UpdateRuntimeWeights();
        }
    }
    public UnityEvent<float> OnSpineWeightChanged;

    private float m_ChestWeight;
    public float ChestWeight
    {
        get => m_ChestWeight;
        set
        {
            m_ChestWeight = value;
            UpdateRuntimeWeights();
        }
    }
    public UnityEvent<float> OnChestWeightChanged;

    private float m_UpperChestWeight;
    public float UpperChestWeight
    {
        get => m_UpperChestWeight;
        set
        {
            m_UpperChestWeight = value;
            UpdateRuntimeWeights();
        }
    }
    public UnityEvent<float> OnUpperChestWeightChanged;

    private float m_ShoulderWeight;
    public float ShoulderWeight
    {
        get => m_ShoulderWeight;
        set
        {
            m_ShoulderWeight = value;
            UpdateRuntimeWeights();
        }
    }
    public UnityEvent<float> OnShoulderWeightChanged;

    private float m_UpperArmWeight;
    public float UpperArmWeight
    {
        get => m_UpperArmWeight;
        set
        {
            m_UpperArmWeight = value;
            UpdateRuntimeWeights();
        }
    }
    public UnityEvent<float> OnUpperArmWeightChanged;

    private float m_LowerArmWeight;
    public float LowerArmWeight
    {
        get => m_LowerArmWeight;
        set
        {
            m_LowerArmWeight = value;
            UpdateRuntimeWeights();
        }
    }
    public UnityEvent<float> OnLowerArmWeightChanged;

    private float m_HandWeight;
    public float HandWeight
    {
        get => m_HandWeight;
        set
        {
            m_HandWeight = value;
            UpdateRuntimeWeights();
        }
    }
    public UnityEvent<float> OnHandWeightChanged;

    private void UpdateRuntimeWeights()
    {
        LayeredBlendMask.SetWeight("Skeleton/Hips", 1, HipWeight);
        LayeredBlendMask.SetWeight("Skeleton/Hips/Spine", 1, SpineWeight);
        LayeredBlendMask.SetWeight("Skeleton/Hips/Spine/Chest", 1, ChestWeight);
        LayeredBlendMask.SetWeight("Skeleton/Hips/Spine/Chest/UpperChest", 1, UpperChestWeight);
        LayeredBlendMask.SetWeight("Skeleton/Hips/Spine/Chest/UpperChest/Left_Shoulder", 1, ShoulderWeight);
        LayeredBlendMask.SetWeight("Skeleton/Hips/Spine/Chest/UpperChest/Left_Shoulder/Left_UpperArm", 1, UpperArmWeight);
        LayeredBlendMask.SetWeight("Skeleton/Hips/Spine/Chest/UpperChest/Left_Shoulder/Left_UpperArm/Left_LowerArm", 1, LowerArmWeight);
        LayeredBlendMask.SetWeight("Skeleton/Hips/Spine/Chest/UpperChest/Left_Shoulder/Left_UpperArm/Left_LowerArm/Left_Hand", 1, HandWeight);
        LayeredBlendMask.SetWeight("Skeleton/Hips/Spine/Chest/UpperChest/Right_Shoulder", 1, ShoulderWeight);
        LayeredBlendMask.SetWeight("Skeleton/Hips/Spine/Chest/UpperChest/Right_Shoulder/Right_UpperArm", 1, UpperArmWeight);
        LayeredBlendMask.SetWeight("Skeleton/Hips/Spine/Chest/UpperChest/Right_Shoulder/Right_UpperArm/Right_LowerArm", 1, LowerArmWeight);
        LayeredBlendMask.SetWeight("Skeleton/Hips/Spine/Chest/UpperChest/Right_Shoulder/Right_UpperArm/Right_LowerArm/Right_Hand", 1, HandWeight);
    }

    private void SyncWeights()
    {
        if(State == WeaponState.None)
        {
            m_HipWeight = 0;
            m_SpineWeight = 0;
            m_ChestWeight = 0;
            m_UpperChestWeight = 0;
            m_ShoulderWeight = 0;
            m_UpperArmWeight = 0;
            m_LowerArmWeight = 0;
            m_HandWeight = 0;
            OnHipWeightChanged?.Invoke(0);
            OnSpineWeightChanged?.Invoke(0);
            OnChestWeightChanged?.Invoke(0);
            OnUpperChestWeightChanged?.Invoke(0);
            OnShoulderWeightChanged?.Invoke(0);
            OnUpperArmWeightChanged?.Invoke(0);
            OnLowerArmWeightChanged?.Invoke(0);
            OnHandWeightChanged?.Invoke(0);
            return;
        }

        m_HipWeight = LayeredBlendMask.GetWeight("Skeleton/Hips", 1);
        m_SpineWeight = LayeredBlendMask.GetWeight("Skeleton/Hips/Spine", 1);
        m_ChestWeight = LayeredBlendMask.GetWeight("Skeleton/Hips/Spine/Chest", 1);
        m_UpperChestWeight = LayeredBlendMask.GetWeight("Skeleton/Hips/Spine/Chest/UpperChest", 1);
        m_ShoulderWeight = LayeredBlendMask.GetWeight("Skeleton/Hips/Spine/Chest/UpperChest/Left_Shoulder", 1);
        m_UpperArmWeight = LayeredBlendMask.GetWeight("Skeleton/Hips/Spine/Chest/UpperChest/Left_Shoulder/Left_UpperArm", 1);
        m_LowerArmWeight = LayeredBlendMask.GetWeight("Skeleton/Hips/Spine/Chest/UpperChest/Left_Shoulder/Left_UpperArm/Left_LowerArm", 1);
        m_HandWeight = LayeredBlendMask.GetWeight("Skeleton/Hips/Spine/Chest/UpperChest/Left_Shoulder/Left_UpperArm/Left_LowerArm/Left_Hand", 1);

        OnHipWeightChanged?.Invoke(m_HipWeight);
        OnSpineWeightChanged?.Invoke(m_SpineWeight);
        OnChestWeightChanged?.Invoke(m_ChestWeight);
        OnUpperChestWeightChanged?.Invoke(m_UpperChestWeight);
        OnShoulderWeightChanged?.Invoke(m_ShoulderWeight);
        OnUpperArmWeightChanged?.Invoke(m_UpperArmWeight);
        OnLowerArmWeightChanged?.Invoke(m_LowerArmWeight);
        OnHandWeightChanged?.Invoke(m_HandWeight);
    }

    #endregion
}

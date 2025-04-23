using LayeredBlendMasks.Runtime;
using StarterAssets;
using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public enum WeaponState
    {
        None,
        Draw,
        Idle,
        Aim
    }

    private WeaponState m_WeaponState;
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
}

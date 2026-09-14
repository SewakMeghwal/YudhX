using System;
using UnityEngine;
using BattleRoyale.Weapons;

namespace BattleRoyale.InventorySystem
{
    public class PlayerWeaponInventory : MonoBehaviour, IWeaponInventory
    {
        [Header("Weapon Mount Socket Transforms")]
        [SerializeField] private Transform rightHandSocket;
        [SerializeField] private Transform primaryBackSocket;
        [SerializeField] private Transform secondaryBackSocket;
        [SerializeField] private Transform holsterSidearmSocket;

        private readonly Weapon[] slots = new Weapon[4]; // 0: Primary, 1: Secondary, 2: Sidearm, 3: Melee
        private WeaponSlotIndex activeSlot = WeaponSlotIndex.Holstered;

        public Weapon ActiveWeapon => activeSlot != WeaponSlotIndex.Holstered ? slots[(int)activeSlot] : null;
        public WeaponSlotIndex ActiveSlot => activeSlot;

        public event Action<WeaponSlotIndex, Weapon> OnWeaponEquipped;
        public event Action<WeaponSlotIndex, Weapon> OnWeaponSwitched;
        public event Action OnWeaponHolstered;

        private void Update()
        {
            HandleSlotHotkeyInputs();
        }

        private void HandleSlotHotkeyInputs()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchToSlot(WeaponSlotIndex.Primary);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchToSlot(WeaponSlotIndex.Secondary);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchToSlot(WeaponSlotIndex.Sidearm);
            else if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchToSlot(WeaponSlotIndex.Melee);
            else if (Input.GetKeyDown(KeyCode.X)) HolsterWeapon();
        }

        public bool EquipWeapon(Weapon weapon, WeaponSlotIndex slot)
        {
            if (weapon == null || slot == WeaponSlotIndex.Holstered) return false;

            int index = (int)slot;
            
            // Unequip current weapon in that slot if exists
            if (slots[index] != null)
            {
                UnequipWeapon(slot, out _);
            }

            slots[index] = weapon;
            weapon.transform.SetParent(GetSocketForSlot(slot));
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;

            OnWeaponEquipped?.Invoke(slot, weapon);

            // Auto-switch to newly equipped weapon if holstered
            if (activeSlot == WeaponSlotIndex.Holstered)
            {
                SwitchToSlot(slot);
            }

            return true;
        }

        public bool UnequipWeapon(WeaponSlotIndex slot, out Weapon unequippedWeapon)
        {
            unequippedWeapon = null;
            if (slot == WeaponSlotIndex.Holstered) return false;

            int index = (int)slot;
            if (slots[index] == null) return false;

            unequippedWeapon = slots[index];
            slots[index] = null;

            if (activeSlot == slot)
            {
                HolsterWeapon();
            }

            return true;
        }

        public void SwitchToSlot(WeaponSlotIndex slot)
        {
            if (slot == WeaponSlotIndex.Holstered)
            {
                HolsterWeapon();
                return;
            }

            int index = (int)slot;
            if (slots[index] == null) return; // Empty slot, cannot switch

            if (activeSlot == slot) return; // Already active

            // Attach current active weapon to back/holster socket
            if (activeSlot != WeaponSlotIndex.Holstered && slots[(int)activeSlot] != null)
            {
                slots[(int)activeSlot].transform.SetParent(GetSocketForSlot(activeSlot));
                slots[(int)activeSlot].transform.localPosition = Vector3.zero;
                slots[(int)activeSlot].transform.localRotation = Quaternion.identity;
                slots[(int)activeSlot].gameObject.SetActive(false);
            }

            // Bring new weapon to hand socket
            activeSlot = slot;
            Weapon newWeapon = slots[index];
            newWeapon.gameObject.SetActive(true);
            if (rightHandSocket != null)
            {
                newWeapon.transform.SetParent(rightHandSocket);
                newWeapon.transform.localPosition = Vector3.zero;
                newWeapon.transform.localRotation = Quaternion.identity;
            }

            OnWeaponSwitched?.Invoke(activeSlot, newWeapon);
        }

        public void HolsterWeapon()
        {
            if (activeSlot != WeaponSlotIndex.Holstered && slots[(int)activeSlot] != null)
            {
                slots[(int)activeSlot].transform.SetParent(GetSocketForSlot(activeSlot));
                slots[(int)activeSlot].transform.localPosition = Vector3.zero;
                slots[(int)activeSlot].transform.localRotation = Quaternion.identity;
                slots[(int)activeSlot].gameObject.SetActive(false);
            }

            activeSlot = WeaponSlotIndex.Holstered;
            OnWeaponHolstered?.Invoke();
        }

        private Transform GetSocketForSlot(WeaponSlotIndex slot)
        {
            switch (slot)
            {
                case WeaponSlotIndex.Primary: return primaryBackSocket != null ? primaryBackSocket : transform;
                case WeaponSlotIndex.Secondary: return secondaryBackSocket != null ? secondaryBackSocket : transform;
                case WeaponSlotIndex.Sidearm: return holsterSidearmSocket != null ? holsterSidearmSocket : transform;
                default: return transform;
            }
        }
    }
}

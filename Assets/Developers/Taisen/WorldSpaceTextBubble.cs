using UnityEngine;
using UnityEngine.UI;


namespace PSoft
{
    public class WorldSpaceTextBubble : MonoBehaviour
    {
        [SerializeField] private RawImage weaponImageSlot; // Image slot used to display a weapon type.

        //* Weapon Type Images *//
        [SerializeField] private Texture swordImage;
        [SerializeField] private Texture maceImage;
        [SerializeField] private Texture hammerImage;
        [SerializeField] private Texture axeImage;
        [SerializeField] private Texture spearImage;
        [SerializeField] private Texture daggerImage;

        // Sets this WorldSpaceTextBubble's weaponImageSlot based on the given type.
        public void SetWeaponRequestImage(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Sword:
                    weaponImageSlot.texture = swordImage;
                    break;
                case WeaponType.Mace:
                    weaponImageSlot.texture = maceImage;
                    break;
                case WeaponType.Hammer:
                    weaponImageSlot.texture = hammerImage;
                    break;
                case WeaponType.Axe:
                    weaponImageSlot.texture = axeImage;
                    break;
                case WeaponType.Spear:
                    weaponImageSlot.texture = spearImage;
                    break;
                case WeaponType.Dagger:
                    weaponImageSlot.texture = daggerImage;
                    break;
                default:
                    Debug.Log("WorldSpaceTextBubble::SetWeaponRequestImage() invalid weaponType");
                    break;
            }
        }
    }
}

using JetBrains.Annotations;
using UnityEngine;

namespace PSoft
{
    [CreateAssetMenu(fileName = "NewWeaponPart", menuName = "Weapon/Part")]
    public class WeaponPart : ScriptableObject
    {
        public WeaponPartType partType;
        public GameObject partPrefab;
        
        // ToDo/Note; Moving these two assembler FS.
        // These attach points only need to be set on the handle, as everything attaches to it.
        [CanBeNull] public Transform pommelAttachPoint;
        [CanBeNull] public Transform hiltAttachPoint;
        [CanBeNull] public Transform handleAttachPoint; // ToDo/Question: We don't need this if the handle is the parent. If handle parent we can just spawn Vector3.zero Quaternion.identity.
        [CanBeNull] public Transform bladeAttachPoint;
        
        // ToDo: Relevant stats (durability, score, etc.).

        // Question: Should I put skins here? Or the assembler? Probably assembler with trim sheet but iunno.
    }
}

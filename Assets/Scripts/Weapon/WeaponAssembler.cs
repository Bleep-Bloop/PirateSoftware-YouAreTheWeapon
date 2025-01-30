using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PSoft
{
    // Getting error so moving this to its own script just out of curiousity.
    // [CreateAssetMenu(fileName = "NewWeaponPart", menuName = "Weapon/Part")]
    // public class WeaponPart : ScriptableObject
    // {
    //     public WeaponPartType partType;
    //     public GameObject partPrefab;
    //     
    //     // ToDo/Note; Moving these two assembler FS.
    //     // These attach points only need to be set on the handle, as everything attaches to it.
    //     [CanBeNull] public Transform pommelAttachPoint;
    //     [CanBeNull] public Transform hiltAttachPoint;
    //     [CanBeNull] public Transform handleAttachPoint; // ToDo/Question: We don't need this if the handle is the parent. If handle parent we can just spawn Vector3.zero Quaternion.identity.
    //     [CanBeNull] public Transform bladeAttachPoint;
    //     
    //     // ToDo: Relevant stats (durability, score, etc.).
    //
    //     // Question: Should I put skins here? Or the assembler? Probably assembler with trim sheet but iunno.
    // }
    
    // ToDo: I guess this will set on the movable thing right? That makes sense to me, but my brain broken and half thinking of unreal and how I would have this outside. Just watch what it gets parented too!
    
    // Handles attaching weapon parts together and switching skins and all that stuff.
    public class WeaponAssembler : MonoBehaviour
    {
        private WeaponPart _currentHandle; // ToDo/Question: Should I make these GameObjects or Weaponparts? I kinda feel like WeaponPart right?
        private WeaponPart _currentBlade;
        private WeaponPart _currentPommel;
        private WeaponPart _currentHilt;
        

        private GameObject _currentHandleGameObject; // Iunno if I am doing this weirdly iunno im dumb.
        private GameObject _currentBladeGameObject; // Iunno if I am doing this weirdly iunno im dumb.

        [SerializeField] private WeaponPart[] handleParts;
        [SerializeField] private WeaponPart[] bladeParts;
        [SerializeField] private WeaponPart[] pommelParts;
        [SerializeField] private WeaponPart[] hiltParts;
        
        // Get from handle which has them!
        [CanBeNull] public Transform pommelAttachPoint;
        [CanBeNull] public Transform hiltAttachPoint;
        [CanBeNull] public Transform handleAttachPoint; 
        [CanBeNull] public Transform bladeAttachPoint;

        private void Awake()
        {
            // OK I need to get the transform points frfrom the scriptable object because it needs the prefab.
            
        }

        public void SetHandle(WeaponPart handlePart)
        {
            // Ensure we do not have multiples of a part.
            if(_currentHandle) Destroy(_currentHandle);
            
            // // The handle is spawned first, so we don't need to attach it to anything. ToDO/Question: Does this need a parent or is the handle the parent? Need to see controller I think.
            // _currentHandle = Instantiate(handlePart, Vector3.zero, Quaternion.identity);
            // _currentHandleGameObject = _currentHandle;
            // // After creating the handle we need to get all the attach points set on it right.
            // _currentHandle.handleAttachPoint = 

            
            _currentHandleGameObject = Instantiate(handlePart.partPrefab, Vector3.zero, Quaternion.identity); // ToDo/Note: Ok definitly looks like I do go GameObject and then I can get the component off of it I guess aha. Or not even save it I can just get when necessary right? Because I set and am good!
            // ToDo: Maybe instead I just save the attach points in the assembler dumbo aha.
            // Get the attach points for all the next parts from our new handle.
            pommelAttachPoint = _currentHandleGameObject.transform.Find("PommelAttachPoint");
            bladeAttachPoint = _currentHandleGameObject.transform.Find("BladeAttachPoint");
            handleAttachPoint = _currentHandleGameObject.transform.Find("HandleAttachPoint");
            hiltAttachPoint = _currentHandleGameObject.transform.Find("HiltAttachPoint");

        }

        public void SetBlade(WeaponPart bladePart)
        {
            // Ensure we do not have multiples of a part.
            if(_currentBlade) Destroy(_currentBlade);
            
            // ToDo/Question: I think I can just use two parameters. Then it will auto child and handle position and rotation right.
            // _currentBlade = Instantiate(bladePart, bladeAttachPoint); // ToDo/Question: If I don't set the parent, does it auto handle it? I feel like it would but I need to double check right.
            _currentBladeGameObject = Instantiate(bladePart.partPrefab, bladeAttachPoint);
            Debug.Log("OK blade spawned!");
        }

        public void SetPommel(WeaponPart pommelPart)
        {
            // Ensure we do not have multiples of a part.
            if(_currentPommel) Destroy(_currentPommel);
            // ToDo/Question: Hmmm so problem now is the attach point, I kinda need two right? First check if we even have pommels LOL.
            _currentPommel = Instantiate(pommelPart, pommelAttachPoint);
        }

        public void SetHilt(WeaponPart hiltPart)
        {
            // Ensure we do not have multiples of a part.
            if (_currentHilt) Destroy(_currentHilt);

            _currentHilt = Instantiate(hiltPart,hiltAttachPoint);
        }
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // ToDo/Note: Just testing the assembly here. GameManager will normally do and Start() should be empty haha.
            SetHandle(handleParts[Random.Range(0, handleParts.Length)]);
            SetBlade(bladeParts[Random.Range(0, bladeParts.Length)]);
            // SetPommel(pommelParts[Random.Range(0, pommelParts.Length)]);
            // SetHilt(hiltParts[Random.Range(0, hiltParts.Length)]);
            Debug.Log("Ok you know it worked but double check.");
        }

        // Update is called once per frame
        void Update()
        { 

        }
    }
}

























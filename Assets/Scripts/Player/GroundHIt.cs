using System;
using PSoft.Player;
using UnityEngine;

namespace PSoft
{
    public class GroundHIt : MonoBehaviour
    {
        
        private PlayerController _player;

        private void Awake()
        {
           _player = gameObject.GetComponentInParent<PlayerController>();
           Debug.Log(_player);
        }

        private void OnCollisionStay(Collision other)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.CompareTag("Ground"))
            {
                Debug.Log(other.gameObject.name);
                _player.CheckGround();
            }
        }
    }
}

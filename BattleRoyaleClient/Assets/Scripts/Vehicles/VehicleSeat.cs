using UnityEngine;

namespace BattleRoyale.Vehicles
{
    public class VehicleSeat : MonoBehaviour
    {
        [SerializeField] private SeatType seatType = SeatType.Driver;
        [SerializeField] private Transform exitPoint;
        [SerializeField] private GameObject occupantPlayer;

        public SeatType SeatType => seatType;
        public Transform ExitPoint => exitPoint != null ? exitPoint : transform;
        public GameObject OccupantPlayer => occupantPlayer;
        public bool IsOccupied => occupantPlayer != null;

        public void AssignOccupant(GameObject player)
        {
            occupantPlayer = player;
            if (player != null)
            {
                player.transform.SetParent(transform);
                player.transform.localPosition = Vector3.zero;
                player.transform.localRotation = Quaternion.identity;
            }
        }

        public void ClearOccupant()
        {
            if (occupantPlayer != null)
            {
                occupantPlayer.transform.SetParent(null);
                occupantPlayer = null;
            }
        }
    }
}

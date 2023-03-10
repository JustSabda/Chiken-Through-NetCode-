using Unity.Netcode;
using UnityEngine;

/// <summary>
/// An example network serializer with both server and owner authority.
/// Love Tarodev
/// </summary>
public class PlayerTransform : NetworkBehaviour {
    /// <summary>
    /// A toggle to test the difference between owner and server auth.
    /// </summary>
    [SerializeField] private bool _serverAuth;
    [SerializeField] private float _cheapInterpolationTime = 0.1f;

    private NetworkVariable<PlayerNetworkState> _playerState;
    private Rigidbody _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();

        var permission = _serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        _playerState = new NetworkVariable<PlayerNetworkState>(writePerm: permission);
    }

    public override void OnNetworkSpawn() {
        if (!IsOwner) Destroy(transform.GetComponent<PlayerController>());
    }

    private void Update() {
        if (IsOwner) TransmitState();
        else ConsumeState();
    }

    #region Transmit State

    private void TransmitState() {
        var state = new PlayerNetworkState {
            Position = _rb.position,
            Rotation = transform.rotation.eulerAngles
        };

        if (IsServer || !_serverAuth)
            _playerState.Value = state;
        else
            TransmitStateServerRpc(state);
    }

    [ServerRpc]
    private void TransmitStateServerRpc(PlayerNetworkState state) {
        _playerState.Value = state;
    }

    #endregion

    #region Interpolate State

    private Vector3 _posVel;
    private float _rotVelX;
    private float _rotVelY;
    private float _rotVelZ;

    private void ConsumeState() {
        // Here you'll find the cheapest, dirtiest interpolation you'll ever come across. Please do better in your game
        _rb.MovePosition(Vector3.SmoothDamp(_rb.position, _playerState.Value.Position, ref _posVel, _cheapInterpolationTime));

        transform.rotation = Quaternion.Euler(
            Mathf.SmoothDampAngle(transform.rotation.eulerAngles.x, _playerState.Value.Rotation.x, ref _rotVelX, _cheapInterpolationTime)
            , Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, _playerState.Value.Rotation.y, ref _rotVelY, _cheapInterpolationTime),
            Mathf.SmoothDampAngle(transform.rotation.eulerAngles.z, _playerState.Value.Rotation.z, ref _rotVelZ, _cheapInterpolationTime));
    }

    #endregion

    private struct PlayerNetworkState : INetworkSerializable {
        private float _posX, _posY , _posZ;
        private short _rotX, _rotY, _rotZ;

        internal Vector3 Position {
            get => new Vector3(_posX, _posY, _posZ);
            set
            {
                _posX = value.x;
                _posY = value.y;
                _posZ = value.z;
            }
        }

        internal Vector3 Rotation {
            get => new Vector3(_rotX, _rotY, _rotZ);
            set
            {
                _rotX = (short)value.x;
                _rotY = (short)value.y;
                _rotZ = (short)value.z;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref _posX);
            serializer.SerializeValue(ref _posY);
            serializer.SerializeValue(ref _posZ);

            serializer.SerializeValue(ref _rotX);
            serializer.SerializeValue(ref _rotY);
            serializer.SerializeValue(ref _rotZ);
        }
    }
}
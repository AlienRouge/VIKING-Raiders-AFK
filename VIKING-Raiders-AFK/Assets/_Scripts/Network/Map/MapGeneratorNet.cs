using System;
using BinaryFormatter;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace _Scripts.Network.Map
{
    public class MapGeneratorNet : MapGenerator, IOnEventCallback
    {

        private void Awake()
        {
            // _instance = this;
            _noiseMapRenderer = GetComponent<NoiseMapRenderer>();
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case (byte)NetEvents.StartBattle:
                    var data = (float[]) photonEvent.CustomData;

                    SetMapData(data);
                    BattleSceneControllerNet.Instance.SetMapController(_mapController);
                    BattleSceneControllerNet.Instance.ShowUi();
                    break;
                default:
                    break;
            }
        }

        private void SetMapData(float[] data)
        {
            CreateTileMap();
            RenderMap(data);
            _mapController.BakeMap();
            SetupSpawnAreas();
        }

        private void CreateTileMap()
        {
            TryInstantiateMap();
            _noiseMapRenderer.ClearTilemaps();
            GenerateSpawnAreas();
        }

        public override MapController GenerateMap()
        {
            GenerateTilemap(Seed);
            _mapController.BakeMap();
            SetupSpawnAreas();

            return _mapController;
        }

        private void RenderMap(float[] noiseMap)
        {
            _noiseMapRenderer.RenderMap(BattleAreaWidth, height, noiseMap, spawnAreaWidth);
        }

        protected override void GenerateBattleArea()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var noiseMap = GenerateBattleAreaNoiseMap();
                RenderMap(noiseMap);

                var riseEventOptions = new RaiseEventOptions()
                {
                    Receivers = ReceiverGroup.Others
                };
                var sendOptions = new SendOptions()
                {
                    Reliability = true
                };

                PhotonNetwork.RaiseEvent((byte)NetEvents.StartBattle, noiseMap, riseEventOptions, sendOptions);
            }
        }

        protected override void TryInstantiateMap()
        {
            _mapController = FindObjectOfType<MapController>();
            if (_mapController == null)
            {
                _mapController = Instantiate(_mapPrefab);
                _mapController.transform.localPosition = new Vector3(
                    -width/2f,
                    -height/2f,
                    1);
            }
        
            _noiseMapRenderer.Init(_mapController.walkableTilemap, _mapController.notWalkableTilemap,
                _mapController.decorTilemap);
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
    }
}
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
            _instance = this;
            _noiseMapRenderer = GetComponent<NoiseMapRenderer>();
        }

        public void OnEvent(EventData photonEvent)
        {
            Debug.Log(23);
            switch (photonEvent.Code)
            {
                case 23:
                    Debug.Log(photonEvent.CustomData);
                    var data = (float[]) photonEvent.CustomData;

                    SetMapData(data);
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
            GenerateTilemap();
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
                var noiseMap = GetNoiseMap();
                RenderMap(noiseMap);

                var riseEventOptions = new RaiseEventOptions()
                {
                    Receivers = ReceiverGroup.Others
                };
                var sendOptions = new SendOptions()
                {
                    Reliability = true
                };

                PhotonNetwork.RaiseEvent(23, noiseMap, riseEventOptions, sendOptions);
            }
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
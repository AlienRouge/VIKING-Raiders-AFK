using _Scripts.Enums;
using BinaryFormatter;
using UnityEngine;

namespace _Scripts.Network.SyncData
{
    public static class Converter
    {
        public static object Deserialize(byte[] bytes)
        {
            return new BinaryConverter().Deserialize<object>(bytes);
        }

        public static byte[] Serialize(object obj)
        {
            return new BinaryConverter().Serialize(obj);
        }
    }

    public struct SyncData
    {
        public string modalName;
        public int heroLevel;
        public Team currentTeam;
    }


    public struct SyncDamageData
    {
        public int ViewId;
        public float Damage;
    }

    public struct SyncNetAction
    {
        public int ViewId;
    }

    public struct SyncReadyClick
    {
        public bool isReady;
        public int player;
    }
}
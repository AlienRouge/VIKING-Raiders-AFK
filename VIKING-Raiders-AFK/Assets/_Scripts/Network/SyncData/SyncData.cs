using _Scripts.Enums;
using BinaryFormatter;
using UnityEngine;

namespace _Scripts.Network.SyncData
{
    public struct SyncData
    {
        public string modalName;
        public int heroLevel;
        public Vector3 spawnPos;
        public Team currentTeam;
        public int ButtonId;

        public static object Deserialize(byte[] bytes)
        {
            return new BinaryConverter().Deserialize<object>(bytes);
        }

        public static byte[] Serialize(object obj)
        {
            return new BinaryConverter().Serialize(obj);
        }
    }
}
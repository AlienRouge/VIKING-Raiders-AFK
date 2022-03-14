using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Enums;
using UnityEngine;

public class SpawnAreaController : MonoBehaviour
{
   private List<SpawnArea> _spawnAreas;

   private void Awake()
   {
      _spawnAreas = FindObjectsOfType<SpawnArea>().ToList();
   }

   public void SetupSpawnArea(Team team, Vector3 scale, Vector3 position)
   {
      var areaTransform = _spawnAreas[0].Team == team ? _spawnAreas[0].transform : _spawnAreas[1].transform;
      areaTransform.localScale = scale;
      areaTransform.localPosition = position;
      
      // foreach (var area in _spawnAreas.Where(area => area.Team == team))
      // {
      //    var areaTransform = area.transform;
      //    areaTransform.localScale = scale;
      //    areaTransform.position = position;
      //    break;
      // }
   }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapCameraData
{
    [Header("맵 카메라가 비추는 범위가 맵 밖으로 초과할건지 유무")] public bool UseCameraOffset;
    public float CameraOffsetMinX;
    public float CameraOffsetMaxX;
    public float CameraOffsetMinY;
    public float CameraOffsetMaxY;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="useCameraOffset">카메라 오프셋 사용 유무</param>
    /// <param name="cameraOffsetMinX"></param>
    /// <param name="cameraOffsetMaxX"></param>
    /// <param name="cameraOffsetMinY"></param>
    /// <param name="cameraOffsetMaxY"></param>
    public MapCameraData(
        bool useCameraOffset,
        float cameraOffsetMinX, 
        float cameraOffsetMaxX, 
        float cameraOffsetMinY,
        float cameraOffsetMaxY)
    {
        UseCameraOffset = useCameraOffset;
        CameraOffsetMinX = cameraOffsetMinX;
        CameraOffsetMaxX = cameraOffsetMaxX;
        CameraOffsetMinY = cameraOffsetMinY;
        CameraOffsetMaxY = cameraOffsetMaxY;
    }
}

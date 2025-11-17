using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SystemVolumeController : MonoBehaviour
{
    // Interface de volumen de Windows
    [ComImport]
    [Guid("5CDF2C82-841E-4546-9722-0CF74078229A")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioEndpointVolume
    {
        int RegisterControlChangeNotify(IntPtr pNotify);
        int UnregisterControlChangeNotify(IntPtr pNotify);
        int GetChannelCount(out uint channelCount);
        int SetMasterVolumeLevel(float levelDb, Guid eventContext);
        int SetMasterVolumeLevelScalar(float volume, Guid eventContext);
        int GetMasterVolumeLevel(out float levelDb);
        int GetMasterVolumeLevelScalar(out float volume);
        int SetMute(bool mute, Guid eventContext);
        int GetMute(out bool mute);
        // Otros métodos no usados
    }

    [ComImport]
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
    class MMDeviceEnumeratorCom { }

    [ComImport]
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDeviceEnumerator
    {
        int EnumAudioEndpoints();
        int GetDefaultAudioEndpoint(int dataFlow, int role, out IntPtr endpoint);
        int GetDevice(string id, out IntPtr device);
        int RegisterEndpointNotificationCallback(IntPtr client);
        int UnregisterEndpointNotificationCallback(IntPtr client);
    }

    void Start()
    {
        SetSystemVolume(0.5f); // 50% de volumen al iniciar
    }

    public void SetSystemVolume(float volume)
    {
        if (volume < 0f) volume = 0f;
        if (volume > 1f) volume = 1f;

        var enumerator = new MMDeviceEnumeratorCom() as IMMDeviceEnumerator;

        enumerator.GetDefaultAudioEndpoint(0, 1, out IntPtr endpoint);

        Guid IID_IAudioEndpointVolume = typeof(IAudioEndpointVolume).GUID;
        Marshal.QueryInterface(endpoint, ref IID_IAudioEndpointVolume, out IntPtr volumePtr);

        IAudioEndpointVolume volumeInterface = (IAudioEndpointVolume)Marshal.GetObjectForIUnknown(volumePtr);

        volumeInterface.SetMasterVolumeLevelScalar(volume, Guid.Empty);

        Marshal.ReleaseComObject(volumeInterface);
        Marshal.Release(endpoint);
    }
}

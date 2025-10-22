using UnityEngine;
using System;
using Sonic_CD;

namespace Retro_Engine
{

    public static class AudioPlayback
    {
      private static AudioSource musicAudioSource;
      private static AudioSource[] sfxAudioSources = new AudioSource[8];
      private static string currentTrack;
      private static MonoBehaviour coroutineRunner;
      public const int MUSIC_STOPPED = 0;
      public const int MUSIC_PLAYING = 1;
      public const int MUSIC_PAUSED = 2;
      public const int MUSIC_LOADING = 3;
      public const int MUSIC_READY = 4;
      public const int NUM_CHANNELS = 8;
      public static Game gameRef;
      public static int numGlobalSFX = 0;
      public static int numStageSFX = 0;
      public static AudioClip[] sfxSamples = new AudioClip[256];
      public static bool[] sfxLoaded = new bool[256];
      public static int[] channelSfxNum = new int[8];
      public static int nextChannelPos;
      public static bool musicEnabled = true;
      public static int musicVolume = 100;
      public static float musicVolumeSetting = 1f;
      public static float sfxVolumeSetting = 1f;
      public static int musicStatus = 0;
      public static int currentMusicTrack;
      public static MusicTrackInfo[] musicTracks = new MusicTrackInfo[16];

      static AudioPlayback()
      {
        for (int index = 0; index < AudioPlayback.musicTracks.Length; ++index)
          AudioPlayback.musicTracks[index] = new MusicTrackInfo();
        
        GameObject audioObject = new GameObject("AudioPlayback");
        musicAudioSource = audioObject.AddComponent<AudioSource>();
        musicAudioSource.loop = false;
        
        coroutineRunner = audioObject.AddComponent<AudioPlaybackCoroutineRunner>();
        
        for (int i = 0; i < 8; i++)
        {
          GameObject sfxObject = new GameObject($"SFXChannel{i}");
          sfxObject.transform.SetParent(audioObject.transform);
          sfxAudioSources[i] = sfxObject.AddComponent<AudioSource>();
        }
      }

      public static void InitAudioPlayback()
      {
        FileData fData = new FileData();
        char[] fileName = new char[32];
        for (int index = 0; index < 8; ++index)
          AudioPlayback.channelSfxNum[index] = -1;
        if (!FileIO.LoadFile("Data/Game/GameConfig.bin".ToCharArray(), fData))
          return;
        byte num1 = FileIO.ReadByte();
        byte num2;
        for (int index = 0; index < (int) num1; ++index)
          num2 = FileIO.ReadByte();
        byte num3 = FileIO.ReadByte();
        for (int index = 0; index < (int) num3; ++index)
          num2 = FileIO.ReadByte();
        byte num4 = FileIO.ReadByte();
        for (int index = 0; index < (int) num4; ++index)
          num2 = FileIO.ReadByte();
        byte num5 = FileIO.ReadByte();
        for (int index1 = 0; index1 < (int) num5; ++index1)
        {
          byte num6 = FileIO.ReadByte();
          for (int index2 = 0; index2 < (int) num6; ++index2)
            num2 = FileIO.ReadByte();
        }
        for (int index3 = 0; index3 < (int) num5; ++index3)
        {
          byte num7 = FileIO.ReadByte();
          for (int index4 = 0; index4 < (int) num7; ++index4)
            num2 = FileIO.ReadByte();
        }
        byte num8 = FileIO.ReadByte();
        for (int index5 = 0; index5 < (int) num8; ++index5)
        {
          byte num9 = FileIO.ReadByte();
          int index6;
          for (index6 = 0; index6 < (int) num9; ++index6)
          {
            byte num10 = FileIO.ReadByte();
            fileName[index6] = (char) num10;
          }
          fileName[index6] = char.MinValue;
          num2 = FileIO.ReadByte();
          num2 = FileIO.ReadByte();
          num2 = FileIO.ReadByte();
          num2 = FileIO.ReadByte();
        }
        byte num11 = FileIO.ReadByte();
        AudioPlayback.numGlobalSFX = (int) num11;
        for (int sfxNum = 0; sfxNum < (int) num11; ++sfxNum)
        {
          byte num12 = FileIO.ReadByte();
          int index;
          for (index = 0; index < (int) num12; ++index)
          {
            byte num13 = FileIO.ReadByte();
            fileName[index] = (char) num13;
          }
          fileName[index] = char.MinValue;
          FileIO.GetFileInfo(fData);
          FileIO.CloseFile();
          AudioPlayback.LoadSfx(fileName, sfxNum);
          FileIO.SetFileInfo(fData);
        }
      }

      public static void ReleaseAudioPlayback()
      {
      }

      public static void ReleaseGlobalSFX()
      {
      }

      public static void ReleaseStageSFX()
      {
      }

      public static void SetGameVolumes(int bgmVolume, int sfxVolume)
      {
        AudioPlayback.musicVolumeSetting = (float) bgmVolume * 0.01f;
        AudioPlayback.SetMusicVolume(AudioPlayback.musicVolume);
        AudioPlayback.sfxVolumeSetting = (float) sfxVolume * 0.01f;
      }

      public static void StopAllSFX()
      {
        for (int index = 0; index < 8; ++index)
        {
          if (AudioPlayback.channelSfxNum[index] > -1 && sfxAudioSources[index] != null)
            sfxAudioSources[index].Stop();
        }
      }

      public static void PauseSound()
      {
        if (musicAudioSource != null && currentTrack != null)
        {
          musicAudioSource.Pause();
          AudioPlayback.musicStatus = 2;
        }
      }

      public static void ResumeSound()
      {
        if (musicAudioSource != null && currentTrack != null)
        {
          musicAudioSource.UnPause();
          AudioPlayback.musicStatus = 1;
        }
      }

      public static void resumeSound()
      {
        ResumeSound();
      }

      public static void releaseStageSFX()
      {
        ReleaseStageSFX();
      }

      public static void SetMusicTrack(char[] fileName, int trackNo, byte loopTrack, uint loopPoint)
      {
        char[] charArray = "Music/".ToCharArray();
        int num = FileIO.StringLength(ref fileName);
        for (int index = 0; index < num; ++index)
        {
          if (fileName[index] == '/')
            fileName[index] = '-';
        }
        int index1 = num - 4;
        if (index1 < 0)
          index1 = 0;
        if (fileName.Length > 0)
          fileName[index1] = char.MinValue;
        FileIO.StrCopy(ref AudioPlayback.musicTracks[trackNo].trackName, ref charArray);
        FileIO.StrAdd(ref AudioPlayback.musicTracks[trackNo].trackName, ref fileName);
        AudioPlayback.musicTracks[trackNo].loop = loopTrack == (byte) 1;
        AudioPlayback.musicTracks[trackNo].loopPoint = loopPoint;
      }

      public static void SetMusicVolume(int volume)
      {
        if (volume < 0)
          volume = 0;
        if (volume > 100)
          volume = 100;
        AudioPlayback.musicVolume = volume;
        if (musicAudioSource != null && currentTrack != null)
        {
          musicAudioSource.volume = AudioPlayback.musicVolumeSetting;
        }
      }

      public static void PlayMusic(int trackNo)
      {
        if (AudioPlayback.musicTracks[trackNo].trackName[0] != '\0')
        {
          string text = new string(AudioPlayback.musicTracks[trackNo].trackName);
          text = text.Remove(FileIO.StringLength(ref AudioPlayback.musicTracks[trackNo].trackName));
          try
          {
            string musicPath;
            if (text.StartsWith("Music/"))
            {
              musicPath = System.IO.Path.Combine(Application.streamingAssetsPath, text);
            }
            else
            {
              musicPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Music", text);
            }
            
            if (!musicPath.EndsWith(".mp3"))
            {
              musicPath += ".mp3";
            }
            
            if (System.IO.File.Exists(musicPath))
            {
              coroutineRunner.StartCoroutine(LoadMusicFile(musicPath, trackNo));
            }
            else
            {
              Debug.LogWarning($"Music file not found: {musicPath}");
              AudioPlayback.musicStatus = 0;
            }
          }
          catch (System.Exception ex)
          {
            Debug.LogError($"Error loading music: {ex.Message}");
            AudioPlayback.musicStatus = 0;
          }
        }
      }
      
      private static System.Collections.IEnumerator LoadMusicFile(string filePath, int trackNo)
      {
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG))
        {
          yield return www.SendWebRequest();
          
          if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
          {
            AudioClip clip = UnityEngine.Networking.DownloadHandlerAudioClip.GetContent(www);
            if (clip != null)
            {
              musicAudioSource.clip = clip;
              musicAudioSource.loop = AudioPlayback.musicTracks[trackNo].loop;
              musicAudioSource.volume = AudioPlayback.musicVolumeSetting;
              musicAudioSource.Play();
              
              currentTrack = System.IO.Path.GetFileNameWithoutExtension(filePath);
              AudioPlayback.currentMusicTrack = trackNo;
              AudioPlayback.musicVolume = 100;
              AudioPlayback.musicStatus = 1;
            }
            else
            {
              AudioPlayback.musicStatus = 0;
            }
          }
          else
          {
            Debug.LogError($"Failed to load music file: {www.error}");
            AudioPlayback.musicStatus = 0;
          }
        }
      }

      public static void StopMusic()
      {
        if (musicAudioSource != null && currentTrack != null)
        {
          musicAudioSource.Stop();
          currentTrack = null;
        }
        AudioPlayback.musicStatus = 0;
      }

      public static void LoadSfx(char[] fileName, int sfxNum)
      {
        FileData fData = new FileData();
        char[] strA = new char[64];
        char[] charArray = "Data/SoundFX/".ToCharArray();
        if (sfxNum <= -1 || sfxNum >= 256)
          return;
        FileIO.StrCopy(ref strA, ref charArray);
        FileIO.StrAdd(ref strA, ref fileName);
        if (!FileIO.LoadFile(strA, fData))
          return;
        
        byte num1 = FileIO.ReadByte();
        num1 = FileIO.ReadByte();
        num1 = FileIO.ReadByte();
        num1 = FileIO.ReadByte();
        num1 = FileIO.ReadByte();
        num1 = FileIO.ReadByte();
        num1 = FileIO.ReadByte();
        num1 = FileIO.ReadByte();
        num1 = FileIO.ReadByte();
        num1 = FileIO.ReadByte();
        num1 = FileIO.ReadByte();
        num1 = FileIO.ReadByte();
        uint num2 = 1;
        for (uint index = 0; num2 > 0U && index < 400U; ++index)
        {
          switch (num2)
          {
            case 1:
              if (FileIO.ReadByte() == (byte) 100)
              {
                ++num2;
                break;
              }
              num2 = 1U;
              break;
            case 2:
              if (FileIO.ReadByte() == (byte) 97)
              {
                ++num2;
                break;
              }
              num2 = 1U;
              break;
            case 3:
              if (FileIO.ReadByte() == (byte) 116)
              {
                ++num2;
                break;
              }
              num2 = 1U;
              break;
            case 4:
              num2 = FileIO.ReadByte() != (byte) 97 ? 1U : 0U;
              break;
          }
        }
        uint num3 = (uint) FileIO.ReadByte() + ((uint) FileIO.ReadByte() << 8) + ((uint) FileIO.ReadByte() << 16) + ((uint) FileIO.ReadByte() << 24) << 1;
        float[] samples = new float[num3 / 2];
        
        for (uint index = 0; index < num3; index += 2U)
        {
          byte sampleByte = FileIO.ReadByte();
          samples[index / 2] = (sampleByte - 128) / 128f;
        }
        
        AudioClip clip = AudioClip.Create(new string(fileName), (int)(num3 / 2), 1, 44100, false);
        clip.SetData(samples, 0);
        
        if (AudioPlayback.sfxLoaded[sfxNum])
          AudioPlayback.sfxSamples[sfxNum] = null;
        AudioPlayback.sfxSamples[sfxNum] = clip;
        AudioPlayback.sfxLoaded[sfxNum] = true;
        FileIO.CloseFile();
      }

      public static void PlaySfx(int sfxNum, byte sLoop)
      {
        for (int index = 0; index < 8; ++index)
        {
          if (AudioPlayback.channelSfxNum[index] == sfxNum)
          {
            if (sfxAudioSources[index] != null)
              sfxAudioSources[index].Stop();
            AudioPlayback.nextChannelPos = index;
            index = 8;
          }
        }
        sfxAudioSources[AudioPlayback.nextChannelPos].clip = sfxSamples[sfxNum];
        sfxAudioSources[AudioPlayback.nextChannelPos].loop = sLoop == (byte) 1;
        sfxAudioSources[AudioPlayback.nextChannelPos].panStereo = 0.0f;
        sfxAudioSources[AudioPlayback.nextChannelPos].volume = AudioPlayback.sfxVolumeSetting;
        sfxAudioSources[AudioPlayback.nextChannelPos].Play();
        AudioPlayback.channelSfxNum[AudioPlayback.nextChannelPos] = sfxNum;
        ++AudioPlayback.nextChannelPos;
        if (AudioPlayback.nextChannelPos != 8)
          return;
        AudioPlayback.nextChannelPos = 0;
      }

      public static void StopSfx(int sfxNum)
      {
        for (int index = 0; index < 8; ++index)
        {
          if (AudioPlayback.channelSfxNum[index] == sfxNum)
          {
            AudioPlayback.channelSfxNum[index] = -1;
            if (sfxAudioSources[index] != null)
              sfxAudioSources[index].Stop();
          }
        }
      }

      public static void SetSfxAttributes(int sfxNum, int volume, int pan)
      {
        for (int index = 0; index < 8; ++index)
        {
          if (AudioPlayback.channelSfxNum[index] == sfxNum)
          {
            if (sfxAudioSources[index] != null)
              sfxAudioSources[index].Stop();
            AudioPlayback.nextChannelPos = index;
            index = 8;
          }
        }
        sfxAudioSources[AudioPlayback.nextChannelPos].clip = sfxSamples[sfxNum];
        sfxAudioSources[AudioPlayback.nextChannelPos].loop = false;
        sfxAudioSources[AudioPlayback.nextChannelPos].panStereo = (float) pan * 0.01f;
        sfxAudioSources[AudioPlayback.nextChannelPos].volume = AudioPlayback.sfxVolumeSetting;
        sfxAudioSources[AudioPlayback.nextChannelPos].Play();
        AudioPlayback.channelSfxNum[AudioPlayback.nextChannelPos] = sfxNum;
        ++AudioPlayback.nextChannelPos;
        if (AudioPlayback.nextChannelPos != 8)
          return;
        AudioPlayback.nextChannelPos = 0;
      }

      public static void Dispose()
      {
        if (musicAudioSource != null)
        {
          musicAudioSource.Stop();
          musicAudioSource = null;
        }
        
        for (int i = 0; i < sfxAudioSources.Length; i++)
        {
          if (sfxAudioSources[i] != null)
          {
            sfxAudioSources[i].Stop();
            sfxAudioSources[i] = null;
          }
        }
      }
    }
    
    public class AudioPlaybackCoroutineRunner : MonoBehaviour
    {
    }
}
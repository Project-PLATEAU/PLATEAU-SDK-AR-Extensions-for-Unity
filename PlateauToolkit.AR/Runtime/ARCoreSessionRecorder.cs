using System.IO;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;

namespace PlateauToolkit.AR
{
    [RequireComponent(typeof(ARSession))]
    public class ARCoreSessionRecorder : MonoBehaviour
    {
        ARSession m_Session;

        string m_PlaybackStatusMessage;
        string m_RecordingStatusMessage;

        static string s_Mp4DirectoryPath;

        void Awake()
        {
            s_Mp4DirectoryPath = Path.Combine(Application.persistentDataPath, "ar-playbacks");
            m_Session = GetComponent<ARSession>();
        }

        void Start()
        {
            if (!Directory.Exists(s_Mp4DirectoryPath))
            {
                Directory.CreateDirectory(s_Mp4DirectoryPath);
            }
        }

        public string[] GetPlaybackPaths()
        {
            return Directory.GetFiles(s_Mp4DirectoryPath);
        }

        static int GetRotation() => Screen.orientation switch
        {
            ScreenOrientation.Portrait => 0,
            ScreenOrientation.LandscapeLeft => 90,
            ScreenOrientation.PortraitUpsideDown => 180,
            ScreenOrientation.LandscapeRight => 270,
            _ => 0
        };

        public readonly struct RecorderController
        {
            readonly ARCoreSessionSubsystem m_Subsystem;

            public RecorderController(ARCoreSessionSubsystem subsystem)
            {
                m_Subsystem = subsystem;
            }

            public void StartRecording(string recordingName)
            {
                ArSession session = m_Subsystem.session;
                using var config = new ArRecordingConfig(session);

                config.SetMp4DatasetFilePath(session, Path.Combine(s_Mp4DirectoryPath, recordingName));
                config.SetRecordingRotation(session, GetRotation());

                ArStatus status = m_Subsystem.StartRecording(config);
                Debug.Log($"StartRecording to {config.GetMp4DatasetFilePath(session)} => {status}");
            }

            public void StopRecording()
            {
                ArStatus status = m_Subsystem.StopRecording();
                Debug.Log($"StopRecording() => {status}");
            }

            public void StartPlayback(string mp4Path)
            {
                ArStatus status = m_Subsystem.StartPlayback(mp4Path);
                Debug.Log($"StartPlayback({mp4Path}) => {status}");
            }

            public void StopPlayback()
            {
                ArStatus status = m_Subsystem.StopPlayback();
                Debug.Log($"StopPlayback() => {status}");
            }

            public bool IsRecording()
            {
                return m_Subsystem.recordingStatus.Recording();
            }

            public bool IsPlayingPlayback()
            {
                return m_Subsystem.playbackStatus.Playing();
            }
        }

        public bool GetController(out RecorderController controller)
        {
            if (m_Session.subsystem is ARCoreSessionSubsystem subsystem)
            {
                ArSession session = subsystem.session;
                if (session == null)
                {
                    controller = new();
                    return false;
                }

                controller = new RecorderController(subsystem);
                return true;
            }

            controller = new();
            return false;
        }
    }
}
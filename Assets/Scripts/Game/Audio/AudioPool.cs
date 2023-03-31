using System;
using CriWare;
using Game.Core;
using UnityEngine;

namespace Game.Audio
{
    public class AudioPoolData : PoolObject
    {
        public CriAtomSource audioSource;

        public override bool Equals(object obj)
        {
            var other = obj as AudioPoolData;
            return audioSource.cueSheet == other?.audioSource.cueSheet && 
                   audioSource.cueName == other?.audioSource.cueName;
        }

        public override int GetHashCode()
        {
            return audioSource != null ? audioSource.GetHashCode() : 0;
        }
    }
    
    public class AudioPool : CustomObjectPool<AudioPoolData>
    {
        private const int MaxCount = 10;
        protected override void OnCreateItem(AudioPoolData poolObject)
        {
            var audioRoot = Managers.Get<IAudioManager>().AudioRoot;
            var audioObj = new GameObject();
            audioObj.transform.SetParent(audioRoot);
            var component = audioObj.AddComponent<CriAtomSource>();
            component.SetPlaybackHandler(RecycleAudio);
            poolObject.audioSource = component;
        }

        protected override void OnRecycleItem(AudioPoolData poolObject)
        {
            poolObject.audioSource.gameObject.SetActive(false);
        }

        private CriAtomSource CreateAudioSource(string name)
        {
            var poolObject = CreateItem();
            if (poolObject != null)
            {
                poolObject.audioSource.cueSheet = "SoundFX";
                poolObject.audioSource.cueName = name;
                poolObject.audioSource.volume = Managers.Get<IAudioManager>().SoundVolume;
                return poolObject.audioSource;
            }

            Debug.LogError("创建AudioSource为空");
            return null;
        }

        public CriAtomSource GetAudioSource(string name)
        {
            var poolObject = GetItem(item => item.audioSource.cueName == name && item.audioSource.status is CriAtomSourceBase.Status.PlayEnd);
            return poolObject == null ? CreateAudioSource(name) : poolObject.audioSource;
        }

        private void RecycleAudio(CriAtomSource audioSource)
        {
            var poolObject = GetItem(item => item.audioSource == audioSource);
            RecycleItem(poolObject);
        }
    }
}
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
        protected override void OnCreateItem(AudioPoolData poolObject)
        {
            var audioRoot = Managers.Get<IAudioManager>().AudioRoot;
            var audioObj = new GameObject();
            audioObj.transform.SetParent(audioRoot);
            var component = audioObj.AddComponent<CriAtomSource>();
            poolObject.audioSource = component;
        }

        private CriAtomSource CreateAudioSource(string name)
        {
            var poolObject = CreateItem();
            if (poolObject != null)
            {
                poolObject.audioSource.cueSheet = "SoundFX";
                poolObject.audioSource.cueName = name;
                return poolObject.audioSource;
            }

            Debug.LogError("创建AudioSource为空");
            return null;
        }

        public CriAtomSource GetAudioSource(string name)
        {
            var poolObject = GetItem(item => item.audioSource.cueName == name && item.audioSource.IsPaused());
            return poolObject == null ? CreateAudioSource(name) : poolObject.audioSource;
        }
    }
}
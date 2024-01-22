// File create date:2021/2/15
using RoachLite.AudioManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
// Created By Yu.Liu
[CreateAssetMenu(fileName = "NewDatabase", menuName = "ResManage/Audio/Database")]
public class AudioDatabaseObject : ScriptableObject {

    public string databaseName;
    public AudioMixerGroup databaseMixer;
    public List<string> groupNames = new List<string>();
    public List<AudioGroupData> audioGroups = new List<AudioGroupData>();
    // Editor编辑界面所需数据
    public bool isExpanded = false;

    public bool AddAudioGroup(string name, AudioGroupData groupData) {
        if (groupData != null) {
            if (!groupNames.Contains(name)) {
                audioGroups.Add(groupData);
                groupNames.Add(name);
                return true;
            }
            Debug.LogWarning($"AUDIO: Cannot Add Audio Group Data due to the same name [{name}].");
        }
        return false;
    }

    public bool CheckGroupData(string name) {
        return groupNames.Contains(name);
    }

    public void RemoveGroupData(string name) {
        if (CheckGroupData(name)) {
            var index = groupNames.IndexOf(name);
            groupNames.RemoveAt(index);
            audioGroups.RemoveAt(index);
        }
    }

    public AudioGroupData GetAudioGroupData(string name) {
        AudioGroupData result = null;
        if (CheckGroupData(name)) {
            var index = groupNames.IndexOf(name);
            result = audioGroups[index];
        }
        return result;
    }
}

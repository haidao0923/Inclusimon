using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode : MonoBehaviour
{
    private static Mode _instance;
    public static Mode Instance { get {return _instance;} }
    public ModeType modeType;
    public bool isChangingMode;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
        } else if (_instance != this) {
            Destroy(this.gameObject);
        }
        modeType = ModeType.Normal;
    }

    public string GetAnimationStringForCurrentMode(ModeType modeType) {
        switch (modeType) {
        case ModeType.Normal:
            return "Normal_Light_Up";
        default:
            return "Colorblind_Light_Up";
        }
    }

    public enum ModeType {
        Normal, Colorblind, Blind, Deaf, SCT, MemoryDeficit
    }

}

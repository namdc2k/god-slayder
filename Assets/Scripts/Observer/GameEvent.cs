using System;
using Player;

namespace Observer{
    public class GameEvent{
        public static Action<float, float> OnChangeInputControl;
        public static Action<float,PlayerAnimationClip> OnChangeAnimationClip;
    }
}
﻿using System.Collections.Generic;

namespace P3DS2U.Editor
{
    public class AnimationNaming
    {
        public static Dictionary<string, string[]> animationNames = new Dictionary<string, string[]>
        {
            {
                "Fight", new[]
                {
                    "Idle",
                    "Appear",
                    "Transform",
                    "Release",
                    "Dropping",
                    "Landing",
                    "Release_without_Landing",
                    "Mega_Upgraded",
                    "Attack",
                    "Attack_2",
                    "Attack_3",
                    "Attack_4",
                    "No_Touch_Attack",
                    "No_Touch_Attack_2",
                    "No_Touch_Attack_3",
                    "No_Touch_Attack_4",
                    "Be_Attacked",
                    "Lost",
                    "Empty",
                    "Eye_Emotion",
                    "Eye_2_Emotion",
                    "Eye_3_Emotion",
                    "Mouth_Emotion",
                    "Mouth_2_Emotion",
                    "Mouth_3_Emotion",
                    "State",
                    "State_2",
                    "State_3",
                    "State_4"
                }
            },
            {
                "Movement", new[]
                {
                    "Idle",
                    "Empty",
                    "Walk",
                    "Run",
                    "Empty_2",
                    "Start_Walk",
                    "End_Walk",
                    "Empty_3",
                    "Start_Run",
                    "End_Run",
                    "Empty_4",
                    "Start_Run_2",
                    "End_Run_2",
                    "Empty_5",
                    "Eye_Emotion",
                    "Eye_2_Emotion",
                    "Eye_3_Emotion",
                    "Mouth_Emotion",
                    "Mouth_2_Emotion",
                    "Mouth_3_Emotion",
                    "State",
                    "State_2",
                    "State_3",
                    "State_4"
                }
            },
            {
                "Pet", new[]
                {
                    "Idle",
                    "Turn",
                    "Look_Back",
                    "Look_Back_Happily",
                    "Falling Asleep",
                    "Sleepy",
                    "Sleepy_Awaken",
                    "Sleeping",
                    "Awaken",
                    "Refuse",
                    "Thinking",
                    "Agree",
                    "Happy",
                    "Very_Happy",
                    "Look_Around",
                    "Rub_Eyes",
                    "Comfortable",
                    "Relax",
                    "Sad",
                    "Salutate",
                    "Happy_2",
                    "Angry",
                    "Begin_Eating",
                    "Eating",
                    "Eating_Finished",
                    "No_Eating",
                    "Empty",
                    "Eye_Emotion",
                    "Eye_2_Emotion",
                    "Eye_3_Emotion",
                    "Mouth_Emotion",
                    "Mouth_2_Emotion",
                    "Mouth_3_Emotion",
                    "State",
                    "State_2",
                    "State_3",
                    "State_4"
                }
            }
        };
    }
}

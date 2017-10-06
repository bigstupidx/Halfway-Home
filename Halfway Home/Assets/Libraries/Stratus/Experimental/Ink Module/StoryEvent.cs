using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stratus
{
  namespace InkModule
  {
    public class StoryEvent : Triggerable
    {
      public enum Scope
      {
        Target,
        Scene
      }

      [Header("Story")]
      [Tooltip("The ink story file to play in .json format")]
      public TextAsset storyFile;
      public Scope scope = Scope.Scene;

      [DrawIf("scope", Scope.Scene, ComparisonType.NotEqual, PropertyDrawingType.DontDraw)]
      [Tooltip("The reader to trigger")]
      public StoryReader reader;

      protected override void OnAwake()
      {
      }

      protected override void OnTrigger()
      {
        switch (scope)
        {
          case Scope.Target:
            reader.gameObject.Dispatch<Story.LoadEvent>(new Story.LoadEvent() { storyFile = this.storyFile });
            break;
          case Scope.Scene:
            Scene.Dispatch<Story.LoadEvent>(new Story.LoadEvent() { storyFile = this.storyFile });
            break;
          default:
            break;
        }

      }
    } 
  }

}
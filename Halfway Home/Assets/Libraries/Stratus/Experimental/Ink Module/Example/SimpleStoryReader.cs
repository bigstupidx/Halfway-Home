using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using System;

namespace Stratus
{
  namespace Modules
  {
    namespace InkModule
    {
      public class SimpleStoryReader : StoryReader<RegexParser> 
      {
        //------------------------------------------------------------------------------------------/
        // Messages
        //------------------------------------------------------------------------------------------/
        protected override void OnBindExternalFunctions(Story story)
        {
          story.runtime.BindExternalFunction("PlayMusic", new Action<string>(PlayMusic));
        }

        protected override void OnConfigureParser(RegexParser parser)
        {          
        }

        protected override void OnLoad(Dictionary<string, Story> stories)
        { 
          
        }

        protected override void OnSave(Dictionary<string, Story> stories)
        {
          
        }

        protected override void OnClear()
        {
          
        }

        //------------------------------------------------------------------------------------------/
        // External functions
        //------------------------------------------------------------------------------------------/
        public void PlayMusic(string trackName)
        {
          Trace.Script("Playing music track '" + trackName + "'");
        }


      }
    }
  }
}
using libx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHSpace
{
    public class BallMgr : Singleton<BallMgr>
    {
        public BallAuto m_ballAuto;
        public override void Init()
        {
            ReadFile();
        }
        private void ReadFile()
        {
            TextAsset textAsset = AssetLoader.Load<TextAsset>(Files.ballAuto);
            string jsonStr = textAsset.text;
            List<BallAuto> ballAutos = FullSerializerAPI.Deserialize(typeof(List<BallAuto>), jsonStr) as List<BallAuto>;
            m_ballAuto = ballAutos[0];
        }
    }
}


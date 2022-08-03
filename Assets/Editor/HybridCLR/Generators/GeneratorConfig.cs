using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HybridCLR.Generators
{
    internal class GeneratorConfig
    {
        /// <summary>
        /// 目前已经根据热更新dll的依赖自动计算需要扫描哪些dll来收集桥接函数。
        /// 只要你的热更新以assembly def形式放到项目中，是不需要改这个的
        /// </summary>
        /// <returns></returns>
        public static List<string> GetExtraAssembiles()
        {
            return new List<string>
            {
                // "mscorlib",
            };
        }

        /// <summary>
        /// 暂时没有仔细扫描泛型，如果运行时发现有生成缺失，先手动在此添加类
        /// </summary>
        /// <returns></returns>
        public static List<Type> PrepareCustomGenericTypes()
        {
            return new List<Type>
            {
                typeof(Action<int, string, Vector3>),
                typeof(Action<int, string, Vector3>),
                typeof(libx.AssetLoader),
                typeof(FullSerializer.fsSerializer),
                typeof(Foundation.StreamFile),
                typeof(FullSerializer.Internal.fsPortableReflection),
                //typeof(Foundation.TextureScaleThread),
                //typeof(EventArgsThree<Vector3,float,System.Action>)
            };
        }

        /// <summary>
        /// 如果提示缺失桥接函数，将提示缺失的签名加入到下列列表是简单的做法。
        /// 这里添加64位App缺失的桥接函数签名
        /// </summary>
        /// <returns></returns>
        public static List<string> PrepareCustomMethodSignatures64()
        {
            return new List<string>
            {
                "vi8i8",
                "i1",
                "i1i8S16i8i1",
                "i1i8i8S32i1",
                "i1i8S8vf4i1",
                "i1i8i16i8i1",
                "i1i8i4",
                "i1i8i4i8",
                "i1i8i8i8",
                "i1i8i16i8",
                "i1i2",
                "i1r4",
                "i1r8",
                "i16r4",
                "i16i8",
                "i1i8i4i4i4i1",
                "i2i8i4",
                "i8i8i2",
                "i8i8i4i4",
                "i1i8i4i8i8",
                "i8i8i1",
                "i8i8i8i4",
                "i8i8i2i2",
                "i8i8vf4vf2",
                "i1i8i8sri1",
                "i1i8i8vf4i1",
                "i8i8vf3r4i4",
                "vi8i4i8i4i4",
                "vi8i8i8i4",
                "vi8i8i4i4",
                "vi8i16i8",
                "vi8i4i1",
                "vi1",
                "vi8r4r4r4r4",
                "vi8i4i4",
                "vi8i1",
                "vi8i2",
                "vi8i4i4i4i1",
                "vi8i4i4i4i4i8",
                "vi8i4i4vf4",
                "vi8r4r4r4",
                "vi8i8i1",
                "vi8vf3i4",
                "vi8i8i4",
                "vi8i8sr",
                "vi8i8vf4",
                "vf4i8i4i4",
                "vf4r4r4r4",
                "S24i8",
                "S24i8i4",
                "S68i4",
                "S40i8",
                "r8i16",
            };
        }

        /// <summary>
        /// 如果提示缺失桥接函数，将提示缺失的签名加入到下列列表是简单的做法。
        /// 这里添加32位App缺失的桥接函数签名
        /// </summary>
        /// <returns></returns>
        public static List<string> PrepareCustomMethodSignatures32()
        {
            return new List<string>
            {
                "vi4i4",
                "i1",
                "i1i8i4",
                "i1i8i4i8",
                "S24i8",
                "vi8i8i8i4",
                "vi8i8i4i4",
                "vi8i16i8",
                "vi8i4i1",
                "i1i8i8i8",
                "i1i8i16i8",
                "i1i2",
                "vi1",
                "i2i8i4",
                "i8i8i2",
                "i8i8i4i4",
                "i1i8i4i8i8",
                "S24i8i4",
                "i8i8i1",
                "vi8i4i8i4i4",
                "i8i8i8i4",
                "vi8r4r4r4r4",
                "vi8i4i4",
                "vi8i1",
                "vi8i4i4i4i1",
                "vi8i4i4i4i4i8",
                "vf4i8i4i4",
                "vi8i4i4vf4",
                "i8i8vf4vf2",
                "S68i4",
                "vf4r4r4r4",
                "vi8r4r4r4",
                "vi8i8i1",
                "vi8vf3i4",
                "i1i8i4i4i4i1",
                "vi8i8i4",
                "vi8i8sr",
                "vi8i8vf4",
                "i1i8i8sri1",
                "i1r4",
                "i16r4",
                "r8i16",
                "vi8i2",
                "S40i8",
                "i16i8",
                "i8i8vf3r4i4",
                "i1r8",
                "i8i8i2i2",
                "i1i8i8vf4i1",
            };
        }
    }
}

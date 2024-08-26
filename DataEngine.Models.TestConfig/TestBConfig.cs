//+-------------------------------------------------+
//|        This source code is auto generated       |
//+-------------------------------------------------+

using DataEngine.Protocal;

namespace DataEngine.Models.TestConfig
{
    public class TestBConfig : ConfigData
    {

        /// <summary>
        /// 数值ID测试
        /// </summary>
        public Int32 TestB_Id { get; private set; }
        /// <summary>
        /// 导航ID测试
        /// </summary>
        public UInt32 TestB_NavId { get; private set; }
        /// <summary>
        /// 字符串值测试
        /// </summary>
        public String TestB_Str { get; private set; }
        /// <summary>
        /// VEC3数组测试
        /// </summary>
        public Vector2[] TestB_Vec3 { get; private set; }

        public TestBConfig(object[] paras) : base(paras)
        {
            TestB_Id = ReadValue<Int32>(paras[0]);
            TestB_NavId = ReadValue<UInt32>(paras[1]);
            TestB_Str = ReadValue<String>(paras[2]);
            TestB_Vec3 = ReadArray<Vector2>(paras[3]);
        }

    }

}

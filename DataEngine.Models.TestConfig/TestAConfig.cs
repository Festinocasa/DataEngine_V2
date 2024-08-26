//+-------------------------------------------------+
//|        This source code is auto generated       |
//+-------------------------------------------------+

using DataEngine.Protocal;

namespace DataEngine.Models.TestConfig
{
    public class TestAConfig : ConfigData
    {

        /// <summary>
        /// 枚举ID测试
        /// </summary>
        public UInt32 TestA_Id { get; private set; }
        /// <summary>
        /// 导航ID测试
        /// </summary>
        public UInt32 TestA_NavId { get; private set; }
        /// <summary>
        /// INT值测试
        /// </summary>
        public Int32 TestA_Int { get; private set; }
        /// <summary>
        /// VEC2值测试
        /// </summary>
        public Vector2 TestA_Vec2 { get; private set; }
        /// <summary>
        /// 字符串值测试
        /// </summary>
        public String TestA_Str { get; private set; }
        /// <summary>
        /// 浮点数组测试
        /// </summary>
        public Single[] TestA_FloatArr { get; private set; }
        /// <summary>
        /// 字符串数组测试
        /// </summary>
        public String[] TestA_StrArr { get; private set; }

        public TestAConfig(object[] paras) : base(paras)
        {
            TestA_Id = ReadValue<UInt32>(paras[0]);
            TestA_NavId = ReadValue<UInt32>(paras[1]);
            TestA_Int = ReadValue<Int32>(paras[2]);
            TestA_Vec2 = ReadValue<Vector2>(paras[3]);
            TestA_Str = ReadValue<String>(paras[4]);
            TestA_FloatArr = ReadArray<Single>(paras[5]);
            TestA_StrArr = ReadArray<String>(paras[6]);
        }

    }


    public static class TESTACONFIG_ID
    {
        /// <summary>
        /// HEX: 0x_00_00_00_00 DEC: 0
        /// </summary>
        public const uint TEST_ID_A = 0b_00000000_00000000_00000000_00000000;
        /// <summary>
        /// HEX: 0x_00_00_00_02 DEC: 2
        /// </summary>
        public const uint TEST_ID_B = 0b_00000000_00000000_00000000_00000010;
        /// <summary>
        /// HEX: 0x_00_00_00_04 DEC: 4
        /// </summary>
        public const uint TEST_ID_C = 0b_00000000_00000000_00000000_00000100;
        /// <summary>
        /// HEX: 0x_00_00_00_06 DEC: 6
        /// </summary>
        public const uint TEST_ID_D = 0b_00000000_00000000_00000000_00000110;


    }

}

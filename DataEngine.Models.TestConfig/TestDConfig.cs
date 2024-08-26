//+-------------------------------------------------+
//|        This source code is auto generated       |
//+-------------------------------------------------+

namespace DataEngine.Models.TestConfig
{
    public class TestDConfig : ConfigData
    {

        /// <summary>
        /// 导航枚举测试
        /// </summary>
        public UInt32 TestD_Id { get; private set; }
        /// <summary>
        /// 导航ID字符值测试
        /// </summary>
        public Char TestD_NavId { get; private set; }

        public TestDConfig(object[] paras) : base(paras)
        {
            TestD_Id = ReadValue<UInt32>(paras[0]);
            TestD_NavId = ReadValue<Char>(paras[1]);
        }

    }


    public static class TESTDCONFIG_ID
    {
        /// <summary>
        /// HEX: 0x_00_00_00_01 DEC: 1
        /// </summary>
        public const uint TEST_NAVID_A = 0b_00000000_00000000_00000000_00000001;
        /// <summary>
        /// HEX: 0x_00_00_00_03 DEC: 3
        /// </summary>
        public const uint TEST_NAVID_B = 0b_00000000_00000000_00000000_00000011;
        /// <summary>
        /// HEX: 0x_00_00_00_05 DEC: 5
        /// </summary>
        public const uint TEST_NAVID_C = 0b_00000000_00000000_00000000_00000101;
        /// <summary>
        /// HEX: 0x_00_00_00_07 DEC: 7
        /// </summary>
        public const uint TEST_NAVID_D = 0b_00000000_00000000_00000000_00000111;
        /// <summary>
        /// HEX: 0x_00_00_00_08 DEC: 8
        /// </summary>
        public const uint TEST_NAVID_E = 0b_00000000_00000000_00000000_00001000;
        /// <summary>
        /// HEX: 0x_00_00_00_09 DEC: 9
        /// </summary>
        public const uint TEST_NAVID_F = 0b_00000000_00000000_00000000_00001001;
        /// <summary>
        /// HEX: 0x_00_00_00_0a DEC: 10
        /// </summary>
        public const uint TEST_NAVID_G = 0b_00000000_00000000_00000000_00001010;
        /// <summary>
        /// HEX: 0x_00_00_00_0b DEC: 11
        /// </summary>
        public const uint TEST_NAVID_H = 0b_00000000_00000000_00000000_00001011;


    }

}

//+-------------------------------------------------+
//|        This source code is auto generated       |
//+-------------------------------------------------+

namespace DataEngine.Models.TestConfig
{
    public class TestCConfig : ConfigData
    {

        /// <summary>
        /// 字符串MapID测试
        /// </summary>
        public String TestC_Id { get; private set; }
        /// <summary>
        /// 浮点值测试
        /// </summary>
        public Single TestC_NavId { get; private set; }

        public TestCConfig(object[] paras) : base(paras)
        {
            TestC_Id = ReadValue<String>(paras[0]);
            TestC_NavId = ReadValue<Single>(paras[1]);
        }

    }

}

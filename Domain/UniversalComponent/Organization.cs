namespace SCAS.Domain.Basic
{
    public class Organization
    {
        // 系统识别码，由系统生成
        public string Id { get; internal set; }

        // 在当前域下的识别码，由系统使用者给定
        public string Sid { get; internal set; }
        // 在当前域下的前缀码，用于秩序册的生成，由系统使用者给定，应为两位数字
        public int PrefixCode { get; internal set; }

        // 组织名
        public string Name { get; internal set; }
    }
};

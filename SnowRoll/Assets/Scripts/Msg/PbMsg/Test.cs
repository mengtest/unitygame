//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: Proto/Test.proto
namespace msg
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MSG_ReqTest")]
  public partial class MSG_ReqTest : global::ProtoBuf.IExtensible
  {
    public MSG_ReqTest() {}
    
    private int _requid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"requid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int requid
    {
      get { return _requid; }
      set { _requid = value; }
    }
    private long _reqguid = default(long);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"reqguid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(long))]
    public long reqguid
    {
      get { return _reqguid; }
      set { _reqguid = value; }
    }
    private string _reqaccount = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"reqaccount", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string reqaccount
    {
      get { return _reqaccount; }
      set { _reqaccount = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MSG_RetTest")]
  public partial class MSG_RetTest : global::ProtoBuf.IExtensible
  {
    public MSG_RetTest() {}
    
    private int _retuid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"retuid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int retuid
    {
      get { return _retuid; }
      set { _retuid = value; }
    }
    private long _retguid = default(long);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"retguid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(long))]
    public long retguid
    {
      get { return _retguid; }
      set { _retguid = value; }
    }
    private string _retaccount = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"retaccount", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string retaccount
    {
      get { return _retaccount; }
      set { _retaccount = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}
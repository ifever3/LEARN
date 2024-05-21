using Mediator.Net.Contracts;

namespace LEARN.model
{
    /// <summary>
    /// 更新员工信息的命令
    /// </summary>
    public class updateStaffcommand:ICommand
    {
         public Guid Id {  get; set; }
        public string Name {  get; set; }
        public string Major { get; set; }
    }

    public class updateStaffresponse : IResponse
    {
        public string Name { get; set; }
        public string Major { get; set; }
    }

}

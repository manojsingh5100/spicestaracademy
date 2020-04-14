using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class AdminControlViewModel
    {
        public List<RoleViewModel> GetBatchList { get; set; }
        public List<SessionMasterViewModel> GetSessionList { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel
{
    public class FeePaymentDetailViewModel
    {
        public FeePaymentDetailViewModel()
        {
            CurrentDate = DateTime.Now.ToShortDateString();
            FeeCollection = new AddFeeCollectionViewModel();
            BankDetail = new BankAndAmountDetailViewModel();
            FeeDetail = new FeeDetailViewModel();
            GetNotification = new GetNotificationViewModel();
            BankDetailList = new List<BankAndAmountDetailViewModel>();
            FeeName = "";
        }
        public int FeePaymentDetailId { get; set; }
        public int FeeDetailId { get; set; }
        public int PaymentModeId { get; set; }
        public string TransectionId { get; set; }
        public Nullable<int> FIN_BankDetailId { get; set; }
        public string RecieptNo { get; set; }
        public bool IsActive { get; set; }
        public int EnteredBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public System.DateTime EnteredDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public string CurrentDate { get; set; }
        public int FeeTypeDetailId { get; set; }
        public int PaymentTypeId { get; set; }
        public decimal Amount { get; set; }
        public string FeePayModeName { get; set; }
        public Nullable<int> FeeTypeId { get; set; }
        public Nullable<int> PT { get; set; }
        public string FeeName { get; set; }
        public bool IsFeePayStandBy { get; set; }
        public int hdnFeeDetailId { get; set; }
        public string SettleDate { get; set; }
        public string SessionInstallmentJson { get; set; }

       // public BankInfoViewModel BankDetail { get; set; }
        public BankAndAmountDetailViewModel BankDetail { get; set; }
        public List<BankAndAmountDetailViewModel> BankDetailList { get; set; }
        public FeeDetailViewModel FeeDetail { get; set; }
        public RegistrationViewModel RegistrationInfo { get; set; }
        public AddFeeCollectionViewModel FeeCollection { get; set; }
        public GetNotificationViewModel GetNotification { get; set; }
        public List<CourseMasterViewModel> GetCourseList { get; set; }
        public List<AddFeeCollectionViewModel> FeeCollectionList { get; set; }
        public IEnumerable<RoleViewModel> PaymentModeList { get; set; }
        public IEnumerable<RoleViewModel> FeeTypeDetailList { get; set; }
        public IEnumerable<RoleViewModel> PaymentTypeList { get; set; }
    }
}

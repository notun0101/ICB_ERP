using Microsoft.AspNetCore.Http;
using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Accounting.Data.Entity.Voucher;
using OPUSERP.Data.Entity.AttachmentComment;
using System.Collections.Generic;

namespace OPUSERP.Areas.Accounting.Models
{
    public class AccountGroupViewModel
    {
        public int accountGroupId { get; set; }
        public int? natureId { get; set; }
        public string groupCode { get; set; }
        public string groupName { get; set; }
        public string groupNameBN { get; set; }
        public string isCashBank { get; set; }
        public int? parentGroupId { get; set; }

        public IEnumerable<GroupNature> groupNatures { get; set; }
        public IEnumerable<AccountGroup> accountGroups { get; set; }
        public IEnumerable<Comment> comments { get; set; }
        public IEnumerable<DocumentPhotoAttachment> photoes { get; set; }
        public IEnumerable<DocumentPhotoAttachment> documents { get; set; }
        public AccountGroup getAccountGroupDetailsById { get; set; }
        
    }

    public class BulkUploadModel
    {
        public IFormFile formFile { get; set; }
        public IEnumerable<UploadedVoucherData> voucherLists { get; set; }
        public List<string> fileList { get; set; }
        //public List<FileList> fileList { get; set; }
    }

    public class FileList
    {
        public string fileName { get; set; }
    }
}

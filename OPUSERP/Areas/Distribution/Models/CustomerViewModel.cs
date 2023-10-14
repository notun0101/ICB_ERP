﻿using OPUSERP.CRM.Data.Entity.Lead;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.Data.Entity.Master;
using OPUSERP.Distribution.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.OtherSales.Data.Entity.Sales;
using OPUSERP.SCM.Data.Entity.MasterData;
using OPUSERP.SCM.Data.Entity.Requisition;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Distribution.Models
{
    public class CustomerViewModel
    {
        //public int resourceId { get; set; }
        //public int relSupplierCustomerResourseId { get; set; }
        //public int? organizationId { get; set; }
        //public int? roleTypeId { get; set; }
        //public int? ledgerId { get; set; }

        //public string nameEnglish { get; set; }
        //public string organizationName { get; set; }
        //public string telephoneResidence { get; set; }
        //public string pabx { get; set; }
        //public string mobileNumberPersonal { get; set; }
        //public string phone { get; set; }       
        //public string mobile { get; set; }       
        //public string email { get; set; }
        //public string nationalID { get; set; }
        //public string nationality { get; set; }

        //public IEnumerable<RelSupplierCustomerResourse> relSupplierCustomerResourses { get; set; }
        //public IEnumerable<Organization> organizations { get; set; }

        //public Resource resource { get; set; }
        //public RelSupplierCustomerResourse relSupplierCustomerResourse { get; set; }


        #region Customer/Resource
        public int? resourceId { get; set; }
        public int relSupplierCustomerResourseId { get; set; }
        public int? organizationId { get; set; }
        public string nameEnglish { get; set; }
        public string organizationName { get; set; }
        public string LicenseNumber { get; set; }

        public string eTinNumber { get; set; }
        public string binNumber { get; set; }

        public string VATNumber { get; set; }

        public string email { get; set; }

        public string alternativeEmail { get; set; }

        public string mobile { get; set; }

        public string phone { get; set; }

        public string telephoneResidence { get; set; }

        public string pabx { get; set; }

        public string mobileNumberPersonal { get; set; }

        public string nationalID { get; set; }

        public string nationality { get; set; }
        #endregion

        #region contacts
        public int? contactid { get; set; }       
        public string resourceName { get; set; }
        public int designationId { get; set; }
        public string[] phoneall { get; set; }
        public string[] mobileall { get; set; }
        public string[] contactall { get; set; }
        public string[] deptall { get; set; }
        public string[] desall { get; set; }

        #endregion

        #region Address
        public int?[] orgAddressID { get; set; }

        public int?[] addressCategoryId { get; set; }

        public int?[] orgDivision { get; set; }
        
        public int?[] orgDistrict { get; set; }
        
        public int?[] orgUpazila { get; set; }
        
        public string[] orgUnion { get; set; }
        
        public string[] orgPostOffice { get; set; }
        
        public string[] orgPostCode { get; set; }
        
        public string[] orgBlockSector { get; set; }
        
        public string[] orgHouseVillage { get; set; }

        public string orgType { get; set; }
        public int? isAuthorized { get; set; }
        #endregion

        #region Item Info
        public int?[] item { get; set; }       
        public int?[] itemidall { get; set; }

        public int? customerItemInfoId { get; set; }
        public int? itemId { get; set; }
        public string itemName { get; set; }
        #endregion
        #region DistributorType
        public int? distributorTypeId { get; set; }
        public string distributorname { get; set; }
        #endregion
        #region Area
        public int? areaid { get; set; }
        public int?[] areaidall { get; set; }
        public string areaname { get; set; }
        public int?[] areainfoidall { get; set; }
        public int? areaInfoId { get; set; }
        #endregion
        #region Team
        public int? teamid { get; set; }
        public int?[] teamnsmidall { get; set; }
        public int?[] teamtsmidall { get; set; }
        public int?[] teamrsmidall { get; set; }
        public string teamname { get; set; }
        public int?[] teaminfoidall { get; set; }
        public int?[] employeeInfoAll { get; set; }
        public int?[] salesLevelIdAll { get; set; }
        public int?[] areaIdAll { get; set; }
        public int? teamInfoId { get; set; }
        #endregion


        public IEnumerable<RelSupplierCustomerResourse> relSupplierCustomerResourses { get; set; }
        public IEnumerable<Organization> organizations { get; set; }
        public RelSupplierCustomerResourse relSupplierCustomerResourse { get; set; }

        public string actionType { get; set; }
        public IEnumerable<DocumentAttachment> photoes { get; set; }
        public IEnumerable<DocumentAttachment> documents { get; set; }
        public IEnumerable<CustomerComment> customerComments { get; set; }
        public IEnumerable<Resource> GetResourceInfos { get; set; }
        public Resource GetResourcebyId { get; set; }
        public Organization GetOrganizationbyId { get; set; }
        public Address GetAddress { get; set; }
        public IEnumerable<Item> items { get; set; }
        public IEnumerable<Designation> designations { get; set; }
        public IEnumerable<AddressCategory> addressCategories { get; set; }
        public IEnumerable<DistributorType> distributorTypes { get; set; }
        public IEnumerable<AddressType> addressTypes { get; set; }
        public IEnumerable<Area> areas { get; set; }
        public IEnumerable<SalesTeamDeployment> salesTeamDeployments { get; set; }
        public IEnumerable<RelCustomerSalesTeamDeployment> relCustomerSalesTeamDeployments { get; set; }
        public IEnumerable<SalesLevel> salesLevels { get; set; }
        //public FuelStationInfoLN vlang { get; set; }
    }
}

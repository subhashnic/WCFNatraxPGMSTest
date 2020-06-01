using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using WCFServiceTemplate;

namespace WCFPGMSFront
{
    [ServiceContract]
    public interface IService1
    {
        #region Transaction
        #region Login

        [OperationContract]
        returndbmlUser UserGetByLoginId(string strLoginId, string strPassword);

        [OperationContract]
        returndbmlDashBoardWorkFlowViewFront DashBoardWorkFlowCount(int intUserId, int intCompanyId);

        [OperationContract]
        returndbmlUser UserViewFrontGetByCompanyId(int intCompanyId);

        [OperationContract]
        returndbmlUser UserViewFrontGetByDepartmentId(int intDepartmentId);

        [OperationContract]
        returndbmlUser UserViewGetByLoginIdUserId(string strLoginId, int intUserId);

        #endregion

        #region Properties / OptionList
        [OperationContract]
        returndbmlProperty PropertiesGetAll();

        [OperationContract]
        returndbmlOptionList OptionListGetByPropertyId(int intPropertyId);

        [OperationContract]
        returndbmlProperty PropertiesGetByPropertyTypeId(int intPropertyTypeId);
        #endregion

        #region Company/Department      
        [OperationContract]
        returndbmlCompanyView CustomerMasterInsertFront(returndbmlCompanyView objreturndbmlCompanyView);

        [OperationContract]
        returndbmlUser UsereMailIdVerification(int intUserId);

        [OperationContract]
        returndbmlUser UserPaswordReset(int intUserId, string strPassword);

        [OperationContract]
        returndbmlCompanyDepartment CompanyDepartmentInsert(returndbmlCompanyDepartment objreturndbmlCompanyDepartment);

        [OperationContract]
        returndbmlCompanyDepartment CompanyDepartmentUpdate(returndbmlCompanyDepartment objreturndbmlCompanyDepartment);

        [OperationContract]
        returndbmlUser UserInsert(returndbmlUser objreturndbmlUser);

        [OperationContract]
        returndbmlUser UserUpdate(returndbmlUser objreturndbmlUser);

        [OperationContract]
        returndbmlCompanyView CompanyViewGetByCompanyId(int intCompanyId);

        [OperationContract]
        returndbmlCompanyDepartment CompanyDepartmentGetByCustomerMasterId(int intCustomerMasterId);

        [OperationContract]
        returndbmlState StateGetAll();

        [OperationContract]
        returndbmlDistrict DistrictGetByStateId(int intStateId);

        [OperationContract]
        returndbmlDistrict DistrictGetAll();
        #endregion

        #region Booking

        #region Basic      
        [OperationContract]
        returndbmlBooking BookingInsert(returndbmlBooking objreturndbmlBooking);

        [OperationContract]
        returndbmlBooking BookingUpdate(returndbmlBooking objreturndbmlBooking);

        [OperationContract]
        returndbmlStatus BookingDeleteAllByBookingId(int intBookingId);

        [OperationContract]
        returndbmlBooking BookingViewGetByBookingId(int intBookingId);

        [OperationContract]
        returndbmlBooking BookingViewGetByCompanyIdStatusPropId(int intCompanyId, int intStatusPropId);
        
        [OperationContract]
        returndbmlBookingSearchView BookingSearchViewGetByCompanyIdFromDateToDateFront(int intCompanyId, DateTime dtFromDate, DateTime dtToDate, int intBPId, int intStatusPropId);

        [OperationContract]
        returndbmlStatus BookingQuotationPIDetailInsertByBookingId(int intDocId);

        [OperationContract]
        returndbmlBookingSearchView RFQBookingSearchViewFrontGetByCompanyIdFromDateToDate(int intCompanyId, DateTime dtFromDate, DateTime dtToDate, int intBPId, int intStatusPropId);

        [OperationContract]
        returndbmlRFQBookingDetail RFQBookingDetailGetByBookingIdBPId(int intBookingId, int intBPId);

        [OperationContract]
        returndbmlBooking RFQBookingDetailInsertByBookingIdBPId(int intRFQBookingId, int intRFQBPId, int intBPId, int intUserId, int intCompanyId);

        [OperationContract]
        returndbmlServiceDateViewFront ServiceDateViewFrontGetByBookingId(int intBookingId);

        [OperationContract]
        returndbmlBooking UpdateServiceDateFrontByBookingIdDayDates(returndbmlServiceDateViewFront objreturndbmlServiceDateViewFront);
        #endregion

        #region Vehicle Componants
        [OperationContract]
        returndbmlListOfVehicleComponent ListOfVehicleComponentInsert(returndbmlListOfVehicleComponent objreturndbmlListOfVehicleComponent);

        [OperationContract]
        returndbmlListOfVehicleComponent ListOfVehicleComponentUpdate(returndbmlListOfVehicleComponent objreturndbmlListOfVehicleComponent);

        [OperationContract]
        returndbmlListOfVehicleComponent ListOfVehicleComponentDeleteByDocIdCompId(int intDocId, int intVehCompId);

        [OperationContract]
        returndbmlListOfVehicleComponent ListOfVehicleComponentGetByDocId(int intDocId);
        #endregion

        #region Tracks/Services
        [OperationContract]
        returndbmlServicesView ServicesGetByBPId(int intBPId);

        [OperationContract]
        returndbmlTrackBookingDetail TrackBookingDetailGetByBookingIdTrackGroupId(int intBookingId, int intTrackGroupId);

        [OperationContract]
        returndbmlBookingStatusTimeSlotView BookingStatusGetByServiceIdTimeSlotPropIdWEFDate(ObservableCollection<int> intlstServiceId, int intTimeSlotId, DateTime dtWED);

        [OperationContract]
        returndbmlTrackBookingDetail TrackBookingDetailInsertFront(returndbmlTrackBookingDetail objreturndbmlTrackBookingDetail);

        [OperationContract]
        returndbmlTrackBookingDetail TrackBookingTimeDetailDeleteFrontByServiceId(int intBookingId, int intTrackGroupId, int intVehicleId, DateTime dtDate, int intServiceId, int intTimeSlotId);
        #endregion

        #region WorkFlow Activity
        [OperationContract]
        returndbmlWorkFlowView WorkFlowViewGetByBPId(int intBPId, int intDocId);

        [OperationContract]
        returndbmlBooking WorkFlowActivityInsert(int intDocId, int intBPId, int intWorkPlowId, int intStatusId, string strRemark, int intCreateId);

        [OperationContract]
        returndbmlWorkFlowActivityTrackView WorkFlowActivityTrackGetByBPIdDocId(int intBPId, int intDocId);
        #endregion

        #region Workshop Booking Detail
        [OperationContract]
        returndbmlWorkshopBookingDetailViewFront WorkshopBookingDetailInsertFront(returndbmlWorkshopBookingDetailViewFront objreturndbmlWorkshopBookingDetailViewFront);

        [OperationContract]
        returndbmlWorkshopBookingDetailViewFront WorkshopBookingDetailDelete(int intDocId, int intWorkshopBookingDetailId);

        [OperationContract]
        returndbmlWorkshopBookingDetailViewFront WorkshopBookingDetailViewFrontGetByBookingId(int intDocId);
        #endregion

        #region Booking Detail AddOnServices
        [OperationContract]
        returndbmlBookingDetailAddOnServicesViewFront BookingDetailAddOnServicesInsertFront(returndbmlBookingDetailAddOnServicesViewFront objreturndbmlBookingDetailAddOnServicesViewFront);

        [OperationContract]
        returndbmlBookingDetailAddOnServicesViewFront BookingDetailAddOnServicesDelete(int intDocId, int intBookingDetailAddOnServicesId);

        [OperationContract]
        returndbmlBookingDetailAddOnServicesViewFront BookingDetailAddOnServicesViewFrontGetByBookingId(int intDocId);
        #endregion

        #region Lab Booking Detail
        [OperationContract]
        returndbmlLabBookingDetailViewFront LabBookingDetailInsertFront(returndbmlLabBookingDetailViewFront objreturndbmlLabBookingDetailViewFront);

        [OperationContract]
        returndbmlLabBookingDetailViewFront LabBookingDetailDelete(int intDocId, int intLabBookingDetailId);

        [OperationContract]
        returndbmlLabBookingDetailViewFront LabBookingDetailViewFrontGetByBookingId(int intDocId);

        [OperationContract]
        returndbmlLablinkVorC LablinkVorCGetAll();
        #endregion

        #endregion

        #endregion


    }
}
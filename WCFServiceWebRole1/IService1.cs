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

        #endregion

        #region Properties
        [OperationContract]
        returndbmlProperty PropertiesGetAll();
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
        returndbmlCompanyDepartment CompanyDepartmentGetByCustomerMasterId(int intCustomerMasterId);

        [OperationContract]
        returndbmlBookingSearchView BookingSearchViewGetByCompanyIdFromDateToDateFront(int intCompanyId, DateTime dtFromDate, DateTime dtToDate, int intBPId, int intStatusPropId);
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

        #endregion

        #endregion


    }
}
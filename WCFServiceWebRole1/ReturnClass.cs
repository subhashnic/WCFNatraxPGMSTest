using System.Collections.ObjectModel;
using WCFServiceTemplate;

namespace WCFPGMSFront
{

    #region Transaction Table
    public class returndbmlStatus
    {
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlUser
    {
        public ObservableCollection<dbmlUserView> objdbmlUserView = new ObservableCollection<dbmlUserView>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlProperty
    {
        public ObservableCollection<dbmlProperty> objdbmlProperty = new ObservableCollection<dbmlProperty>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    
    public class returndbmlCompanyDepartment
    {
        public ObservableCollection<dbmlCompanyDepartment> objdbmlCompanyDepartment = new ObservableCollection<dbmlCompanyDepartment>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlListOfVehicleComponent
    {
        public ObservableCollection<dbmlListOfVehicleComponent> objdbmlListOfVehicleComponent = new ObservableCollection<dbmlListOfVehicleComponent>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    
    public class returndbmlBooking
    {
        public ObservableCollection<dbmlBookingView> objdbmlBookingList = new ObservableCollection<dbmlBookingView>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlBookingSearchView
    {
        public ObservableCollection<dbmlBookingSearchView> objdbmlBookingSearchView = new ObservableCollection<dbmlBookingSearchView>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlBookingStatusTimeSlotView
    {
        public ObservableCollection<dbmlBookingStatusTimeSlotView> objdbmlBookingStatusTimeSlotView = new ObservableCollection<dbmlBookingStatusTimeSlotView>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlServicesView
    {
        public ObservableCollection<dbmlServicesView> objdbmlServicesView = new ObservableCollection<dbmlServicesView>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlTrackBookingDetail
    {
        public ObservableCollection<dbmlTrackBookingDetail> objdbmlTrackBookingDetail = new ObservableCollection<dbmlTrackBookingDetail>();
        public ObservableCollection<dbmlTrackBookingTimeDetail> objdbmlTrackBookingTimeDetail = new ObservableCollection<dbmlTrackBookingTimeDetail>();
        public ObservableCollection<dbmlTrackBookingTimeSummary> objdbmlTrackBookingTimeSummary = new ObservableCollection<dbmlTrackBookingTimeSummary>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlWorkFlowView
    {
        public ObservableCollection<dbmlWorkFlowView> objdbmlWorkFlowView = new ObservableCollection<dbmlWorkFlowView>();
     
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlCompanyView
    {
        public ObservableCollection<dbmlCompanyView> objdbmlCompanyView = new ObservableCollection<dbmlCompanyView>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlWorkshopBookingDetailViewFront
    {
        public ObservableCollection<dbmlWorkshopBookingDetailViewFront> objdbmlWorkshopBookingDetailViewFront = new ObservableCollection<dbmlWorkshopBookingDetailViewFront>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlBookingDetailAddOnServicesViewFront
    {
        public ObservableCollection<dbmlBookingDetailAddOnServicesViewFront> objdbmlBookingDetailAddOnServicesViewFront = new ObservableCollection<dbmlBookingDetailAddOnServicesViewFront>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlLabBookingDetailViewFront
    {
        public ObservableCollection<dbmlLabBookingDetailViewFront> objdbmlLabBookingDetailViewFront = new ObservableCollection<dbmlLabBookingDetailViewFront>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlOptionList
    {
        public ObservableCollection<dbmlOptionList> objdbmlOptionList = new ObservableCollection<dbmlOptionList>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlLablinkVorC
    {
        public ObservableCollection<dbmlLablinkVorC> objdbmlLablinkVorC = new ObservableCollection<dbmlLablinkVorC>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlRFQBookingDetail
    {
        public ObservableCollection<dbmlBookingView> objdbmlBookingView = new ObservableCollection<dbmlBookingView>();
        public ObservableCollection<dbmlListOfVehicleComponent> objdbmlListOfVehicleComponent = new ObservableCollection<dbmlListOfVehicleComponent>();
        public ObservableCollection<dbmlTrackBookingDetail> objdbmlTrackBookingDetail = new ObservableCollection<dbmlTrackBookingDetail>();
        public ObservableCollection<dbmlTrackBookingTimeDetail> objdbmlTrackBookingTimeDetail = new ObservableCollection<dbmlTrackBookingTimeDetail>();
        public ObservableCollection<dbmlTrackBookingTimeSummary> objdbmlTrackBookingTimeSummary = new ObservableCollection<dbmlTrackBookingTimeSummary>();
        public ObservableCollection<dbmlLabBookingDetailViewFront> objdbmlLabBookingDetailViewFront = new ObservableCollection<dbmlLabBookingDetailViewFront>();
        public ObservableCollection<dbmlWorkshopBookingDetailViewFront> objdbmlWorkshopBookingDetailViewFront = new ObservableCollection<dbmlWorkshopBookingDetailViewFront>();
        public ObservableCollection<dbmlBookingDetailAddOnServicesViewFront> objdbmlBookingDetailAddOnServicesViewFront = new ObservableCollection<dbmlBookingDetailAddOnServicesViewFront>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlServiceDateViewFront
    {
        public ObservableCollection<dbmlServiceDateViewFront> objdbmlServiceDateViewFront = new ObservableCollection<dbmlServiceDateViewFront>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlWorkFlowActivityTrackView
    {
        public ObservableCollection<dbmlWorkFlowActivityTrackView> objdbmlWorkFlowActivityTrackView = new ObservableCollection<dbmlWorkFlowActivityTrackView>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion


}
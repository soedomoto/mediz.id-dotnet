using MedizID.UI.Services.Generated;
using MedizID.UI.Services.Generated.Models;
using Vizor.Icons.Tabler;

public class AppointmentLayoutState
{
    private bool _isAuthenticated = false;
    public bool IsAuthenticated 
    { 
        get => _isAuthenticated;
        set 
        { 
            if (_isAuthenticated != value)
            {
                _isAuthenticated = value;
                OnAuthenticationChanged?.Invoke(value);
            }
        }
    }
    public event Action<bool>? OnAuthenticationChanged;
    public bool IsAuthenticating = false;
    private AppointmentDetailResponse? _appointment;
    public AppointmentDetailResponse? Appointment
    {
        get => _appointment;
        set
        {
            _appointment = value;
            OnFacilityChanged?.Invoke(value);
        }
    }
    public event Action<AppointmentDetailResponse?>? OnFacilityChanged;
    public Dictionary<string, (string Title, IconData? Icon)> Tabs = new()
    {
        [""] = ("Home", TablerSvgIcon.BuildingHospital),
        ["anamnesis"] = ("Anamnesis", TablerSvgIcon.FileText),
        ["diagnosis"] = ("Diagnosis", TablerSvgIcon.ClipboardCheck),
        ["prescription"] = ("Prescription", TablerSvgIcon.Pills),
        ["odontogram"] = ("Odontogram", TablerSvgIcon.Dental),
        ["laboratory"] = ("Laboratory", TablerSvgIcon.Flask),
        ["procedure"] = ("Procedure", TablerSvgIcon.Scissors),
        ["immunization"] = ("Immunization", TablerSvgIcon.Vaccine),
        ["family-planning"] = ("Family Planning", TablerSvgIcon.BabyCarriage),
        ["adolescent-health"] = ("Adolescent Health", TablerSvgIcon.School),
        ["maternal-child-health"] = ("Maternal & Child Health", TablerSvgIcon.BabyCarriage),
        ["sexual-transmitted-infections"] = ("Sexual & Transmitted Infections", TablerSvgIcon.HeartBroken),
        ["hiv-counseling"] = ("HIV Counseling", TablerSvgIcon.ShieldCheck),
    };
    public string _activeTabKey = "";
    public string ActiveTabKey
    {
        get => _activeTabKey;
        set
        {
            _activeTabKey = value;
            OnActiveTabKeyChanged?.Invoke(value);
        }
    }
    public event Action<string?>? OnActiveTabKeyChanged;

    public async Task FetchAppointmentAsync(MedizIdApiClient ApiClient, Guid AppointmentId)
    {
        Appointment = await ApiClient.Api.V1.Appointments[AppointmentId].GetAsync();
    }
}